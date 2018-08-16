using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartyFrameDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

    PartyFrames partyFrame;
    RectTransform partyRect;
    LayoutElement layout;

    bool isDragging = false;

    private void Start()
    {
        partyFrame = GetComponent<PartyFrames>();
        partyRect = GetComponentInParent<PartyFrameController>().transform as RectTransform;
        layout = GetComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (partyFrame.unit == null)
        {
            isDragging = false;
            return;
        }
        isDragging = true;
        layout.ignoreLayout = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = Camera.main.nearClipPlane; //distance of the plane from the camera

        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 61.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        layout.ignoreLayout = false;

        if (RectTransformUtility.RectangleContainsScreenPoint(partyRect, screenPoint))
        {
            //Debug.Log("Party frame hit!!");
            //TODO Reposition the unit
        }
        else
        {
            partyFrame.RemoveUnit();
        }
        
    }
}
