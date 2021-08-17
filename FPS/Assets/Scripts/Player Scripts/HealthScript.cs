using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;
    public bool is_Player, is_Boar, is_Cannibal;
    private bool is_Dead;

    private EnemyAudio enemyAudio;

    private PlayerStats player_Stats;
    void Awake()
    {
        if(is_Boar || is_Cannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //insert enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
        if(is_Player)
        {
            //create player stats
            player_Stats = GetComponent<PlayerStats>();
        }
    }
    public void ApplyDamage(float damage)
    {
        if (is_Dead)
        {
            return;     //if we are dead, dont execute the rest of the code
        }
        health -= damage;    //subtract health when damaged
        if (is_Player)
        {
            //display the health UI
            player_Stats.Display_HealthStats(health);
        }
        if (is_Boar || is_Cannibal)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;    //detect damage and chase after player
            }
        }
        if (health <= 0f)
        {
            PlayerDied();
            is_Dead = true;
        }
    }
    void PlayerDied()
    {
        if(is_Cannibal)     //we dont have the dead animation prefab for cannibal, so code it manually like this....
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);   //backward angular fall

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            //play death sound and disbale game object
            StartCoroutine(DeadSound());
            //EnemyManager -> spawn more enemies
            EnemyManager.Instance.EnemyDied(true);
        }
        if(is_Boar)       //we have dead animation for boar, so we do like this....
        {
            navAgent.velocity = Vector3.zero;      //stop the navAgent
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();
            StartCoroutine(DeadSound());
            //EnemyManager -> spawn more enemies
            EnemyManager.Instance.EnemyDied(false);
        }
        if(is_Player)   //if player dies, turn everything off
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);   //Get all enemies which are currently avaliable in game
            for(int i = 0; i <= enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }
            //Call enemy manager to stop spawning of enemies
            EnemyManager.Instance.StopSpawning();
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }
        if(tag == Tags.PLAYER_TAG)     
        {
            Invoke("RestartGame", 3f);      //if player dies, restart the the whole game after 3 sec
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);   //if enemy dies, turn off the GameObject simply!
        }
    }
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.play_DeadSound();
    }
}
