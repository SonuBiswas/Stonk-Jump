using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using com.adjust.sdk;

public class AdjustExample : MonoBehaviour
{

    void Start()
    {
        // import this package into the project:
        // https://github.com/adjust/unity_sdk/releases

#if UNITY_IOS
        /* Mandatory - set your iOS app token here */
        InitAdjust("YOUR_IOS_APP_TOKEN_HERE");
#elif UNITY_ANDROID
        /* Mandatory - set your Android app token here */
        InitAdjust("r04q252pzg8w");
#endif
    }


    private void InitAdjust(string adjustAppToken)
    {
        var adjustConfig = new AdjustConfig(
            adjustAppToken,
            AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
            true
        );
        adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
        adjustConfig.setSendInBackground(true);
        new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename

        // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

        //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
        //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
        //});

        Adjust.start(adjustConfig);

    }

}