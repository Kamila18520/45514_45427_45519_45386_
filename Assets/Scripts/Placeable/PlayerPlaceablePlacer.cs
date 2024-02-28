using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerPlaceablePlacer : MonoBehaviour
{
    [SerializeField] float placeDistance = 1f;
    // [SerializeField] PlaceableController prefabToPlace;
    [SerializeField] PlaceableController[] prefabsToPlace;


    [Header("Weapons")]
    [SerializeField] GameObject WeaponsController;

    [Header("Ghost Settings")]
    //[SerializeField] Transform placeableGhostTransform;
    [SerializeField] Transform[] placeableGhostTransforms;

    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color incorrectColor = Color.red;
    [SerializeField] Color lowEnergyColor = Color.blue;

    Material placeableGhostMaterial;
    MeshFilter placeableGhostMeshFilter;
    MeshRenderer placeableGhostMeshRenderer;
    bool isMaterialCorrectColor;

    [Header("Place Settings")]
    [SerializeField] LayerMask layerMask;

    [Header("PlayerEnergy")]
    [SerializeField] SOFloatVariable energyVariable;

    //  enum State { Idle, Placing }
    //  State currentState = State.Idle;

    readonly Collider[] overlapBoxResult = new Collider[1];

    Color transparentIncorrectColor;
    Color transparentCorrectColor;
    Color transparentLowEnergytColor;

    private int currentTurret;

    PlayerInputActions.PlayerPlacingActions playerPlacingActions;

    private void Awake()
    {
        playerPlacingActions = GetComponent<PlayerInputController>().Input.PlayerPlacing;



        transparentIncorrectColor = new Color(incorrectColor.r, incorrectColor.g, incorrectColor.b, 0.5f);
        transparentCorrectColor = new Color(correctColor.r, correctColor.g, correctColor.b, 0.5f);
        transparentLowEnergytColor = new Color(lowEnergyColor.r, lowEnergyColor.g, lowEnergyColor.b, 0.5f);

        currentTurret = 0;
        ChooseTurret(currentTurret);

        //---------
        //  placeableGhostMeshFilter = placeableGhostTransform.GetComponent<MeshFilter>();
        //  placeableGhostMeshRenderer = placeableGhostTransform.GetComponent<MeshRenderer>();
        //
        //  SetPlaceableItem(prefabToPlace);
        //
        //  placeableGhostMaterial = placeableGhostMeshRenderer.material;
        //  placeableGhostMeshRenderer.enabled = currentState == State.Placing ? true : false;
        //
        //  // set force color to incorrect
        //  isMaterialCorrectColor = true;
        //  SetGhostPositionAndColor(false);
    }



    private void ChooseTurret(int i)
    {
        placeableGhostMeshFilter = placeableGhostTransforms[i].GetComponent<MeshFilter>();
        placeableGhostMeshRenderer = placeableGhostTransforms[i].GetComponent<MeshRenderer>();

        SetPlaceableItem(prefabsToPlace[i]);

        placeableGhostMaterial = placeableGhostMeshRenderer.material;
        placeableGhostMeshRenderer.enabled = false;

        // set force color to incorrect
        isMaterialCorrectColor = true;
        SetGhostPositionAndColor(false);

        enabled = false;
    }

    private void OnDestroy()
    {

    }


    private void OnEnable()
    {
        placeableGhostMeshRenderer.enabled = true;
    }


    private void OnDisable()
    {
        placeableGhostMeshRenderer.enabled = false;
    }

    private void Update()
    {


        PlacingUpdate();

        if (Input.GetKeyDown(KeyCode.E))
        {
            placeableGhostTransforms[currentTurret].gameObject.SetActive(false);
            currentTurret++;

            if (currentTurret >= prefabsToPlace.Length)
                currentTurret = 0;
            placeableGhostTransforms[currentTurret].gameObject.SetActive(true);

            ChooseTurret(currentTurret);

        }
    }

    private void SetPlaceableItem(PlaceableController placeable)
    {
        placeableGhostMeshFilter.sharedMesh = prefabsToPlace[currentTurret].PlaceableMesh;
        // prefabsToPlace = placeable; 

    }

    //  private void SelectCorrectState()
    //  {
    //      if (Input.GetKeyDown(KeyCode.Tab))
    //      {
    //          switch (currentState)
    //          {
    //              case State.Idle:
    //                  {
    //                      WeaponsController.SetActive(false);
    //                      placeableGhostMeshRenderer.enabled = true;
    //                      currentState = State.Placing;
    //
    //                  }
    //
    //                  return;
    //              case State.Placing:
    //                  {
    //                      WeaponsController.SetActive(true);
    //
    //                      placeableGhostMeshRenderer.enabled = false;
    //
    //                      currentState = State.Idle;
    //                  }
    //
    //                  return;
    //          }
    //
    //      }
    //  }

    public Vector3 DebugCheckPosition;
    public bool DebugIsCheckPositionFree;
    private void PlacingUpdate()
    {
        var energyCost = prefabsToPlace[currentTurret].PlaceableCost;
        var isAllowToSpawn = energyVariable.Variable.Value >= energyCost;
        var placePosition = Vector3Int.FloorToInt(transform.position + transform.forward * placeDistance) + Vector3.up * 0.5f;

        if (isAllowToSpawn)
        {
            var boxCastPosition = placePosition + Vector3.up * 0.5f;
            var collidersCount = Physics.OverlapBoxNonAlloc(boxCastPosition, Vector3.one * 0.49f,
                overlapBoxResult, Quaternion.identity, layerMask);
            isAllowToSpawn = collidersCount == 0;
        }

        SetGhostPositionAndColor(isAllowToSpawn, placePosition);


        if (isAllowToSpawn && playerPlacingActions.Build.WasPerformedThisFrame())
        {
            energyVariable.Variable.Value -= energyCost;
            Instantiate(prefabsToPlace[currentTurret], placePosition, Quaternion.identity);
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

        placeableGhostTransforms[currentTurret].SetPositionAndRotation(placePosition, Quaternion.identity);

    }




}
