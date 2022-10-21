using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MeshSetting/Rect", fileName = "rect_mesh_setting")]
public class RectMeshSetting:MeshSetting
{
    [Header("������")]
    [Range(0.1f, 1000)]
    public float width = 10;
    [Header("���򶥵���")]
    [Range(2, 1000)]
    public int widthVertCnt = 12;

    [Space(5)]
    [Header("������")]
    [Range(0.1f, 1000)]
    public float height = 1;
    [Header("���򶥵���")]
    [Range(2, 1000)]
    public int heightVertCnt = 3;

    [Space(5)]
    [Header("���߷���(����)")]
    public Vector3 normal = Vector3.up;
}
