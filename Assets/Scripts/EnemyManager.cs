using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyBehavior> enemies;

    [SerializeField]
    GameObject player;

    EnemyBehavior attackingEnemy;

    private float attackRadius = 5f;
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
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null )
                enemies[i].UpdateEnemyState();
            //enemies[i].ChangeState(EnemyStateHandler.EnemyState.moveInCircles);
        }
    }

    List<EnemyBehavior> GetEnemiesInAttackRadius()
    {
        List<EnemyBehavior> attackRadiusEnemies = new List<EnemyBehavior>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Vector3.Distance(enemies[i].transform.position, player.transform.position) <= attackRadius)
                attackRadiusEnemies.Add(enemies[i]);
        }

        return attackRadiusEnemies;
    }
}
