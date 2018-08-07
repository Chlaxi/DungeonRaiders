using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO Swap positions
public class PartyFrames : MonoBehaviour {

    public int partyIndex;
    private int unitIndex;

    public PlayerUnit unit;
    private RectTransform rectT;

    public Image icon;

    private void Start()
    {
        rectT = GetComponent<RectTransform>();
    }

    public void AddUnit(PlayerUnit unit)
    {
        if(this.unit != null)
        {
            Debug.Log("There's already a unit in this slot(" + partyIndex + ")");
            RemoveUnit();
        }
        this.unit = unit;

        PlayerController.instance.AddToParty(unit, partyIndex);
        icon.sprite = unit.GetComponent<CharacterStats>().unitClass.icon;
//        Debug.Log("Unit added to party");
    }

    public void RemoveUnit()
    {
        unit = null;
        PlayerController.instance.RemoveFromParty(partyIndex);
        
        icon.sprite = null;
        Debug.Log("Unit removed from party");
    }

    public void OnClick()
    {
        if(unit != null) CharacterPanelController.instance.Open(unit);
        
    }
}
