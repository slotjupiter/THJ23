using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static THJ.ArrowTranslator;

namespace THJ
{
    public class TileCursorSystem : MonoBehaviour
    {
        GameInfo gameInfo;

        public GameObject cursor;

        CharacterInfo character;
        PathFinder pathFinder;
        TileRangeFinder rangeFinder;
        ArrowTranslator arrowTranslator;
        List<OverlayTile> path;
        List<OverlayTile> rangeFinderTiles;

        private void Awake()
        {
            if (!gameInfo) gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Start()
        {
            pathFinder = new PathFinder();
            rangeFinder = new TileRangeFinder();
            arrowTranslator = new ArrowTranslator();

            path = new List<OverlayTile>();
            rangeFinderTiles = new List<OverlayTile>();
        }

        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

                if (rangeFinderTiles.Contains(tile) && !gameInfo.isMoving)
                {
                    path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);

                    foreach (var item in rangeFinderTiles)
                    {
                        MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                    }

                    for (int i = 0; i < path.Count; i++)
                    {
                        var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                        var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                        var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                        path[i].SetSprite(arrow);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    tile.ShowTile();

                    if (character == null)
                    {
                        character = Instantiate(gameInfo.characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnLine(tile);
                        GetInRangeTiles();
                    }
                    else
                    {
                        gameInfo.isMoving = true;
                        tile.gameObject.GetComponent<OverlayTile>().HideTile();
                    }
                }
            }

            if (path.Count > 0 && gameInfo.isMoving)
            {
                MoveAlongPath();
            }
        }

        private void MoveAlongPath()
        {
            var step = gameInfo.MoveSpeed * Time.deltaTime;

            foreach (var item in rangeFinderTiles)
            {
                MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
            }

            float zIndex = path[0].transform.position.z;
            character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(path[0]);
                path.RemoveAt(0);
            }

            if (path.Count == 0)
            {
                GetInRangeTiles();
                gameInfo.isMoving = false;
            }

        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            character.standingOnTile = tile;
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private void GetInRangeTiles()
        {
            rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), gameInfo.MovementRange);

            foreach (var item in rangeFinderTiles)
            {
                item.ShowTile();
            }
        }
    }
}

