using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private bool _dieParented;

    [Header("Movement")]
    public float showSpeed = 2f;
    public float resetSpeed = 3f;

    [Header("References")]
    public GameObject mainCamera;
    public Die die;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        if (die == null)
        {
            die = FindFirstObjectByType<Die>();
            if (die == null)
                Debug.LogWarning("Box: no Die component found in the scene. Assign it via the Inspector.");
        }
    }

    public bool MoveBoxToView()
    {
        // Parent the die to the box the moment it has landed, so it travels with the box
        if (!_dieParented && die != null && die.LandedWithUpFace)
        {
            die.FreezePhysics();
            die.model.transform.SetParent(transform, true);
            _dieParented = true;
        }

        // Rotate the box so its forward face points toward the camera.
        // Fall back to Vector3.forward as the up vector when the camera is directly above or below.
        var directionToCamera = (mainCamera.transform.position - transform.position).normalized;
        var upVector = (Mathf.Abs(Vector3.Dot(directionToCamera, Vector3.up)) > 0.99f) ? Vector3.forward : Vector3.up;
        var targetRotation = Quaternion.LookRotation(directionToCamera, upVector);

        var targetPosition = mainCamera.transform.position / 3;

        var positionReached = Vector3.Distance(transform.position, targetPosition) < 0.01f;
        var rotationReached = Quaternion.Angle(transform.rotation, targetRotation) < 0.5f;

        if (!positionReached || !rotationReached)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, targetPosition, showSpeed * Time.deltaTime),
                Quaternion.Slerp(transform.rotation, targetRotation, showSpeed * Time.deltaTime));

            return false;
        }

        return true;
    }

    public bool ResetPosition()
    {
        // Unparent the die before resetting so it can return to its own anchor
        if (_dieParented && die != null)
        {
            die.model.transform.SetParent(null, true);
            _dieParented = false;
        }

        var positionReached = Vector3.Distance(transform.position, _originalPosition) < 0.01f;
        var rotationReached = Quaternion.Angle(transform.rotation, _originalRotation) < 0.5f;

        if (!positionReached || !rotationReached)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, resetSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotation, resetSpeed * Time.deltaTime);

            return false;
        }

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        return true;
    }
}

