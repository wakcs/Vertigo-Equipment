using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupHat : PickupInteract
{
    public override bool PrimaryUseDown()
    {
        if (transform.parent)
        {
            // Detach if on a head socket, else put it on head socket
            if (transform.parent.CompareTag(headTag))
            {
                Detach();
            }
            else if(playerRef)
            {
                playerRef.AttachToSocket(transform, playerRef.SelectSocket(SocketType.HEAD));
            }
            return true;
        }
        return false;
    }
}
