using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim  //will determine how we are going to aim our weapon NONE for AXE, SELF_AIM for GUNS and AIM for BOWS/SPEAR
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType //Multiple is only for Assualt Rifle in this case
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType  //Different types of Projectiles 
{
    BULLET,
    ARROW,
    SPEAR,
    NONE
}
public class WeaponsHandler : MonoBehaviour
{
    private Animator anim;
    public WeaponAim weapon_Aim;     // to determine if the weapon can aim on its own or not

    [SerializeField]   //Not all gonna have it!
    private GameObject muzzleFlash;

    [SerializeField]
    private AudioSource shoot_Sound, reload_Sound;

    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject attack_Point; // To detremine if we hit the enemy or not with melee weapons

    void Awake()
    {
        anim = GetComponent<Animator>();

    }
    public void ShootAnimation()
    {
        anim.SetTrigger(AnimationTags.SHOOT_TRIGGER);   //taken from tagholder.cs
    }
    public void Aim(bool canAim)
    {
        anim.SetBool(AnimationTags.AIM_PARAMETER, canAim);  // can aim -> true or else false
    }
    void Turn_On_Muzzleflash()
    {
        muzzleFlash.SetActive(true);
    }
    void Turn_Off_Muzzleflash()
    {
        muzzleFlash.SetActive(false);
    }
    void Play_ShootSound()
    {
        shoot_Sound.Play();
    }
    void Play_ReloadSound()
    {
        reload_Sound.Play();
    }
    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if(attack_Point.activeInHierarchy)     //turn off if it did not turn on by its own
        {
            attack_Point.SetActive(false);
        }
    }
}



