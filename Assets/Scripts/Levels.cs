using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
public class Levels : MonoBehaviour 
{
    public GameManager gameManagerScript;
    public List<Level> myLevels = new List<Level>();
    public static Levels instance = null;
    public List<Booster> myShopBoosters = new List<Booster>();
    public string jsonStringPath;
    public JsonData itemDataLevels;
    public JsonData itemDataStore;
    public static Levels GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Levels>();
            }

            return instance;
        }
    }
    public int currentLevel;
    public void Start()
    {
        currentLevel = 1;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadLevelsAndBoosters()
    {
        //#if UNITY_ANDROID
        //       jsonStringPath = "jar:file://" + Application.dataPath + "!/assets/"+ "levelsData.json";
        //       WWW reader = new WWW(Application.streamingAssetsPath + "/levelsData.json");
        //       while (!reader.isDone) { }
        //       jsonStringPath = reader.text;
        //#else
        //       jsonStringPath = File.ReadAllText(Application.streamingAssetsPath + "/levelsData.json");
        //#endif
        //            itemDataLevels = JsonMapper.ToObject(jsonStringPath);

        //PlayFabManager.GetInstance.getCatalogWithItems(); // load store json <<<<
        //PlayFabManager.GetInstance.getLevelsFromPlayFab(); // load levels json <<<<
        ////Debug.Log(itemDataLevels["levels"][0]["passed"]);

        //for (int i = 0; i < itemDataLevels["levels"].Count; i++)
        //{
        //    Debug.Log("adding levels SHould be before going into the main menu");
        //    bool IfPassed;
        //    bool.TryParse(itemDataLevels["levels"][i]["passed"].ToString(), out IfPassed);
        //    int NrOfSpawnees;
        //    int.TryParse(itemDataLevels["levels"][i]["spawnees"].ToString(), out NrOfSpawnees);
        //    int NrOfEnemies;
        //    int.TryParse(itemDataLevels["levels"][i]["enemies"].ToString(), out NrOfEnemies);
        //    float AmountOfTime;
        //    float.TryParse(itemDataLevels["levels"][i]["time"].ToString(), out AmountOfTime);
        //    bool IfMove;
        //    bool.TryParse(itemDataLevels["levels"][i]["moved"].ToString(), out IfMove);
        //    int clicks;
        //    int.TryParse(itemDataLevels["levels"][i]["clicks"].ToString(), out clicks);
        //    myLevels.Add(new Level(IfPassed, NrOfSpawnees, NrOfEnemies, AmountOfTime, IfMove, clicks));
        //    Debug.Log(myLevels[0].time.ToString());
        //}

        //for (int i = 0; i < itemDataStore["Catalog"].Count; i += 3)
        //{
        //    int itemId;
        //    int.TryParse(itemDataLevels["Catalog"][i]["ItemId"].ToString(), out itemId);
        //    string Name = itemDataLevels["Catalog"][i]["ItemId"].ToString();
        //    string DisplayName = itemDataLevels["Catalog"][i]["DisplayName"].ToString();
        //    int Effect;
        //    int.TryParse(itemDataLevels["Catalog"][i + 1]["CustomData"].ToString(), out Effect);
        //    int Gems;
        //    int.TryParse(itemDataLevels["Catalog"][i]["VirtualCurrencyPrices"]["GE"].ToString(), out Gems);
        //    myShopBoosters.Add(new Booster(itemId, DisplayName, Effect, Name, Gems));
        //}    
    }
    public Level ReturnALevels(int index)
    {
        return myLevels[index];
    }
    public float ReturnAvailableTime()
   {
       return myLevels[currentLevel-1].time;
   }
    public void LoadLevel(Level currentLevel)
   {      
       GameManager.GetInstance.numberOfSpawns = currentLevel.spawnees;
       GameManager.GetInstance.maxTime = currentLevel.time;
       GameManager.GetInstance.numberOfEnemies = currentLevel.enemies;
       GameManager.GetInstance.MovingOnes = currentLevel.movingOnes;

       GameManager.GetInstance.clicksAvailable = currentLevel.clicksAvailablePerLevel;
   }
    public void IncreaseCurrentLevel()
    {
        ++currentLevel;
    }  
}
