using UnityEngine;

public class WorldController : MonoBehaviour
{
    #region Members

    [Header("World Dimensions")]
    public int sizeX;
    public int sizeZ;

    [Space]

    [Header("Grid settings")]
    [Range(20.0f, 60.0f)]
    public int chunkSize = 10;
    public float tileSize = 1.0f;

    #endregion

    /// <summary>
    /// The world.
    /// </summary>
    public World World { get; private set; }
    /// <summary>
    /// The visual gameobjects for chunks.
    /// </summary>
    public GameObject[,] ChunksGameobjects { get; private set; }

    public Material redMaterial;

    private void Start()
    {
        // Creating world data and visual
        this.World = new World(this.sizeX, this.sizeZ, Vector3.zero, this.chunkSize);
        this.ChunksGameobjects = new GameObject[this.World.ChunksX, this.World.ChunksZ];

        // Generating all the chunks of the world visually
        for (int x = 0; x < this.World.ChunksX; x++)
        {
            for (int z = 0; z < this.World.ChunksZ; z++)
            {
                this.GenerateChunk(this.World.Chunks[x, z], this.ChunksGameobjects[x, z]);
            }
        }
    }

    /// <summary>
    /// Generates a chunk from given data on the given visual gameobject.
    /// </summary>
    /// <param name="chunk">The chunk data.</param>
    /// <param name="go">The visual gameobject.</param>
    private void GenerateChunk(Chunk chunk, GameObject go)
    {
        // Instanciating visual object at correct position with needed components
        go = new GameObject($"Chunk_{chunk.Origin.x}_{chunk.Origin.z}", new[] { typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider) });

        go.transform.position = chunk.Origin;
        go.transform.parent = this.transform;

        // Creating the mesh and assign it to be rendered later
        var mesh = new Mesh();
        var meshFilter = go.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
        int nbTiles = chunk.NbTiles;

        chunk.MeshData.Vertices = new Vector3[nbTiles * 4]; // Each tile has 4 vertices.
        chunk.MeshData.Triangles = new int[nbTiles * 2 * 3]; // Each tile has 2 triangles which have 3 points.
        chunk.MeshData.Normals = new Vector3[nbTiles * 4]; // Same amount than vertices
        chunk.MeshData.Uvs = new Vector2[nbTiles * 4]; // Same amount than vertices

        // For each tile of the chunk, we create 4 vertices and 2 triangles
        for (int z = 0, currentTile = 0; z < chunk.SizeZ; z++)
        {
            for (int x = 0; x < chunk.SizeX; x++)
            {
                chunk.MeshData.Vertices[currentTile * 4] = new Vector3(x, 0, z);
                chunk.MeshData.Vertices[(currentTile * 4) + 1] = new Vector3(x, 0, (z + 1));
                chunk.MeshData.Vertices[(currentTile * 4) + 2] = new Vector3((x + 1), 0, z);
                chunk.MeshData.Vertices[(currentTile * 4) + 3] = new Vector3((x + 1), 0, (z + 1));

                chunk.MeshData.Triangles[(6 * currentTile)] = (currentTile * 4);
                chunk.MeshData.Triangles[(6 * currentTile) + 1] = (currentTile * 4 + 1);
                chunk.MeshData.Triangles[(6 * currentTile) + 2] = (currentTile * 4 + 2);
                chunk.MeshData.Triangles[(6 * currentTile) + 3] = (currentTile * 4 + 1);
                chunk.MeshData.Triangles[(6 * currentTile) + 4] = (currentTile * 4 + 3);
                chunk.MeshData.Triangles[(6 * currentTile) + 5] = (currentTile * 4 + 2);

                chunk.MeshData.Normals[currentTile * 4] = Vector3.up;
                chunk.MeshData.Normals[(currentTile * 4) + 1] = Vector3.up;
                chunk.MeshData.Normals[(currentTile * 4) + 2] = Vector3.up;
                chunk.MeshData.Normals[(currentTile * 4) + 3] = Vector3.up;

                chunk.MeshData.Uvs[(currentTile * 4)] = new Vector2((float)(x + chunk.Origin.x)/this.World.SizeX, (float)(z + chunk.Origin.z)/ this.World.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 1] = new Vector2((float)(x + chunk.Origin.x) / this.World.SizeX, (float)((z + chunk.Origin.z) + 1) / this.World.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 2] = new Vector2((float)((x + chunk.Origin.x) + 1)/ this.World.SizeX, (float)(z + chunk.Origin.z) / this.World.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 3] = new Vector2((float)((x + chunk.Origin.x) + 1) / this.World.SizeX, (float)((z + chunk.Origin.z) + 1) / this.World.SizeZ);

                currentTile++;
            }
        }

        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = this.redMaterial;

        // Rendering chunk
        mesh.vertices = chunk.MeshData.Vertices;
        mesh.triangles = chunk.MeshData.Triangles;
        mesh.normals = chunk.MeshData.Normals;
        mesh.uv = chunk.MeshData.Uvs;

        mesh.RecalculateNormals();
    }
}
