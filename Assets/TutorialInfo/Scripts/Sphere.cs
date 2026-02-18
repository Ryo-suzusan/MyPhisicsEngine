using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Sphere : MonoBehaviour
{
    [SerializeField] Vector3 position = new Vector3(0, 0, 0);
    [SerializeField] float radius = 1;

    [SerializeField] Vector3 speed = new Vector3(1, 1, 1);

    [SerializeField, UnityEngine.Range(3, 360)] int dividedInVertical = 6;
    [SerializeField, UnityEngine.Range(3, 360)] int dividedInHorizontal = 6;

    Mesh mesh;
    MeshFilter meshFilter;


    public Sphere(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
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
            // てっぺん
            vertices.Add(new Vector3(0, radius, 0) + position);

            float x, y, z;
            for (int p = 1; p < dividedInVertical; p++)
            {
                // y軸方向に分割
                y = Mathf.Cos(Mathf.Deg2Rad * p * 180f / dividedInVertical) * radius;
                // 0 < t < 1. 半径に対する縮小率
                float t = Mathf.Sin(Mathf.Deg2Rad * p * 180f / dividedInVertical) * radius;

                for (int q = 0; q < dividedInHorizontal; q++)
                {
                    x = Mathf.Cos(Mathf.Deg2Rad * q * 360f / dividedInHorizontal) * t;
                    z = Mathf.Sin(Mathf.Deg2Rad * q * 360f / dividedInHorizontal) * t;
                    vertices.Add(new Vector3(x, y, z) + position);
                }
            }

            // 底
            vertices.Add(new Vector3(0, -radius, 0) + position);
        }

        // 三角形生成
        {
            // てっぺん
            for (int i = 0; i < dividedInHorizontal; i++)
            {
                if (i == dividedInHorizontal - 1)
                {
                    triangles.Add(0);
                    triangles.Add(1);
                    triangles.Add(i + 1);
                    break;

                }
                triangles.Add(0);
                triangles.Add(i + 2);
                triangles.Add(i + 1);
            }

            for (int p = 0; p < dividedInVertical - 2; p++)
            {
                var firstIndexInLayer = p * dividedInHorizontal + 1;

                for (int q = 0; q < dividedInHorizontal; q++)
                {
                    ///一周してきた最後の組のみ、例外
                    if (q == dividedInHorizontal - 1)
                    {
                        triangles.Add(firstIndexInLayer + q);
                        triangles.Add(firstIndexInLayer);
                        triangles.Add(firstIndexInLayer + dividedInHorizontal);

                        triangles.Add(firstIndexInLayer + q);
                        triangles.Add(firstIndexInLayer + dividedInHorizontal);
                        triangles.Add(firstIndexInLayer + q + dividedInHorizontal);

                        break;
                    }

                    triangles.Add(firstIndexInLayer + q);
                    triangles.Add(firstIndexInLayer + q + 1);
                    triangles.Add(firstIndexInLayer + q + 1 + dividedInHorizontal);

                    triangles.Add(firstIndexInLayer + q);
                    triangles.Add(firstIndexInLayer + q + dividedInHorizontal + 1);
                    triangles.Add(firstIndexInLayer + q + dividedInHorizontal);
                }
            }

            // 底
            for (int i = 0; i < dividedInHorizontal; i++)
            {
                if (i == dividedInHorizontal - 1)
                {
                    triangles.Add(vertices.Count - 1);
                    triangles.Add(vertices.Count - 1 - dividedInHorizontal + i);
                    triangles.Add(vertices.Count - 1 - dividedInHorizontal);
                    break;

                }
                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 1 - dividedInHorizontal + i);
                triangles.Add(vertices.Count - dividedInHorizontal + i);
            }
        }

        // レンダリング
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetIndices(triangles, MeshTopology.Triangles, 0);  // MeshTopologyを変更すれば別の描画形式にできる
        mesh.RecalculateNormals();
        mesh.name = "MySphere";

        meshFilter.mesh = mesh;
    }

    public void SimulatePhisics()
    {
        position += speed;
    }
}
