using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace THJ
{
    public class OverlayTile : MonoBehaviour
    {
        [ReadOnly] public int distanceFromStart;
        [ReadOnly] public int distanceFromEnd;
        public int sumDistance { get { return distanceFromStart + distanceFromEnd; } }
        public bool isBlocked = false;

        public OverlayTile Previous;
        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
        public List<Sprite> arrows;

        SpriteRenderer spriteRenderer;
        Color defaultColor;
        Color hideColor;

        private void Start()
        {
            hideColor = new(0, 0, 0, 0);
            if (gameObject.GetComponent<SpriteRenderer>())
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HideTile();
            }
        }

        public void ShowTile()
        {
            spriteRenderer.DOColor(defaultColor, 0.15f);
        }

        public void HideTile()
        {
            spriteRenderer.DOColor(hideColor, 0.15f);
        }

        // public void SetSprite(ArrowDirection d)
        // {
        //     if (d == ArrowDirection.None)
        //         GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        //     else
        //     {
        //         GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
        //         GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
        //         GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        //     }
        // }
    }
}


