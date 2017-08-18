using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterThirdCamera : CenterBasedCamera
{
    override public void LateUpdate()
    {
        var targetPlainPos = Vector3.ProjectOnPlane(target.position, Vector3.up);
        var centerPlainPos = Vector3.ProjectOnPlane(center.position, Vector3.up);
        var cameraPlainPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);

        var centerToTargetDir = (targetPlainPos - centerPlainPos).normalized;

        var newDestinition = target.position + target.forward * currentZoom + Vector3.up * 30;
        transform.position = newDestinition;
        transform.LookAt(target.position + Vector3.up * pinch);
    }

}
