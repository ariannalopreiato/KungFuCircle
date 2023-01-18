using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemies : EnemyBehavior
{
    protected override void SetCanAttack()
    {
        //melee enemies can attack when they are within the melee attack radius,
        //if they are not already attacking and if they are in the camera viewport
        if (Vector3.Distance(gameObject.transform.position, point.position) < attackRadiusMelee
          && !canAttack && !isOutsideOfCameraView)
        {
            canAttack = true;
        }
    }
    protected override void SetIsFar()
    {
        if (Vector3.Distance(gameObject.transform.position, point.position) > midRadius)
            isFar = true;
    }

    public override void WalkToPlayer()
    {

        //move towards the player
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
        if (isFar)
        {
            //if the enemy has entered the mid radius, it's not considered far anymore
            if (Vector3.Distance(transform.position, point.position) <= midRadius)
                isFar = false;
        }
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
        //move towards the target
        transform.position = Vector3.MoveTowards(transform.position, point.position, runningSpeed * Time.deltaTime);

        //when it reaches the position attack
        if (Vector3.Distance(transform.position, point.position) < 2f)
        {
            print("melee attack");
            //when it attacks it can't attack anymore (already performed the attack)
            canAttack = false;
        }
    }

    public override void WalkBack()
    {
        //walk back to the starting position
        transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos) < 1f)
        {
            print("eh");
            canAttack = true;
        }
    }

    public override void Idle()
    {
        transform.position = transform.position;
    }
}
