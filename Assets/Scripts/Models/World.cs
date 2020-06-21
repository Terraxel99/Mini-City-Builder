using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World
{
    #region Members

    private int sizeX;
    private int sizeZ;

    #endregion

    /// <summary>
    /// The chunks.
    /// </summary>
    public Chunk[,] Chunks { get; private set; }
    /// <summary>
    /// The origin position of the world (bottom left of the chunk)
    /// </summary>
    public Vector3 Origin { get; private set; }
    /// <summary>
    /// Number of chunks on X axis.
    /// </summary>
    public int ChunksX { get; private set; }
    /// <summary>
    /// Number of chunks on Z axis.
    /// </summary>
    public int ChunksZ { get; private set; }
    /// <summary>
    /// The size of a chunk.
    /// </summary>
    public int ChunkSize { get; private set; }
    /// <summary>
    /// The number of tiles in the world.
    /// </summary>
    public int NbTiles
    {
        get
        {
            return this.sizeX * this.sizeZ;
        }
    }
    /// <summary>
    /// The number of chunks in the world.
    /// </summary>
    public int NbChunks
    {
        get 
        { 
            return this.ChunksX * this.ChunksZ; 
        }
    }
    
    public World(int sizeX, int sizeZ, Vector3 origin, int chunkSize = 10)
    {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        this.ChunkSize = chunkSize;
        this.Origin = origin;

        this.CreateChunks();
    }

    /// <summary>
    /// Creates all the chunk data of the world.
    /// </summary>
    private void CreateChunks()
    {
        // Calculating number of chunks
        this.ChunksX = Mathf.CeilToInt((float)this.sizeX / (float)this.ChunkSize);
        this.ChunksZ = Mathf.CeilToInt((float)this.sizeZ / (float)this.ChunkSize);

        this.Chunks = new Chunk[this.ChunksX, this.ChunksZ];

        for (int x = 0; x < this.ChunksX; x++)
        {
            for(int z = 0; z < this.ChunksZ; z++)
            {
                // If we are at the last chunk on the X axis, we might need to adapt the chunk size on X axis since it might be an incomplete chunk
                // If it is not the last chunk, the chunk has a full square size.
                // Complete size of world if the chunk was fully filled - real world size = nb of void tiles => size of chunk - nb of void tiles = nb of remaining filled tiles.
                int chunkSizeX = (x == (this.ChunksX - 1)) ?
                    this.ChunkSize - ((this.ChunkSize * this.ChunksX) - this.sizeX) :
                    this.ChunkSize;

                // Repeating operation for Z axis
                int chunkSizeZ = (z == (this.ChunksZ - 1)) ?
                    this.ChunkSize - ((this.ChunkSize * this.ChunksZ) - this.sizeZ) :
                    this.ChunkSize;

                // Calculating origin and creating chunk
                var chunkOrigin = new Vector3((x * this.ChunkSize), 0f, (z * this.ChunkSize));
                this.Chunks[x, z] = new Chunk(chunkSizeX, chunkSizeZ, chunkOrigin); 
            }
        }
        
    }
}
