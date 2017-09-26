using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyLightingShaderGUI : ShaderGUI
{
	enum SmoothnessSource
	{
		Uniform, Albedo, Metallic
	}

	Material target;
	MaterialEditor editor;
	MaterialProperty[] properties;

	static ColorPickerHDRConfig emissionConfig =
		new ColorPickerHDRConfig(0f, 99f, 1f / 99f, 3f);

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		//base.OnGUI(materialEditor, properties);
		
		this.editor = materialEditor;
		this.target = editor.target as Material;
		this.properties = properties;

		DoMain();
		GUILayout.Space(15);
		DoSecondary();
	}

	void RecordAction (string label)
	{
		editor.RegisterPropertyChangeUndo(label);
	}

	bool IsKeywordEnabled(string keyword)
	{
		return target.IsKeywordEnabled(keyword);
	}

	void SetKeyword(string keyword, bool state)
	{
		if(state)
		{
			foreach(Material m in editor.targets)
			{
				m.EnableKeyword(keyword);
			}
		}
		else
		{
			foreach(Material m in editor.targets)
			{
				m.DisableKeyword(keyword);
			}
		}
	}

	MaterialProperty FindProperty(string name)
	{
		return FindProperty(name, properties);
	}

	static GUIContent staticLabel = new GUIContent();

	static GUIContent MakeLabel (string text, string tooltip = null)
	{
		staticLabel.text = text;
		staticLabel.tooltip = tooltip;
		return staticLabel;
	}

	static GUIContent MakeLabel(MaterialProperty property, string tooltip = null)
	{
		staticLabel.text = property.displayName;
		staticLabel.tooltip = tooltip;
		return staticLabel;
	}

	private void DoMain()
	{
		GUILayout.Label("Main Maps", EditorStyles.boldLabel);

		MaterialProperty mainTex = FindProperty("_MainTex");
		
		editor.TexturePropertySingleLine(
			MakeLabel(mainTex, "Main diffuse texture"), mainTex, FindProperty("_Tint"));

		DoMetallic();
		DoSmoothness();
		DoNormals();
		DoOcclusion();
		DoEmission();
		DoDetailMask();
		editor.TextureScaleOffsetProperty(mainTex);
	}

	private void DoDetailMask()
	{
		MaterialProperty map = FindProperty("_DetailMask");
		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(
				MakeLabel(map, "Detail Mask (A)"), map);
		}
		if(EditorGUI.EndChangeCheck())
		{
			SetKeyword("_DETAIL_MASK", map.textureValue);
		}
	}

	private void DoOcclusion()
	{
		MaterialProperty map = FindProperty("_OcclusionMap");
		Texture tex = map.textureValue;

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(
				MakeLabel(map, "Occlusion (G)"), map,
				tex ? FindProperty("_OcclusionStrenght") : null);
		}
		if(EditorGUI.EndChangeCheck() && tex != map.textureValue)
		{
			SetKeyword("_OCCLUSION_MAP", map.textureValue);
		}
	}

	private void DoEmission()
	{
		MaterialProperty map = FindProperty("_EmissionMap");
		Texture tex = map.textureValue;

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertyWithHDRColor(
				MakeLabel(map, "Emission (RGB)"), map,
				tex ? FindProperty("_Emission") : null, emissionConfig, false);
		}
		if(EditorGUI.EndChangeCheck() && tex != map.textureValue)
		{
			SetKeyword("_EMISSION_MAP", map.textureValue);
		}
	}

	private void DoSmoothness()
	{
		SmoothnessSource source = SmoothnessSource.Uniform;
		if(IsKeywordEnabled("_SMOOTHNESS_ALBEDO"))
		{
			source = SmoothnessSource.Albedo;
		}
		else if (IsKeywordEnabled("_SMOOTHNESS_METALLIC"))
		{
			source = SmoothnessSource.Metallic;
		}

		MaterialProperty slider = FindProperty("_Smoothness");

		EditorGUI.indentLevel += 2;
		editor.ShaderProperty(slider, MakeLabel(slider));
		EditorGUI.indentLevel += 1;

		EditorGUI.BeginChangeCheck();
		{
			source = (SmoothnessSource)EditorGUILayout.EnumPopup(MakeLabel("Source"), source);
			EditorGUI.indentLevel -= 3;
		}
		if(EditorGUI.EndChangeCheck())
		{
			RecordAction("Smoothness Source");
			SetKeyword("_SMOOTHNESS_ALBEDO", source == SmoothnessSource.Albedo);
			SetKeyword("_SMOOTHNESS_METALLIC", source == SmoothnessSource.Metallic);
		}
	}

	private void DoMetallic()
	{
		MaterialProperty map = FindProperty("_MetallicMap");
		Texture tex = map.textureValue;

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(MakeLabel(map, "Metallic (R)"), map,
				tex ? null : FindProperty("_Metallic"));
		}
		if(EditorGUI.EndChangeCheck() && tex != map.textureValue)
		{
			SetKeyword("_METALLIC_MAP", map.textureValue);
		}
	}

	private void DoNormals()
	{
		MaterialProperty map = FindProperty("_Normal");
		Texture tex = map.textureValue;

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(MakeLabel(map), map,
				tex ? FindProperty("_NormalScale") : null);
		}
		if(EditorGUI.EndChangeCheck() && tex != map.textureValue)
		{
			SetKeyword("_NORMAL_MAP", map.textureValue);
		}
	}

	private void DoSecondary()
	{
		GUILayout.Label("Secondary Maps", EditorStyles.boldLabel);

		MaterialProperty detailTex = FindProperty("_DetailTex");

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(
				MakeLabel(detailTex, "Main diffuse texture"), detailTex);
		}
		if(EditorGUI.EndChangeCheck())
		{
			SetKeyword("_DETAIL_ALBEDO_MAP", detailTex.textureValue);
		}

		DoDetailNormals();

		editor.TextureScaleOffsetProperty(detailTex);
	}

	private void DoDetailNormals()
	{
		MaterialProperty map = FindProperty("_DetailNormal");
		Texture tex = map.textureValue;

		EditorGUI.BeginChangeCheck();
		{
			editor.TexturePropertySingleLine(MakeLabel(map), map,
				tex ? FindProperty("_DetailNormalScale") : null);
		}
		if(EditorGUI.EndChangeCheck() && tex != map.textureValue)
		{
			SetKeyword("_DETAIL_NORMAL_MAP", map.textureValue);
		}
	}
}
