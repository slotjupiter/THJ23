using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using THJ;
using TMPro;
using UnityEngine;

public class ArmsMinigame : MonoBehaviour
{
    [TabGroup("Setup")] public GameObject armsGamePanel;
    [TabGroup("Setup")] public GameObject closeBtn;
    [TabGroup("Setup")] public TMP_Text gameHeader;

    [TabGroup("Game")] public List<Transform> spawnPoint;
    [TabGroup("Game")] public List<GameObject> goldStore;

    int ans1;
    int ans2;
    int ans3;
    int ans4;

    [TabGroup("Gold")] public List<Sprite> goldSprite;
    [TabGroup("Gold")] public List<GoldDragMinigame> goldDrag;

    [TabGroup("Answer")] public List<MathRingBtn> otherSlot;
    [TabGroup("Answer")] public List<MathRingBtn> numbersSlot;
    [TabGroup("Answer")] public List<GameObject> scaleSlot;
    public int currentIndex = 0;
    private List<string> tokens = new();
    GameInfo gameInfo;

    private void Start()
    {
        gameInfo = FindObjectOfType<GameInfo>();
        SetGoldValue();
        foreach (var obj in scaleSlot)
            obj.SetActive(false);
    }

    [Button]
    public void OpenArmsMinigame()
    {
        AudioController.Instance.PlayFX("Popup");

        SetGoldValue();
        armsGamePanel.SetActive(true);
    }

    public void ClosePanel()
    {
        AudioController.Instance.PlayFX("Popup");

        armsGamePanel.SetActive(false);
        foreach (var other in otherSlot)
            other.Reset();
        foreach (var number in numbersSlot)
            number.Reset();

        Withdraw(ref ans4, ref currentIndex);
        Withdraw(ref ans3, ref currentIndex);
        Withdraw(ref ans2, ref currentIndex);
        Withdraw(ref ans1, ref currentIndex);
    }

    private void SetGoldValue()
    {
        int limit = 0;
        Utils.ShuffleList(goldDrag);
        for (int i = 0; i < goldDrag.Count; i++)
        {
            if (limit > 2) limit = 0;
            goldDrag[i].currentImage.sprite = goldSprite[limit];
            goldDrag[i].goldValue = GoldValue(limit);
            goldDrag[i].gameObject.name = "Gold " + GoldValue(limit);
            limit++;
        }
    }

    private int GoldValue(int index)
    {
        switch (index)
        {
            case 0:
                return 1;
            case 1:
                return 3;
            case 2:
                return 5;
            default:
                return 1;
        }
    }

    public void ResetToSpawn(Transform targetTransform)
    {
        int rand = Random.Range(0, spawnPoint.Count);
        targetTransform.position = spawnPoint[rand].position;
    }

    public void CheckAnswer()
    {
        if (NoZeroValues())
        {
            string sumValue
            = otherSlot[0].displayText.text.ToString()
            + numbersSlot[0].currentValue.ToString()
            + otherSlot[1].displayText.text.ToString()
            + otherSlot[2].displayText.text.ToString()
            + numbersSlot[1].currentValue.ToString()
            + otherSlot[3].displayText.text.ToString()
            + otherSlot[4].displayText.text.ToString()
            + otherSlot[5].displayText.text.ToString()
            + numbersSlot[2].currentValue.ToString()
            + otherSlot[6].displayText.text.ToString()
            + otherSlot[7].displayText.text.ToString()
            + numbersSlot[3].currentValue.ToString()
            + otherSlot[8].displayText.text.ToString();

            if (string.IsNullOrEmpty(sumValue))
                return;
            else
            {
                tokens = new();
                tokens = TokenizeExpression(sumValue);
                int result = EvaluateExpression(tokens);
                if (result == 24)
                {
                    gameInfo.minigamesSystem.armsWin = true;
                    StartWinSequence();
                }
            }
        }
        else
            Debug.Log("Can't calculated");
    }

    private void StartWinSequence()
    {
        StartCoroutine(GiveArmsSequences(gameInfo.minigamesSystem.armsWin));
    }

    IEnumerator GiveArmsSequences(bool _boolCheck)
    {
        closeBtn.SetActive(false);
        yield return new WaitUntil(() => _boolCheck = true);
        foreach (var btn in goldDrag)
        {
            btn.gameObject.SetActive(false);
        }
        gameInfo.inventorySystem.CreateItemBox(gameInfo.minigamesSystem.armsKey);
        gameInfo.collectHandsKey = true;
        gameInfo.UpdateProgressText(25);
        yield return new WaitForSeconds(1.5f);
        gameInfo.dialogueSystem.SetForceTextWithTimeEnd("You acquired arms as a reward.", 1.5f, () =>
        {
            ClosePanel();
            gameInfo.uiController.OpenLore("Arms");
        });
    }

