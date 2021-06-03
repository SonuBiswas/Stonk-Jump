using Cinemachine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using System.Collections;
using System.Globalization;

using TMPro;
using UnityEngine;

public class JetpackPlayer : MonoBehaviour
{
    GameManagerScript gameManager;
    [SerializeField] float Force,ForwardSpeed,UpForce,lerpValue,MoneyValue;
    bool goUp;
    Rigidbody rb;
    [SerializeField] ParticleSystem jetParticle,SpeedUpParticle,SpeedDropParticle;
    [SerializeField] LineRenderer line;
    [SerializeField] TextMeshProUGUI TotalMoneyText,AdditinolMoney,liveMoneyText;
    CultureInfo us;
    float yPos,_UpForce,xPos;
    Transform LineObj;
    [SerializeField] Color _Red, _Green;
    [SerializeField] Animator anim;
    int up = Animator.StringToHash("Up"),Flying= Animator.StringToHash("Flying");
    [SerializeField] AnimationCurve curve;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Gradient JumpGradint, FallGradient;
    [SerializeField] CinemachineVirtualCamera secondCam;
    [SerializeField] Transform rayPoint;
    [SerializeField] LayerMask layer;
    bool gameOver;
    float TotalMoney,EarenedMoney;
    DOTweenAnimation aditionalMonyAnimation;
    [SerializeField] GameObject Obstackles,HoldToJump,LiveMoneyObj;
    float initialtime;
    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        _UpForce = UpForce;
        us = new CultureInfo("en-US");
        rb = GetComponent<Rigidbody>();
        LineObj = line.transform;
        line.positionCount = 1;
        drawLine();
        TotalMoney = PlayerPrefs.GetFloat("TotalMoney", 10000);
        TotalMoneyText.text = TotalMoney.ToString("C0", us);
        aditionalMonyAnimation = AdditinolMoney.GetComponent<DOTweenAnimation>();

