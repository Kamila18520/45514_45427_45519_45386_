using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHolder : MonoBehaviour
{
    //[SerializeField] List<GameObject> weapons;
    [SerializeField] Queue<WeaponController> weapons = new();
    WeaponController currentWeapon;

    PlayerInputActions.PlayerShootingActions playerShootingActions;

    private void Awake()
    {
        playerShootingActions = GetComponentInParent<PlayerInputController>().Input.PlayerShooting;

        playerShootingActions.Shoot.performed += OnShoot;
        playerShootingActions.NextWeapon.performed += OnNextWeapon;

        foreach (Transform child in transform)
        {
            if (child == transform) continue;

            child.gameObject.SetActive(false);
            weapons.Enqueue(child.gameObject.GetComponent<WeaponController>());

        }

        SelectNextWeapon();
    }

    private void OnDestroy()
    {
        playerShootingActions.Shoot.performed -= OnShoot;
        playerShootingActions.NextWeapon.performed -= OnNextWeapon;
    }


    private void OnNextWeapon(InputAction.CallbackContext ctx)
    {
        SelectNextWeapon();
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        currentWeapon.Shoot();
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

    //  private void Update()
    //   {
    //      if(Input.GetMouseButtonDown(1))
    //          SelectNextWeapon();

    //       if (Input.GetMouseButton(0))
    //           currentWeapon.Shoot();

    //}
}
