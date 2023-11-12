
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour
{
    [TabGroup("Face")] public SkinProperties skinProperties;
    [TabGroup("Face")] public Color defaultColor;
    [TabGroup("Face")] public Color halfColor;
    [TabGroup("Face")] public Color criticalColor;
    [TabGroup("Face")] public Image faceBG;
    [TabGroup("Win")] public GameObject winButton;

    private void Start()
    {
        winButton.SetActive(false);
        skinProperties.SetSkin(skinProperties.aliveSkin);
    }

    public void ActiveWinButton()
    {
        winButton.SetActive(true);
    }

    public void SetBGColor(Color targetColor)
    {
        if (faceBG.color != targetColor)
        {
            faceBG.color = targetColor;
        }
    }

}
