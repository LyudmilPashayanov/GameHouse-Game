using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class updatingLives : MonoBehaviour {
    public Text lives;
    public Text clicks;
    public Text Gems;
    public Button PlayButton;
    public Button LevelsButton;
    public Button Lives1;
    public Button Lives2;
    public Button Lives3;
    public Button Clicks1;
    public Button Clicks2;
    public Button Clicks3;
    public Button Time1;
    public Button Time2;
    public Button Time3;
    public Text BoostClicks3;
    public Text BoostClicks6;
    public Text BoostClicks10;
    public Text BoostTime2;
    public Text BoostTime3;
    public Text BoostTime4;
    public GameObject panelStore;
    public GameObject panelBoosts;
    public Text BoostClickName3;
    public Text BoostClickName6;
    public Text BoostClickName10;
    public Text BoostTimeName2;
    public Text BoostTimeName3;
    public Text BoostTimeName4;
    public Text PriceClick3;
    public Text PriceClick6;
    public Text PriceClick10;
    public Text PriceTime2;
    public Text PriceTime3;
    public Text PriceTime4;
	void Start () 
    {
        lives.text= "lives:" + ProfileManager.GetInstance.Lives.ToString();
        foreach (Booster item in Levels.GetInstance.myShopBoosters)
        {
            if (item.ItemName == "Booster_3_Clicks")
            {
                BoostClickName3.text = item.DisplayName;
            }
            if (item.ItemName == "Booster_6_Clicks")
            {
                BoostClickName6.text = item.DisplayName;
            }
            if (item.ItemName == "Booster_10_Clicks")
            {
                BoostClickName10.text = item.DisplayName;
            }
            if (item.ItemName == "Booster_2_Seconds")
            {
                BoostTimeName2.text = item.DisplayName;
            }
            if (item.ItemName == "Booster_3_Seconds")
            {
                BoostTimeName3.text = item.DisplayName;
            }
            if (item.ItemName == "Booster_4_Seconds")
            {
                BoostTimeName4.text = item.DisplayName;
            }
        }
      //  clicks.text = "clicks:" + ProfileManager.GetInstance.ClicksAvailable.ToString();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
           
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_3_Clicks")
                {
                    PriceClick3.text = item.GemsPrice.ToString();
                }
                if (item.ItemName == "Booster_6_Clicks")
                {
                    PriceClick6.text = item.GemsPrice.ToString(); ;
                }
                if (item.ItemName == "Booster_10_Clicks")
                {
                    PriceClick10.text = item.GemsPrice.ToString();
                }
                if (item.ItemName == "Booster_2_Seconds")
                {
                    PriceTime2.text = item.GemsPrice.ToString();
                }
                if (item.ItemName == "Booster_3_Seconds")
                {
                    PriceTime3.text = item.GemsPrice.ToString();
                }
                if (item.ItemName == "Booster_4_Seconds")
                {
                    PriceTime4.text = item.GemsPrice.ToString();
                }
            }           
        }
        if (ProfileManager.GetInstance.Lives <= 0) //|| ProfileManager.GetInstance.ClicksAvailable<=0
        {
            PlayButton.interactable = false;
            LevelsButton.interactable = false;

        }
	}
    public void Update()
    {
        lives.text = "lives:" + ProfileManager.GetInstance.Lives.ToString();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            lives.text = "lives:" + ProfileManager.GetInstance.Lives.ToString();
            Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
            int price = 0;
            if(ProfileManager.GetInstance.Gems <1)
            {
                Lives1.interactable = false;
            }
            else
            {
                Lives1.interactable = true;
            }
            if (ProfileManager.GetInstance.Gems < 2)
            {
                Lives2.interactable = false;
            }
            else
            {
                Lives2.interactable = true;
            }
            if (ProfileManager.GetInstance.Gems < 3)
            {
                Lives3.interactable = false;
            }
            else
            {
                Lives3.interactable = true;
            }
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_3_Clicks")
                {
                    price = item.GemsPrice;
                    if(ProfileManager.GetInstance.Gems < price)
                    {
                        Clicks1.interactable = false;
                    }
                    else
                    {
                        Clicks1.interactable = true;
                    }
                }
                if (item.ItemName == "Booster_6_Clicks")
                {
                    price = item.GemsPrice;
                    if (ProfileManager.GetInstance.Gems < price)
                    {
                        Clicks2.interactable = false;
                    }
                    else
                    {
                        Clicks2.interactable = true;
                    }
                }
                if (item.ItemName == "Booster_10_Clicks")
                {
                    price = item.GemsPrice;
                    if (ProfileManager.GetInstance.Gems < price)
                    {
                        Clicks3.interactable = false;
                    }
                    else
                    {
                        Clicks3.interactable = true;
                    }
                }
                if (item.ItemName == "Booster_2_Seconds")
                {
                    price = item.GemsPrice;
                    if (ProfileManager.GetInstance.Gems < price)
                    {
                        Time1.interactable = false;
                    }
                    else
                    {
                        Time1.interactable = true;
                    }
                }
                if (item.ItemName == "Booster_3_Seconds")
                {
                    price = item.GemsPrice;
                    if (ProfileManager.GetInstance.Gems < price)
                    {
                        Time2.interactable = false;
                    }
                    else
                    {
                        Time2.interactable = true;
                    }
                }
                if (item.ItemName == "Booster_4_Seconds")
                {
                    price = item.GemsPrice;
                    if (ProfileManager.GetInstance.Gems < price)
                    {
                        Time3.interactable = false;
                    }
                    else
                    {
                        Time3.interactable = true;
                    }
                }
            }
        }

    }
	public void GiveALife() // playfab ready.
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "LifeBtn")
        {
            if (ProfileManager.GetInstance.Gems >= 1)
            {
                ProfileManager.GetInstance.Lives++;
                ProfileManager.GetInstance.Gems--;
                PlayFabManager.GetInstance.GiveCurrency("LI", 1);
                PlayFabManager.GetInstance.SubstractCurrency("GE", 1);
                lives.text = "lives:" + ProfileManager.GetInstance.Lives.ToString();
                Gems.text = "gems:"+ ProfileManager.GetInstance.Gems.ToString();
                if (ProfileManager.GetInstance.Lives > 0) // && ProfileManager.GetInstance.ClicksAvailable > 0
                {
                    PlayButton.interactable = true;
                    LevelsButton.interactable = true;
                }
                ProfileManager.GetInstance.Save();
            }
            else
            {
                Debug.Log("You dont have enough gems");
            }
        }
        if (name == "LifeBtn3")
        {
            if (ProfileManager.GetInstance.Gems >= 2)
            {
                ProfileManager.GetInstance.Lives += 3;
                ProfileManager.GetInstance.Gems -= 2;
                PlayFabManager.GetInstance.GiveCurrency("LI", 3);
                PlayFabManager.GetInstance.SubstractCurrency("GE", 2);
                lives.text = "lives:" + ProfileManager.GetInstance.Lives.ToString();
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();

                if (ProfileManager.GetInstance.Lives > 0) //&& ProfileManager.GetInstance.ClicksAvailable > 0
                {
                    PlayButton.interactable = true;
                    LevelsButton.interactable = true;
                }
                ProfileManager.GetInstance.Save();
            }
            else
            {
                Debug.Log("You dont have enough gems");
            }
        }
        if (name == "LifeBtn5")
        {
            if (ProfileManager.GetInstance.Gems >= 3)
            {
                ProfileManager.GetInstance.Lives += 5;               
                ProfileManager.GetInstance.Gems -= 3;
                PlayFabManager.GetInstance.GiveCurrency("LI", 5);
                PlayFabManager.GetInstance.SubstractCurrency("GE", 3);
                lives.text = "lives:" + ProfileManager.GetInstance.Lives.ToString();
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                if (ProfileManager.GetInstance.Lives > 0) // && ProfileManager.GetInstance.ClicksAvailable > 0
                {
                    PlayButton.interactable = true;
                    LevelsButton.interactable = true;
                }
                ProfileManager.GetInstance.Save();
            }
            else
            {
                Debug.Log("You dont have enough gems");
            }
        }       
    }
    public void GiveTenClicks()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "ClicksBtn3")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_3_Clicks")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_3_Clicks");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
        if (name == "ClicksBtn6")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_6_Clicks")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_6_Clicks");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
       if (name == "ClicksBtn10")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_10_Clicks")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_10_Clicks");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
        if (name == "TimeBtn2")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_2_Seconds")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_2_Seconds");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
        if (name == "TimeBtn3")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_3_Seconds")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_3_Seconds");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
        if (name == "TimeBtn4")
        {
            int price = 0;
            foreach (Booster item in Levels.GetInstance.myShopBoosters)
            {
                if (item.ItemName == "Booster_4_Seconds")
                {
                    price = item.GemsPrice;
                    PlayFabManager.GetInstance.PurchaseItem(item.BoosterId, item.CurrencyOfItem, price);
                }
            }
            if (ProfileManager.GetInstance.Gems >= price)
            {

                ProfileManager.GetInstance.AddBooster("Booster_4_Seconds");
                ProfileManager.GetInstance.Gems -= price;
                Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
                ProfileManager.GetInstance.Save();
            }
        }
    }
    public void GiveGems() // playfab ready.
    {
        PlayFabManager.GetInstance.GiveCurrency("GE",1);
        ProfileManager.GetInstance.Gems++;
        Gems.text = "gems:" + ProfileManager.GetInstance.Gems.ToString();
        
        ProfileManager.GetInstance.Save();
    }
    public void OpenStore()
    {      
        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "StoreButton" || name == "OpenStore" || name == "OpenStore1")
        {
            panelStore.SetActive(true);                   
        }
        if(name == "closeBtn")
        {
            panelStore.SetActive(false);
            foreach (KeyValuePair<string, int> kvp in ProfileManager.GetInstance.boosters)
            {
                Debug.Log("Key = " + kvp.Key + "value = " + kvp.Value);
            }
            
        }
    }
    public void OpenBoosts()
    {       
        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "BoostersBtn")
        {
            panelBoosts.SetActive(true);            
        }
        if (name == "closeBtn")
        {

            panelBoosts.SetActive(false);
        }
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_3_Clicks"))
            BoostClicks3.text = "x" + ProfileManager.GetInstance.boosters["Booster_3_Clicks"];
        else
            BoostClicks3.text = "x0";
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_6_Clicks"))
            BoostClicks6.text = "x" + ProfileManager.GetInstance.boosters["Booster_6_Clicks"];
        else
            BoostClicks6.text = "x0";
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_10_Clicks"))
            BoostClicks10.text = "x" + ProfileManager.GetInstance.boosters["Booster_10_Clicks"];
        else
            BoostClicks10.text = "x0";
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_2_Seconds"))
            BoostTime2.text = "x" + ProfileManager.GetInstance.boosters["Booster_2_Seconds"];
        else
            BoostTime2.text = "x0";
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_3_Seconds"))
            BoostTime3.text = "x" + ProfileManager.GetInstance.boosters["Booster_3_Seconds"];
        else
            BoostTime3.text = "x0";
        if (ProfileManager.GetInstance.boosters.ContainsKey("Booster_4_Seconds"))
            BoostTime4.text = "x" + ProfileManager.GetInstance.boosters["Booster_4_Seconds"];
        else
            BoostTime4.text = "x0";
        if(name=="closeBtn1")
        {
            panelBoosts.SetActive(false);
        }
    }
}
