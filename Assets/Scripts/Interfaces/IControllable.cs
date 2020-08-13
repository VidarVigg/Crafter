using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{

    void AttachControls();
    void DetachControls();
    void Move(Vector3 direction);
    void Interact();
    void RightMouseToggle(bool toggle);
    void Sprint(bool sprinting);
    void RotateCharacter(Vector3 vector);
    void ActivateAbility();

}
