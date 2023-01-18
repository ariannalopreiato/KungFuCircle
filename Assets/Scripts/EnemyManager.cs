using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyBehavior> enemies;

    [SerializeField]
    GameObject player;

    [SerializeField]
    int maxEnemyAmtInAttackRange = 4;

    EnemyBehavior attackingEnemy = null;

    bool canNewAttackerBeSet;
    private float lastAttackTime;
    private float attackWaitTime = 2f;

    private void Start()
    {
        canNewAttackerBeSet = true;
    }

    void Update()
    {
        //update states
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].isCollidingWithOtherEnemies)
            {
                int randomBehavior = Random.Range(0, 1);
                if(randomBehavior == 0)
                    enemies[i].ChangeState(EnemyBehavior.EnemyState.walkingBack);
                else
                    enemies[i].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
            }

            //update the enemies' state
            enemies[i].UpdateEnemyState();
        }

        //timer for when to set a new attacker
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

        FarEnemies();
        EnemiesAbleToAttack();
        //RemainingEnemies();
    }

    void EnemiesAbleToAttack()
    {
        //enemies that are in range to attack
        var enemiesInAttackRadius = GetEnemiesInAbleToAttackRadius();

        //get a random enemy to attack
        int randomIdx = Random.Range(0, enemiesInAttackRadius.Count);

        //loop through all the enemies that can attack
        for (int i = 0; i < enemiesInAttackRadius.Count; ++i)
        {
            if (randomIdx == i && canNewAttackerBeSet)
            {
                //set attacking enemy
                attackingEnemy = enemiesInAttackRadius[i];
                canNewAttackerBeSet = false;
            }

            //if the current enemy is not the attacking one
            if (attackingEnemy != enemiesInAttackRadius[i])
            {
                int randomBehavior = Random.Range(0, 2);
                //get the non attacking enemies to circulate the player
                if (randomBehavior == 0)
                    enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
                else
                    enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.idle);
            }

            if(i > maxEnemyAmtInAttackRange)
            {
                enemiesInAttackRadius[i].ChangeState(EnemyBehavior.EnemyState.walkingBack);
                enemiesInAttackRadius.RemoveAt(i);
            }
        }

        //if the attacking enemy is still in action
        if (attackingEnemy != null)
        {
            //the attacking enemy should attack the player
            attackingEnemy.ChangeState(EnemyBehavior.EnemyState.attacking);
            if(!attackingEnemy.canAttack)
            {
                attackingEnemy.ChangeState(EnemyBehavior.EnemyState.walkingBack);
                attackingEnemy = null;
            }
        }
    }

    void FarEnemies()
    {
        var farEnemies = GetFarEnemies();

        for (int i = 0; i < farEnemies.Count; ++i)
            farEnemies[i].ChangeState(EnemyBehavior.EnemyState.movingToTarget);
    }

    void RemainingEnemies()
    {
        var remainingEnemies = GetRemainingEnemies();

        for (int i = 0; i < remainingEnemies.Count; ++i)
            remainingEnemies[i].ChangeState(EnemyBehavior.EnemyState.moveInCircles);
    }

    List<EnemyBehavior> GetEnemiesInAbleToAttackRadius()
    {
        List<EnemyBehavior> attackRadiusEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].canAttack)
            {
                attackRadiusEnemies.Add(enemies[i]);
            }
        }

        return attackRadiusEnemies;
    }

    List<EnemyBehavior> GetFarEnemies()
    {
        List<EnemyBehavior> farEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].isFar)
            {
                farEnemies.Add(enemies[i]);
            }
        }

        return farEnemies;
    }

    List<EnemyBehavior> GetRemainingEnemies()
    {
        List<EnemyBehavior> remainingEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            //if it's not far and it can't attack
            if (!enemies[i].isFar && !enemies[i].canAttack)
            {
                remainingEnemies.Add(enemies[i]);
            }
        }

        return remainingEnemies;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canNewAttackerBeSet = true;
    }
}