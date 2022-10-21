using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RectMeshCreator:Creator
{
    public override void OnEnable()
    {
        shape = "矩形";
        defName = "rect";
        createSetting = () => {
            return new RectMeshSetting();
        };

        base.OnEnable();
    }

    protected override void DrawSetting()
    {
        var temp = (RectMeshSetting)EditorGUILayout.ObjectField("配置参数：", setting as RectMeshSetting, typeof(RectMeshSetting), false);
        if (temp is RectMeshSetting)
            setting = temp;
    }

    protected override void Create()
    {
        if (!(setting is RectMeshSetting)) return;

        var rectSetting = setting as RectMeshSetting;
        float width = rectSetting.width;
        float height = rectSetting.height;
        int wvCnt = rectSetting.widthVertCnt;
        int hvCnt = rectSetting.heightVertCnt;

        float perW = width / (wvCnt - 1);
        float perH = height / (hvCnt - 1);
        int row = hvCnt - 1;
        int col = wvCnt - 1;
        
        Vertices[] verts = new Vertices[wvCnt * hvCnt * 6];
        for (int h = 0; h < row; h++)
        {
            for (int w = 0; w < col; w++)
            {
                int index = (h * col + w) * 6;
                int nw = w + 1;
                int nh = h + 1;
                var lb = new Vertices()
                {
                    pos = new Vector3(w * perW, 0, h * perH),
                    uv = new Vector2((float)w / col, (float)h / row),
                    color = GetCol(w, col),
                };
                var rb = new Vertices()
                {
                    pos = new Vector3(nw * perW, 0, h * perH),
                    uv = new Vector2((float)nw / col, (float)h / row),
                    color = GetCol(nw, col),
                };
                var rt = new Vertices()
                {
                    pos = new Vector3(nw * perW, 0, nh * perH),
                    uv = new Vector2((float)nw / col, (float)nh / row),
                    color = GetCol(nw, col),
                };
                var lt = new Vertices()
                {
                    pos = new Vector3(w * perW, 0, nh * perH),
                    uv = new Vector2((float)w / col, (float)nh / row),
                    color = GetCol(w, col),
                };


                if(h%2==0)
                {
                    verts[index] = lb;
                    verts[index + 1] = rt;
                    verts[index + 2] = rb;

                    verts[index + 3] = lb;
                    verts[index + 4] = lt;
                    verts[index + 5] = rt;
                }
                else
                {
                    verts[index] = lt;
                    verts[index + 1] = rb;
                    verts[index + 2] = lb;

                    verts[index + 3] = lt;
                    verts[index + 4] = rt;
                    verts[index + 5] = rb;
                }
            }
        }
        
        var vertices = new Vector3[verts.Length];
        var uv = new Vector2[verts.Length];
        var colors = new Color[verts.Length];
        var triangles = new int[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            vertices[i] = verts[i].pos;
            colors[i] = verts[i].color;
            uv[i] = verts[i].uv;
            triangles[i] = i;
        }

        CreateMeshAsset(vertices, uv, triangles, colors);
    }

    private Color GetCol(int i, int max)
    {
        if (max <= 0) return default;
        float half = max / 2;
        var col = new Color(Mathf.Min(i, max - i)/ half, 0, 0, 1);
        return col;
    }

    private Color GetColPow(int i, int max)
    {
        if (max <= 0) return default;
        float half = Mathf.Pow(max / 2, 2);
        var col = new Color(Mathf.Pow(Mathf.Min(i, max - i), 2) / half, 0, 0, 1);
        return col;
    }
}
