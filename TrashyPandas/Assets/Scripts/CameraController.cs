using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Singleton
    public static CameraController instance;

    private void Awake()
    {
        instance = this;

    }
    #endregion

    [SerializeField] private Vector2 offset;

    private void Start()
    {
        CombatManager.instance.combatStartedDelegate += StartCombat;
        CombatManager.instance.combatEndedDelegate += EndCombat;
        offset = movementCamPosition;
    }

    private Vector3 desiredPosition;

    private bool isLerping;
    public bool isMoving = false;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Transform playerParty;
    [SerializeField] private Vector2 battleCamPosition;
    [SerializeField] private Vector2 movementCamPosition;

    private void FixedUpdate()
    {
        LerpToOffset();
    }

    public void StartCombat()
    {
        offset = battleCamPosition;
    }

    public void EndCombat()
    {
        offset = movementCamPosition;
    }

    public void LerpToOffset()
    {
        desiredPosition = new Vector3(playerParty.position.x + offset.x, playerParty.position.y + offset.y, transform.position.z);
        Vector3 startPos = transform.position;

        float journeyLength = Vector3.Distance(startPos, desiredPosition);

            float distCovered = cameraSpeed * Time.fixedDeltaTime;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, fracJourney);
    }
}
