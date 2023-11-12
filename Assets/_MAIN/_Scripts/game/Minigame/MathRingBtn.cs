using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using THJ;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MathRingBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum MathTextMode
    {
        Ring, Operator, Numbers
    }

    [TabGroup("Info")] public MathTextMode textMode;
    [TabGroup("Info")] public int starterIndex = 0;
    [TabGroup("Info")] public int textIndex = 0;
    [TabGroup("Info")] public TMP_Text displayText;
    [TabGroup("Info")] public CanvasGroup displayCanvasgroup;
    [TabGroup("Setup")] public List<string> ringText;
    public string currentString { get; set; }
    public int currentValue { get; set; } = 0;
    [TabGroup("Setup")] public Color defaultColor;
    [TabGroup("Setup")] public Color hoverColor;
    [TabGroup("Setup"), ShowIf("@textMode == MathTextMode.Numbers")] public Color NotHaveColor;

    Image borderImg;
    GameInfo gameInfo;

    private void Awake()
    {
        borderImg = GetComponent<Image>();
        gameInfo = FindObjectOfType<GameInfo>();
        if (textMode != MathTextMode.Numbers)
        {
            displayText.text = ringText[textIndex];
            currentString = ringText[textIndex];
            AdjustAlpha(currentString);
        }
        else
        {
            currentValue = 0;
            currentString = currentValue.ToString();
            displayText.text = currentString;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentValue != 0)
            borderImg.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentValue != 0)
            borderImg.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.Instance.PlayFX("Click");
        if (textMode != MathTextMode.Numbers)
        {
            textIndex++;
            if (textIndex > ringText.Count - 1) textIndex = 0;
            displayText.text = ringText[textIndex];
            currentString = ringText[textIndex];
            AdjustAlpha(currentString);
        }
        else if (textMode == MathTextMode.Numbers)
        {
            currentValue = -currentValue;
            currentString = currentValue.ToString();
            displayText.text = currentString;
        }
        gameInfo.minigamesSystem.armsMinigame.CheckAnswer();
    }

    public void SetNumbers(int value)
    {
        currentValue = value;
        currentString = currentValue.ToString();
        displayText.text = currentString;

        if (currentValue == 0)
        {
            borderImg.color = NotHaveColor;
        }
        else
        {
            borderImg.color = defaultColor;
        }
    }

    public void Reset()
    {
        if (textMode == MathTextMode.Numbers)
        {
            currentValue = 0;
            currentString = currentValue.ToString();
            displayText.text = currentString;
            borderImg.color = NotHaveColor;
        }

        textIndex = starterIndex;
    }

    private void AdjustAlpha(string currentString)
    {
        if (string.IsNullOrEmpty(currentString)) displayCanvasgroup.alpha = 0.5f;
        else displayCanvasgroup.alpha = 1f;
    }
}
