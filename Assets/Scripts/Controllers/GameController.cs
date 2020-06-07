using Constants = Assets.Scripts.Constants;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// The world.
    /// </summary>
    public World World { get; private set; }

    public GameObject groundPrefab;
    public GameObject cubePrefab;

    private void Awake()
    {
        this.CreateWorld();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 buildPosition = new Vector3(Mathf.Floor(hit.point.x), (Mathf.Floor(hit.point.y) + .5f), Mathf.Floor(hit.point.z));
                Instantiate(this.cubePrefab, buildPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Creates the world.
    /// </summary>
    private void CreateWorld()
    {
        var rnd = new System.Random();
        int width = rnd.Next(Constants.WorldDimensions.SmallWorld.MIN_WIDTH, Constants.WorldDimensions.SmallWorld.MAX_WIDTH);
        int height = rnd.Next(Constants.WorldDimensions.SmallWorld.MIN_HEIGHT, Constants.WorldDimensions.SmallWorld.MAX_HEIGHT);

        this.World = new World(width, height);

        var ground = Instantiate(groundPrefab, this.transform);
        ground.name = Constants.GameobjectsName.GROUND_NAME;
        ground.tag = Constants.Tags.GROUND_TAG;

        Vector3 currentScale = ground.transform.localScale;
        ground.transform.localScale = new Vector3(currentScale.x * this.World.Width,
                                                  currentScale.y,
                                                  currentScale.z * this.World.Height);
    }
}
