using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    [Header("Movement")]
    public float lerpSpeed;

    [Header("References")]
    public GameObject mainCamera;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public bool CanMoveBoxToView()
    {
        var targetPosition = mainCamera.transform.position / 3;
        var targetRotation = Quaternion.LookRotation(-mainCamera.transform.forward);

        if (transform.position != targetPosition && transform.rotation != targetRotation)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime), 
                Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetOriginalPosition()
    {
        if (transform.position != _originalPosition && transform.rotation != _originalRotation)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, 0.05f);
            return false;
        }

        return true;
    }
}
