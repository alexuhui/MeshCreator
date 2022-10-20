using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Creator
{
    private MyEndNameEditAction endNameEditAction;

    protected MeshSetting setting;
    protected string shape;
    protected string defName;
    protected Func<MeshSetting> createSetting;

    private MeshSetting newSetting;

    public virtual void OnEnable()
    {
        endNameEditAction = new MyEndNameEditAction();
        endNameEditAction.OnConfirm += OnConfirm;
        endNameEditAction.OnCancel += OnCancel;
    }

    public virtual void OnDisable()
    {
        endNameEditAction.OnConfirm -= OnConfirm;
        endNameEditAction.OnCancel -= OnCancel;
    }

    public virtual void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        GUILayout.Label(string.Format("{0}生成器", shape));

        if(createSetting!= null)
            DrawSetting(createSetting);
    }

    private void DrawSetting(Func<MeshSetting> createSetting)
    {
        GUILayout.BeginHorizontal();
        DrawSetting();
        if (GUILayout.Button("新建配置"))
        {
            newSetting = createSetting();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(newSetting.GetInstanceID(), endNameEditAction, string.Format("Assets/Setting/{0}_mesh_setting", defName), AssetPreview.GetMiniThumbnail(newSetting), "");
        }
        GUILayout.EndHorizontal();
    }

    protected virtual void DrawSetting()
    {
    }

    protected virtual void Create()
    {
        throw new Exception(this.GetType() + "未实现Create方法");
    }

    protected virtual void OnConfirm(int instanceId, string pathName, string resourceFile)
    {
        if(newSetting.GetInstanceID() == instanceId)
        {
            AssetDatabase.CreateAsset(newSetting, pathName+".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    protected virtual void OnCancel(int instanceId, string pathName, string resourceFile)
    {
        Debug.LogFormat("OnCancel instanceId {0}  pathName {1}  resourceFile {2}", instanceId, pathName, resourceFile);
    }
}
