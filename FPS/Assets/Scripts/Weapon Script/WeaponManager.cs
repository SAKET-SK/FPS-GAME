using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]          // is editable in the inspector panel
    private WeaponsHandler[] weapons;

    private int current_Weapon_Index;

    // Start is called before the first frame update
    void Start()
    {
        current_Weapon_Index = 0;
        weapons[current_Weapon_Index].gameObject.SetActive(true);   // using gameobject because for getting access as the weapon handler is a script
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))     //basically the keys on the alpha numeric keyboard i.e on right side number keyboard
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnSelectedWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnSelectedWeapon(5);
        }
    }
    void TurnOnSelectedWeapon(int weaponIndex)
    {
        if(current_Weapon_Index == weaponIndex)  // to avoid the animation of re-drawing out the weapon
        {
            return;
        }      // exit the function and no below lines will be executed for animation
        //turn off the selcted weapon
        weapons[current_Weapon_Index].gameObject.SetActive(false);
        //turn on the selcted weapon
        weapons[weaponIndex].gameObject.SetActive(true);
        //store the current selected weapon index
        current_Weapon_Index = weaponIndex;
    }
    public WeaponsHandler GetCurrentSelectedWeapon()
    {
        return weapons[current_Weapon_Index];
    }
}
