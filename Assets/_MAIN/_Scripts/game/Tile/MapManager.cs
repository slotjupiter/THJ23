using System.Collections;
using System.Collections.Generic;
using THJ;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public Tilemap mainTilemap;
    public GameObject overlayTilePrefab;
    public GameObject overlayContainer;
    public Dictionary<Vector2Int, GameObject> map;

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
            Debug.Log("GET TILEMAP");
            map = new Dictionary<Vector2Int, GameObject>();

            BoundsInt bounds = mainTilemap.cellBounds;

            // for (int z = bounds.min.z; z > bounds.min.z; z--)
            // {
            int z = 0;
            Debug.Log("1");
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Debug.Log("2");

                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {

                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    Debug.Log("3 = " + tileLocation + ", " + tileKey);
                    if (mainTilemap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        Debug.Log("44444444");
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = mainTilemap.GetCellCenterWorld(tileLocation);
                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = mainTilemap.GetComponent<TilemapRenderer>().sortingOrder;
                        map.Add(tileKey, overlayTile);
                    }
                }
            }
            // }
        }
    }
}
