using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{

    LayoutElement layout;
    Image image;
    public HealthBarScript healthBar;
    public bool isActive = false;

    private Status status;
    private int value;
    private int duration;

    private void Start()
    {
        layout = GetComponent<LayoutElement>();
        image = GetComponent<Image>();
        healthBar = GetComponentInParent<HealthBarScript>();
    }

    public void SetInfo(Status info)
    {

        //duration = (int)info.duration;
        status = info;
    }

    public void ShowIcon(Status info)
    {
        if(layout==null)
            layout = GetComponent<LayoutElement>();
        if(image==null)
            image = GetComponent<Image>();

        layout.ignoreLayout = false;
        image.enabled = true;
        
        image.sprite = info.GetEffect().statusIcon;
        
        isActive = true;
        SetInfo(info);
    }

    public void HideIcon()
    {
        layout.ignoreLayout = true;
        image.enabled = false;
        isActive = false;
    }

    public string GetDesription()
    {
        return status.GetDescription();

    }

    public string GetName()
    {
        return status.name;
    }

    public void OnPointerEnter()
    {

        healthBar.ShowTooltip(this);
    }

    public void OnPointerLeave()
    {
        healthBar.HideTooltip();
    }



}
