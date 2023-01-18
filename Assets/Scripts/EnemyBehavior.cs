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
    protected Transform point;

    [SerializeField]
    protected float speed = 2f;

    [SerializeField]
    protected float runningSpeed = 4f;

    [SerializeField]
    protected float attackRadiusMelee = 10f;

    protected Vector3 target;

    //[SerializeField]
    //protected float farRadius = 30f;

    [SerializeField]
    protected float midRadius = 20f;

    private Camera camera;

    protected bool rotateOpposite = false;
    protected Vector3 startPos;

    protected bool isOutsideOfCameraView = false;
    public bool canAttack = false;
    public bool isFar = false;
    public bool isCollidingWithOtherEnemies = false;

    public enum EnemyState
    {
        idle, movingToTarget, moveInCircles, walkingBack, attacking
    };

    protected IdleState idleState = new IdleState();
    protected MoveToPlayer moveToPlayerState = new MoveToPlayer();
    protected Circulate circulateState = new Circulate();
    protected WalkBack walkBackState = new WalkBack();
    protected Attack attackState = new Attack();

    protected State currentState;

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
        //find the camera
        var cameras = Camera.allCameras;
        camera = cameras[0];

        //set starting state
        currentState = idleState;

        //save the starting position of enemy
        startPos = gameObject.transform.position;

        target = startPos;

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
        SetIsFar();
        SetCanAttack();

        //check if the enemy is outside or inside the camera viewport
        Vector3 viewportPosition = camera.WorldToViewportPoint(gameObject.transform.position);
        if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1)
            gameObject.GetComponent<EnemyBehavior>().isOutsideOfCameraView = true;
        else
            gameObject.GetComponent<EnemyBehavior>().isOutsideOfCameraView = false;
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }

    public bool GetIsFar()
    {
        return isFar;
    }

    protected abstract void SetCanAttack();

    protected abstract void SetIsFar();

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

    public abstract void WalkToPlayer();

    public abstract void Circulate();

    public abstract void Attack();

    public abstract void WalkBack();

    public abstract void Idle();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            isCollidingWithOtherEnemies = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            isCollidingWithOtherEnemies = false;
    }
}