using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstep_Sound;

    [SerializeField]
    private AudioClip[] footstep_clip;

    private CharacterController character_Controller;

    [HideInInspector]
    public float volume_Min, volume_Max;

    private float accumulated_Distance;

    [HideInInspector]
    public float step_Distance;


    void Awake()
    {
        footstep_Sound = GetComponent<AudioSource>();
        character_Controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstepSound();
    }

    void CheckToPlayFootstepSound()
    {
        //if character is not on the ground
        if(!character_Controller.isGrounded)
        {
            return;                         //by executing this line of code, we will directly exit the function
        }
        if(character_Controller.velocity.sqrMagnitude > 0)
        {
            //accumulated distance is the value how far we can go E.G :- Make a step or sprintor move while crouching
            //until we play footstep sound
            accumulated_Distance += Time.deltaTime;
            if(accumulated_Distance > step_Distance)
            {
                footstep_Sound.volume = Random.Range(volume_Min, volume_Max);
                footstep_Sound.clip = footstep_clip[Random.Range(0, footstep_clip.Length)];
                footstep_Sound.Play();
                accumulated_Distance = 0f;
            }
        }
        else
        {

        }

    }
}