using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;

public class MyFB_SDK : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }
    void Start()
    {
        x = 0;
    }
    int x;
   public void SendEvevnt()
   {
        x++;
        LogLevelCompleteEvent(x.ToString());
   }

    public void LogLevelCompleteEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters["level"] = level;
        FB.LogAppEvent(
            "Level Complete",null,
            parameters
        );
    }
     public void LogLevelStartedEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters["level"] = level;
        FB.LogAppEvent(
            "Level Started",null,
            parameters
        );
    }


    void OnApplicationPause(bool pauseStatus)
    {
        // Check the pauseStatus to see if we are in the foreground
        // or background


        if (!pauseStatus)
        {
            //app resume
            if (FB.IsInitialized)
            {
                
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() =>
                {
                    FB.ActivateApp();
                });
            }
        }
    }
}
