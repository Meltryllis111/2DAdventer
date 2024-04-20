using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

[RequireComponent(typeof(AudioDefination))]
public class Sign : MonoBehaviour
{
    private Animator anim;
    private PlayerInputControl pin;
    public Transform player;
    public GameObject signSprite;
    private bool canPress;
    private Iinteractable targetItem;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        pin = new PlayerInputControl();
        pin.Enable();
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        pin.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;

            switch (d.device)
            {
                case Keyboard:
                    anim.Play("Keyboard");
                    break;
                case XInputController:
                    anim.Play("Xbox");
                    break;
            }
        }
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = player.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>().PlayAudio();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<Iinteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
