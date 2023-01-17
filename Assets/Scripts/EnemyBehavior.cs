using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    public Transform point;

    public float speed = 2f;
    private bool rotateOpposite = false;
    private Vector3 startPos;
    public enum EnemyState
    {
        idle, movingToPlayer, moveInCircles, walkingBack, attacking
    };

    IdleState idleState = new IdleState();
    MoveToPlayer moveToPlayerState = new MoveToPlayer();
    Circulate circulateState = new Circulate();
    WalkBack walkBackState = new WalkBack();
    Attack attackState = new Attack();

    State currentState;

    public void ChangeState(EnemyState newState)
    {
        // if (currentState != null)
        // currentState.OnLeaveState()

        switch (newState)
        {
            case EnemyState.idle:
                currentState = idleState;
                break;
            case EnemyState.movingToPlayer:
                currentState = moveToPlayerState;
                break;
            case EnemyState.moveInCircles:
                currentState = circulateState;
                break;
            case EnemyState.walkingBack:
                currentState = walkBackState;
                break;
            case EnemyState.attacking:
                currentState = attackState;
                break;
        }

        // currentState.OnEnterState();
    }

    void Start()
    {
        currentState = idleState;
        startPos = gameObject.transform.position;
        //randomize in which way the enemy it's going to move (clockwise or anticlockwise)
        int randomSign = UnityEngine.Random.Range(0, 2);
        if (randomSign == 0)
            rotateOpposite = true;
        else
            rotateOpposite = false;
    }

    private Vector3 GetDirection()
    {
        if (rotateOpposite)
            return transform.position + (-1) * transform.right * speed * Time.deltaTime;
        else
            return transform.position + transform.right * speed * Time.deltaTime;
    }

    public void UpdateEnemyState()
    {
        currentState.UpdateState(gameObject);
    }

    //==============================================================================
    //STATE BEHAVIORS
    //==============================================================================

    public void WalkToPlayer()
    {
        transform.LookAt(point);
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }

    public void Circulate()
    {
        //Look at the target(the player) -> defines the direction vector in which it's looking
        transform.LookAt(point);
        //Calculate the position in which you are supposed to be going 
        Vector3 targetPosition = GetDirection();
        //Move to the calculated position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Attack()
    {
        transform.LookAt(point);
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }

    public void WalkBack()
    {
        transform.LookAt(point);
        transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
    }

    //    private bool IsWithinForwardRadius(Vector3 currentPos)
    //    {
    //        var distancePlayerEnemy = Vector3.Distance(currentPos, point.position);
    //        if (distancePlayerEnemy <= walkingForwardRadius)
    //            return true;

    //        return false;
    //    }

    //    private bool IsOutOfFurthestRadius(Vector3 currentPos)
    //    {
    //        var distancePlayerEnemy = Vector3.Distance(currentPos, point.position);
    //        if (distancePlayerEnemy >= furthestPoint)
    //            return true;

    //        return false;
    //    }
}