using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    [Header("Movement")]
    public float showSpeed = 2f;
    public float resetSpeed = 3f;

    [Header("References")]
    public GameObject mainCamera;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public bool MoveBoxToView()
    {
        var targetPosition = mainCamera.transform.position / 3;
        var targetRotation = Quaternion.LookRotation(-mainCamera.transform.forward);

        if (transform.position != targetPosition && transform.rotation != targetRotation)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, targetPosition, showSpeed * Time.deltaTime),
                Quaternion.Lerp(transform.rotation, targetRotation, showSpeed * Time.deltaTime));

            return false;
        }

        return true;
    }

    public bool ResetPosition()
    {
        if (transform.position != _originalPosition && transform.rotation != _originalRotation)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, resetSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, resetSpeed * Time.deltaTime);

            return false;
        }

        return true;
    }
}
