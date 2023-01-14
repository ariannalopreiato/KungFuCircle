using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //The target that the camera should follow
    public Transform target;

    //The distance between the camera and the target
    public float distance = 10.0f;

    //The height of the camera above the target
    public float height = 9.0f;

    //The smoothness of the camera movement
    public float smoothness = 0.5f;

    void LateUpdate()
    {
        //Calculate the desired position for the camera
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        //Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness);

        //Look at the target game object
        transform.LookAt(target);
    }
}
