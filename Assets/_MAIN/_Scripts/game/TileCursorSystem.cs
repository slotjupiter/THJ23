using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace THJ
{
    public class TileCursorSystem : MonoBehaviour
    {
        public GameObject cursor;
   
        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                GameObject overlayTile = hit.Value.collider.gameObject;
                cursor.transform.position = overlayTile.transform.position;
                cursor.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

                if (Input.GetMouseButtonDown(0))
                {
                    overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
        }

        public RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

            if(hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
}

