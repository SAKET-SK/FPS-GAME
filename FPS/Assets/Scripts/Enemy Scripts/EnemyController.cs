using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;     //because we are using our nav mesh agent (For walakable distance calculation)

public enum EnemyState   //enemy should chase and attack the player
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f;    //how far enemny needs to be away from player before it starts chasing player; if less than given value  enemy will start chasing the player.
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;
    private void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy_State = EnemyState.PATROL;
        patrol_Timer = patrol_For_This_Time;
        attack_Timer = wait_Before_Attack;   //when enemy gets to player, attack him
        current_Chase_Distance = chase_Distance;   //memorise the value of chase distance so we could put it back
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }
    void Patrol()
    {
        navAgent.isStopped = false;     //tell navAgent that he can move i.e enable its movement
        navAgent.speed = walk_Speed;
        patrol_Timer += Time.deltaTime;
        if(patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }
        if(navAgent.velocity.sqrMagnitude > 0) //Meaning the enemy is moving
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }
        //test the distance between player and enemy
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);   //bcoz while chasing, one doesnt walk, they run!!
            enemy_State = EnemyState.CHASE;
            //play the audio that signifies that you have been spotted.
            enemy_Audio.play_ScreamSound();
        }
    }
    void Chase()
    {
        //enebale the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;
        navAgent.SetDestination(target.position); //set the player's destination as target so as to chase (run towards) the player for attack.
        if (navAgent.velocity.sqrMagnitude > 0) //Meaning the enemy is moving
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance)  //if distance between enemy and player is less than attack distance
        {
            //stop the animations immidiately
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            if(chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance)  //if player runs away from enemy
        {
            //then stop running after the player
            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;    //reset it to get new patrol destination right away
            patrol_Timer = patrol_For_This_Time;

            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
    }
    void Attack()
    {
        navAgent.velocity = Vector3.zero;    //stop the enemy completely
        navAgent.isStopped = true;
        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();
            attack_Timer = 0f;     //resetting or else the enemy will atatck infinite number of times
            enemy_Audio.play_AttackSound();
            //play attack sound
        }
        //if player runs away
        if (Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }
    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;  //if the movement point is outside of the bounds the store new position in this variable
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1); //-1 means include all the game layers
        navAgent.SetDestination(navHit.position);
    }
    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)     //turn off if it did not turn on by its own
        {
            attack_Point.SetActive(false);
        }
    }
    public EnemyState Enemy_State
    {
        get;   set;
    }
}
