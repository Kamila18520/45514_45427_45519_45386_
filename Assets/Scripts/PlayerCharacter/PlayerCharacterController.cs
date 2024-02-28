using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed;

    PlayerInputActions.PlayerMovementBaseActions playerMovementInput;


    //
    //[Header("Movement Settings")]
    //[SerializeField] private float velocity = 5;
    //[SerializeField] private float sprintModificator = 3;
    //[SerializeField] private float staminaUse = 0.5f;
    //[SerializeField] private LayerMask layerMask;
    //
    //private const float fallJumpModif = 30;
    //
    //private float yMovement = -9.81f;
    //
    private void Awake()
    {

        playerMovementInput = GetComponent<PlayerInputController>().Input.PlayerMovementBase;
        //  characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //  var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var camera = Camera.main;
        var moveInput = playerMovementInput.MoveDirection.ReadValue<Vector2>();
        var input = new Vector3(moveInput.x, moveInput.y, 0f);
        input = camera.transform.TransformDirection(input);


        //characterController.Move(input * speed * Time.deltaTime);

        characterController.SimpleMove(input * speed);
        var lookDirection = playerMovementInput.LookDirection.ReadValue<Vector2>();
        if (lookDirection != Vector2.zero)
            transform.forward = new Vector3(lookDirection.x, 0f, lookDirection.y);










        //    var ray = camera.ScreenPointToRay(Input.mousePosition);
        //     var plane = new Plane(transform.up, transform.position);

        //     float distance;
        //    if(plane.Raycast(ray, out distance)) 
        //     {
        //         Vector3 hitPoint = ray.GetPoint(distance);
        //         transform.forward = hitPoint - transform.position;

        //      }


        //   var movementValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //
        //   movementValue *= velocity;
        //   movementValue *= Time.deltaTime;
        //
        //   characterController.Move(new Vector3(movementValue.x, yMovement * Time.deltaTime, movementValue.y));
        //   if (characterController.velocity.sqrMagnitude > 0.1)
        //       transform.forward = new Vector3(movementValue.x, 0f, movementValue.y);
        //
        //   if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        //       yMovement = 10f;
        //
        //   yMovement = Mathf.Max(-9.81f, yMovement - Time.deltaTime * fallJumpModif);
    }

}
