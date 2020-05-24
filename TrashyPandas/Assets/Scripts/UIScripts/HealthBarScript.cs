using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HealthBarScript : MonoBehaviour {

    ICharacter unit;
    CharacterStats stats;

    public bool isHidden;
    CanvasGroup canvasGroup;

    public CBTController CBT;


    public Image turnImage;

    private bool isBleeding = false;

    private bool hasHot = false;
    [SerializeField] private StatusToolTip toolTip;
    public Transform buffBar;
    [SerializeField] private GameObject StatusIconPrefab;
    public List<StatusIcon> buffIcons = new List<StatusIcon>();

    [SerializeField] private Slider slider;

    public Color playerColour;
    public Color enemyColour;

    [SerializeField]
    private Image turnBorder;

    private List<BleedStatus> bleeds = new List<BleedStatus>();
    private List<HealOverTimeStatus> hots = new List<HealOverTimeStatus>();

    private void Awake()
    {
        isHidden = false;       
    }

    public void SetupBar()
    {
        unit = GetComponentInParent<ICharacter>();
        stats = GetComponentInParent<CharacterStats>();

        unit.onHealthChange += OnHealthChange;
 
        canvasGroup = GetComponent<CanvasGroup>();
        SetPotentialTargetBorder(false);
        HideBar(false);
        if (GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    public void StartTurn()
    {
        //Set the turn indicator

            
    }

    public void OnHealthChange()
    {
        slider.maxValue = unit.stats.MaxHealth;
        slider.value = unit.stats.CurrentHealth;
    }

    public void SetPotentialTargetBorder(bool canTarget)
    {

        turnBorder.enabled = canTarget;
        SetTargetBorder();
        
    }


    public void HideBar(bool hide)
    {


        isHidden = hide;
        if (hide)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            OnHealthChange();
        }
    }

    public void SetTargetBorder()
    {
        if (unit == null) return;

        if (unit.isPlayer())
        {
            turnBorder.color = playerColour;
        }
        else
        {
            turnBorder.color = enemyColour;
        }
    }

    public void OnHit(EffectHitInfo hitInfo)
    {

        CBT.OnHit(hitInfo);
    }

    /// <summary>
    /// Applies an effect over time
    /// </summary>
    /// <param name="effect"></param>
    public void OnEffectOverTime(StatusEffect effect)
    {
        //Debug.Log("Effect over time of type "+effect.abilityType+" HitType="+effect.hitType);

        int value = effect.hitInfo.hitValue;

        //This is where the info should update?

        if(effect.type == StatusType.HoT)
        {
            ManageHots();
            CBT.OnHoT(value);
            return;
        }

        switch (effect.type)
        {
            case StatusType.Bleed:
                ManageBleeds();
                CBT.OnBleed(value);
                break;
            case StatusType.Poison:
                //CBT.OnPosion(value);
                break;
        }
    }

    public void SetTurnIcon()
    {
        if (unit.hasHadTurn)
        {
            turnImage.color = new Color(210, 215, 70, 0);
        }
        else
        {
            turnImage.color = new Color(210, 215, 70, 255);
        }
    }

    public StatusIcon AddNewStatus(StatusEffect status)
    {
        for (int i = 0; i < buffIcons.Count; i++)
        {
            if (buffIcons[i].isActive)
                continue;

            //buffIcons[i].ShowIcon(effect);
            return buffIcons[i];
        }

        StatusIcon statusIcon = Instantiate(StatusIconPrefab, buffBar).GetComponent<StatusIcon>();
        buffIcons.Add(statusIcon);
        return statusIcon;
        //Store index, so it can be updated and hidden.
    }

    public void RemoveIconDynamically(Status status)
    {
        //Get the actual index.
        int index = 0;
        if (index < 0 || index > buffIcons.Count)
            return;

        buffIcons[index].HideIcon();
    }




    public void UpdateStatusIndex(int oldIndex, int newIndex)
    {
        //Set icon index to new index
    }

    public void ApplyHoT(HealOverTimeStatus status)
    {
        Debug.Log("HoT info: Damage: " + status.dice + " Effect duration: " + status.duration);
        hots.Add(status);
        //AddNewStatus(status);
    }

    public void ApplyBleed(BleedStatus status)
    {
        Debug.Log("Bleed info: Damage: " + status.dice + " Effect duration: " + status.duration);
        bleeds.Add(status);
       // AddNewStatus(status);

    }

    //TODO make more universal, to accept all types of status effects. Make a struct?
    private void ManageBleeds()
    {
        int bleedCount = 0;
        for (int i = 0; i < unit.currentEffects.Count; i++)
        {
            if(unit.currentEffects[i].GetEffect().type == StatusType.Bleed)
            {
          
                //bleedIcon.SetInfo(unit.currentEffects[i]);
                if (unit.currentEffects[i].duration > 0)
                    bleedCount++;
            }
        }

        isBleeding = (bleedCount > 0) ? true : false;

       // bleedIcon.ShowIcon(isBleeding);
    }

    private void ManageHots()
    {
        int hotsCount = 0;
        for (int i = 0; i < unit.currentEffects.Count; i++)
        {
            if (unit.currentEffects[i].GetEffect().type == StatusType.HoT)
            {
                //hotIcon.SetInfo(unit.currentEffects[i]);
                if(unit.currentEffects[i].duration>0)
                    hotsCount++;
            }
        }

        hasHot = (hotsCount > 0) ? true : false;
        

       // hotIcon.ShowIcon(hasHot);
    }

    public void ShowTooltip(StatusIcon icon)
    {
        if (toolTip.SetupTooltip(icon))
            if (toolTip.isHidden)
                toolTip.ShowTooltip();
    }

    public void HideTooltip()
    {
        if (toolTip.isHidden) return;
        toolTip.HideTooltip();
    }

}
