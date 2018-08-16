using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    [SerializeField] private float speed;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    private bool isMovingWithClick;

    private Rigidbody2D rigid;
    private bool canMove = true;
    private Vector3 m_Velocity = Vector3.zero;

    private Animator[] animators;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ICharacter[] units = GetComponentsInChildren<ICharacter>();
        animators = new Animator[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            animators[i] = units[i].GetComponent<Animator>();
        }

        CombatManager.instance.combatStartedDelegate += Stop;
    }


    private void FixedUpdate()
    {
        canMove = !CombatManager.InCombat;


        if (canMove && !isMovingWithClick)
        {
            Move(Input.GetAxis("Horizontal"));
        }
    }

    public void Move(float direction)
    {
        if (CombatManager.InCombat || !canMove)
        {
         

        }
        float deltaX = (direction * speed * Time.deltaTime);
        Vector3 targetVelocity = new Vector2(deltaX, 0);


        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        SetAnim(deltaX);

    }


    private void SetAnim(float speed)
    {
        if (animators.Length > 0)
        {
            foreach (Animator animator in animators)
            {
                animator.SetFloat("Speed", speed);
            }
        }
    }

    public bool IsMovingWithClick
    {
        get
        {
            return isMovingWithClick;
        }

        set
        {
            isMovingWithClick = value;
        }
    }

    public void Stop()
    {
        Debug.Log("Stop moving!");
        SetAnim(0);
    }

}
