using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Object = UnityEngine.Object;

public class Creator
{
    private bool foldout = false;

    private MeshSetting _setting;
    protected MeshSetting setting
    {
        get { return _setting; }
        set {
            if (value == _setting) return;
            _setting = value;
            settingEditor = Editor.CreateEditor(_setting);
        }
    }
    private Editor settingEditor;


    protected string shape;
    protected string defName;
    protected Func<MeshSetting> createSetting;

    private MeshSetting newSetting;
    private Mesh newMesh;

    //private AssetEndNameEditAction assetEndNameEditAction;

    private string name;

    public virtual void OnEnable()
    {
        name = string.Format("{0}生成器", shape);
        //assetEndNameEditAction = ScriptableObject.CreateInstance<AssetEndNameEditAction>();
    }

    public virtual void OnDisable()
    {
    }

    public void Excute()
    {
        GUILayout.Space(10);
        GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        foldout = EditorGUILayout.Foldout(foldout, name, true);
        if (!foldout) return;
        OnGUI();
    }

    public virtual void OnGUI()
    {
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
            Action cb = () =>{ setting = newSetting; };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                newSetting.GetInstanceID(),
                GetEndNameEditAction(newSetting, cb), 
                string.Format("Assets/Setting/Conf/{0}_mesh_setting", defName), 
                AssetPreview.GetMiniThumbnail(newSetting), 
                ".asset");
        }
        GUILayout.EndHorizontal();
        DrawSettingProperty();
    }

    protected virtual void DrawSetting()
    {
    }

    private void DrawSettingProperty()
    {
        if(settingEditor != null)
            settingEditor.OnInspectorGUI();
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

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(newMesh.GetInstanceID(), GetEndNameEditAction(newMesh), string.Format("Assets/Res/Mesh/{0}_mesh", defName), AssetPreview.GetMiniThumbnail(newMesh), ".mesh");
    }

    private AssetEndNameEditAction GetEndNameEditAction(Object obj, Action cb = null)
    {
        var o = ScriptableObject.CreateInstance<AssetEndNameEditAction>();
        o.Init(obj, cb);
        return o;
    }
}
