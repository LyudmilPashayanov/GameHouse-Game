using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level  
{
    public bool passed {get;set;}
    public int spawnees;
    public int enemies;
    public float time;
    public bool movingOnes;
    public int clicksAvailablePerLevel;
   

    public Level(bool isPassed,int spawnees,int enemiesAdded, float time, bool movingOnes,int clicks)
    {
        this.spawnees = spawnees;
        this.time = time;
        this.clicksAvailablePerLevel = clicks;
        this.enemies = enemiesAdded;
        this.passed = isPassed;
        this.movingOnes = movingOnes;
    }
}
