using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler{

    public PlayerUnit unit;
    private CharacterStats stats;
    new public Text name;
    public Text unitClass;
    public Text level;
    public Image icon;
    public Image levelUpIcon;
    private LayoutElement layout;

    private Canvas canvas;
    private Transform content;
    private Image panelImage;

    [SerializeField] private Color partyColor;
    [SerializeField] private Color defaultColor;

    private bool isSetUp = false;
    private bool isInParty;
    public int heroIndex;

    public bool SetupHeroPanel(PlayerUnit unit)
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        content = FindObjectOfType<HeroListUI>().transform;
        layout = GetComponent<LayoutElement>();
        panelImage = GetComponent<Image>();

        this.unit = unit;
        stats = unit.GetComponent<CharacterStats>();

        if (unit != null && stats != null) isSetUp = true;
//        Debug.Log(name + " was set up?" + isSetUp);
        return isSetUp;
    }

    public bool SetupHeroPanelAsPartyMember(PlayerUnit unit)
    {
        if (SetupHeroPanel(unit))
        {
            isInParty = true;
        }
        return isSetUp;
    }

    public void UpdatePanel()
    {
        if (!isSetUp)
        {
            Debug.Log("Isn't set up");
            return;
        }

        name.text = unit.name;
        unitClass.text = stats.unitClass.name;
        level.text = stats.level.ToString();
        icon.sprite = stats.unitClass.icon;

        if (stats.CanLevelUp)
        {
            levelUpIcon.enabled = true;
        }
        else
        {
            levelUpIcon.enabled = false;
        }
    }

    public void OnClick()
    {
        CharacterPanelController.instance.Open(unit);
    }

    public void OnDestroy()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      //  layout.ignoreLayout = true;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {


        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 61.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended");

        Drop(eventData);
        transform.SetParent(content);
     //   layout.ignoreLayout = false;
        transform.localPosition = Vector3.zero;

    }

    public void Drop(PointerEventData eventData)
    {
        Vector3 screenPoint = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        List<RaycastResult> hitObjects = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, hitObjects);
        if (hitObjects.Count <= 0) return;

        if (hitObjects[0].gameObject.tag == "UIDropPoint")
        {
            PartyFrames frame = hitObjects[0].gameObject.GetComponent<PartyFrames>();

            if(frame == null)
            {
            }
            else
            {
                if(!isInParty)
                    AddToParty(frame);
            }
        }
   
    }

    public void AddToParty(PartyFrames frame)
    {
        isInParty = true;
        panelImage.color = partyColor;
        frame.AddUnit(unit);
    }

    public void RemoveFromParty()
    {
        isInParty = false;
        panelImage.color = defaultColor;
    }

}
