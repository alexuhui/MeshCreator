using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class MyEndNameEditAction : EndNameEditAction
{
    public event Action<int, string, string> OnConfirm;
    public event Action<int, string, string> OnCancel;

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        OnConfirm?.Invoke(instanceId, pathName, resourceFile);
    }

    public override void Cancelled(int instanceId, string pathName, string resourceFile)
    {
        OnCancel?.Invoke(instanceId, pathName, resourceFile);
    }
}
