using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{

    LayoutElement layout;
    Image image;

    private int value;
    private int duration;

    private void Start()
    {
        layout = GetComponent<LayoutElement>();
        image = GetComponent<Image>();
    }

    public void SetInfo(StatusEffect info)
    {
        Debug.Log(info.ToString());
        value = (int)info.power;
        duration = (int)info.duration;
    }

    public void ShowIcon(bool show)
    {
        if (show)
        {
            layout.ignoreLayout = false;
            image.enabled = true;
        }
        else
        {
            layout.ignoreLayout = true;
            image.enabled = false;
        }
    }

    public void ShowTooltip()
    {
        //Get a list of values and duration from the first corrosponding list.
        Debug.Log(gameObject.name + " has a value of " + value + " and lasts for " + duration + " turns");
    }

}
