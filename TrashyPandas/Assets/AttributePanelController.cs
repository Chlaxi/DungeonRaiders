using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributePanelController : MonoBehaviour {

    private CharacterStats stats;
    public AttributeContainer[] statContainers;

    public CanvasGroup levelButton;
    public Text levelButtonText;

    public bool isLevelingUp = false;

    public int availablePoints = 0;

    private void Start()
    {
        PlayerController.instance.unitInfoChanged += GetInfo;
    }

    public void Setup(CharacterStats stats)
    {
        this.stats = stats;
        if (stats.CanLevelUp) availablePoints = stats.availableAttPoints;

        int i = 0;
        foreach (AttributeContainer att in statContainers)
        {
            att.Setup(stats, i);
            i++;
        }
        GetInfo();
    }


    public void GetInfo()
    {
        isLevelingUp = stats.CanLevelUp;
        foreach (AttributeContainer att in statContainers)
        {
            att.GetInfo();        
        }
        UpdateUI();
        
    }

    private void UpdateUI()
    {
        LevelButtonVisibiliy();
    }

    public void ApplyStatChanges()
    {
        int[] tempStats = new int[6];
        for (int i = 0; i < statContainers.Length; i++)
        {
            tempStats[i] = statContainers[i].statValue;
        }


        stats.ApplyLevel(tempStats);
        GetInfo();
        PlayerController.instance.unitInfoChanged();
    }

    public void OnStatButtonClick()
    {
        LevelButtonVisibiliy();
        foreach (AttributeContainer container in statContainers)
        {
            container.ButtonVisibility();
        }
    }

    public void LevelButtonVisibiliy()
    {

        if (!isLevelingUp)
        {
            levelButton.alpha = 0;
            levelButton.interactable = false;
            return;
        }

        levelButton.alpha = 1;
        if (availablePoints > 0)
        {
            levelButtonText.text = availablePoints.ToString();
            levelButton.interactable = false;
        }

        if(availablePoints == 0)
        {
            levelButtonText.text = "Level up!";
            levelButton.interactable = true;
        }

       
    }

    private void OnDestroy()
    {
        PlayerController.instance.unitInfoChanged -= GetInfo;
    }


}
