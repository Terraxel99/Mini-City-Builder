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
    /// <summary>
    /// The state of the tile.
    /// </summary>
    public TileState State { get; set; }

    public Tile(Vector3 position, TileState state = TileState.EMPTY)
    {
        position.y = 0;
        this.Position = position;

        this.State = state;
    }

    public Tile(int x, int z, TileState state = TileState.EMPTY) 
        : this(new Vector3(x, 0, z), state) 
    { }
}
