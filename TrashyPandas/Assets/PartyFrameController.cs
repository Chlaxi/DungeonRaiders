using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyFrameController : MonoBehaviour {

    public PartyFrames[] partyFrames = new PartyFrames[4];

    public void SetupParty(PlayerUnit[] units)
    {
        for (int i = 0; i < 4; i++)
        {
            if (units[i] == null)
                continue;
            
            FindObjectOfType<HeroListUI>().SetupPartyList(units[i],partyFrames[i]);
        }

    }


}
