using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSetting : ScriptableObject
{
}

public class RectMeshSetting:MeshSetting
{
    public float width = 10;
    public float height = 1;
    public int widthVertCnt = 12;
    public int heightVertCnt = 3;
    public Vector3 normal = Vector3.up;
}
