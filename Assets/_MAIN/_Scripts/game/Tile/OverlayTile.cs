using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using static THJ.ArrowTranslator;

namespace THJ
{
    public class OverlayTile : MonoBehaviour
    {
        GameInfo gameInfo;
        [ReadOnly] public int distanceFromStart;
        [ReadOnly] public int distanceFromEnd;
        public int sumDistance { get { return distanceFromStart + distanceFromEnd; } }
        public bool canMoveTo = false;
        public bool isBlocked = false;

        public OverlayTile Previous;
        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
        public List<Sprite> arrows;

        SpriteRenderer spriteRenderer;
        Color defaultColor;
        Color showColor;
        Color hideColor;

        private void Start()
        {
            gameInfo = FindObjectOfType<GameInfo>();
            hideColor = new(0, 0, 0, 0);
            if (gameObject.GetComponent<SpriteRenderer>())
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
            showColor = new(defaultColor.r, defaultColor.g, defaultColor.b, 1f);
        }

        private void Update()
        {
            if (gameInfo.isMoving)
            {
                HideTile();
            }
        }

        public void ShowTile()
        {
            canMoveTo = true;
            spriteRenderer.color = showColor;
        }

        public void HideTile()
        {
            canMoveTo = false;
            spriteRenderer.color = hideColor;
        }

        public void SetSprite(ArrowDirection d)
        {
            if (d == ArrowDirection.None)
                GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
            else
            {
                GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
                GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
                // GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}


