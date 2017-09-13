using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterThirdCamera : CenterBasedCamera
{
    Vector3 destinition;
    float lerpFactor;

    private void Start()
    {
        destinition = transform.position;
    }

    override public void LateUpdate()
    {
        var targetPlainPos = Vector3.ProjectOnPlane(target.position, Vector3.up);
        var centerPlainPos = Vector3.ProjectOnPlane(center.position, Vector3.up);
        var cameraPlainPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
        
        var newDestinition = target.position - ( target.forward - Vector3.up ) * currentZoom;

        if(newDestinition != destinition)
        {
            destinition = newDestinition;
            lerpFactor = 0;
        }

        transform.position = Vector3.Lerp(transform.position, newDestinition, 0.5f);
        transform.LookAt(target.position + Vector3.up * pinch);

        lerpFactor += 0.001f;
    }   

}
