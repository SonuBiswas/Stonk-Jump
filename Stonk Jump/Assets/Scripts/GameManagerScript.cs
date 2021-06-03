using DG.Tweening;
using MoreMountains.NiceVibrations;
//using LionStudios;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] toDisable,Confetti;
    public GameObject LevelCompletePanal;
    public float waitbeforePanalShow, panalLerpValue;public int Score;
    public TextMeshProUGUI ScoreText,LevelCompleteMsg,CurrentLevelTxt;
    public string LevelSucessMsg, LevelFaildMsg;
    public Color LevelSucessColor, LevelFaildColor;
    public Button NextButton, RetryButton;
    public int LevelStartingPoint,LevelloopingPoint,currentLevel;
    MyFB_SDK FB_SDK;
    public void levelComplete()
    {
        //Analytics.Events.LevelComplete(levelName);
        FB_SDK.LogLevelCompleteEvent(levelName);

        LevelcompleteEffect();
        
        if (toDisable.Length != 0)
        {
            foreach (var item in toDisable)
            {
                item.SetActive(false);
            }
        }

        StartCoroutine(showBtns(true));

       

        StartCoroutine(ShowPanal(LevelSucessMsg,LevelSucessColor));
    } 
    public void levelFaild()
    {
       
        
        if (toDisable.Length != 0)
        {
            foreach (var item in toDisable)
            {
                item.SetActive(false);
            }
        }

        StartCoroutine(showBtns(false));
        StartCoroutine(ShowPanal(LevelFaildMsg,LevelFaildColor));
    }
    int myFloat=0;

    IEnumerator showBtns(bool IsPass)
    {
        if (waitbeforePanalShow > 0.0f)
        {
            yield return new WaitForSeconds(waitbeforePanalShow);

        }
        if (IsPass == true)
        {
            NextButton.gameObject.SetActive(true);
            NextButton.gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }
        else
        {
            RetryButton.gameObject.SetActive(true);
            RetryButton.gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }
    }
    IEnumerator ShowPanal(string msg, Color c)
    {
        if (waitbeforePanalShow > 0.0f)
        {
            yield return new WaitForSeconds(waitbeforePanalShow);

        }
        LevelCompletePanal.SetActive(true);
        LevelCompletePanal.GetComponent<DOTweenAnimation>().DOPlay();

        LevelCompleteMsg.color = c;
        LevelCompleteMsg.text = msg;
        Score -=1;
        Score = Score * 10;
        DOTween.To(() => myFloat, x => myFloat = x, Score, 0.5f);
        
    }
    void LevelcompleteEffect()
    {
        if (Confetti.Length != 0)
        {
            foreach (var item in Confetti)
            {
                item.SetActive(true);
            }
        }
        
        //Time.timeScale = .5f;
    }
    string levelName;
    void Start()
    {

       FB_SDK = FindObjectOfType<MyFB_SDK>();

        LevelCompletePanal.SetActive(false);

        NextButton.onClick.AddListener(() => Next());
        RetryButton.onClick.AddListener(() => Retry());

        NextButton.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(false);

        int loopingNumber = PlayerPrefs.GetInt("LoopNumber", 0);
        int LevelNumber = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LastPlayedLevel", LevelNumber);

        //currentLevel = (LevelNumber + (loopingNumber * LevelloopingPoint));
        currentLevel = (loopingNumber > 0) ? (loopingNumber+LevelloopingPoint) : LevelNumber;

        levelName = currentLevel.ToString();
        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "Level "+levelName;
        }
        //Analytics.Events.LevelStarted(levelName);
        FB_SDK.LogLevelStartedEvent(levelName);
    }
    private void Update()
    {
        if (ScoreText != null)
        {
            ScoreText.text = myFloat.ToString(levelName);
        }
    }

    public void Next()
    {
        NextButton.interactable = false;
        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);

        NextButton.transform.DOPunchScale(Vector3.one, .4f, 1, 0).OnComplete(() => doNext());

       


    }
    public void Retry()
    {

        RetryButton.interactable = false;
        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);

        RetryButton.transform.DOPunchScale(Vector3.one, .4f, 1, 0).OnComplete(() => doRetry());
        

    }

    void doNext()
    {
        NextButton.gameObject.SetActive(false);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == LevelloopingPoint)
        {
            int loop = PlayerPrefs.GetInt("LoopNumber", 0);
            loop += 1;
            PlayerPrefs.SetInt("LoopNumber", loop);

            SceneManager.LoadScene(LevelloopingPoint);

        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);

        }
    }
    void doRetry()
    {
        RetryButton.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
