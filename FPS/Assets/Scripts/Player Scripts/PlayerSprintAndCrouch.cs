﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprint_speed = 10f;
    public float crouch_speed = 2f;
    public float move_speed = 5f;

    private Transform look_Root;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;

    private bool is_Crouching;

    private PlayerFootsteps player_Footsteps;
    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;

    private float walk_step_Distance = 0.4f;
    private float sprint_step_Distance = 0.25f;
    private float crouch_step_Distance = 0.5f;

    private PlayerStats player_Stats;
    private float sprint_Value = 100f;
    public float sprint_Threshold = 10f;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        look_Root = transform.GetChild(0);

        player_Footsteps = GetComponentInChildren<PlayerFootsteps>();

        player_Stats = GetComponent<PlayerStats>();
    }
    void Start()
    {
        player_Footsteps.volume_Min = walk_Volume_Min;
        player_Footsteps.volume_Max = walk_Volume_Max;
        player_Footsteps.step_Distance = walk_step_Distance;
    }
    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if(sprint_Value > 0)   //if we have stamina we can sprint
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
            {
                playerMovement.speed = sprint_speed;

                player_Footsteps.step_Distance = sprint_step_Distance;
                player_Footsteps.volume_Min = sprint_Volume;
                player_Footsteps.volume_Max = sprint_Volume;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = move_speed;

            player_Footsteps.step_Distance = walk_step_Distance;
            player_Footsteps.volume_Min = walk_Volume_Min;
            player_Footsteps.volume_Max = walk_Volume_Max;
            
        }
        if(Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            sprint_Value = sprint_Threshold * Time.deltaTime;
            if(sprint_Value <= 0f)
            {
                sprint_Value = 0f;
                playerMovement.speed = move_speed;
                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;
            }
            player_Stats.Display_StaminaStats(sprint_Value);
        }
        else
        {
            if(sprint_Value != 100f)
            {
                sprint_Value += (sprint_Threshold / 2f) * Time.deltaTime;
                player_Stats.Display_StaminaStats(sprint_Value);
                if(sprint_Value > 100f)
                {
                    sprint_Value = 100f;
                }
            }
        }
    }

    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(is_Crouching) //if crouching -> stand up
            {
                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_speed;

                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;

                is_Crouching = false;
            }
            else  //if not -> crouch
            {
                look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
                playerMovement.speed = crouch_speed;

                player_Footsteps.step_Distance = crouch_step_Distance;
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;

                is_Crouching = true;
            }
        }
    }
}
