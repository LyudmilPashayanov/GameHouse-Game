using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancesOfAnItem 
{
    public string instanceOfTheItem;
    public string itemName;
    public InstancesOfAnItem(string instance, string name)
    {
        this.instanceOfTheItem = instance;
        this.itemName = name;
    }
}
