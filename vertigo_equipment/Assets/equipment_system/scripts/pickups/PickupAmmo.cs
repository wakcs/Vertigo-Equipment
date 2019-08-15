using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupInteract
{
    public override bool PrimaryUseDown()
    {
        if (playerRef && transform.parent) {
            PickupGun otherHand = null;
            if (transform.parent.CompareTag(leftTag))
            {
                otherHand = playerRef.GetAttachedObject(SocketType.RIGHT) as PickupGun;
            }
            else if(transform.parent.CompareTag(rightTag))
            {
                otherHand = playerRef.GetAttachedObject(SocketType.LEFT) as PickupGun;
            }
            if (otherHand && otherHand.Reload())
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }
        else
        {
            Debug.LogWarning("No player reference or parent found");
            return false;
        }
    }
}
