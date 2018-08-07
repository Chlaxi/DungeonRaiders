using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElementDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public const string dropPoint_tag = "UIDropPoint";

    private bool isDragging = false;
    private Transform objectToDrag;
    private Image objectDragImage;
    private Vector3 screenPoint;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        //  layout.ignoreLayout = true;
    //    transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {


        screenPoint = Input.mousePosition;
        screenPoint.z = Camera.main.nearClipPlane; //distance of the plane from the camera
        Debug.Log(screenPoint.z);

        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void Drop(PointerEventData eventData)
    {
        Debug.Log("DROP!");

        Vector3 screenPoint = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        Debug.Log(screenPoint);
        PartyFrames[] partyFrames = FindObjectsOfType<PartyFrames>();

        foreach (PartyFrames frame in partyFrames)
        {
            RectTransform partyRect = frame.transform as RectTransform;
            Debug.Log(partyRect.rect.ToString());

            if (RectTransformUtility.RectangleContainsScreenPoint(partyRect, screenPoint))
            {
                Debug.Log("Party frame hit!!");
            }
            else
            {
                Debug.Log("No party frame hit");
            }
        }

    }
}
