using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
  [Serializable]
  public class PlayersData
{
    public int LivesAvailable;
    public int LevelReached;
    //public int ClicksAvailable;
    public int GemsAvailable;
    public int lastPlayedLevel;
    public string items; // items used to represent the boosts as a string in a dictionary
    public List<float> LevelsHighScore; // 0th index represents 0th level in MyLevels<list>. 
    public PlayersData(int Lives,int reached,List<float> highScores,int Gems,int lastPlayed,string boosts)
    {
        this.LivesAvailable = Lives;
        this.LevelReached = reached;
        //this.ClicksAvailable = clicks;
        this.LevelsHighScore = highScores;
        this.GemsAvailable = Gems;
        this.lastPlayedLevel = lastPlayed;
        this.items = boosts;
    }   
}
