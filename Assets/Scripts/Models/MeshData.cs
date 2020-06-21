using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    /// <summary>
    /// The vertices.
    /// </summary>
    public Vector3[] Vertices { get; set; }
    /// <summary>
    /// The triangles.
    /// </summary>
    public int[] Triangles { get; set; }
    /// <summary>
    /// The Uvs.
    /// </summary>
    public Vector2[] Uvs { get; set; }
    /// <summary>
    /// The normals.
    /// </summary>
    public Vector3[] Normals { get; set; }
}
