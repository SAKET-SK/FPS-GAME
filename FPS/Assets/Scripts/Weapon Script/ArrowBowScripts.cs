using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScripts : MonoBehaviour
{
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactivate_Timer = 3f;
    public float damage = 15f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject",deactivate_Timer);
    }
    public void Launch(Camera mainCam) //Lauching spear or arrow
    {
        myBody.velocity = mainCam.transform.forward * speed;      //going to happen in playerattack.cs
        transform.LookAt(transform.position + myBody.velocity);      //look at where it goes
    }
    void DeactivateGameObject()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider target)   //Determine if we hit the enemny or not
    {
        
    }
}
