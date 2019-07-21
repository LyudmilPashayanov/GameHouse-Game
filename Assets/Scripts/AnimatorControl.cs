using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControl : MonoBehaviour {
   public GameObject StartAnimator;
   public void Awake()
    {
        if (Scene1Manager.GetInstance.LevelsButton == false)
        {        
            GameManager.GetInstance.enabled = false;
            StartCoroutine("StartAnimation");
        }
    }
    IEnumerator StartAnimation()
    {      
        StartAnimator.SetActive(true);
        Time.timeScale = 0;
        float pauseTime = Time.realtimeSinceStartup + 1.7f;
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }
        StartAnimator.SetActive(false);
        GameManager.GetInstance.enabled = true;
        Debug.Log("make it true");      
        Time.timeScale = 1;        
    }
}
