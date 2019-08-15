using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerInteractor interactor;
    public Image progressL, progressR;
    public RectTransform quitDialogue;
    private const string quitButton = "Quit";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp(quitButton))
        {
            if (quitDialogue)
                quitDialogue.gameObject.SetActive(!quitDialogue.gameObject.activeSelf);
            Application.Quit();
        }
    }

    private void OnGUI()
    {
        if (interactor)
        {
            FillImage(interactor.interactLeft, progressL);
            FillImage(interactor.interactRight, progressR);
        }
    }

    private void FillImage(PlayerInteractor.ShortLongPressStruct pressStruct, Image image)
    {
        if(!image)
        {
            return;
        }

        if (pressStruct.isHolding && !pressStruct.hasLongPressed)
        {
            image.fillAmount = (Time.time - pressStruct.pressTime) / interactor.longPressDuration;
        }
        else
        {
            image.fillAmount = 0;
        }
    }
}
