using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    /// <summary>
    /// The tiles in the chunk.
    /// </summary>
    public Tile[,] Tiles { get; private set; }
    /// <summary>
    /// The data of the mesh.
    /// </summary>
    public MeshData MeshData { get; set; }
    /// <summary>
    /// The origin coordinates of the chunk.
    /// </summary>
    public Vector3 Origin { get; private set; }
    /// <summary>
    /// The size of the chunk on the X axis.
    /// </summary>
    public int SizeX { get; private set; }
    /// <summary>
    /// The size of the chunk on the Z axis.
    /// </summary>
    public int SizeZ { get; private set; }
    /// <summary>
    /// The number of tiles in the chunk.
    /// </summary>
    public int NbTiles
    {
        get
        {
            return this.SizeX * this.SizeZ;
        }
    }

    public Chunk(int sizeX, int sizeZ, Vector3 origin)
    {
        this.SizeX = sizeX;
        this.SizeZ = sizeZ;
        this.Origin = origin;

        this.MeshData = new MeshData();

        this.CreateTiles();
    }

    private void CreateTiles()
    {
        this.Tiles = new Tile[this.SizeX, this.SizeZ];

        for (int x = 0; x < this.SizeX;  x++)
        {
            for (int z = 0; z < this.SizeZ; z++)
            {
                var tilePosition = new Vector3(x, 0f, z) + this.Origin;
                this.Tiles[x, z] = new Tile(tilePosition, Tile.TileType.EMPTY);
            }
        }
    }
}
