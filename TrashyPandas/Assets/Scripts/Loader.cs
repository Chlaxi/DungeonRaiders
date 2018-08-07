using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject playerController;          //GameManager prefab to instantiate.

    void Awake()
    {
        if (PlayerController.instance == null)

            //Instantiate gameManager prefab
            Instantiate(playerController);
    }
    
}
