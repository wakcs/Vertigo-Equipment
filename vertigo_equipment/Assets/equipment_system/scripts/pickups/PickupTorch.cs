using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTorch : PickupInteract
{
    private Light torchLight;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        torchLight = GetComponentInChildren<Light>();
    }

    public override bool PrimaryUseDown()
    {
        if(torchLight)
        {
            torchLight.enabled = !torchLight.enabled;
            return true;
        }
        return false;
    }
}
