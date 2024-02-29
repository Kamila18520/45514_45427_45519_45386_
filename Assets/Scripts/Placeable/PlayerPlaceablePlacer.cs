using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerPlaceablePlacer : MonoBehaviour
{
    [SerializeField] float placeDistance = 1f;
    [SerializeField] PlaceableController prefabToPlace;


    [SerializeField] GameObject WeaponsController;

    [Header("Ghost Settings")]
    [SerializeField] Transform placeableGhostTransform;


    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color incorrectColor = Color.red;


    Material placeableGhostMaterial;
    MeshFilter placeableGhostMeshFilter;
    MeshRenderer placeableGhostMeshRenderer;
    bool isMaterialCorrectColor;

    [Header("Place Settings")]
    [SerializeField] LayerMask layerMask;

    [Header("PlayerEnergy")]
    [SerializeField] SOFloatVariable energyVariable;

    enum State { Idle, Placing }
    State currentState = State.Idle;

    readonly Collider[] overlapBoxResult = new Collider[1];

    [Header("InputActions")]
    [SerializeField] InputActionReference selectBuildModeReference;
    [SerializeField] InputActionReference selectWeaponModeReference;
    [SerializeField] InputActionReference buildReference;

    InputAction selectBuildModeAction;
    InputAction selectWeaponModeAction;
    InputAction buildAction;



    Color transparentIncorrectColor;
    Color transparentCorrectColor;
    //  Color transparentLowEnergytColor;




    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        selectBuildModeAction = playerInput.actions[selectBuildModeReference.action.name];
        selectWeaponModeAction = playerInput.actions[selectWeaponModeReference.action.name];
        buildAction = playerInput.actions[buildReference.action.name];


        selectBuildModeAction.performed += SelectPlacingMode;
        selectWeaponModeAction.performed += SelectWeaponMode;

        transparentIncorrectColor = new Color(incorrectColor.r, incorrectColor.g, incorrectColor.b, 0.5f);
        transparentCorrectColor = new Color(correctColor.r, correctColor.g, correctColor.b, 0.5f);


        placeableGhostMeshFilter = placeableGhostTransform.GetComponent<MeshFilter>();
        placeableGhostMeshRenderer = placeableGhostTransform.GetComponent<MeshRenderer>();

        placeableGhostMaterial = placeableGhostMeshRenderer.material;
        placeableGhostMeshRenderer.enabled = currentState == State.Placing ? true : false;


        SetPlaceableItem(prefabToPlace);




    }


    void Update()
    {
        if (currentState == State.Placing)
            PlacingUpdate();

    }



    private void SelectPlacingMode(InputAction.CallbackContext ctx)
    {
        placeableGhostMeshRenderer.enabled = true;
        selectWeaponModeAction.actionMap.Enable();
        selectBuildModeAction.actionMap.Disable();
        WeaponsController.SetActive(false);

        placeableGhostMeshFilter = placeableGhostTransform.GetComponent<MeshFilter>();
        placeableGhostMeshRenderer = placeableGhostTransform.GetComponent<MeshRenderer>();


        // set force color to incorrect
        isMaterialCorrectColor = true;
        SetGhostPositionAndColor(false);


        currentState = State.Placing;
    }

    private void SelectWeaponMode(InputAction.CallbackContext obj)
    {
        placeableGhostMeshRenderer.enabled = false;
        selectWeaponModeAction.actionMap.Disable();
        selectBuildModeAction.actionMap.Enable();
        WeaponsController.SetActive(true);
        currentState = State.Idle;

    }



    private void SetPlaceableItem(PlaceableController placeable)
    {
        placeableGhostMeshFilter.sharedMesh = prefabToPlace.PlaceableMesh;
        //prefabsToPlace = placeable; 

    }


    private void PlacingUpdate()
    {
        var energyCost = prefabToPlace.PlaceableCost;
        var isAllowToSpawn = energyVariable.Variable.Value >= energyCost;
        var placePosition = Vector3Int.FloorToInt(transform.position + Vector3.one * 0.5f + transform.forward * placeDistance);

        if (isAllowToSpawn)
        {
            var boxCastPosition = placePosition + Vector3.up * 0.5f;
            var collidersCount = Physics.OverlapBoxNonAlloc(boxCastPosition, Vector3.one * 0.49f,
                overlapBoxResult, Quaternion.identity, layerMask, QueryTriggerInteraction.Ignore);
            isAllowToSpawn = collidersCount == 0;
        }

        SetGhostPositionAndColor(isAllowToSpawn, placePosition);


        if (isAllowToSpawn && buildAction.WasPerformedThisFrame())
        {
            energyVariable.Variable.Value -= energyCost;
            Instantiate(prefabToPlace, placePosition, Quaternion.identity);
        }

    }

    private void SetGhostPositionAndColor(bool isActive, Vector3 placePosition = default)
    {
        if (isMaterialCorrectColor != isActive)
        {
            placeableGhostMaterial.color = isActive ? transparentCorrectColor : transparentIncorrectColor;
            isMaterialCorrectColor = isActive;
        }
        placeableGhostMaterial.color = isActive ? transparentCorrectColor : transparentIncorrectColor;
        placeableGhostTransform.SetPositionAndRotation(placePosition, Quaternion.identity);




    }

}