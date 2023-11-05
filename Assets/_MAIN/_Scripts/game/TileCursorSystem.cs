using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static THJ.ArrowTranslator;

namespace THJ
{
    public class TileCursorSystem : MonoBehaviour
    {
        GameInfo gameInfo;

        public GameObject cursor;
        public RenderTexture renderTexture;
        public RawImage displayRawImage;

        public CharacterInfo character { get; set; }
        PathFinder pathFinder;
        TileRangeFinder rangeFinder;
        ArrowTranslator arrowTranslator;
        List<OverlayTile> path;
        List<OverlayTile> rangeFinderTiles;

        public Vector2Int currentGridPoint { get; set; }
        public Vector2Int nextGridPoint { get; set; }

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

            UpdateRenderTextureSize();
        }

        void Update()
        {
            UpdateRenderTextureSize();
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue && hit.Value.collider.tag != "Furniture")
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                if (tile != null && !gameInfo.isMoving)
                {
                    cursor.transform.position = tile.transform.position;
                    cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
                }

                if (rangeFinderTiles.Contains(tile) && !gameInfo.isMoving && gameInfo.MovementRange > 0 && !gameInfo.diceSystem.OnRollingDice)
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

                if (Input.GetMouseButtonDown(0) && tile != null && tile.canMoveTo)
                {
                    tile.ShowTile();
                    gameInfo.isMoving = true;
                    tile.gameObject.GetComponent<OverlayTile>().HideTile();
                }
            }

            if (path.Count > 0 && gameInfo.isMoving)
            {
                MoveAlongPath();
            }
        }

        public void ActivePath()
        {
            GetInRangeTiles();
        }

        int currentMove;
        bool doMoveProcess = false;
        private void MoveAlongPath()
        {
            var step = gameInfo.MoveSpeed * Time.deltaTime;

            foreach (var item in rangeFinderTiles)
            {
                MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
            }

            if (path[0]) nextGridPoint = path[0].grid2DLocation;
            if (path[0] && path[0].nearestFurniture) character.SetSortingOrder(path[0].standLayerOrder);
            else character.SetSortingOrder(2);

            character.MoveAnimation(currentGridPoint, nextGridPoint, true);
            float zIndex = path[0].transform.position.z;
            character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
            {
                AudioController.Instance.PlayFX("Walk");
                PositionCharacterOnLine(path[0]);
                currentGridPoint = path[0].grid2DLocation;
                if (!doMoveProcess)
                {
                    doMoveProcess = true;
                    currentMove = gameInfo.MovementRange - path.Count;
                    if (currentMove <= 0) currentMove = 0;
                    gameInfo.SetMovementRange(currentMove);
                }

                path.RemoveAt(0);
            }

            if (path.Count == 0)
            {
                character.MoveAnimation(currentGridPoint, nextGridPoint, false);
                if (gameInfo.MovementRange != 0)
                {
                    doMoveProcess = false;
                    gameInfo.canRollDice = false;
                    GetInRangeTiles();
                }
                else if (gameInfo.MovementRange == 0)
                {
                    doMoveProcess = false;
                    nextGridPoint = Vector2Int.zero;
                    gameInfo.movingPhase = false;
                    gameInfo.canRollDice = true;
                }

                gameInfo.isMoving = false;
            }
        }

        public void PositionCharacterOnLine(OverlayTile tile)
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

        void UpdateRenderTextureSize()
        {
            // Get the current screen resolution
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            if (renderTexture == null || renderTexture.width != screenWidth || renderTexture.height != screenHeight)
            {
                if (renderTexture != null)
                {
                    renderTexture.Release();
                    // Destroy(renderTexture);
                }

                renderTexture = new RenderTexture(screenWidth, screenHeight, 24);
                renderTexture.name = "TempDisplayTexture";
                renderTexture.filterMode = FilterMode.Bilinear;

                Camera.main.targetTexture = renderTexture;
                displayRawImage.texture = renderTexture;
                Debug.Log("UPDATE TEXTURE");
            }
        }

        public void GetInRangeTiles()
        {
            rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), gameInfo.MovementRange);

            foreach (var item in rangeFinderTiles)
            {
                if (!gameInfo.equipmentSystem.equipLegs)
                    item.ShowTile(1);
                else if (gameInfo.equipmentSystem.equipLegs && gameInfo.diceSystem.usingMeatDice)
                    item.ShowTile(2);
                else
                    item.ShowTile(0);
            }
        }
    }
}

