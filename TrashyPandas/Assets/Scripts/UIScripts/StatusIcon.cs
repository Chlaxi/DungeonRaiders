using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{

    LayoutElement layout;
    Image image;

    public bool isActive = false;

    private string tooltip;
    private int value;
    private int duration;

    private void Start()
    {
        layout = GetComponent<LayoutElement>();
        image = GetComponent<Image>();
    }

    public void SetInfo(Status info)
    {

        //duration = (int)info.duration;
        tooltip = info.GetDescription();
    }

    public void ShowIcon(Status info)
    {

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

    public void ShowTooltip()
    {
        Debug.Log(tooltip);
    }

}
