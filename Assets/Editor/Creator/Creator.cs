using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class Creator
{
    protected MeshSetting setting;
    protected string shape;
    protected string defName;
    protected Func<MeshSetting> createSetting;

    private MeshSetting newSetting;
    private Mesh newMesh;


    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public virtual void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        GUILayout.Label(string.Format("{0}生成器", shape));

        if(createSetting!= null)
            DrawSetting(createSetting);

        if (setting != null)
            DrawCreate();
    }

    private void DrawSetting(Func<MeshSetting> createSetting)
    {
        GUILayout.BeginHorizontal();
        DrawSetting();
        if (GUILayout.Button("新建配置"))
        {
            newSetting = createSetting();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(newSetting.GetInstanceID(), new AssetEndNameEditAction(newSetting), string.Format("Assets/Setting/Conf/{0}_mesh_setting", defName), AssetPreview.GetMiniThumbnail(newSetting), ".asset");
        }
        GUILayout.EndHorizontal();
    }

    protected virtual void DrawSetting()
    {
    }

    protected virtual void DrawCreate()
    {
        GUILayout.Space(5);
        if (GUILayout.Button("创建"))
            Create();
    }

    protected virtual void Create()
    {
        throw new Exception(this.GetType() + "未实现Create方法");
    }

    protected virtual void CreateMeshAsset(Vector3[] vertices, Vector2[] uv, int[] triangles, Color[] colors = null)
    {
        newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.colors = colors;
        newMesh.triangles = triangles;

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(newMesh.GetInstanceID(), new AssetEndNameEditAction(newMesh), string.Format("Assets/Res/Mesh/{0}_mesh", defName), AssetPreview.GetMiniThumbnail(newMesh), ".mesh");
    }

    protected virtual void OnConfirm(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object obj = null;
        if(newSetting?.GetInstanceID() == instanceId)
        {
            obj = newSetting;
        }else if(newMesh?.GetInstanceID() == instanceId)
        {
            obj = newMesh;
        }

        //if (obj == null) return;
        string path = pathName + resourceFile;
        //var ori = AssetDatabase.LoadAssetAtPath(path, obj.GetType());
        //if (ori != null)
        //{
        //    if(!EditorUtility.DisplayDialog("文件已存在", string.Format("{0}已存在，是否覆盖？", path), "覆盖", "取消"))
        //    {
        //        Debug.LogFormat("取消了 {0} 的创建", path);
        //        return;
        //    }
        //    AssetDatabase.DeleteAsset(path);
        //}

        SaveAsset(obj, path);
    }

    private void SaveAsset(UnityEngine.Object obj, string path)
    {
        AssetDatabase.CreateAsset(obj, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    protected virtual void OnCancel(int instanceId, string pathName, string resourceFile)
    {
        if(newSetting != null)
        {
            UnityEngine.Object.Destroy(newSetting);
            newSetting = null;
        }    
        if(newMesh != null)
        {
            UnityEngine.Object.Destroy(newMesh);
            newMesh = null;
        }
        Debug.LogFormat("OnCancel instanceId {0}  pathName {1}  resourceFile {2}", instanceId, pathName, resourceFile);
    }
}
