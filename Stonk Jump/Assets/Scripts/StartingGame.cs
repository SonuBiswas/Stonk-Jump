using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

public class StartingGame : MonoBehaviour
{
    public LevelLoder levelLoader;
   public void Play()
   {
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);

        int lastPlayedLevel = PlayerPrefs.GetInt("LastPlayedLevel",1);
        // SceneManager.LoadScene(lastPlayedLevel);
        levelLoader.loadLevel(lastPlayedLevel);
        
   }
}
