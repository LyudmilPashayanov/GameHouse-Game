using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using System.Text;
using System.IO;
using PlayFab;
using PlayFab.ClientModels;
public class ProfileManager : MonoBehaviour
{    
    public static ProfileManager instance = null;
    public Text LivesAvailable;
    public int Lives = 10;
    public int Gems =0;
    public string jsonData = "";
    int MaxLevel;
    // public int ClicksAvailable=100;
    public Dictionary<string, int> boosters;
    public List<InstancesOfAnItem> myInstancesOfItems = new List<InstancesOfAnItem>();
    public int LastPlayedLevel = 1;
    public List<float> levelsHighScores; // X-th element represents the myLevels[Xth element] in the list of levels.
    public Text debuggingText;
    public static ProfileManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ProfileManager>();
            }

            return instance;
        }
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
    void OnApplicationQuit()
    {
        Save();
    }
    public int SeeMaxLevel()
    {
        MaxLevel = 0;
        foreach (Level lvl in Levels.GetInstance.myLevels)
        {
            if (lvl.passed == true)
            {
                MaxLevel++;
            }

        }
        return MaxLevel;
    }
    public void Save()
    {
        string savingBoostsInString = GetLine(boosters);
        PlayersData data = new PlayersData(Lives, SeeMaxLevel(), levelsHighScores, Gems, LastPlayedLevel, savingBoostsInString);
        jsonData = JsonUtility.ToJson(data);
        PlayFabManager.GetInstance.SaveUserData();
    }
    public void Load()
    {
        PlayersData loadedData = JsonUtility.FromJson<PlayersData>(jsonData);       
        for (int i = 0; i < loadedData.LevelReached; i++)
        {
            Levels.GetInstance.myLevels[i].passed = true;
        }
        levelsHighScores = loadedData.LevelsHighScore;
        PlayFabManager.GetInstance.getCurrency(); // sets the currency "GEMS" and "LIVES" from PlayFab.       
        LastPlayedLevel = loadedData.lastPlayedLevel;
        boosters = new Dictionary<string, int>();
        PlayFabManager.GetInstance.getInventoryFromPlayFab();

        //string[] tokens = loadedData.items.Split(new char[] { ':', ',' },       <-- used for making a dictionary from a string.
        //  StringSplitOptions.RemoveEmptyEntries);
        //for (int i = 0; i < tokens.Length; i += 2)  
        //{
        //    string name = tokens[i];
        //    string freq = tokens[i + 1];

        //    // Parse the int (this can throw)
        //    int count = int.Parse(freq);
        //    // Filling in the value in the sorted dictionary
        //    if (boosters.ContainsKey(name))
        //    {
        //        boosters[name] += count;
        //    }
        //    else
        //    {
        //        boosters.Add(name, count);
        //    }
        //}
    }
  //  Saving Dictionaries as a string so I can save them into the PlayerPref. Not used so I can make a class "Boosters" and have more flexibility with it.
    public void AddBooster(string boosterName)
    {
        if (boosters.ContainsKey(boosterName))
        {
            Debug.Log("increasing the boost");
            boosters[boosterName]++;
        }
        else
        {
            Debug.Log("adding a boost into the dict");
            boosters.Add(boosterName, 1);
        }
    }
    public string instanceIdOfItem;
    public void DeleteBooster(string boosterName)
    {     
        PlayFabManager.GetInstance.ConsumeAnItem(boosterName, 1, getInstanceOfItem(boosterName));
        boosters[boosterName]--;
        if (boosters[boosterName] == 0)
        {
            boosters.Remove(boosterName);
        }
    }
     public InstancesOfAnItem getInstanceOfItem(string itemId)
    {
        foreach (InstancesOfAnItem item in myInstancesOfItems)
        {
          if(item.itemName == itemId)
          {
              return item;
          }
        }
        return null;
    }
    string GetLine(Dictionary<string, int> d)
    {
        // Build up each line one-by-one and then trim the end
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<string, int> pair in d)
        {
            builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
        }
        string result = builder.ToString();
        // Remove the final delimiter
        result = result.TrimEnd(',');
        return result;
    }
}


      



