using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SocketType
{
    NONE,
    LEFT,
    RIGHT,
    HEAD
}
public class PlayerInteractor : MonoBehaviour
{
    public struct ShortLongPressStruct
    {
        public SocketType socketType;
        public string buttonName;
        public float pressTime;
        public bool isHolding;
        public bool hasLongPressed;
        public ShortLongPressStruct(SocketType socketType, string buttonName)
        {
            this.socketType = socketType;
            this.buttonName = buttonName;
            pressTime = 0;
            isHolding = false;
            hasLongPressed = false;
        }
    }

    public Transform handL, handR, head;
    [HideInInspector]
    public BaseInteract closestInteract = null;
    public ShortLongPressStruct interactLeft = new ShortLongPressStruct(SocketType.LEFT, "InteractL");
    public ShortLongPressStruct interactRight = new ShortLongPressStruct(SocketType.RIGHT, "InteractR");
    public readonly float longPressDuration = 0.5f;

    private const string useLeft = "Fire1", useRight = "Fire2";

    // Update is called once per frame
    protected void Update()
    {
        HandleLongShortPress(ref interactLeft);
        HandleLongShortPress(ref interactRight);

        HandleUseInput(SocketType.LEFT, useLeft);
        HandleUseInput(SocketType.RIGHT, useRight);

        // Cannot use HandelUseInput since it asks 2 inputs
        if (GetAttachedObject(SocketType.HEAD)) {
            if (Input.GetButtonDown(useLeft) && Input.GetButtonDown(useRight))
            {
                GetAttachedObject(SocketType.HEAD).PrimaryUseDown();
            }
            else if (Input.GetButtonUp(useLeft) && Input.GetButtonUp(useRight))
            {
                GetAttachedObject(SocketType.HEAD).PrimaryUseUp();
            }
        }
    }

    public bool AttachToSocket(Transform attachingObject, Transform socket)
    {
        // Will not attach to anything except my sockets
        if(socket != handL && socket != handR && socket != head)
        {
            return false;
        }
        if(socket.childCount != 0)
        {
            return attachingObject.IsChildOf(socket);
        }
        attachingObject.SetParent(socket, false);
        attachingObject.localPosition = Vector3.zero;
        attachingObject.localEulerAngles = Vector3.zero;
        if (attachingObject.GetComponent<Rigidbody>() != null)
        {
            attachingObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        return true;
    }

    public Transform SelectSocket(SocketType socket)
    {
        switch (socket)
        {
            case SocketType.LEFT:
                return handL;
            case SocketType.RIGHT:
                return handR;
            case SocketType.HEAD:
                return head;
            default:
                return null;
        }
    }

    public PickupInteract GetAttachedObject(Transform socket)
    {
        Transform attachedTrans = null;
        if (socket && socket.childCount != 0)
        {
            attachedTrans = socket.GetChild(0);
        }
        return attachedTrans ? attachedTrans.GetComponent<PickupInteract>() : null;
    }
    public PickupInteract GetAttachedObject(SocketType socket)
    {
        Transform socketTrans = SelectSocket(socket);
        return GetAttachedObject(socketTrans);
    }

    private void HandleLongShortPress(ref ShortLongPressStruct pressStruct)
    {
        Transform socket = SelectSocket(pressStruct.socketType);
        PickupInteract attachedObj = GetAttachedObject(socket);
        if (Input.GetButton(pressStruct.buttonName))
        {
            if(!pressStruct.isHolding)
            {
                pressStruct.pressTime = Time.time;
                pressStruct.isHolding = true;
            }
            if(!pressStruct.hasLongPressed && 
                (Time.time - pressStruct.pressTime) >= longPressDuration)
            {
                // When longpressed, either interact with holding object
                // Or closest object if not holding any
                if (attachedObj)
                {
                    attachedObj.Interact(socket);
                }
                else if (closestInteract)
                {
                    closestInteract.Interact(socket);
                    closestInteract = null;
                }
                pressStruct.hasLongPressed = true;
            }
        }
        else if (Input.GetButtonUp(pressStruct.buttonName))
        {
            // When shortpressed, try attached object's secondary use
            // Interact with closest object if that fails
            if (!pressStruct.hasLongPressed &&
                (!attachedObj || !attachedObj.SecondaryUseDown())
                && closestInteract)
            {
                closestInteract.Interact(socket);
                closestInteract = null;
            }           
            pressStruct.hasLongPressed = false;
            pressStruct.isHolding = false;
        }
    }

    private void HandleUseInput(SocketType socket, string buttonName)
    {
        if (GetAttachedObject(socket))
        {
            if (Input.GetButtonDown(buttonName))
            {
                GetAttachedObject(socket).PrimaryUseDown();
            }
            else if (Input.GetButtonUp(buttonName))
            {
                GetAttachedObject(socket).PrimaryUseUp();
            }
        }
    }
}