using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Play : MonoBehaviour
{
    public Levels LevelsScript;
    public GameObject GameManagerScript;
    public Scene1Manager SceneManagerScript;
    public GameObject panel;
    public GameObject animation;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("worked");
    }
    public void MainMenu()
    {       
        SceneManager.LoadScene("MainMenu");
    }   
    IEnumerator StartAnimationToLoadLevel()
    {
        GameManager.GetInstance.ResetLevel();
        GameManager.GetInstance.enabled = false;
        animation.SetActive(true);
        Debug.Log("before waiting");    
            yield return new WaitForSeconds(1.7f);
            Debug.Log("AFTER waiting");
        animation.SetActive(false);
        GameManager.GetInstance.enabled = true;
        GameManager.GetInstance.ResetLevel();
        GameManager.GetInstance.IncreaseDifficulty();
        GameManager.GetInstance.Spawn();
        GameManager.GetInstance.startTime = Time.time;
        GameManager.GetInstance.Update();      
        Time.timeScale = 1;       
    }
      public void LoadNextLevel()
    {
        StartCoroutine(StartAnimationToLoadLevel());   
    }

      IEnumerator StartAnimationToRetrylevel()
      {

          GameManager.GetInstance.ResetLevel();
          GameManager.GetInstance.enabled = false;
          animation.SetActive(true);
          Debug.Log("before waiting");
          yield return new WaitForSeconds(1.7f);
          Debug.Log("AFTER waiting");
          animation.SetActive(false);
          GameManager.GetInstance.enabled = true;
          GameManager.GetInstance.ResetLevel();
          Levels.GetInstance.currentLevel = GameManager.GetInstance.currentLevel;
          GameManager.GetInstance.Spawn();
          GameManager.GetInstance.startTime = Time.time;
          GameManager.GetInstance.Update();
      }

    public void RetryLevel()
    {
        StartCoroutine(StartAnimationToRetrylevel());     
    }
     IEnumerator StartAnimationToLoadSpecificLevel(string indexOfLevel1)
    {
        panel.SetActive(false);
        Scene1Manager.GetInstance.PlayButton = false;
        Scene1Manager.GetInstance.LevelsButton = false;
        GameManager.GetInstance.ResetLevel();
        GameManager.GetInstance.enabled = false;       
        animation.SetActive(true);
        Debug.Log("before waiting");
        Time.timeScale = 0;
        float pauseTime = Time.realtimeSinceStartup + 1.7f;
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }
        Debug.Log("AFTER waiting");
        animation.SetActive(false);
        GameManager.GetInstance.enabled = true;      
        string name = EventSystem.current.currentSelectedGameObject.name;
        indexOfLevel1 = name;
        int indexForLevel;
        int.TryParse(name, out indexForLevel);
        Debug.Log(indexForLevel + "index for level ");
        GameManager.GetInstance.currentLevel = indexForLevel + 1;
        Levels.GetInstance.currentLevel = GameManager.GetInstance.currentLevel;
        ProfileManager.GetInstance.LastPlayedLevel = indexForLevel + 1;
        GameManager.GetInstance.ResetLevel();
        Levels.GetInstance.LoadLevel(Levels.GetInstance.ReturnALevels(indexForLevel));
        GameManager.GetInstance.Spawn();
        GameManager.GetInstance.startTime = Time.time;
        GameManager.GetInstance.Update();
        Time.timeScale = 1;
    }
    public void LoadSpecificLevel(string indexOfLevel)
    {
        StartCoroutine(StartAnimationToLoadSpecificLevel(indexOfLevel));   
    }
    public void GetButtonClicked()
    {
        name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(name);
        if (name == "PlayButton")
        {
           Scene1Manager.GetInstance.PlayButton = true;
        }
        if (name == "LevelsButton")
        {
            Scene1Manager.GetInstance.LevelsButton = true;
        }
        if(name == "Main Menu")
        {
            Scene1Manager.GetInstance.LevelsButton = false;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
