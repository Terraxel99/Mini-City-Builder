using UnityEngine;

public class WorldController : MonoBehaviour
{
    #region Members

    [Header("World Dimensions")]
    public int sizeX;
    public int sizeZ;

    [Space]

    [Header("Grid settings")]
    [Range(10.0f, 128.0f)]
    public int chunkSize = 10;
    public float tileSize = 1.0f;

    [Header("Textures")]
    public Texture2D TilesTextures;
    public int TileResolution = 16;
    public Material mapMaterial;

    #endregion

    /// <summary>
    /// The world.
    /// </summary>
    public World World { get; private set; }
    /// <summary>
    /// The visual gameobjects for chunks.
    /// </summary>
    public GameObject[,] ChunksGameobjects { get; private set; }


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

                chunk.MeshData.Uvs[(currentTile * 4)] = new Vector2((float)x / chunk.SizeX, (float)z / chunk.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 1] = new Vector2((float)x / chunk.SizeX, (float)(z + 1) / chunk.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 2] = new Vector2((float)(x + 1)/ chunk.SizeX, (float)z / chunk.SizeZ);
                chunk.MeshData.Uvs[(currentTile * 4) + 3] = new Vector2((float)(x + 1) / chunk.SizeX, (float)(z + 1) / chunk.SizeZ);

                currentTile++;
            }
        }


        // Rendering chunk
        mesh.vertices = chunk.MeshData.Vertices;
        mesh.triangles = chunk.MeshData.Triangles;
        mesh.normals = chunk.MeshData.Normals;
        mesh.uv = chunk.MeshData.Uvs;

        mesh.RecalculateNormals();

        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = new Material(mapMaterial);
        renderer.sharedMaterials[0].mainTexture = this.BuildTexture(chunk);
    }

    private Texture2D BuildTexture(Chunk chunk)
    {
        int nbRows = this.TilesTextures.height / this.TileResolution;
        int tileTexturePerRow = this.TilesTextures.width / this.TileResolution;

        Debug.Log(chunk.SizeX);

        var texture = new Texture2D(this.TileResolution * chunk.SizeX, this.TileResolution * chunk.SizeZ);

        for (int z = 0; z < chunk.SizeZ; z++)
        {
            for (int x = 0; x < chunk.SizeX; x++)
            {
                int offset = Random.Range(0, 4);
                Color[] colors = this.TilesTextures.GetPixels(16 * offset, 0, this.TileResolution, this.TileResolution);

                texture.SetPixels(x * this.TileResolution, z * this.TileResolution, this.TileResolution, this.TileResolution, colors);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();

        return texture;
    }
}
