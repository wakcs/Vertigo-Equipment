using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupRock : PickupInteract
{
    public float throwForce = 100;

    public override bool PrimaryUseDown()
    {
        Detach();
        if (rigidBody)
        {
            rigidBody.AddForce(transform.forward * Time.fixedDeltaTime * throwForce, ForceMode.Impulse);
        }
        return true;
    }
}
