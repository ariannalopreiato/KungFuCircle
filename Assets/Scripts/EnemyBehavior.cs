using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    public Transform point;
    public float speed = 2f;
    private bool isMoving = true;
    private bool rotateOpposite = false;

    void Start()
    {
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
            isMoving = !isMoving;
        }
        if (isMoving)
        {
            //Look at the target (the player) -> defines the direction vector in which it's looking
            transform.LookAt(point);
            //Calculate the position in which you are supposed to be going 
            Vector3 targetPosition = GetDirection();
            //Move to the calculated position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    Vector3 GetDirection()
    {
        if(rotateOpposite)
            return transform.position + (-1) * transform.right * speed * Time.deltaTime;
        else
            return transform.position + transform.right * speed * Time.deltaTime;
    }
}