using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{


    public Transform target;
    public Transform center;

    public float pinch;
    public float currentZoom;



    private void LateUpdate()
    {
        var targetPlainPos = Vector3.ProjectOnPlane(target.position, Vector3.up);
        var centerPlainPos = Vector3.ProjectOnPlane(center.position, Vector3.up);
        var cameraPlainPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
        
        var centerToTargetDir = (targetPlainPos - centerPlainPos).normalized;

        transform.position = centerToTargetDir * currentZoom + targetPlainPos + Vector3.up * pinch;
        transform.LookAt(centerPlainPos);
    }


}
