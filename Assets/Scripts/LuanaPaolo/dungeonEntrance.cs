using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonEntrance : MonoBehaviour
{
    public string nameScene;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            SceneManager.LoadScene(nameScene);
        }
    }
}
