using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{

    public int duration;
    public Dice dice;
    public int modifier;
    public StatusIcon statusIcon;
    private StatusEffect effect;
    private string description;

    public Status(StatusEffect effect, StatusIcon icon)
    {
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

        if (statusIcon == null)
        {
            Debug.LogWarning("No icon setup for " + effect.name);
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

        return description + " \n Time left: "+duration;
    }
}
