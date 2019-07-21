using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SavingProgress : MonoBehaviour {

    public Levels LevelsScript;
    public Button thisButton;
	void Start () 
    {
            string name = thisButton.name;
            int indexForLevel;
            int.TryParse(name, out indexForLevel);
            if (Levels.GetInstance.ReturnALevels(indexForLevel).passed == true)// moga da checkna s primerno index 2
            {
                Debug.Log("in the code");
                thisButton.interactable = true;
            }
	}
   
}
