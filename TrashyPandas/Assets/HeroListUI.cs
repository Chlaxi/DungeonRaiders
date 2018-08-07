using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : MonoBehaviour {

    public GameObject HeroPanel;

    private RectTransform selfRect;

    public int maxUnits;
    private int currentUnits = 0;

    public Text unitText;

    private void Awake()
    {
        selfRect = GetComponent<RectTransform>();
        Debug.Log("unit List is ready");
    }

    

    public void SetupList()
    {
        PlayerController.instance.unitListChange += UpdateHeroList;
        PlayerController.instance.unitInfoChanged += UpdateInfo;

        foreach (PlayerUnit unit in PlayerController.instance.GetAllUnits())
        {
            if (unit == null) continue;
            AddHero(unit);
        }
      //  Debug.Log(currentUnits);
    }

    public void SetupPartyList(PlayerUnit unit, PartyFrames frame)
    {
        if (unit == null || frame == null) return;

        AddHeroToParty(unit, frame);
    }

    /// <summary>
    /// When the list changes. i.e. a unit has been added or removed to/from the list
    /// </summary>
    /// <param name="unit"></param>
    public void UpdateHeroList(PlayerUnit unit)
    {
       

        if (PlayerController.instance.GetAllUnits().Length > currentUnits)
        {
            Debug.Log("A new unit was found!");
            AddHero(unit);
        }
        else if(currentUnits>0)
        {
            //Removing a unit
            RemoveHero(unit);
        }
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        Debug.Log("Updating info");
        HeroPanel[] panels = GetComponentsInChildren<HeroPanel>();
        unitText.text = currentUnits + " / " + maxUnits;
        foreach (HeroPanel panel in panels)
        {

            panel.UpdatePanel();
            
        }
    }

    public void AddHero(PlayerUnit unit)
    {
        if (currentUnits >= maxUnits) return;

        GameObject newPanel = Instantiate(HeroPanel, gameObject.transform);
        if (newPanel.GetComponent<HeroPanel>().SetupHeroPanel(unit))
        {
            float heightToAdd = HeroPanel.GetComponent<RectTransform>().rect.height;
            selfRect.sizeDelta = new Vector2(0, selfRect.rect.height + heightToAdd);
            currentUnits++;
        }
        else{
            Debug.Log("Setup failed");
            Destroy(newPanel);
        }
    }

    public void RemoveHero(PlayerUnit unit)
    {
        if (currentUnits <= 0) return;

        HeroPanel panel = FindHeroPanel(unit);
        if(panel != null)
        { 
            float heightToSubtract = HeroPanel.GetComponent<RectTransform>().rect.height;
            selfRect.sizeDelta = new Vector2(0, selfRect.rect.height - heightToSubtract);
            Destroy(panel.gameObject);
            currentUnits--;
        }
    }

    public HeroPanel FindHeroPanel(PlayerUnit unit)
    {
        HeroPanel[] panels = GetComponentsInChildren<HeroPanel>();

        foreach (HeroPanel panel in panels)
        {
            if (panel.unit.Equals(unit))
            {     
                return panel;
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        PlayerController.instance.unitListChange -= UpdateHeroList;
        PlayerController.instance.unitInfoChanged -= UpdateInfo;
    }

    public void AddHeroToParty(PlayerUnit unit, PartyFrames frame)
    {
        HeroPanel panel = FindHeroPanel(unit);
        if (panel == null) return;

        panel.AddToParty(frame);
    }

    public void RemoveHeroFromParty(PlayerUnit unit)
    {
        HeroPanel panel = FindHeroPanel(unit);
        if (panel == null) return;

        panel.RemoveFromParty();
    }

}
