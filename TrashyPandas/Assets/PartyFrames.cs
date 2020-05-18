using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO Swap positions
public class PartyFrames : MonoBehaviour {

    public int partyIndex;
    private int unitIndex;
    public HeroPanel heroRef;
    public PlayerUnit unit;

    private RectTransform rectT;

    public Image icon;

    private Image border;
    public bool isHighlit = false;
    [SerializeField] private Color highlight_color;
    [SerializeField] private Color normal_color;

    private void Start()
    {
        rectT = GetComponent<RectTransform>();
        border = GetComponent<Image>();
    }

    public void AddUnit(PlayerUnit unit, HeroPanel heropanel)
    {
        if(this.unit != null)
        {
            Debug.Log("There's already a unit in this slot(" + partyIndex + ")");
            RemoveUnit();
        }
        this.unit = unit;
        heroRef = heropanel;
        PlayerController.instance.AddToParty(unit, partyIndex);
        icon.sprite = unit.GetComponent<CharacterStats>().unitClass.icon;
//        Debug.Log("Unit added to party");
    }

    public void Replace(PlayerUnit unit, HeroPanel heropanel)
    {
        Deselect();
        this.unit = unit;
        heroRef = heropanel;
        icon.sprite = unit.GetComponent<CharacterStats>().unitClass.icon;
    }

    public void RemoveUnit()
    {
        Deselect();
        unit = null;
        PlayerController.instance.RemoveFromParty(partyIndex);
        heroRef = null;
        icon.sprite = null;
     
    }

    public void OnClick()
    {
        if (unit == null || heroRef == null)
            return;

        heroRef.Select();
        CharacterPanelController.instance.SetUnitIndex(partyIndex);
        Highlight();
        
    }

    public void Highlight()
    {
        border.color = highlight_color;
        isHighlit = true;
        if (heroRef != null)
            heroRef.HighLight();
    }

    public void Deselect()
    {
        //if (CharacterPanelController.instance.GetUnitIndex() == partyIndex)
        if(heroRef!= null && heroRef.GetPanelState())  
            return;

        if (heroRef != null && isHighlit)
        {
            isHighlit = false;
            heroRef.Deselect();
        }
        isHighlit = false;
        border.color = normal_color;    
    }
}
