using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 0.0f;

    void Update()
    {
        //Get input for horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Calculate the movement vector based on the input and speed
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;

        //Move the game object based on the calculated movement vector
        transform.position += movement;
    }
}