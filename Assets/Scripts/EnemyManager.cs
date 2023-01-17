using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyBehavior> enemies;

    [SerializeField]
    GameObject player;

    EnemyBehavior attackingEnemy = null;

    private float attackRadius = 10f;
    private float backAttackRadius;
    private float idleRadius = 40f;
    private float frontIdleRadius;
    private float meleeRange = 2f;

    bool canNewAttackerBeSet;
    private float lastAttackTime;
    private float attackWaitTime = 5f;

    private void Start()
    {
        backAttackRadius = attackRadius + 3f;
        frontIdleRadius = idleRadius - 5f;
        canNewAttackerBeSet = true;
    }

    void Update()
    {
        //update states
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
                enemies[i].UpdateEnemyState();
        }

        if (!canNewAttackerBeSet)
        {
            float timeSinceLastAttack = Time.time - lastAttackTime;

            // Check if enough time has passed since the last attack
            if (timeSinceLastAttack >= attackWaitTime)
            {
                canNewAttackerBeSet = true;
                // Update the last attack time
                lastAttackTime = Time.time;
            }
        }   

        AttackingRadiusEnemies();
    }

    void AttackingRadiusEnemies()
    {
        //enemies that are in range to attack
        var enemiesInAttackRadius = GetEnemiesInAbleToAttackRadius();

        //get a random enemy to attack
        int randomIdx = Random.Range(0, enemiesInAttackRadius.Count);

        for (int i = 0; i < enemiesInAttackRadius.Count; ++i)
        {
            if (randomIdx == i && attackingEnemy == null && enemiesInAttackRadius[i].canAttack && canNewAttackerBeSet)
            {
                //set attacking enemy
                attackingEnemy = enemiesInAttackRadius[i];
                canNewAttackerBeSet = false;
            }

            if (enemiesInAttackRadius[i].canAttack && attackingEnemy != enemiesInAttackRadius[i])
            {
                int randomBehavior = Random.Range(0, 2);
                //get the non attacking enemies to circulate the player
                if(randomBehavior == 0)
                    enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
                else
                    enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.idle);
            }
            
            if(!enemiesInAttackRadius[i].canAttack)
            {
                enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.walkingBack);
            }
        }

        if (attackingEnemy != null)
        {
            //the attacking enemy should attack the player
            attackingEnemy.ChangeState(EnemyBehavior.EnemyState.attacking);

            //if the attacking enemy attacked, set it to null
            if (Vector3.Distance(player.transform.position, attackingEnemy.transform.position) < meleeRange)
            {
                attackingEnemy.canAttack = false;
                attackingEnemy = null;
            }
        }
    }

    List<EnemyBehavior> GetEnemiesInAbleToAttackRadius()
    {
        List<EnemyBehavior> attackRadiusEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Vector3.Distance(enemies[i].transform.position, player.transform.position) <= attackRadius)
            {
                attackRadiusEnemies.Add(enemies[i]);
            }
        }

        return attackRadiusEnemies;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canNewAttackerBeSet = true;
    }
}