using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    void Start()
    {
        if (objectToFollow == null)
        {
            objectToFollow = GameObject.Find("Player").transform;
        }
        cameraOffset = transform.position - objectToFollow.position;
    }
    private void LateUpdate()
    {
        Vector3 newpos = objectToFollow.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newpos, smoothFactor);
    }

    internal void SwithObjectToFollow(Transform transform)
    {
        objectToFollow = transform;
    }
}
