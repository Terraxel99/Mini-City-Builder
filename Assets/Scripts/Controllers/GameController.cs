using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// The world.
    /// </summary>
    public World World { get; private set; }

    public GameObject tilePrefab;

    private void Start()
    {
        this.CreateWorld();
    }

    /// <summary>
    /// Creates the world.
    /// </summary>
    private void CreateWorld()
    {
        this.World = new World();

        foreach (var tile in this.World.GetAllTiles())
        {
            Instantiate(tilePrefab, tile.Position, Quaternion.identity);
        }
    }
}
