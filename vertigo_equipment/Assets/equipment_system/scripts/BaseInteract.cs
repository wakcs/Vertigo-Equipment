using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteract : MonoBehaviour
{
    protected PlayerInteractor playerRef;
    protected MeshRenderer[] meshRenders;
    protected List<Color> meshColors;
    protected Color focusColor = Color.red;
    public bool isFocused = false;

    protected readonly string playerTag = "Player";
    protected const string leftTag = "Left", rightTag = "Right", headTag = "Head";

    protected void Start()
    {
        meshRenders = GetComponentsInChildren<MeshRenderer>();
        meshColors = new List<Color>();
        foreach (MeshRenderer render in meshRenders)
        {
            meshColors.Add(render.material.color);
        }
    }

    private void FixedUpdate()
    {
        if(playerRef && playerRef.closestInteract == this)
        {
            if (!isFocused)
            {
                ChangeFocus(true);
            }
        }
        else if(isFocused)
        {
            ChangeFocus(false);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(playerTag))
        {
            playerRef = other.transform.GetComponent<PlayerInteractor>();
        }
    }
    protected void OnTriggerStay(Collider other)
    {
        // Set focus to this object if not picked up and closest (in range) of the player
        if (playerRef && !IsPickedUp() && playerRef.closestInteract != this)
        {
            if (!playerRef.closestInteract)
            {
                playerRef.closestInteract = this;
            }
            float ourDistance = Vector3.Distance(playerRef.transform.position, 
                transform.position);
            float theirDistance = Vector3.Distance(playerRef.transform.position,
                playerRef.closestInteract.transform.position);
            if (ourDistance < theirDistance)
            {
                playerRef.closestInteract = this;
            }
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(playerTag) && playerRef)
        {
            if (playerRef.closestInteract == this)
            {
                playerRef.closestInteract = null;
            }
            playerRef = null;
        }
    }

    public abstract bool Interact(Transform interactor);

    private void ChangeFocus(bool isInFocus)
    {
        for (int i = 0; i < meshRenders.Length; ++i)
        {
            meshRenders[i].material.color = isInFocus ? focusColor : meshColors[i];
        }
        isFocused = isInFocus;
    }

    public bool IsPickedUp()
    {
        return transform.parent &&
            (transform.parent.CompareTag(leftTag) ||
            transform.parent.CompareTag(rightTag) ||
            transform.parent.CompareTag(headTag));
    }
}
