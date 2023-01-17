using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public enum State
{
    idle, movingToPlayer, moveInCircles, walkingBack
}

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    public Transform point;
    public float speed = 2f;
    private bool rotateOpposite = false;
    private State enemyState = State.idle;

    [SerializeField]
    float walkingForwardRadius = 3f;
    [SerializeField]
    private float furthestPoint = 7f;
    Vector3 startPos;

    void Start()
    {
        startPos = gameObject.transform.position;
        //randomize in which way the enemy it's going to move (clockwise or anticlockwise)
        int randomSign = UnityEngine.Random.Range(0, 2);
        if (randomSign == 0)
            rotateOpposite = true;
        else
            rotateOpposite = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemyState = State.idle;
        }

        if (enemyState == State.moveInCircles)
        {
            //Look at the target (the player) -> defines the direction vector in which it's looking
            transform.LookAt(point);
            //Calculate the position in which you are supposed to be going 
            Vector3 targetPosition = GetDirection();
            //Move to the calculated position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if(enemyState == State.movingToPlayer)
        {
            transform.LookAt(point);
            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
            if(IsWithinForwardRadius(transform.position))
            {
                enemyState = State.walkingBack;
            }
        }

        if(enemyState == State.walkingBack)
        {
            transform.LookAt(point);
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if(IsOutOfFurthestRadius(transform.position))
            {
                enemyState = State.movingToPlayer;
            }
        }
    }

    private Vector3 GetDirection()
    {
        if(rotateOpposite)
            return transform.position + (-1) * transform.right * speed * Time.deltaTime;
        else
            return transform.position + transform.right * speed * Time.deltaTime;
    }

    private bool IsWithinForwardRadius(Vector3 currentPos)
    {
        var distancePlayerEnemy = Vector3.Distance(currentPos, point.position);
        if (distancePlayerEnemy <= walkingForwardRadius)
            return true;

        return false;
    }

    private bool IsOutOfFurthestRadius(Vector3 currentPos)
    {
        var distancePlayerEnemy = Vector3.Distance(currentPos, point.position);
        if (distancePlayerEnemy >= furthestPoint)
            return true;

        return false;
    }

    public void SetState(State state)
    {
        enemyState = state;
    }
}