    List<string> TokenizeExpression(string expression)
    {
        List<string> tokens = new List<string>();
        int i = 0;

        while (i < expression.Length)
        {
            char c = expression[i];

            if (char.IsDigit(c) || (c == '-' && (i == 0 || "+-*/(".Contains(expression[i - 1].ToString()))))
            {
                // If the character is a digit or a minus sign at the beginning or after an operator or opening parenthesis,
                // accumulate the digits to form a number (positive or negative)
                int numStart = i;
                i++;

                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    i++;
                }

                string numStr = expression.Substring(numStart, i - numStart);
                tokens.Add(numStr);
            }
            else if (c == '(' || c == ')' || "+-*/".Contains(c.ToString()))
            {
                tokens.Add(c.ToString());
                i++;
            }
            else
            {
                // Skip other characters like spaces
                i++;
            }
        }

        return tokens;
    }


    int EvaluateExpression(List<string> tokens)
    {
        Stack<int> numStack = new Stack<int>();
        Stack<string> opStack = new Stack<string>();

        foreach (var token in tokens)
        {
            if (int.TryParse(token, out int num))
            {
                numStack.Push(num);
            }
            else if ("+-*/".Contains(token))
            {
                while (opStack.Count > 0 && Precedence(opStack.Peek()) >= Precedence(token) && numStack.Count >= 2)
                {
                    numStack.Push(PerformOperation(opStack.Pop(), numStack.Pop(), numStack.Pop()));
                }
                opStack.Push(token);
            }
            else if (token == "(")
            {
                opStack.Push(token);
            }
            else if (token == ")")
            {
                while (opStack.Count > 0 && opStack.Peek() != "(" && numStack.Count >= 2)
                {
                    numStack.Push(PerformOperation(opStack.Pop(), numStack.Pop(), numStack.Pop()));
                }

                if (opStack.Count > 0 && opStack.Peek() == "(")
                {
                    opStack.Pop(); // Pop the "("
                }
                else
                {
                    // Handle mismatched parentheses or other error
                    return 0;
                }
            }
        }

        while (opStack.Count > 0 && numStack.Count >= 2)
        {
            numStack.Push(PerformOperation(opStack.Pop(), numStack.Pop(), numStack.Pop()));
        }

        if (numStack.Count == 1)
        {
            return numStack.Pop();
        }
        else
        {
            // Handle an error due to mismatched operators or other issues
            return 0;
        }
    }

    int PerformOperation(string op, int b, int a)
    {
        Debug.Log(op + ", " + b + ", " + a);
        switch (op)
        {
            case "+":
                return a + b;
            case "-":
                return a - b;
            case "*":
                return a * b;
            case "/":
                return a / b;
            default:
                return 0;
        }
    }

    int Precedence(string op)
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
                return 2;
            default:
                return 0;
        }
    }

    public void UpdateNumbers(GoldDragMinigame targetGO = null, bool decrease = false)
    {
        SetNumbers(ref currentIndex, targetGO, decrease);
    }

    private void SetNumbers(ref int index, GoldDragMinigame targetGO = null, bool decrease = false)
    {
        if (index > 4 && !decrease || index < 0 && decrease) return;
        switch (index)
        {
            case 0:
                if (!decrease)
                    Deposit(targetGO, ref ans1, ref index);
                break;
            case 1:
                if (!decrease)
                    Deposit(targetGO, ref ans2, ref index);
                else
                    Withdraw(ref ans1, ref index);
                break;
            case 2:
                if (!decrease)
                    Deposit(targetGO, ref ans3, ref index);
                else
                    Withdraw(ref ans2, ref index);
                break;
            case 3:
                if (!decrease)
                    Deposit(targetGO, ref ans4, ref index);
                else
                    Withdraw(ref ans3, ref index);
                break;
            case 4:
                if (decrease)
                    Withdraw(ref ans4, ref index);
                break;
        }
    }

    private void Deposit(GoldDragMinigame targetGO, ref int _targetInt, ref int index)
    {
        AudioController.Instance.PlayFX("GoldDrop");
        targetGO.gameObject.SetActive(false);
        goldStore.Add(targetGO.gameObject);
        _targetInt = targetGO.goldValue;
        numbersSlot[index].SetNumbers(_targetInt);
        scaleSlot[index].SetActive(true);
        index++;
    }

    private void Withdraw(ref int _targetInt, ref int index)
    {
        if (_targetInt == 0) return;
        AudioController.Instance.PlayFX("Popup");
        index--;
        if (index < 0) index = 0;
        goldStore[index].SetActive(true);
        goldStore[index].GetComponent<GoldDragMinigame>().Reset();
        ResetToSpawn(goldStore[index].transform);
        goldStore.RemoveAt(index);

        _targetInt = 0;
        numbersSlot[index].SetNumbers(_targetInt);
        scaleSlot[index].SetActive(false);
    }

    private bool NoZeroValues()
    {
        foreach (var item in numbersSlot)
        {
            if (item.currentValue == 0)
                return false;
        }

        return true;
    }
}
