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

    [SerializeField]
    Camera camera;

    private float attackRadius = 10f;
    private float backAttackRadius;
    private float idleRadius = 30f;
    private float frontIdleRadius;
    private float meleeRange = 2f;

    bool canNewAttackerBeSet;
    private float lastAttackTime;
    private float attackWaitTime = 5f;

    private void Start()
    {
        frontIdleRadius = attackRadius + 5f;
        backAttackRadius = attackRadius + 3f;
        canNewAttackerBeSet = true;
    }

    void Update()
    {
        //update states
        for (int i = 0; i < enemies.Count; ++i)
        {
            //check if the enemies are within the camera viewport
            Vector3 viewportPosition = camera.WorldToViewportPoint(enemies[i].transform.position);
            if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1)
                enemies[i].GetComponent<EnemyBehavior>().isOutsideOfCameraView = true;
            else
                enemies[i].GetComponent<EnemyBehavior>().isOutsideOfCameraView = false;

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

        IdleRadiusEnemies();
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
            if (randomIdx == i && attackingEnemy == null && enemiesInAttackRadius[i].canAttack 
                    && canNewAttackerBeSet && !enemiesInAttackRadius[i].isOutsideOfCameraView)
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

    void IdleRadiusEnemies()
    {
        var enemiesInIdleRadius = GetEnemiesInIdleRadius();

        for (int i = 0; i < enemiesInIdleRadius.Count; ++i)
            enemiesInIdleRadius[i].ChangeState(EnemyBehavior.EnemyState.movingToTarget);

        for (int j = 0; j < enemies.Count; ++j)
        {
            if (Vector3.Distance(enemies[j].transform.position, player.transform.position) <= frontIdleRadius &&
                Vector3.Distance(enemies[j].transform.position, player.transform.position) > attackRadius)
            {
                enemies[j].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
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

    List<EnemyBehavior> GetEnemiesInIdleRadius()
    {
        List<EnemyBehavior> idleRadiusEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Vector3.Distance(enemies[i].transform.position, player.transform.position) <= idleRadius &&
                Vector3.Distance(enemies[i].transform.position, player.transform.position) > frontIdleRadius)
            {
                idleRadiusEnemies.Add(enemies[i]);
            }
        }

        return idleRadiusEnemies;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canNewAttackerBeSet = true;
    }
}