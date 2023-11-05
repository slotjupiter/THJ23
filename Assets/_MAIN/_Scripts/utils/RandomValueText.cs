using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomValueText : MonoBehaviour
{
    [SerializeField] private bool showDigits;
    [SerializeField] private float maxNumber;
    [SerializeField] private float minNumber;
    [SerializeField] private float offsetValue;
    [SerializeField] private float tickRate;
    [SerializeField] private TMP_Text displayText;

    private void Start()
    {
        InvokeRepeating(nameof(RunRandomNumber), 1f, tickRate);
    }

    private void RunRandomNumber()
    {
        if (displayText)
        {
            float randomCurrentNum = Random.Range(minNumber, maxNumber);
            float randomOffset = Random.Range(-offsetValue, offsetValue);
            float currentVal = randomCurrentNum + randomOffset;
            if (!showDigits)
                displayText.text = ((int)currentVal).ToString();
            else
                displayText.text = currentVal.ToString(".00");
        }
    }
}
