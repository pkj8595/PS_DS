using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    bool CanMove { get; }

    void CommandMoveTo(Vector3 targetPosition);
}
