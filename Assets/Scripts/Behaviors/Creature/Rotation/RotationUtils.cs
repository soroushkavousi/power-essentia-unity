using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RotationUtils
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private bool _isRotating = false;
    [SerializeField] private Vector2 _centerOffsetPosition;
    [SerializeField] private Number _rotationSpeed;
    private readonly MonoBehaviour _owner;

    public bool IsRotating => _isRotating;

    public RotationUtils(MonoBehaviour owner, Vector2 centerOffsetPosition = default, Number rotationSpeed = default)
    {
        _owner = owner;
        _centerOffsetPosition = centerOffsetPosition;
        _rotationSpeed = rotationSpeed ?? new Number(1f);
    }

    public void RotateToTarget(Vector2 targetPosition)
    {
        if (_isRotating)
            return;
        _isRotating = true;
        var fromPosition = (Vector2)_owner.transform.position + _centerOffsetPosition;
        var vectorToTarget = targetPosition - fromPosition;
        float angle = Mathf.Atan2(vectorToTarget.y, Mathf.Abs(vectorToTarget.x)) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _owner.StartCoroutine(RotateToTargetSmoothly(targetRotation));
    }

    private IEnumerator RotateToTargetSmoothly(Quaternion targetRotation)
    {
        var angleDiff = Quaternion.Angle(targetRotation, _owner.transform.rotation);
        if (angleDiff > 4)
        {
            var rotationSpeedRatio = 8 / angleDiff * _rotationSpeed.Value;
            var currentRotationRatio = 0f;
            while (currentRotationRatio != 1f)
            {
                currentRotationRatio += rotationSpeedRatio;
                currentRotationRatio = Mathf.Clamp(currentRotationRatio, 0, 1);
                _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, currentRotationRatio);
                yield return new WaitForSeconds(0.017f);
            }
        }
        _owner.transform.rotation = targetRotation;
        _isRotating = false;
        //TODO
        //if (callbackAction != default)
        //    callbackAction();
    }
}
