using System.Collections;
using UnityEngine;

public class RotationUtils
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private bool _isRotating = false;
    private Vector2 _centerOffsetPosition;
    private readonly MonoBehaviour _owner;

    public bool IsRotating => _isRotating;

    public RotationUtils(MonoBehaviour owner, Vector2 centerOffsetPosition = default)
    {
        _owner = owner;
        _centerOffsetPosition = centerOffsetPosition;
    }

    public void RotateToTarget(Vector2 targetPosition)
    {
        if (_isRotating)
            return;
        _isRotating = true;
        var fromPosition = (Vector2)_owner.transform.position + _centerOffsetPosition;
        var vectorToTarget = targetPosition - fromPosition;
        //Todo Abs?
        float angle = Mathf.Atan2(vectorToTarget.y, Mathf.Abs(vectorToTarget.x)) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        _owner.StartCoroutine(RotateToTargetSmoothly(targetRotation));
    }

    private IEnumerator RotateToTargetSmoothly(Quaternion targetRotation)
    {
        var angleDiff = Quaternion.Angle(targetRotation, _owner.transform.rotation);
        if (angleDiff > 5)
        {
            var rotationSpeedRatio = 18 / angleDiff;
            var currentRotationRatio = 0f;
            while (currentRotationRatio != 1f)
            {
                currentRotationRatio += rotationSpeedRatio;
                currentRotationRatio = Mathf.Clamp(currentRotationRatio, 0, 1);
                _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, currentRotationRatio);
                yield return new WaitForSeconds(0.025f);
            }
        }
        _owner.transform.rotation = targetRotation;
        _isRotating = false;
        //TODO
        //if (callbackAction != default)
        //    callbackAction();
    }
}
