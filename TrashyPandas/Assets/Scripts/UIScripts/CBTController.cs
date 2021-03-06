﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class CBTController : MonoBehaviour {

    private Animator animator;

    public Text CBTText;
    public Color[] CBTColor = new Color[1];
    public Image CBTTypeIcon;
    public Text CBTCrit;
   


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnHit(EffectHitInfo hitInfo)
    {
        
        string value = hitInfo.hitValue.ToString();

        CBTCrit.enabled = hitInfo.isCrit;

        if (!hitInfo.isHit)
        {
            value = "Miss";
            CBTText.color = CBTColor[2];
            CBTText.text = value;
            TriggerCBT();
            return;
        }



        switch (hitInfo.hitType)
        {
            case HitType.Hit:
                CBTText.color = CBTColor[0];
                break;

            case HitType.Heal:
                CBTText.color = CBTColor[1];
                break;


            case HitType.DoT:
                Debug.Log("Dot Applied");
                
                /* if(bleed) {
                  CBTText.color
                 }*/
                
                 /*if(poison) {
                 CBTText.color
                   
                 }*/
                 
                 
                break;


            case HitType.HoT:
               // CBTText.color = CBTColor[4];
                break;

            default:
                value = hitInfo.hitValue.ToString();
                break;
        }

        CBTText.text = value;
        CBTCrit.color = CBTText.color;
        TriggerCBT();
        

    }

   

    public void OnBleed(int damage)
    {
        Debug.Log("Show bleed CBT");
        CBTText.text = damage.ToString();
        CBTCrit.color = CBTColor[0];
        TriggerCBT();
    }

    public void OnHoT(int heal)
    {
        CBTText.text = heal.ToString();
        CBTCrit.color = CBTColor[1];
        TriggerCBT();
    }

    public void Pass()
    {
            
            CBTText.color = CBTColor[2];
            CBTText.text = "Pass";
            TriggerCBT();
    }

    private void TriggerCBT()
    {
        animator.SetTrigger("Default");
    }
}
