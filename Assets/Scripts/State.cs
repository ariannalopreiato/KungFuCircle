using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void OnLeaveState(GameObject currentEnemy);
    public abstract void UpdateState(GameObject currentEnemy);

    public string stateString;

    public bool IsTheSameState(State state)
    {
        if (stateString == state.stateString)
            return true;

        return false;
    }
}

public class IdleState : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {
        
    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().Idle();
        stateString = EnemyBehavior.EnemyState.idle.ToString();
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
        stateString = EnemyBehavior.EnemyState.movingToTarget.ToString();
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
        stateString = EnemyBehavior.EnemyState.moveInCircles.ToString();
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
        stateString = EnemyBehavior.EnemyState.walkingBack.ToString();
    }
}

public class Attack : State
{
    public override void OnLeaveState(GameObject currentEnemy)
    {

    }

    public override void UpdateState(GameObject currentEnemy)
    {
        currentEnemy.GetComponent<EnemyBehavior>().Attack();
        stateString = EnemyBehavior.EnemyState.attacking.ToString();
    }
}