using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusToolTip : MonoBehaviour
{
    public bool isHidden = true;

    [SerializeField] private Animator animator;

    [SerializeField] private Text statusName;
    [SerializeField] private Text description;


    public bool SetupTooltip(StatusIcon status)
    {
        
        if (status == null) return false;

        statusName.text = status.GetName();
        description.text = status.GetDesription();

        return true;
    }

    public void ShowTooltip()
    {
        if (!isHidden) return;

        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void HideTooltip()
    {
        if (isHidden) return;

        animator.SetTrigger("Hide");
        isHidden = true;
    }
}
