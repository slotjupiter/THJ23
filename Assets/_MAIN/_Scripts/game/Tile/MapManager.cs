using System.Collections;
using System.Collections.Generic;
using THJ;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public Tilemap mainTilemap;
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        //Create overlay tiles and put to parent.
        if (mainTilemap)
        {
            BoundsInt bounds = mainTilemap.cellBounds;

            for (int z = bounds.max.z; z > bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        var tileLocation = new Vector3Int(x, y, z);

                        if (mainTilemap.HasTile(tileLocation))
                        {
                            var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                            var cellWorldPos = mainTilemap.GetCellCenterWorld(tileLocation);
                            overlayTile.transform.position = new(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z + 1);
                        }

                    }
                }
            }
        }
    }
}
