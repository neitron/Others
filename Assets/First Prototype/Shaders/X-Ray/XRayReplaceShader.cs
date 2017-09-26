using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class XRayReplaceShader : MonoBehaviour
{
	[SerializeField]
	private Shader xRayShader;
    [SerializeField, Range(0.0f, 1.0f)]
    private float globalXRayVisibility = 1.0f;

    private void OnValidate()
	{
        Shader.SetGlobalFloat ( "_GlobalXRayVisibility", globalXRayVisibility );
	}

	private void OnEnable()
	{
		GetComponent<Camera>().SetReplacementShader( xRayShader, "XRay" );
	}

	private void OnDisable()
	{
		GetComponent<Camera>().ResetReplacementShader();
	}
}
