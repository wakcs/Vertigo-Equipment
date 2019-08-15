using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PickupInteract : BaseInteract
{
    public float discardForce = 500;

    protected Rigidbody rigidBody;

    protected new void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
    }

    public override bool Interact(Transform interactor)
    {
        if(!interactor)
        {
            return false;
        }

        if (IsPickedUp())
        {
            // Drop it when interactor is the parent
            if (transform.IsChildOf(interactor))
            {
                Detach();
                return true;
            }
            return false;
        }
        else if(playerRef)
        {
            return playerRef.AttachToSocket(transform, interactor);
        }
        return false;
    }

    // Each pickup is required to have a primary use on key down, on key up and secondary are optional.
    public abstract bool PrimaryUseDown();
    public virtual bool PrimaryUseUp()
    {
        return false;
    }
    public virtual bool SecondaryUseDown()
    {
        return false;
    }
    public virtual bool SecondaryUseUp()
    {
        return false;
    }

    protected void Detach()
    {
        transform.SetParent(null);
        if (rigidBody)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForce(transform.forward * Time.fixedDeltaTime * discardForce, ForceMode.VelocityChange);
        }
    }
}
