using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

   
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
	void OnMouseDown()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        if (GameManager.GetInstance.parentOfSpawnee.childCount > 1)
        {
            StartCoroutine("WaitingTime");
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
       
	}

    IEnumerator WaitingTime()
    {
        
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<ParticleSystem>().Stop();
        Debug.Log("waited");
        GameObject.Destroy(gameObject);
        
    }
   
   
   

}
