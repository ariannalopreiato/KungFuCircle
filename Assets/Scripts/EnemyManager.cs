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
    private float idleRadius = 30f;
    private float frontIdleRadius;
    private float attackRange = 2f;

    private void Start()
    {
        backAttackRadius = attackRadius + 3f;
        frontIdleRadius = idleRadius - 5f;
    }

    void Update()
    {
        //update states
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
                enemies[i].UpdateEnemyState();
        }

        AttackingRadiusEnemies();
       // WalkingBackAfterAttack();
    }

    void AttackingRadiusEnemies()
    {
        //enemies that are in range to attack
        var enemiesInAttackRadius = GetEnemiesInAbleToAttackRadius();

        //get a random enemy to attack
        int randomIdx = Random.Range(0, enemiesInAttackRadius.Count);

        for (int a = 0; a < enemiesInAttackRadius.Count; ++a)
        {
            if (randomIdx == a && attackingEnemy == null && enemiesInAttackRadius[a].canAttack)
            {
                //set attacking enemy
                attackingEnemy = enemiesInAttackRadius[a];
            }

            if (enemiesInAttackRadius[a].canAttack && attackingEnemy != enemiesInAttackRadius[a])
            {
                int randomBehavior = Random.Range(0, 2);
                //get the non attacking enemies to circulate the player
                if(randomBehavior == 0)
                    enemiesInAttackRadius[a].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
                else
                    enemiesInAttackRadius[a].ChangeState(EnemyBehavior.EnemyState.idle);
            }
            
            if(!enemiesInAttackRadius[a].canAttack)
            {
                enemiesInAttackRadius[a].ChangeState(EnemyBehavior.EnemyState.walkingBack);
            }
        }

        if (attackingEnemy != null)
        {
            //the attacking enemy should attack the player
            attackingEnemy.ChangeState(EnemyBehavior.EnemyState.attacking);

            //if the attacking enemy attacked, set it to null
            if (Vector3.Distance(player.transform.position, attackingEnemy.transform.position) < attackRange)
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

    List<EnemyBehavior> GetEnemiesInAttackRadius()
    {
        List<EnemyBehavior> attackRadiusEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Vector3.Distance(enemies[i].transform.position, player.transform.position) <= attackRange)
                attackRadiusEnemies.Add(enemies[i]);
        }

        return attackRadiusEnemies;
    }
}