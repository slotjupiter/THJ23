using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    Color hideColor;

    private void Start()
    {
        hideColor = new(0, 0, 0, 0);
        if (gameObject.GetComponent<SpriteRenderer>()) spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void ShowTile()
    {
        spriteRenderer.DOColor(defaultColor, 0.15f);
    }

    public void HideTile()
    {
        spriteRenderer.DOColor(hideColor, 0.15f);
    }
}
