using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public new string name;
    public int duration;
    public Dice dice;
    public int modifier;
    public StatusIcon statusIcon;
    private StatusEffect effect;
    private string description;

    public Status(StatusEffect effect, StatusIcon icon)
    {
        name = effect.name;
        duration = effect.duration;
        dice = effect.dice;
        modifier = effect.modifier;
        this.effect = effect;
        statusIcon = icon;
        description = effect.description;
        Debug.Log("New status," + effect.ToString() + " has been added");
        InitialEffect();
    }

    public void InitialEffect()
    {
        Debug.Log("Status initiated");
        if (effect == null)
        {
            Debug.LogError("effect was null");
            statusIcon.HideIcon();
            return;
        }

        effect.InitialEffect();
        statusIcon.ShowIcon(this);
    }

    public void ApplyEffect()
    {
        if (effect == null)
        {
            statusIcon.HideIcon();
            Debug.LogError("effect is not defined");
            return;
        }

        effect.ApplyEffect();
        duration--;

        if (statusIcon == null)
        {
            Debug.LogError("No icon setup for " + effect.name);
            //SetIcon(CBT.EnableIconDynamically(effect));
            return;
        }
        statusIcon.SetInfo(this);

        //redundant?
        if (IsFinished())
        {
            statusIcon.HideIcon();
            return;
        }
    }

    /// <summary>
    /// Checks whether the effect is finished. Once the duration runs out, EndEffect is called.
    /// </summary>
    /// <returns>Returns whether there's still duration left on the ability</returns>
    public bool IsFinished()
    {
        if (duration > 0)
            return false;

        effect.EndEffect();
        return true;
    }

    public StatusEffect GetEffect()
    {
        return effect;
    }

    public string GetDescription()
    {
        string newDescription = description;
        newDescription = newDescription.Replace("@dice", dice.ToString());
        newDescription = newDescription.Replace("@modifier", modifier.ToString());
        newDescription = newDescription.Replace("@duration", duration.ToString());

        return newDescription + " \nTime left: "+duration;
    }

    public void SetIcon(StatusIcon icon)
    {
        if (icon == null)
            return;

        if (statusIcon != null)
        {
            //There's already an icon. Remove the one we have. Before setting a new.
            return;
        }

        statusIcon = icon;
        statusIcon.ShowIcon(this);
    }
}
