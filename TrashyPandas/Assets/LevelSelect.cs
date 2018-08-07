using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

    public void OnClick()
    {
        PlayerController.instance.EmbarkOnMission();
    }
}
