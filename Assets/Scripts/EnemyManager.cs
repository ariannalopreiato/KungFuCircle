using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyBehavior[] enemies;

    void Update()
    {
        for(int i = 0; i < enemies.Length; ++i) 
        {
            enemies[i].SetState(State.moveInCircles);
        }
    }
}