        if (LiveMoneyObj != null) 
        {
            liveMoneyText = LiveMoneyObj.GetComponentInChildren<TextMeshProUGUI>();
            liveMoneyText.text= TotalMoney.ToString("C0", us);
          
            LiveMoneyObj.SetActive(false);
            StartCoroutine(LiveMoneyUpdate());
        }
        initialtime = Time.timeScale;
    }

    private Vector3 velo;
    [SerializeField] float smooth;
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.timeScale != 1.7f) Time.timeScale = 1.7f;
                rb.velocity = new Vector3(0, 0, rb.velocity.z);

                DOTween.To(() => yPos, x => yPos = x, 1, 0.3f).SetEase(curve);

                drawLine();

                if (!jetParticle.isPlaying) jetParticle.Play(true);

                goUp = true;
                if (HoldToJump.activeSelf == true) StartCoroutine(disableHold());
                if (TotalMoneyText.gameObject.activeSelf == true) TotalMoneyText.gameObject.SetActive(false);
                if (LiveMoneyObj != null)
                {
                    if (!LiveMoneyObj.activeSelf) LiveMoneyObj.SetActive(true);
                }
                    MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
                if (!SpeedUpParticle.isStopped) SpeedUpParticle.Stop(true);
                if (!SpeedDropParticle.isStopped) SpeedDropParticle.Stop(true);

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.timeScale != 1.3f) Time.timeScale = 1.3f;

                DOTween.To(() => yPos, x => yPos = x, 0, 0.5f).SetEase(Ease.InCubic);

                drawLine();
                if (!jetParticle.isStopped) jetParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                goUp = false;

                if (!SpeedUpParticle.isStopped) SpeedUpParticle.Stop(true);
                if (!SpeedDropParticle.isStopped) SpeedDropParticle.Stop(true);

            }


            // goUp = Input.GetMouseButton(0);
            rb.useGravity = !goUp;
            anim.SetBool(up, goUp);

            
            EarenedMoney = (transform.position.y * MoneyValue);
            


        }



        if (rb.velocity.y > -5)
        {
            //trail.startColor = _Green;
            trail.colorGradient = JumpGradint;
        }
        else
        {
            //trail.startColor = _Red;
            trail.colorGradient = FallGradient;

        }


        if(LiveMoneyObj != null)
        {
            if (LiveMoneyObj.activeSelf)
            {

                //liveMoneyText.text = (TotalMoney + EarenedMoney).ToString("C0", us);

                //int m = (int)(TotalMoney + EarenedMoney);
                //if (m % 6 == 0)
                //{
                //    liveMoneyText.text = m.ToString("C0", us);


                //}

                if (rb.velocity.y > -5)
                {
                    //trail.startColor = _Green;
                    liveMoneyText.color = _Green;
                }
                else
                {
                    //trail.startColor = _Red;
                    liveMoneyText.color = _Red;


                }
            }
        }

        if (Physics.Raycast(rayPoint.position, Vector3.down, 1f, layer)&& (rb.velocity.x < .5f))
        {
           
           anim.SetBool(Flying, false);

        }
        else
        {
            anim.SetBool(Flying,true);

        }

        
        line.SetPosition(line.positionCount - 1, LineObj.position);

    }

    private void FixedUpdate()
    {

        if (goUp)
        {
            //rb.velocity =  new Vector3(ForwardSpeed, rb.velocity.y, rb.velocity.z);
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(ForwardSpeed, rb.velocity.y, rb.velocity.z),0.15f);
        }
        rb.position = new Vector3(rb.position.x, rb.position.y + (yPos * Time.fixedDeltaTime * UpForce), rb.position.z);
    }

    private void LateUpdate()
    {
        //Camtarget.position = Vector3.SmoothDamp(Camtarget.position, transform.position, ref velo, smooth);
    }
    void drawLine()
    {
        line.positionCount += 1;
        line.SetPosition(line.positionCount - 1, LineObj.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gain"))
        {
            
            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);

            var _gainloss = other.GetComponent<gainLoss>();
            if (_gainloss != null)
            {
                if (_gainloss.gain)
                {
                    if (!doingGainloss) StartCoroutine(DoGainLoss(true, _gainloss.value));
                }
                else
                {
                    if (!doingGainloss) StartCoroutine(DoGainLoss(false, _gainloss.value));

                }
                //Destroy(other.gameObject);
                other.GetComponent<SphereCollider>().enabled = false;
            }
        }

        if (other.name == "finishline")
        {
            Finish();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            trail.enabled = false;
            line.endColor = _Green;

            DOTween.To(() => line.widthMultiplier, x => line.widthMultiplier = x, 2.5f, 0.5f).SetEase(Ease.Linear);

            Obstackles.SetActive(false);
            //line.widthMultiplier = 4.1f;
            secondCam.Priority = 11;

        }

        if (other.CompareTag("Obs"))
        {
            dead();
            //goUp = false;
            //yPos = 0;
            //rb.useGravity = true;
        }
    }
    bool doingGainloss;
    
    IEnumerator DoGainLoss (bool gain , float value)
    {
        doingGainloss = true;
        
        if (gain)
        {
            UpForce *= value;
            //DOTween.To(() => UpForce, x => UpForce = x, UpForce *= value, 0.3f).SetEase(Ease.Linear);

            rb.useGravity = false;
            yPos = 1;
            if (!jetParticle.isPlaying) jetParticle.Play(true);
            trail.startColor = _Green;
            if (SpeedUpParticle != null)
            {
                SpeedUpParticle.Play(true);
            }


        }
        else
        {

            //UpForce -= value;
            goUp = false;
            rb.useGravity = true;

            yPos = 0;
            Time.timeScale = 2.5f;
            if (!jetParticle.isStopped) jetParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //trail.startColor = _Red;
            SpeedDropParticle.Play(true);
        }
        yield return new WaitForSeconds(1.5f);
        UpForce = _UpForce;
       // DOTween.To(() => UpForce, x => UpForce = x, _UpForce, .5f).SetEase(Ease.Linear);

        //yPos = 0;
        //rb.useGravity = true;
        //if (!jetParticle.isStopped) jetParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        
        if(SpeedUpParticle.isPlaying)SpeedUpParticle.Stop(true);


        if (SpeedDropParticle.isPlaying) SpeedDropParticle.Play(true);

        doingGainloss = false;
        
    }

    IEnumerator LiveMoneyUpdate()
    {
        while (!gameOver)
        {
            liveMoneyText.text = (TotalMoney + EarenedMoney).ToString("C0", us);
            yield return new WaitForSeconds(.2f);
        }
       
    }
    IEnumerator drawMultiple()
    {
        for (int i = 0; i < 8; i++)
        {
            drawLine();
            yield return new WaitForSeconds(.1f);
        }
    }
   
    IEnumerator disableHold()
    {
        yield return new WaitForSeconds(1);
        HoldToJump.SetActive(false);
    }

    void dead()
    {
        if (gameOver) return;
        MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        goUp = false;
        yPos = 0;
        gameOver = true;

        if (LiveMoneyObj != null)
        {
            LiveMoneyObj.SetActive(false);
        }

        gameManager.levelFaild();
    }
    void Finish()
    {
        if (gameOver) return;
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);

        goUp = false;
        yPos = 0;
        gameOver = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        float returnPercentage = ((EarenedMoney / TotalMoney) * 100);


        TotalMoney += EarenedMoney;
        TotalMoneyText.text = TotalMoney.ToString("C0", us);

        //AdditinolMoney.color = _Green;
        //AdditinolMoney.text = "+"+((int)returnPercentage).ToString() + "%";

        if (LiveMoneyObj != null)
        {
          LiveMoneyObj.SetActive(false);
        }
        TotalMoneyText.gameObject.SetActive(true);
        //aditionalMonyAnimation.DOPlay();
        PlayerPrefs.SetFloat("TotalMoney", TotalMoney);
        StartCoroutine(finished());
    }
    IEnumerator finished()
    {
        yield return new WaitForSeconds(1.5f);
        gameManager.levelComplete();

    }
}
