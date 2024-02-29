using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHolder : MonoBehaviour
{
    //[SerializeField] List<GameObject> weapons;
    [SerializeField] Queue<WeaponController> weapons = new();
    WeaponController currentWeapon;

    [Header("InputActions")]
    [SerializeField] InputActionReference shootReference;
    [SerializeField] InputActionReference selectNextWeaponReference;

    InputAction shootAction;
    InputAction selectNextWeaponAction;

    private void Awake()
    {
        var playerInput = GetComponentInParent<PlayerInput>();
        shootAction = playerInput.actions[shootReference.action.name];
        selectNextWeaponAction = playerInput.actions[selectNextWeaponReference.action.name];
        shootAction.actionMap.Enable();
        
        foreach (Transform child in transform)
        {
            if (child == transform) continue;

            child.gameObject.SetActive(false);
            weapons.Enqueue(child.gameObject.GetComponent<WeaponController>());

        }

        SelectNextWeapon();
    }


    private void SelectNextWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
            weapons.Enqueue(currentWeapon);
        }

        currentWeapon = weapons.Dequeue();
        currentWeapon.gameObject.SetActive(true);

    }

      private void Update()
       {
          if(selectNextWeaponAction.WasPerformedThisFrame())
              SelectNextWeapon();

           if (shootAction.WasPerformedThisFrame())
               currentWeapon.Shoot();

    }
}
