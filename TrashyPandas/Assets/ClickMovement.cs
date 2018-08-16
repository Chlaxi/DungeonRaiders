using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMovement : MonoBehaviour {

    CharacterController2D controller;
    RectTransform rect;
    [SerializeField] private Vector2 clickPosition;
    [SerializeField] float width;
    [SerializeField] float offset;
    [SerializeField] private bool isHeld;

    private void Start()
    {
        controller = FindObjectOfType<CharacterController2D>();
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        controller.IsMovingWithClick = isHeld;

        if (!isHeld) return;

        clickPosition = Input.mousePosition;
        float direction = 0;

        if (clickPosition.x > (width / 2) + offset)
            direction = 1;

        else if (clickPosition.x < (width / 2) - offset)
            direction = -1;

        controller.Move(direction);
    }

    public void OnClick()
    {
        if (CombatManager.InCombat) return;

        isHeld = true;
        width = rect.rect.width;
    }

    public void OnRelease()
    {
        
        isHeld = false;
    }

}
