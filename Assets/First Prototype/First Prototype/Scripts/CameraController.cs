using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{


    public Transform target;

    public float pinch;
    public float currentZoom;

    public Vector3 offset;



    virtual public void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pinch);
    }
}
