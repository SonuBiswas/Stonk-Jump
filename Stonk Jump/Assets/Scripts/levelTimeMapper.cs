using System.Collections;
using UnityEngine;

public class levelTimeMapper : MonoBehaviour
{
    [SerializeField] int _time;
    [SerializeField] bool autoStart;
    void Start()
    {
        if(autoStart)
        mappTime(_time); 
    }

   public void mappTime(int t)
   {
        if (t == 0)
        {
            t = _time;
        }
        StartCoroutine(startTimer(t));
   }

    IEnumerator startTimer(int _t)
    {
        while (_t > 0)
        {
            yield return new WaitForSeconds(1);

            _t--;
            print("Level Time remaining  " + _t);
            _time = _t;

        }
        print(".......Level Time Over........  " + _t);
        _time = _t;
    }
}
