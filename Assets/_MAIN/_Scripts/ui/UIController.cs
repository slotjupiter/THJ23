using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using THJ;
using UnityEngine;

public class UIController : MonoBehaviour
{
    GameInfo gameInfo;
    public CanvasGroup menuCanvasgroup;
    public TextAnimator_TMP pressAnykeyAnim;

    private void Awake()
    {
        menuCanvasgroup.alpha = 1f;
        gameInfo = FindObjectOfType<GameInfo>();
    }

    private void Update()
    {
        if (gameInfo)
        {
            if (!gameInfo.gameStart && pressAnykeyAnim.allLettersShown)
            {
                if (Input.anyKey)
                {
                    gameInfo.gameStart = true;
                    menuCanvasgroup.DOFade(0f, 0.5f).OnComplete(() => menuCanvasgroup.gameObject.SetActive(false));
                }
            }
        }
    }
}
