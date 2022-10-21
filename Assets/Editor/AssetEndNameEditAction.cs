
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetEndNameEditAction : EndNameEditAction
{
    public Object assetObj;
    public Action callback;

    public void Init(Object obj, Action cb = null)
    {
        assetObj = obj;
        callback = cb;
    }

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        string path = pathName + resourceFile;
        var ori = AssetDatabase.LoadAssetAtPath(path, assetObj.GetType());
        if (ori != null)
        {
            if (!EditorUtility.DisplayDialog("文件已存在", string.Format("{0}已存在，是否覆盖？", path), "覆盖", "取消"))
            {
                Debug.LogFormat("取消了 {0} 的创建", path);
                return;
            }

            assetObj.name = ori.name;
            EditorUtility.CopySerialized(assetObj, ori);
        }
        else
        {
            AssetDatabase.CreateAsset(assetObj, path);
        }
       
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        callback?.Invoke();
    }

    public override void Cancelled(int instanceId, string pathName, string resourceFile)
    {
        string path = pathName + resourceFile;
        UnityEngine.Object.Destroy(assetObj);
        Debug.LogFormat("OnCancel instanceId {0}  path {1}", instanceId, path);
    }
}
