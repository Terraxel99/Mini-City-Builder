using UnityEngine;

public class Tile
{
    /// <summary>
    /// The tile position (bottom left of the tile)
    /// </summary>
    public Vector3 Position { get; set; }
    /// <summary>
    /// The type of the tile.
    /// </summary>
    public TileType Type { get; set; }

    public Tile(Vector3 position, TileType type = TileType.EMPTY)
    {
        position.y = 0;
        this.Position = position;

        this.Type = type;
    }

    public Tile(int x, int z, TileType type = TileType.EMPTY) 
        : this(new Vector3(x, 0, z), type) 
    { }

    public enum TileType
    {
        UNKNOWN,
        EMPTY,
        BUILDING,
        BUILT,
    }
}
