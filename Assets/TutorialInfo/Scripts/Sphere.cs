using System;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] Vector3 position = new Vector3(0, 0, 0);
    [SerializeField] float radius = 0;

    [SerializeField] Vector3 speed = new Vector3(1, 1, 1);

    public Sphere(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public void Simulate()
    {
        position += speed;
    }
}
