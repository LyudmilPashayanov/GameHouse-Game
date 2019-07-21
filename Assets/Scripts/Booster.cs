using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster{
    public string BoosterId;
    public string DisplayName;
    public int ValueEffect;
    public string ItemName;
    public int GemsPrice;
    public string CurrencyOfItem;
    public Booster(string Id,string DisplayText,int ValueEffect,string Name,int GemsPrice,string Currency)
    {
        this.BoosterId = Id;
        this.DisplayName = DisplayText;
        this.ValueEffect = ValueEffect;
        this.ItemName = Name;
        this.GemsPrice = GemsPrice;
        this.CurrencyOfItem = Currency;
    }


}
