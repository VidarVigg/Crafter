using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputOverrider
{
    void ApplyInputSet(InputModes inputMode);
}
