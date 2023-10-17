using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace THJ
{
    public class GameInfo : MonoBehaviour
    {
        [ReadOnly, ShowInInspector, TabGroup("Info")] public int CurrentDiceValue { get => currentDiceValue; }
        private int currentDiceValue;
        [ReadOnly, ShowInInspector, TabGroup("Info")] public int MovementRange { get; set; }

        [TabGroup("Info")] public float MoveSpeed = 3f;
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool isMoving { get; set; }
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool canRollDice { get; set; }

        [TabGroup("GameStatus"), ShowInInspector, ReadOnly] public bool gameStart { get; set; }


        public GameObject characterPrefab;

        public TileCursorSystem tileCursorSystem { get; set; }

        private void Awake()
        {
            gameStart = false;
            tileCursorSystem = FindObjectOfType<TileCursorSystem>();
        }

        private void Start()
        {
            isMoving = false;
            canRollDice = true;
        }

        public void SetDiceValue(int value)
        {
            if (value > 0)
                currentDiceValue = value;
        }

        public void SetMovementRange(int value)
        {
            MovementRange = value;
        }
    }
}

