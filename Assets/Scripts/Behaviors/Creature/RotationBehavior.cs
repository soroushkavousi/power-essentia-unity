using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private bool _isRotating = false;

    public bool IsRotating => _isRotating;

    private void Update()
    {
       
    }

    public void FeedData()
    {
       
    }

    public void RotateToTarget(Vector2 targetPosition, Vector2 centerOffsetPosition = default)
    {
        if (_isRotating)
            return;
        _isRotating = true;
        var fromPosition = (Vector2)transform.position + centerOffsetPosition;
        var vectorToTarget = targetPosition - fromPosition;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        StartCoroutine(RotateToTargetSmoothly(targetRotation));
    }

    private IEnumerator RotateToTargetSmoothly(Quaternion targetRotation)
    {
        var angleDiff = Quaternion.Angle(targetRotation, transform.rotation);
        if (angleDiff > 5)
        {
            var rotationSpeedRatio = 18 / angleDiff;
            var currentRotationRatio = 0f;
            while (currentRotationRatio != 1f)
            {
                currentRotationRatio += rotationSpeedRatio;
                currentRotationRatio = Mathf.Clamp(currentRotationRatio, 0, 1);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationRatio);
                yield return new WaitForSeconds(0.025f);
            }
        }
        transform.rotation = targetRotation;
        _isRotating = false;
        //TODO
        //if (callbackAction != default)
        //    callbackAction();
    }
}
