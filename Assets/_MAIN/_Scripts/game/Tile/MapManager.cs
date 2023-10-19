using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace THJ
{
    public class MapManager : MonoBehaviour
    {
        GameInfo gameInfo;
        public static MapManager Instance;

        public Tilemap mainTilemap;
        public GameObject overlayTilePrefab;
        public GameObject overlayContainer;
        public Dictionary<Vector2Int, OverlayTile> map;
        public GameObject interactButton;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            if (!gameInfo) gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Start()
        {
            //Create overlay tiles and put to parent.
            if (mainTilemap)
            {
                CreateOverlayTiles();
                CreatePlayer();
            }
        }

        private void CreateOverlayTiles()
        {
            map = new Dictionary<Vector2Int, OverlayTile>();
            BoundsInt bounds = mainTilemap.cellBounds;

            // for (int z = bounds.min.z; z > bounds.min.z; z--)
            // {
            int z = 0;
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    var tileType = mainTilemap.GetTile(tileLocation);

                    if (mainTilemap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = mainTilemap.GetCellCenterWorld(new Vector3Int(x, y, z));
                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = mainTilemap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);

                        map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());

                        if (tileType.name == "Furniture")
                            overlayTile.GetComponent<OverlayTile>().isBlocked = true;

                        if (tileType.name == "Ground0")
                        {
                            overlayTile.GetComponent<OverlayTile>().name = "Near Furniture";
                            overlayTile.GetComponent<OverlayTile>().nearestFurniture = true;
                            overlayTile.GetComponent<OverlayTile>().standLayerOrder = 0;
                        }
                    }
                }
            }
            // }
        }

        private void CreatePlayer()
        {
            //CreateCharacter
            if (gameInfo.tileCursorSystem.character == null)
            {
                Random rand = new Random();
                OverlayTile randomTile = map.ElementAt(rand.Next(0, map.Count)).Value;
                if (!randomTile.isBlocked)
                {
                    gameInfo.tileCursorSystem.character = Instantiate(gameInfo.characterPrefab).GetComponent<THJ.CharacterInfo>();
                    gameInfo.tileCursorSystem.PositionCharacterOnLine(randomTile);
                    gameInfo.tileCursorSystem.currentGridPoint = randomTile.grid2DLocation;
                }
                else
                {
                    CreatePlayer();
                }
            }
        }

        public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();
            Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            return surroundingTiles;
        }
    }

}
