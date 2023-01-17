using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void OnLeaveState(GameObject currentEnemy);
    public abstract void UpdateState(GameObject currentEnemy);
}

public class IdleState : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {
        
    }

    public override void UpdateState(GameObject currentEnemy)
    {

    }
}

public class MoveToPlayer : State
{ 
    public override void OnLeaveState(GameObject currentEnemy)
    {

    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().WalkToPlayer();
    }
}

public class Circulate : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {

    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().Circulate();
    }
}

public class WalkBack : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {

    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().WalkBack();
    }
}

public class Attack : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().WalkBack();
    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().Attack();
    }
}