using FishNet.Object;
using UnityEngine;

public class NetMoveTest : NetworkBehaviour
{
    
    void Update()
    {
        if (!IsOwner) return;

        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += 5.0f * movementInput * Time.deltaTime;
    }
}
