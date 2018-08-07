using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttributeContainer : MonoBehaviour {


    private CharacterStats stats;

    public int statValue;
    public Button levelUp;
    public Button levelDown;
    public Text modifier;
    public Text rawValue;

    private int lockedStatValue;
    private int statIndex;

    AttributePanelController controller;

    private void Start()
    {
        controller = GetComponentInParent<AttributePanelController>();
    }

    public void Setup(CharacterStats stats, int index)
    {
        this.stats = stats;
        statIndex = index;

    }

    public void GetInfo()
    {

        statValue = stats.GetRawStat(statIndex);
        lockedStatValue = statValue;

        UpdateUI();
    }

    public void UpdateUI()
    {
        rawValue.text = statValue.ToString();
        ButtonVisibility();
        UpdateModifier();
    }

    public void OnRankUp()
    {
        if(stats.availableAttPoints > 0)
        {
            statValue++;
            controller.availablePoints--;
            controller.LevelButtonVisibiliy();
            UpdateUI();
        }
    }

    public void OnRankDown()
    {
        if(statValue > lockedStatValue)
        {
            statValue--;
            controller.availablePoints++;
            controller.LevelButtonVisibiliy();
            UpdateUI();
        }
    }

    public void ButtonVisibility()
    {
        if (controller == null)
        {
            throw new System.Exception("No controller");
        }

        if(controller.availablePoints == 0)
        {
            levelUp.interactable = false;
        }
        else
        {
            levelUp.interactable = true;
        }

        if(statValue > lockedStatValue)
        {
            levelDown.interactable = true;
        }
        else
        {
            levelDown.interactable = false;
        }

    }

    private void UpdateModifier()
    {
        modifier.text = stats.GetModifier(statValue).ToString();
        
    }
}
