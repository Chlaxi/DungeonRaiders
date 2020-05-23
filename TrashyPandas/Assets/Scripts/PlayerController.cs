using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO Refactor the whole "add/remove units" system. Move it to another script?
public class PlayerController : MonoBehaviour {

    #region Singleton

    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Debug.Log("This is not the singleton! Exterminate!");
            GameObject.Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    public delegate void UnitListChange(PlayerUnit unit);
    public UnitListChange unitListChange;

    public PlayerHeroInfo playerHeroInfo;

    public delegate void PartyChanged();
    public PartyChanged partyChanged;


    public delegate void UnitInfoChanged();
    public UnitInfoChanged unitInfoChanged;

    public int maxUnits = 4;


    [SerializeField]
    private ICharacter currentTarget;

    private int currentAbilitySlot;


    public ICharacter CurrentTarget
    {
        get
        {
            return currentTarget;
        }

        set
        {
            currentTarget = value;
            if (CombatManager.InCombat)
            {
                CombatUIController.instance.UpdateRightPanel(currentTarget);
            }
        }
    }

    public void AddUnit(PlayerUnit unit)
    {
        if (playerHeroInfo.availableUnits.Count < maxUnits)
        {

            playerHeroInfo.availableUnits.Add(unit);

            unitInfoChanged();
            //unitListChange(unit);
        }
    }

    public void RemoveUnit(int unitIndex)
    {
        if (playerHeroInfo.availableUnits.Count == 0) return;

        PlayerUnit unit = playerHeroInfo.availableUnits[unitIndex];

        if (unit == null) return;

        if (playerHeroInfo.availableUnits.Contains(unit))
        {
            playerHeroInfo.availableUnits.Remove(unit);
            unitInfoChanged();
            //unitListChange(unit);
        }
    }


    public void RemoveUnit(PlayerUnit unit)
    {
        if (playerHeroInfo.availableUnits.Count == 0) return;

        if (unit == null) return;

        if (playerHeroInfo.availableUnits.Contains(unit))
        {
            playerHeroInfo.availableUnits.Remove(unit);
            unitInfoChanged();
            //unitListChange(unit);
        }
    }

    public PlayerUnit[] GetAllUnits()
    {   
        return playerHeroInfo.GetAllUnits();
    }

    public void SetMaxUnits(int index)
    {
        maxUnits = index;
    }
    
    public void EmbarkOnMission()
    {
        if (GetPartySize() < 1)
        {
            Debug.Log("Too few members in the party. Did not embark");
            return;
        }

        SceneManager.LoadScene(1);
        //Embark on the mission!
    }

    public void ReturnToTown()
    {
        if(CombatManager.instance != null)
        {
            if (CombatManager.InCombat) return;

            SceneManager.LoadScene(0);
        }
    }

    public void AddToParty(PlayerUnit unit, int partyIndex)
    {
        if (partyIndex >= playerHeroInfo.party.Length || partyIndex < 0) return;
      //  Debug.Log("Adding " + unit + " to the party");
        playerHeroInfo.AddUnitToParty(unit, partyIndex);
        RemoveUnit(unit);
        partyChanged();
    }

    public void RemoveFromParty(int index)
    {
        if (index >= playerHeroInfo.party.Length || index < 0) return;

        if (playerHeroInfo.party[index] == null) return;

        AddUnit(playerHeroInfo.party[index]);
        FindObjectOfType<HeroListUI>().RemoveHeroFromParty(playerHeroInfo.party[index]);
        playerHeroInfo.party[index] = null;
        partyChanged();

    }

    /// <summary>
    /// Switches the position of two party members
    /// </summary>
    /// <param name="a">First party member</param>
    /// <param name="b">Second party member</param>
    public void SwitchPlace(int a, int b)
    {
        playerHeroInfo.SwitchPlace(a, b);
    }

    //TODO Make this smarter, by using TownController, etc.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("level: " + scene.name);
        if(scene.buildIndex==1)
        {
            PartyController partyController = FindObjectOfType<PartyController>();
            StartCoroutine(partyController.SetupParty(playerHeroInfo.party));
        }

        if (scene.buildIndex == 0)
        {
            Debug.Log("Town loaded");

            //TODO if the unit is in the party, don't add it to available, but still create the hero panel
            FindObjectOfType<HeroListUI>().SetupList();
            FindObjectOfType<PartyFrameController>().SetupParty(playerHeroInfo.party);

            unitInfoChanged();

        }

    }

    public int GetPartySize()
    {
        return playerHeroInfo.GetPartySize();
    }

    public void SpawnUnits()
    {
        foreach(PlayerUnit unit in playerHeroInfo.party)
        {
            unit.SpawnCharacter();
        }
    }

}
