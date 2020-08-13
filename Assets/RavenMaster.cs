
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenMaster : ControllableCharacter
{

    [SerializeField]
    private PlayerManager player;

    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private float minimumAltitude;

    [SerializeField]
    private float maxAltitude;

    [SerializeField]
    private float startAltitude;

    [SerializeField]
    private float climbSpeed;

    // For visualization purpose;
    [SerializeField]
    private GameObject pointVisualizer;

    [SerializeField]
    private float wingFlapForce;

    [SerializeField]
    private float wingFlapFrequency;

    [SerializeField]
    private float constantUpwardsForce;

    private float tick;

    private enum PetStates
    {
        Null,
        Climb,
        Active,
        Returning,
        Idle,
        AutoMove,
    }
    private PetStates petState;

    private void Start()
    {
        SetPetState(PetStates.Idle);
    }


    private void Update()
    {

    }
    private void FixedUpdate()
    {

        switch (petState)
        {
            case PetStates.Climb:

                MoveToCorrectPosition();
                break;
            case PetStates.Active:
                Active();
                break;
            case PetStates.AutoMove:

                AutoMove();
                break;
            case PetStates.Returning:

                Return();
                break;
            case PetStates.Idle:

                transform.position = startPosition.position;
                transform.rotation = startPosition.rotation;
                break;
            case PetStates.Null:
                return;
        }
        rigidbody.velocity = movementVector;
    }

    private void Active()
    {
        
        rigidbody.AddForce(Vector3.up * constantUpwardsForce);

        if (transform.position.y < startAltitude)
        {
            SetPetState(PetStates.Climb);
        }
    }

    private void AutoMove()
    {
        //Instantiate(pointVisualizer, GeneratedAutoMovePoint(), Quaternion.identity);
    }

    //private Vector3 GeneratedAutoMovePoint()
    //{
    //    Vector3 rand = (Random.insideUnitSphere.normalized /** sphereSize*/) + transform.position;
    //    Vector3 randomPosition = new Vector3(rand.x, transform.position.y, rand.z);
    //    return randomPosition;
    //}

    private void SetPetState(PetStates newState)
    {
        petState = newState;
    }

    private void MoveToCorrectPosition()
    {

        if (transform.position.y < startAltitude)
        {
            if ((tick += Time.deltaTime) >= wingFlapFrequency)
            {
                rigidbody.AddForce(Vector3.up * wingFlapForce, ForceMode.Impulse);
                Debug.Log("Flap");
                tick -= wingFlapFrequency;
            }
            Debug.Log("Climbing");
        }
        else
        {
            if (rigidbody.velocity.y < 0)
            {
                SetPetState(PetStates.Active);
                AttachControls();
                Debug.Log("Reached target Altitude");


            }
        }

    }


    public void Activate()
    {
        rigidbody.isKinematic = false;
        //movementSpeed += climbSpeed;
        //active = true;
        SetPetState(PetStates.Climb);
    }


    public override void Move(Vector3 direction)
    {
        base.Move(direction);
        if (direction == Vector3.zero)
        {
            Debug.Log("No Input");
        }
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override void RightMouseToggle(bool toggle)
    {
        base.RightMouseToggle(toggle);
    }

    public override void RotateCharacter(Vector3 vector)
    {
        base.RotateCharacter(vector);
    }

    //public void ChangeAltitude(int dir)
    //{
    //    rigidbody.velocity += new Vector3(0, dir, 0) * verticalSpeed;
    //}

    public override void ActivateAbility()
    {

        Debug.Log("SwitchBack");
        DetachControls();
        player.AttachControls();
        SetPetState(PetStates.Returning);

    }

    public void Return()
    {
        rigidbody.velocity = (startPosition.position - transform.position).normalized * movementSpeed;


        Vector3 target = startPosition.position - transform.position;
        float angle = Mathf.Atan2(target.x, target.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20f * Time.deltaTime);


        if ((startPosition.position - transform.position).sqrMagnitude < 0.1f)
        {
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            SetPetState(PetStates.Idle);
            return;
        }
    }
    public override void Sprint(bool sprinting)
    {
        base.Sprint(sprinting);
    }
}
