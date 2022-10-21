
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
            if (!EditorUtility.DisplayDialog("�ļ��Ѵ���", string.Format("{0}�Ѵ��ڣ��Ƿ񸲸ǣ�", path), "����", "ȡ��"))
            {
                Debug.LogFormat("ȡ���� {0} �Ĵ���", path);
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
