using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public Scene1Manager SceneManager;
    public ProfileManager ProfileManager;
    public Levels LevelsManager;
    public PlayFabManager PlayFabManager;
    void Awake()
    {
        if (Scene1Manager.instance == null)
        {
            Instantiate(SceneManager);
        }
        
        if (Levels.instance == null)
        {
            Instantiate(LevelsManager);
        }
        if (ProfileManager.instance == null)
        {
            Instantiate(ProfileManager);
        }
        if(PlayFabManager.instance == null)
        {
            Instantiate(PlayFabManager);
        }
    }
}
