using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemies : EnemyBehavior
{
    public override void WalkToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }

    public override void Circulate()
    {
        //Calculate the position in which you are supposed to be going 
        Vector3 targetPosition = GetDirection();
        //Move to the calculated position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public override void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, point.position, runningSpeed * Time.deltaTime);
    }

    public override void WalkBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos) < 1f)
            canAttack = true;
    }

    public override void Idle()
    {
        transform.position = transform.position;
    }
}
