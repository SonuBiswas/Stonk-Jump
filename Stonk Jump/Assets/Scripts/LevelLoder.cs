using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoder : MonoBehaviour
{
    // public GameObject LoadingPanal;
    // public Slider LoadingSlider;
    
    private void Start()
    {
        //MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);

        int lastPlayedLevel = PlayerPrefs.GetInt("LastPlayedLevel", 1);
        // SceneManager.LoadScene(lastPlayedLevel);
        loadLevel(lastPlayedLevel);
    }
    public void loadLevel(int SceneIndex)
    {
        StartCoroutine(LoadAsync(SceneIndex));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation opration = SceneManager.LoadSceneAsync(sceneIndex);
       // LoadingPanal.SetActive(true);
        while (!opration.isDone)
        {
            float progress = Mathf.Clamp01(opration.progress / .9f);
            //LoadingSlider.value = progress;
            yield return null;
        }
    }
}
