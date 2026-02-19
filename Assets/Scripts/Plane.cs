using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Plane : MonoBehaviour
{
    [SerializeField] Vector3 position = new Vector3(0, 0, 0);   // 中心座標
    [SerializeField] float width = 1;
    [SerializeField] float height = 1;

    Mesh mesh;
    MeshFilter meshFilter;


    public Plane(Vector3 position, float witdh, float height)
    {
        this.position = position;
        this.width = witdh;
        this.height = height;
    }

    private void Start()
    {
        mesh = GetComponent<Mesh>();
        meshFilter = GetComponent<MeshFilter>();
    }

    private void FixedUpdate()
    {
        SimulatePhisics();
        CreateMesh();
    }

    private void CreateMesh()
    {
        // リスト作成
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // 頂点設定
        {
            float halfW = width / 2;
            float halfH = height / 2;
            vertices.Add(new Vector3(halfW, 0, halfH) + position);
            vertices.Add(new Vector3(halfW, 0, -halfH) + position);
            vertices.Add(new Vector3(-halfW, 0, -halfH) + position);
            vertices.Add(new Vector3(-halfW, 0, halfH) + position);
        }

        // 三角形生成
        {
            triangles = new List<int>
            {
                0, 1, 2,
                2, 3, 0
            };
        }

        // レンダリング
        mesh = new Mesh();
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetIndices(triangles, MeshTopology.Triangles, 0);  // MeshTopologyを変更すれば別の描画形式にできる
        mesh.RecalculateNormals();
        mesh.name = "MyPlane";

        meshFilter.mesh = mesh;
    }

    public void SimulatePhisics()
    {
    }
}
