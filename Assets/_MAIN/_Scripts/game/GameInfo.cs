using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace THJ
{
    public class GameInfo : MonoBehaviour
    {
        [ReadOnly, TabGroup("Info")] public int CurrentDiceValue { get => currentDiceValue; }
        [SerializeField] private int currentDiceValue;

        [ReadOnly, TabGroup("Info")] public int MovementRange { get => movementRange; }
        [SerializeField] private int movementRange = 1;

        [TabGroup("Info")] public float MoveSpeed = 3f;
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool isMoving { get; set; }

        public GameObject characterPrefab;

        private void Start()
        {
            isMoving = false;
        }

        public void SetDiceValue(int value)
        {
            if (value > 0)
                currentDiceValue = value;
        }

        public void SetMovementRange(int value)
        {
            if (value > 0)
                movementRange = value;
        }
    }
}

