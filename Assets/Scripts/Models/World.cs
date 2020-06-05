using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World
{
    private static readonly int DEFAULT_WORLD_WIDTH = 10;
    private static readonly int DEFAULT_WORLD_HEIGHT = 10;

    /// <summary>
    /// List of all the tiles.
    /// </summary>
    public List<Tile> Tiles { get; set; }

    /// <summary>
    /// Height of the world.
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// Width of the world.
    /// </summary>
    public int Width { get; private set; }

    public World(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        this.Tiles = new List<Tile>();
        this.CreateTiles();
    }

    public World() : this(DEFAULT_WORLD_WIDTH, DEFAULT_WORLD_HEIGHT) { }

    /// <summary>
    /// Populates the list of tiles.
    /// </summary>
    private void CreateTiles()
    {
        for (int x = 0; x < this.Width; x++)
        {
            for (int z = 0; z < this.Height; z++)
            {
                this.Tiles.Add(new Tile(x, z, Tile.TileType.EMPTY));
            }
        }
    }

    /// <summary>
    /// Returns a tile at given X and Z coordinates.
    /// </summary>
    /// <param name="x">The X position in the world.</param>
    /// <param name="z">The Z position in the world.</param>
    /// <returns>The tile.</returns>
    public Tile GetTileAt(int x, int z)
    {
        return this.Tiles.FirstOrDefault((tile) => tile.Position.x == x && tile.Position.z == z);
    }

    /// <summary>
    /// Gets the list of all the tiles.
    /// </summary>
    /// <returns>The tile list.</returns>
    public List<Tile> GetAllTiles()
    {
        return this.Tiles;
    }
}
