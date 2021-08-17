using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;
    public float fireRate = 15f;     // for assault rifle
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;   // to check whether we zommed in or not
    private Camera mainCam;
    private GameObject crosshair;
    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;
    [SerializeField]
    private Transform arrow_Bow_StartPosition;
    void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        //Just getting Look Child child, Fp Camera and finding the Animator component of Fp Camera
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        mainCam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }
    void WeaponShoot()
    {
        //For Assault Rifle
        if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire) //Mouse button 0 is left mouse button
            {
                nextTimeToFire = Time.time + 1f / fireRate;    //will make us fire every 0.15 sec of fire as fireRate = 15f
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        //For every other regular weapon i.e single fire weapon
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                //handle axe
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();     // As axe does'nt fire, it only requires animation
                }
                //handle shoot (Revolver and Shotgun)
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    //if we have an arrow or spear instead of bullet
                    if(is_Aiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            //throw arrow
                            ThrowArrowOrSpear(true);
                        }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            //throw spear
                            ThrowArrowOrSpear(false);
                        }
                    }

                }
            }
        }
    }
    void ZoomInAndOut()
    {
        //for aiming with out camera on the weapon
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if(Input.GetMouseButtonDown(1))  // for press and hold right mouse button
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);    // we don't need crossair while aiming down the sight!!
            }
            if (Input.GetMouseButtonUp(1))  //release right mouse button
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);    // we need crossair while not aiming down the sight!!
            }
        }// <- if we need to aim the weapon ourselves
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM) //i.e arrow or spear
        {
            if(Input.GetMouseButtonDown(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        }
    }
    void ThrowArrowOrSpear(bool throwArrow)  //if true then throw
    {
        if(throwArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);   //creates a copy of the prefab
            arrow.transform.position = arrow_Bow_StartPosition.position;
            arrow.GetComponent<ArrowBowScripts>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spear_Prefab);   //creates a copy of the prefab
            spear.transform.position = arrow_Bow_StartPosition.position;
            spear.GetComponent<ArrowBowScripts>().Launch(mainCam);
        }
    }
    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))  //creating the infinite line for bullet to travel in forward direction
        {
            print("We Hit : " + hit.transform.gameObject.name);
            if(hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
}
