using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform lookAtTarget;

    public float up;

    public float currentZoom = 10;

    public float observeAngle = 135;

    private void LateUpdate()
    {
        var tp = target.position;
        tp.y = 0.0f;

        var lp = lookAtTarget.position;
        lp.y = 0.0f;

        var cp = transform.position;
        cp.y = 0.0f;

        var playerToTowerDir = (tp - lp).normalized;
        var towerToCamDir = (cp - lp).normalized;

        var res = ( Vector3.Dot(-playerToTowerDir, towerToCamDir) * 0.5f + 0.5f ) * 180;
        
        if (res > observeAngle)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + playerToTowerDir * currentZoom + Vector3.up * up, 0.01f);
            transform.LookAt(lp);
        }
         

    }
}
