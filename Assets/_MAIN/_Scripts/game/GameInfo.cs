using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace THJ
{
    public class GameInfo : MonoBehaviour
    {
        [ReadOnly] public int currentDiceValue;

        public void SetDiceValue(int value)
        {
            if (value > 0)
                currentDiceValue = value;
        }
    }
}

