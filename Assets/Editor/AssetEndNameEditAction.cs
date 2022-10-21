
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class AssetEndNameEditAction : EndNameEditAction
{
    public Object assetObj;

    public AssetEndNameEditAction(Object obj)
    {
        assetObj = obj;
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
            //AssetDatabase.DeleteAsset(path);
        }

        AssetDatabase.CreateAsset(assetObj, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public override void Cancelled(int instanceId, string pathName, string resourceFile)
    {
        string path = pathName + resourceFile;
        UnityEngine.Object.Destroy(assetObj);
        Debug.LogFormat("OnCancel instanceId {0}  path {1}", instanceId, path);
    }
}
