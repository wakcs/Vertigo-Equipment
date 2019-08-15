using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteract : BaseInteract
{
    public bool isBoomerangAnim = false;
    public bool canSelfActivate = true;
    public bool canBeUsedOnce = false;
    public WorldInteract[] triggerObjects;

    private Animation interactAnim;
    private AnimationState state;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        interactAnim = GetComponentInChildren<Animation>();
        if (interactAnim)
        {
            foreach (AnimationState state in interactAnim)
            {
                this.state = state;
                this.state.speed *= -1;
                break;
            }
        }
        EnableTrigger(canSelfActivate);
    }

    protected void Update()
    {
        if (isBoomerangAnim && state)
        {
            if(state.time >= state.length - Time.deltaTime)
            {
                state.time = state.length;
                state.speed = -1;
                interactAnim.Play();
            }
        }
    }

    public override bool Interact(Transform interactor)
    {
        if (canSelfActivate || (interactor && interactor.GetComponent<WorldInteract>()))
        {
            // Trigger animation if it has any, will reverse next occurrence
            if (state && !interactAnim.isPlaying)
            {
                state.time = state.speed > 0 ? state.length : 0;
                state.speed *= -1;
                interactAnim.Play();
            }

            // Trigger interact for other objects if set
            foreach (WorldInteract triggerObject in triggerObjects)
            {
                triggerObject.Interact(transform);
            }
            if(canBeUsedOnce)
            {
                EnableTrigger(false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void EnableTrigger(bool enabled)
    {
        foreach (SphereCollider trigger in GetComponentsInChildren<SphereCollider>())
        {
            if (trigger.isTrigger)
            {
                trigger.enabled = enabled;
            }
        }
    }
}
