using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerInputActions Input { get; private set; }

    PlayerPlaceablePlacer placeablePlacer;


    private void Awake()
    {
        placeablePlacer = GetComponent<PlayerPlaceablePlacer>();

        Input = new PlayerInputActions();
        Input.PlayerMovementBase.Enable();
        Input.PlayerShooting.Enable();


        Input.PlayerMovementBase.SwitchPlacingMode.performed += SwitchPlacingMode;

    }

    private void OnDestroy()
    {

        Input.PlayerMovementBase.SwitchPlacingMode.performed -= SwitchPlacingMode;
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void Dispose()
    {
        Input.Dispose();
    }


    public void SwitchPlacingMode(InputAction.CallbackContext ctx)
    {
        var wasEnable = placeablePlacer.enabled;
        placeablePlacer.enabled = !wasEnable;
        if (wasEnable)
        {
            Input.PlayerPlacing.Disable();
            Input.PlayerShooting.Enable();

        }
        else
        {
            Input.PlayerPlacing.Enable();
            Input.PlayerShooting.Disable();
        }


    }



}
