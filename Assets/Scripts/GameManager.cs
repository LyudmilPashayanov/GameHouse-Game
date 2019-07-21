using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour {  
    public List<GameObject> spawneesList =  new List<GameObject>();
    public List<GameObject> enemiesList = new List<GameObject>();
    public Transform parentOfSpawnee;
    public Transform parentOfEnemies;
    public bool StateOfGamePlay = false;
    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject buttonWin;
    public GameObject buttonLose;
    public GameObject levelPanel;
    GameObject parentSpawns;
    public int numberOfSpawns = 3;
    public int numberOfEnemies;
    public Text timerText;
    public float startTime;
    public float maxTime = 3.0f;
    bool win = false;
    public bool lose = false;
    string seconds;
    //string minutes;
    public Levels LevelsScript;
    public int currentLevel = 1;
    public Text DownText;
    public int clicksAvailable;
    public Text ClicksText;
    public int Lives;
    public Text LivesAvailable;
    float score;
    public bool MovingOnes;
    public Text upperText;
    public GameObject myBadEffect;
    public static GameManager instance = null;
    public GameObject AnimationScript;
    int lastPlayedLevel=1;
    public Button boost3;
    public Text BoostClicks3;
    public Button boost6;
    public Text BoostClicks6;
    public Button boost10;
    public Text BoostClicks10;
    public Button boost2Time;
    public Text BoostTime2;
    public Button boost3Time;
    public Text BoostTime3;
    public Button boost4Time;
    public Text BoostTime4;
    public Text WinUpper;
    public Text WinDown;
    float ContinueTime;
    public bool ContinueWithClicks = false;
    public bool ContinueWithTime = false;
    public static GameManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
    public void Start()
    {         
            Lives = ProfileManager.GetInstance.Lives;
            //clicksAvailable = ProfileManager.GetInstance.ClicksAvailable;
            LivesAvailable.text = "lives: " + Lives.ToString();
            ClicksText.text = "clicks:  " + clicksAvailable.ToString();
            Debug.Log("[GameManager] level" + currentLevel + " started");
            if ((Scene1Manager.GetInstance.LevelsButton == true))
            {
                levelPanel.SetActive(true);
                return;
            }
            if (Scene1Manager.GetInstance.PlayButton == true)
            {
                Debug.Log("LAST PLAYED LEVEL IS : "+lastPlayedLevel);
                ////StartCoroutine("StartAnimation"); // start animation;
                currentLevel = ProfileManager.GetInstance.LastPlayedLevel;
                //// LevelsScript.GetComponent<Levels>().LoadLevel(LevelsScript.GetComponent<Levels>().myLevels[0]); << OLD SCRIPT
                Levels.GetInstance.currentLevel = currentLevel;
                Levels.GetInstance.LoadLevel(Levels.GetInstance.ReturnALevels(currentLevel-1)); // NEW ONE<<<
            }
            Spawn();
            startTime = Time.time;
    }
    public void Update()
    {
            if ((Scene1Manager.GetInstance.LevelsButton == true))
            {
                return;
            }
            ProfileManager.GetInstance.Lives = Lives;
            if (Lives <= 0)
            {
                panelLose.SetActive(true);
                upperText.text = "You just run";
                DownText.text = "out of Lives";
                return;
            }
            //ProfileManager.GetInstance.ClicksAvailable = clicksAvailable;
            if (win)
            {
                try
                {
                    Levels.GetInstance.myLevels[Levels.GetInstance.currentLevel].passed = true;
                }
                catch
                {
                    ProfileManager.GetInstance.LastPlayedLevel = 1;
                    Levels.GetInstance.currentLevel = currentLevel;
                    WinUpper.text = "You passed all levels";
                    WinDown.text = "Congratulations!!!";
                    panelWin.SetActive(true);
                    Debug.Log("you won the whole game.");
                    return;
                }
                    if (ProfileManager.GetInstance.levelsHighScores[Levels.GetInstance.currentLevel - 1] <=0)
                    {
                        WinUpper.text = "New Highscore:";
                        Debug.Log("NEW HIGHSCORE SAVED<");
                        ProfileManager.GetInstance.levelsHighScores[Levels.GetInstance.currentLevel - 1] = score;
                        panelWin.SetActive(true);
                        WinDown.text = score.ToString("F0");
                        buttonWin.SetActive(true);
                        StateOfGamePlay = false;
                        return;
                    }
                    else if (ProfileManager.GetInstance.levelsHighScores[Levels.GetInstance.currentLevel-1] <= score)
                    {
                        //new highscore text displayed
                        WinUpper.text = "New Highscore:";
                        ProfileManager.GetInstance.levelsHighScores[Levels.GetInstance.currentLevel-1] = score;                 
                        panelWin.SetActive(true);
                        WinDown.text = score.ToString("F0");
                        buttonWin.SetActive(true);
                        StateOfGamePlay = false;
                        return;
                    }
                    else
                    {
                        WinUpper.text = "Your score:";
                        panelWin.SetActive(true);
                        WinDown.text = score.ToString("F0") + " / Your Highscore: " + ProfileManager.GetInstance.levelsHighScores[Levels.GetInstance.currentLevel-1].ToString("F0");
                        buttonWin.SetActive(true);
                        StateOfGamePlay = false;
                        return;
                    }              
            }
            if (lose)
            {               
                panelLose.SetActive(true);
                buttonLose.SetActive(true);
                StateOfGamePlay = false;               
                return;
            }
            Timer();     
    }
    public void Spawn()
    {
        parentSpawns = GameObject.Find("ParentOfSpawnees");
        for (int i = 0; i < numberOfSpawns ; i++)
        {
            int rnd = Random.Range(0, 3);
            GameObject mySpawnee = Instantiate(spawneesList[rnd], new Vector2(Random.Range(-5.7f, 1.1f), Random.Range(3.75f, -4.3f)), Quaternion.identity);
             mySpawnee.transform.SetParent(parentOfSpawnee, false);
            //Instantiate(myGoodEffect, mySpawnee.transform);     
        }
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int rnd = Random.Range(0, 2);
            GameObject mySpawnee = Instantiate(enemiesList[rnd], new Vector2(Random.Range(-5.7f, 1.1f), Random.Range(3.75f, -4.3f)), Quaternion.identity);
            mySpawnee.transform.SetParent(parentOfEnemies, false);
            Instantiate(myBadEffect, mySpawnee.transform);
        }
    }
    float secondsFloat;
    float availableTime;
    public void Timer()
    {
        if (ContinueWithClicks == true)
        {
            //availableTime = Levels.GetInstance.ReturnAvailableTime();   
            float t = (Time.time - startTime) +  ContinueTime;
            seconds = (t % 60).ToString("F2");
            timerText.text = "time: " + seconds + "/ " + availableTime.ToString("F2");
            ClicksText.text = "clicks:  " + clicksAvailable.ToString();
            float.TryParse(seconds, out secondsFloat);           
        }
        else if( ContinueWithTime == true)
        {          
            float t = (Time.time - startTime) + ContinueTime;
            seconds = (t % 60).ToString("F2");
            timerText.text = "time: " + seconds + "/ " + availableTime.ToString("F2");
            ClicksText.text = "clicks:  " + clicksAvailable.ToString();
            float.TryParse(seconds, out secondsFloat);
        }
        else
        {
            //float availableTime = LevelsScript.GetComponent<Levels>().ReturnAvailableTime();
            availableTime = Levels.GetInstance.ReturnAvailableTime();
            float t = Time.time-startTime;
            //minutes = ((int)t / 60).ToString();
            seconds = (t % 60).ToString("F2");
            timerText.text = "time: " + seconds + "/ " + availableTime.ToString("F2");
            ClicksText.text = "clicks:  " + clicksAvailable.ToString();
            float.TryParse(seconds, out secondsFloat);
        }       
        if(MovingOnes == false)
        {
            foreach (Transform child in parentOfSpawnee)
            {
                
                child.GetComponent<MovementScript>().enabled = false;
            }
            foreach (Transform child in parentOfEnemies)
            {
                
                child.GetComponent<MovementScript>().enabled = false;
            }
        }
        if (parentSpawns.transform.childCount <= 0)
        {
            Debug.Log("win");
           ProfileManager.GetInstance.LastPlayedLevel++;
            win = true;
            score = 5000 / (secondsFloat * 100 ); // CALCULATING THE SCORE<<<<<<
            timerText.color = Color.green;
            foreach (Transform child in parentOfEnemies)
            {
                child.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<GameDevils>().enabled = false;
                child.GetComponent<MovementScript>().enabled = false;
            }
            ProfileManager.GetInstance.Save();
            return;
        }
        if(numberOfEnemies>parentOfEnemies.childCount)
        {
            PlayFabManager.GetInstance.SubstractCurrency("LI", 1);
            Lives--;
            LivesAvailable.text = "lives: " + Lives.ToString();
            foreach (Transform child in parentOfSpawnee)
            {
                child.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<Game>().enabled = false;
                child.GetComponent<MovementScript>().enabled = false;
            }
            foreach (Transform child in parentOfEnemies)
            {
                child.GetComponent<MovementScript>().enabled = false;
                child.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<GameDevils>().enabled = false;
            }
            upperText.text = "You clicked";
            DownText.text = "a Devil!";
            ProfileManager.GetInstance.Save();
            lose = true;
            return;
        }
        if (secondsFloat >= maxTime)
        {
            ContinueTime = secondsFloat;
            PlayFabManager.GetInstance.SubstractCurrency("LI", 1);
            Lives--;
            LivesAvailable.text = "lives: " + Lives.ToString();
            upperText.text = "You just run";
            DownText.text = "out of time";
            foreach(Transform child in parentOfSpawnee )
            {
                child.GetComponent<MovementScript>().enabled = false;
                child.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<Game>().enabled = false;
            }
           foreach(Transform child in parentOfEnemies)
           {
               child.GetComponent<MovementScript>().enabled = false;
               child.GetComponent<Collider2D>().enabled = false;
               child.GetComponent<GameDevils>().enabled = false;
           }
           ProfileManager.GetInstance.Save();
           lose = true;
           boost2Time.gameObject.SetActive(true);
           boost3Time.gameObject.SetActive(true);
           boost4Time.gameObject.SetActive(true);
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_2_Seconds"))
               BoostTime2.text = "x" + ProfileManager.GetInstance.boosters["Booster_2_Seconds"];
           else
           {
               BoostTime2.text = "x0";
               boost2Time.interactable = false;
           }
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_3_Seconds"))
               BoostTime3.text = "x" + ProfileManager.GetInstance.boosters["Booster_3_Seconds"];
           else
           {
               BoostTime3.text = "x0";
               boost3Time.interactable = false;
           }
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_4_Seconds"))
               BoostTime4.text = "x" + ProfileManager.GetInstance.boosters["Booster_4_Seconds"];
           else
           {
               BoostTime4.text = "x0";
               boost4Time.interactable = false;
           }
            timerText.color = Color.red;
            return;
        }
        if (clicksAvailable <= 0)
        {
            ContinueTime = secondsFloat;
            PlayFabManager.GetInstance.SubstractCurrency("LI", 1);
            Lives--;
            LivesAvailable.text = "lives: " + Lives.ToString();
            upperText.text = "You just run";
            DownText.text = "out of clicks";
            foreach(Transform child in parentOfSpawnee )
            {
                child.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<Game>().enabled = false;
                child.GetComponent<MovementScript>().enabled = false;
            }
           foreach(Transform child in parentOfEnemies)
           {
               child.GetComponent<Collider2D>().enabled = false;
               child.GetComponent<GameDevils>().enabled = false;
               child.GetComponent<MovementScript>().enabled = false;
           }
           boost3.gameObject.SetActive(true);
           boost6.gameObject.SetActive(true);
           boost10.gameObject.SetActive(true);
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_3_Clicks"))
               BoostClicks3.text = "x" + ProfileManager.GetInstance.boosters["Booster_3_Clicks"];
           else
           {
               BoostClicks3.text = "x0";
               boost3.interactable = false;
           }
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_6_Clicks"))
               BoostClicks6.text = "x" + ProfileManager.GetInstance.boosters["Booster_6_Clicks"];
           else
           {
               BoostClicks6.text = "x0";
               boost6.interactable = false;
           }
           if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_10_Clicks"))
               BoostClicks10.text = "x" + ProfileManager.GetInstance.boosters["Booster_10_Clicks"];
           else
           {
               BoostClicks10.text = "x0";
               boost10.interactable = false;
           }
           ProfileManager.GetInstance.Save();
            lose = true;
            ClicksText.color = Color.red;
            return;
        }
        Rect bounds = new Rect(0, 0, Screen.width, Screen.height);
        if (Input.GetMouseButtonDown(0) && bounds.Contains(Input.mousePosition))
        {          
            clicksAvailable--;
        }
    }  
    public void IncreaseDifficulty()
    {
        try
        {
            Levels.GetInstance.IncreaseCurrentLevel();
            currentLevel = Levels.GetInstance.currentLevel;
            // LevelsScript.GetComponent<Levels>().LoadLevel(LevelsScript.GetComponent<Levels>().myLevels[currentLevel - 1]); OLD script        
            Level aux = Levels.GetInstance.ReturnALevels(Levels.GetInstance.currentLevel - 1);

            Levels.GetInstance.LoadLevel(aux); // NEW ONE

            Debug.Log("[GameManager] IncreaseDifficulty AND" + currentLevel + " started");
        }
        catch
        {
            Debug.Log("show some UI and stuff.");
        }
    }
    public void ResetLevel()
    {
        
        foreach (Transform child in parentOfSpawnee)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parentOfEnemies)
        {
            Destroy(child.gameObject);
        }
        //LevelsScript.GetComponent<Levels>().LoadLevel(LevelsScript.GetComponent<Levels>().myLevels[currentLevel- 1]); OLD script
        //Levels.GetInstance.LoadLevel(Levels.instance.ReturnALevels(currentLevel - 1)); NEW ONE
        panelLose.SetActive(false);
        panelWin.SetActive(false);
        buttonLose.SetActive(false);
        buttonWin.SetActive(false);
        win = false;
        lose = false;
        ContinueWithClicks = false;
        ContinueWithTime = false;
        clicksAvailable = Levels.GetInstance.ReturnALevels(Levels.GetInstance.currentLevel - 1).clicksAvailablePerLevel;
        ClicksText.color = Color.black;
        timerText.color = Color.black;
    }
    public void ContinueGameWithClicks()
    {
            ContinueWithTime = false;
            ContinueWithClicks = true;
            PlayFabManager.GetInstance.GiveCurrency("LI", 1);
            Lives++;           
            string name = EventSystem.current.currentSelectedGameObject.name;
            if (name == "Boost3Btn" && ProfileManager.GetInstance.boosters["Booster_3_Clicks"] >= 1)
            {
                ProfileManager.GetInstance.DeleteBooster("Booster_3_Clicks");
                foreach (Booster item in  Levels.GetInstance.myShopBoosters)
                {
                    if (item.ItemName == "Booster_3_Clicks")
                    {
                        clicksAvailable = item.ValueEffect;                        
                    }
                }
            }
            if (name == "Boost6Btn" && ProfileManager.GetInstance.boosters["Booster_6_Clicks"] >= 1)
            {
                ProfileManager.GetInstance.DeleteBooster("Booster_6_Clicks");
                foreach (Booster item in Levels.GetInstance.myShopBoosters)
                {
                    if (item.ItemName == "Booster_6_Clicks")
                    {
                        clicksAvailable = item.ValueEffect;
                    }
                }
            }
            if (name == "Boost10Btn" && ProfileManager.GetInstance.boosters["Booster_10_Clicks"] >= 1)
            {
                ProfileManager.GetInstance.DeleteBooster("Booster_10_Clicks");
                foreach (Booster item in Levels.GetInstance.myShopBoosters)
                {
                    if (item.ItemName == "Booster_10_Clicks")
                    {
                        clicksAvailable = item.ValueEffect;
                    }
                }
            }          
            foreach(Transform child in parentOfSpawnee )
            {
                child.GetComponent<Collider2D>().enabled = true;
                child.GetComponent<Game>().enabled = true;
                child.GetComponent<MovementScript>().enabled = true;
            }
           foreach(Transform child in parentOfEnemies)
           {
               child.GetComponent<Collider2D>().enabled = true;
               child.GetComponent<GameDevils>().enabled = true;
               child.GetComponent<MovementScript>().enabled = true;
           }
            lose = false;
            panelLose.SetActive(false);
            ClicksText.text = "clicks:  " + clicksAvailable.ToString();
            ClicksText.color = Color.black;
            boost3.gameObject.SetActive(false);
            boost6.gameObject.SetActive(false);
            boost10.gameObject.SetActive(false);
            getStartTime();
            ProfileManager.GetInstance.Save();
            return;
     }
    public void ContinueGameWithTime()
    {
        ContinueWithClicks = false;
        ContinueWithTime = true;
        PlayFabManager.GetInstance.GiveCurrency("LI", 1);
        Lives++;
        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "Boost2Time" && ProfileManager.GetInstance.boosters["Booster_2_Seconds"] >= 1)
        {
            ProfileManager.GetInstance.DeleteBooster("Booster_2_Seconds");
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_2_Seconds")
                {
                    float addedTime;
                      float.TryParse(item.ValueEffect.ToString(), out addedTime);
                      float.TryParse(item.ValueEffect.ToString(), out addedTime);
                      maxTime += addedTime;
                      availableTime += addedTime;
                }
            }
        }
        if (name == "Boost3Time" && ProfileManager.GetInstance.boosters["Booster_3_Seconds"] >= 1)
        {
            ProfileManager.GetInstance.DeleteBooster("Booster_3_Seconds");
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_3_Seconds")
                {
                    float addedTime;
                    float.TryParse(item.ValueEffect.ToString(), out addedTime);
                    float.TryParse(item.ValueEffect.ToString(), out addedTime);
                    maxTime += addedTime;
                    availableTime += addedTime;
                }
            }
        }
        if (name == "Boost4Time" && ProfileManager.GetInstance.boosters["Booster_4_Seconds"] >= 1)
        {
            ProfileManager.GetInstance.DeleteBooster("Booster_4_Seconds");
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_4_Seconds")
                {
                 float addedTime;
                      float.TryParse(item.ValueEffect.ToString(), out addedTime);
                      float.TryParse(item.ValueEffect.ToString(), out addedTime);
                      maxTime += addedTime;
                      availableTime += addedTime;
                }
            }
        }
        foreach (Transform child in parentOfSpawnee)
        {
            child.GetComponent<Collider2D>().enabled = true;
            child.GetComponent<Game>().enabled = true;
            child.GetComponent<MovementScript>().enabled = true;
        }
        foreach (Transform child in parentOfEnemies)
        {
            child.GetComponent<Collider2D>().enabled = true;
            child.GetComponent<GameDevils>().enabled = true;
            child.GetComponent<MovementScript>().enabled = true;
        }
        lose = false;
        panelLose.SetActive(false);
        ClicksText.text = "clicks:  " + clicksAvailable.ToString();
        ClicksText.color = Color.black;
        timerText.color = Color.black;
        boost2Time.gameObject.SetActive(false);
        boost3Time.gameObject.SetActive(false);
        boost4Time.gameObject.SetActive(false);
        getStartTime();
       ProfileManager.GetInstance.Save();
        return;                
    }
    public void getStartTime()
    {
        startTime = Time.time;
    }
 }
  

