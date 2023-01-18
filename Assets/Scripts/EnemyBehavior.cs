using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.SceneManagement;

public abstract class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    public Transform point;

    public float speed = 2f;
    public float runningSpeed = 4f;
    protected bool rotateOpposite = false;
    protected Vector3 startPos;

    public bool isOutsideOfCameraView = false;
    public bool canAttack = true;
    public enum EnemyState
    {
        idle, movingToTarget, moveInCircles, walkingBack, attacking
    };

    IdleState idleState = new IdleState();
    MoveToPlayer moveToPlayerState = new MoveToPlayer();
    Circulate circulateState = new Circulate();
    WalkBack walkBackState = new WalkBack();
    Attack attackState = new Attack();

    State currentState;

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.OnLeaveState(gameObject);

        switch (newState)
        {
            case EnemyState.idle:
                currentState = idleState;
                break;
            case EnemyState.movingToTarget:
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

    private void Update()
    {
        //Look at the target(the player) -> defines the direction vector in which it's looking
        transform.LookAt(point);
    }

    protected Vector3 GetDirection()
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

    public State GetCurrentState()
    {
        return currentState;
    }

    //==============================================================================
    //STATE BEHAVIORS
    //==============================================================================

    public abstract void WalkToTarget();

    public abstract void Circulate();

    public abstract void Attack();

    public abstract void WalkBack();

    public abstract void Idle();
}