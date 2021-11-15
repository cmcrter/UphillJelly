// Retrieved from https://github.com/pharan/Unity-MeshSaver/blob/master/MeshSaver/Editor/MeshSaverEditor.cs

using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshSaverEditor
{
	[MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
	public static void SaveMeshInPlace(MenuCommand menuCommand)
	{
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMesh(m, m.name, false, true);
	}

	[MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
	public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
	{
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMesh(m, m.name, true, true);
	}

	public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
	{
		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		if (string.IsNullOrEmpty(path)) return;

		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

		if (optimizeMesh)
			MeshUtility.Optimize(meshToSave);

		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
	}

	public static void SaveMeshAtPath(Mesh mesh, string path, bool makeNewInstance, bool optimizeMesh)
	{
		if (string.IsNullOrEmpty(path)) return;

		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

		if (optimizeMesh)
			MeshUtility.Optimize(meshToSave);

		Mesh exitingMesh = AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)) as Mesh;
		if (exitingMesh != null)
        {
			AssetDatabase.DeleteAsset(path);
		}

		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
	}
}