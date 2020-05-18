using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelect : MonoBehaviour {

    [SerializeField] private Button button;
    public bool canEmbark;

    private void Awake()
    {
        PlayerController.instance.partyChanged += CheckPartySize;
    }

    private void Start()
    {
        CheckPartySize();
    }

    public void OnClick()
    {
        if (canEmbark)
        {
            PlayerController.instance.EmbarkOnMission();
            return;
        }

        //Show dialogue warning? if less than full party.
    }

    private void OnDestroy()
    {
        PlayerController.instance.partyChanged -= CheckPartySize;
    }

    private void CheckPartySize()
    {
        if(PlayerController.instance.GetPartySize() < 1)
        {
            button.interactable = false;
            canEmbark = false;
        }
        else
        {
            button.interactable = true;
            canEmbark = true;
        }
    }

}
