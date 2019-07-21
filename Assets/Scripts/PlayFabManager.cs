using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine.SceneManagement;
using LitJson;
public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance = null;
    public Text debug;
    public string TitleIdGame = "898D";
    public static PlayFabManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayFabManager>();
            }

            return instance;
        }
    }
    private void Awake()
    {
        debug.text = "asdasdasd";
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        PlayFabSettings.TitleId = TitleIdGame;

        debug.text = PlayFabSettings.TitleId.ToString(); 
        Login();
        StartCoroutine(Retry());
    }
    public void Login()
    {
        debug.text = "into login method";
     
        // Login with Android ID
        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest() {
            TitleId = "898D",
            CreateAccount = true,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
        }, result => {
            debug.text = "result okay";
            Debug.Log("Logged in");
            // Refresh available items 
            
            GetUserData();
        }, error =>
        {
            debug.text = error.ErrorDetails.ToString();
            Debug.LogError(error.GenerateErrorReport());
        });     
    }
    public void GetUserData()
    {
        //loading from PlayFab: gettin json file:
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
        Keys = null
        },
            (result) =>
            {
                if (result.Data == null || !result.Data.ContainsKey("progress"))
                {
                    debug.text = "in the coroutine";
                    // NEEED TO WAIT UNTIL DATA IS CREATED!!!
                    StartCoroutine(StartCreatingProfile());
                }
                else
                {
                    debug.text = "getting already existing data";
                    Debug.Log("getting already existing data..");
                    ProfileManager.GetInstance.jsonData = result.Data["progress"].Value;
                    debug.text = "data got!!!!!!!!";
                    Debug.Log(result.Data["progress"].Value);
                    debug.text = "Loading Levels...";
                    StartCoroutine(WaitTheLoad()); // wait for this one.
                }
            },
           (error) =>
           {
               debug.text = error.ErrorDetails.ToString();
               Debug.Log("data couldnt be get");
           }
               );
    }
    IEnumerator StartCreatingProfile()
    {
        AddNewPlayerData();
        Debug.Log("before waiting");
        yield return new WaitForSeconds(5f);
        StartCoroutine(StartLoadingLevelsAndStore());
        Debug.Log("before waiting");
        yield return new WaitForSeconds(5f);
        GetUserData();

    }
    public void AddNewPlayerData()
    {
        debug.text = "data should be created";
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            
            Data = new Dictionary<string, string>()
            {
                 
                 {"progress","{\"LivesAvailable\":1,\"LevelReached\":1,\"GemsAvailable\":1,\"lastPlayedLevel\":1,\"items\":\"click3:0,click6:0,time2:0,click10:0,time4:0\",\"LevelsHighScore\":[0,0,0,0,0,0,0,0,0,0,0,0]}"}
            }

        },
            (result2) =>
            {
                result2.DataVersion = 0;
                debug.text = "adding data for the first time";
                Debug.Log("data changed.");
                
            },
           (error2) =>
           {
               debug.text = "couldnt add data for the first time" + error2.ErrorMessage;

               Debug.Log(error2.Error);
           }
               );
    }
    public void getCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            ProfileManager.GetInstance.Gems = result.VirtualCurrency["GE"];
            ProfileManager.GetInstance.Lives = result.VirtualCurrency["LI"];
            Debug.Log("Inventory Accessed: " + result.VirtualCurrency["GE"] + " gems." + "Lives:  " + result.VirtualCurrency["LI"]);
        },
        (error) =>
        {
            Debug.Log("couldnt get currency. Error: " + error.Error);
        }
        );
    }
    public void SaveUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {

            Data = new Dictionary<string, string>()
            {
                 {"progress",ProfileManager.GetInstance.jsonData}
            }
        },
            (result) =>
            {
                Debug.Log("data updated successfully.");
            },
           (error) =>
           {
               Debug.Log(error.Error);
           }
               );
    }
    public void GiveCurrency(string CurrencyName, int amount)
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
        {
            VirtualCurrency = CurrencyName,
            Amount = amount
        },
        (result) =>
        {
            Debug.Log("one currency Added.");
        },
        (error) =>
        {
            Debug.Log("couldn't add a currency. Error: " + error.Error);
        }

            );
    }
    public void SubstractCurrency(string CurrencyName,int amount)
    {
        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
        {
            VirtualCurrency = CurrencyName,
            Amount = amount
        },
        (result) =>
        {
            Debug.Log("one currency substracted!");
        },
        (error) =>
        {
            Debug.Log("couldn't substract a currency. Error: " + error.Error);
        }

            );
    }
    public void PurchaseItem(string itemIdFromShop,string currencyName,int priceOfItem)
    {

        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest() 
        {
            ItemId = itemIdFromShop,
            VirtualCurrency = currencyName,
            Price = priceOfItem
        
        },
        (result) =>
        {
            ProfileManager.GetInstance.myInstancesOfItems.Add(new InstancesOfAnItem( result.Items[0].ItemInstanceId,result.Items[0].ItemId));
            Debug.Log("item bought: " + result.Items[0].ItemInstanceId + result.Items[0].ItemId);
            Debug.Log("item purchased successfully");
        },
        (error) =>
        {
            Debug.Log("error in purchasing this item: " + error.ErrorDetails);
            Debug.Log("no ENOUGH MONEY?");
        }
            );
    }    
    public void getCatalogWithItems()
    {
        string Currency= "";
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = "1"
        },
        (result) =>
        {
            // Levels.GetInstance.itemDataStore = result.Catalog[1]; // trqbva da go napravq json file         
            Debug.Log("catalog got successfully!");
            debug.text = "Loading Store...";
            for (int i = 0; i < result.Catalog.Count; i ++)
            {
                string itemId=result.Catalog[i].ItemId.ToString();
                string Name = result.Catalog[i].ItemId.ToString();
                string DisplayName = result.Catalog[i].DisplayName.ToString();
                int Effect;
                int.TryParse(result.Catalog[i].CustomData.ToString(), out Effect);
                int Gems;
                int.TryParse(result.Catalog[i].VirtualCurrencyPrices["GE"].ToString(), out Gems);
                Dictionary<string, uint> d = new Dictionary<string, uint>();
                d = result.Catalog[i].VirtualCurrencyPrices;
                List<string> list = new List<string>(d.Keys); 
                foreach (string k in list)
                {
                    Currency = k;           
                }              
                Levels.GetInstance.myShopBoosters.Add(new Booster(itemId, DisplayName, Effect, Name, Gems,Currency));            
            } 
        },
        (error) =>
        {
            Debug.Log("something wend wrong while getting the catalog:" + error.ErrorMessage);
        }
            );
    }
    public JsonData itemDataLevels;
    public void getLevelsFromPlayFab()
    {        
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
        (result) =>
        {

            itemDataLevels = JsonMapper.ToObject(result.Data["levels"]);
            Debug.Log("Levels got successfully!");
            StartCoroutine(LoadLevels());           
        },
        (error) =>
        {
            Debug.Log("Got error getting Levels:");
            Debug.Log(error.GenerateErrorReport());
        }
    );
    }
    public void getInventoryFromPlayFab()
    {
        int counter = 0;
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
       (result) =>
       {
           foreach (var item in result.Inventory)
           {             
            counter = 0;
               for (int i = 0; i < result.Inventory.Count; i++)
			    {
			        if(item.ItemId == result.Inventory[i].ItemId)
                     {
                         counter++;
                     }
			    }
               if (!ProfileManager.GetInstance.boosters.ContainsKey(item.ItemId))
               {
                   ProfileManager.GetInstance.boosters.Add(item.ItemId, counter);
               }
            }
           Debug.Log("Inventory Loaded.");
       },
       (error) =>
       {
           Debug.Log("failed to load my inventory: " + error.ErrorMessage);
       }
   );
    }
    public void getInstanceOfAnItem()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
       (result) =>
       {
           foreach (var item in result.Inventory)
           {
                  ProfileManager.GetInstance.myInstancesOfItems.Add(new InstancesOfAnItem(item.ItemInstanceId, item.ItemId));
               Debug.Log(item.ItemInstanceId + item.ItemId);
           }
           Debug.Log("instance Of an Item got successfully.");        
       },
       (error) =>
       {
           Debug.Log("failed to get the instance of an item: " + error.ErrorMessage);
       }
   );
    }
    public void ConsumeAnItem(string ItemId,int ConsumeAmount,InstancesOfAnItem InstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
            {
            ItemInstanceId = InstanceId.instanceOfTheItem,
            ConsumeCount = ConsumeAmount         
            },
      (result) =>
      {
          result.RemainingUses = 1;
          Debug.Log("Item Consumed successfully.");
          ProfileManager.GetInstance.myInstancesOfItems.Remove(InstanceId); //removes from the instances
      },
      (error) =>
      {
          Debug.Log("failed to Consume the item: "+"itemId-"+ItemId +"consumeAmount= "+ConsumeAmount+"instanceId"+InstanceId+ error.ErrorDetails  + ErrorCallback.Combine());
      }
  );
    }
    IEnumerator LoadLevels()
    {
        Debug.Log("waiting COROUTINE LOAD LEVELS");
        yield return new WaitForSeconds(4f);
        debug.text="LoadingLevels";
        for (int i = 0; i < itemDataLevels["levels"].Count; i++)
        {
            bool IfPassed;
            bool.TryParse(itemDataLevels["levels"][i]["passed"].ToString(), out IfPassed);
            int NrOfSpawnees;
            int.TryParse(itemDataLevels["levels"][i]["spawnees"].ToString(), out NrOfSpawnees);
            int NrOfEnemies;
            int.TryParse(itemDataLevels["levels"][i]["enemies"].ToString(), out NrOfEnemies);
            float AmountOfTime;
            float.TryParse(itemDataLevels["levels"][i]["time"].ToString(), out AmountOfTime);
            bool IfMove;
            bool.TryParse(itemDataLevels["levels"][i]["moved"].ToString(), out IfMove);
            int clicks;
            int.TryParse(itemDataLevels["levels"][i]["clicks"].ToString(), out clicks);
            Levels.GetInstance.myLevels.Add(new Level(IfPassed, NrOfSpawnees, NrOfEnemies, AmountOfTime, IfMove, clicks));
        }
    }
    IEnumerator StartLoadingLevelsAndStore()
    {
        ////getCatalogWithItems();
        //getLevelsFromPlayFab();
        //Debug.Log("before waiting");
        //yield return new WaitForSeconds(5f);
        //Levels.GetInstance.LoadLevelsAndBoosters();
        //getLevelsFromPlayFab();
        Debug.Log("startLoadingLeevlsAndStore COROUTINE");
        yield return new WaitForSeconds(5f);
        GetUserData();
    }
    IEnumerator WaitTheLoad()
    {
    //Levels.GetInstance.LoadLevelsAndBoosters();
    getLevelsFromPlayFab();
    getCatalogWithItems();
    getInstanceOfAnItem();
    debug.text = "loading Items";
    Debug.Log("\"WAIT THE LOAD\" COROUTINE");
    yield return new WaitForSeconds(5f);
    ProfileManager.GetInstance.Load();
    debug.text = "loading";
    SceneManager.LoadScene("MainMenu");
    Debug.Log("data got from playfab");
    }
    IEnumerator Retry()
    {   
        yield return new WaitForSeconds(20f);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Log In Scene"))
        {
            debug.text = "Retring to log in...";
            Login();
        }
        else
        {
            Debug.Log("sadfsssssssssssssssssssssss");
        }
    }
}
