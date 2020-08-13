
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
    private float verticalSpeed;

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

    private enum PetStates
    {
        Null,
        Activated,
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
        rigidbody.velocity = movementDirection.normalized * movementSpeed;
        switch (petState)
        {
            case PetStates.Activated:
                Debug.Log("Activated");
                MoveToCorrectPosition();
                break;
            case PetStates.Active:
                Debug.Log("Active");
                break;
            case PetStates.AutoMove:
                Debug.Log("AutoMove");
                AutoMove();
                break;
            case PetStates.Returning:
                Debug.Log("Returning");
                Return();
                break;
            case PetStates.Idle:
                Debug.Log("Idle");
                transform.position = startPosition.position;
                transform.rotation = startPosition.rotation;
                break;
            case PetStates.Null:
                return;
        }
    }

    private void AutoMove()
    {
        //Instantiate(pointVisualizer, GeneratedAutoMovePoint(), Quaternion.identity);
    }

    private Vector3 GeneratedAutoMovePoint()
    {
        Vector3 rand = (Random.insideUnitSphere.normalized /** sphereSize*/) + transform.position;
        Vector3 randomPosition = new Vector3(rand.x, transform.position.y, rand.z);
        return randomPosition;
    }

    private void SetPetState(PetStates newState)
    {
        petState = newState;
    }

    private void MoveToCorrectPosition()
    {

        if (transform.position.y < startAltitude)
        {
            Move(Vector3.up);
            Debug.Log("Climbing");
        }
        else
        {

            AttachControls();
            movementSpeed -= climbSpeed;
            SetPetState(PetStates.Active);
            return;
        }

    }


    public void Activate()
    {
        rigidbody.isKinematic = false;
        movementSpeed += climbSpeed;
        //active = true;
        SetPetState(PetStates.Activated);
    }


    public override void Move(Vector3 direction)
    {
        base.Move(direction);
        // If no Movement input is given auto movement will commence
        if (direction == Vector3.zero)
        {
            SetPetState(PetStates.AutoMove);
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
