using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace THJ
{
    public class CharacterInfo : MonoBehaviour
    {
        [ReadOnly] public OverlayTile standingOnTile;
        [ReadOnly] public CurrentCharacterDirection currentCharacterDirection;
        [ReadOnly] public CurrentCharacterState currentCharacterState;

        public enum CurrentCharacterDirection
        {
            Left, Right, Forward, Back
        }

        public enum CurrentCharacterState
        {
            Idle, Move, Interact
        }

        [TabGroup("Front")] public GameObject playerFront;
        [TabGroup("Front"), SpineAnimation] public string frontIdleAnimation;
        [TabGroup("Front"), SpineAnimation] public string frontWalkAnimation;
        [TabGroup("Front"), SpineAnimation] public string frontInteractAnimation;
        [TabGroup("Back")] public GameObject playerBack;
        [TabGroup("Back"), SpineAnimation] public string backIdleAnimation;
        [TabGroup("Back"), SpineAnimation] public string backWalkAnimation;
        [TabGroup("Back"), SpineAnimation] public string backInteractAnimation;

        SkeletonAnimation frontAnimation;
        Spine.AnimationState frontState;
        SkeletonAnimation backAnimation;
        Spine.AnimationState backState;

        private void Start()
        {
            frontAnimation = playerFront.gameObject.GetComponent<SkeletonAnimation>();
            frontState = frontAnimation.AnimationState;
            backAnimation = playerBack.gameObject.GetComponent<SkeletonAnimation>();
            backState = backAnimation.AnimationState;
        }

        public void MoveAnimation(Vector2Int from, Vector2Int to, bool moving)
        {
            Vector2Int direction = new Vector2Int(to.x - from.x, to.y - from.y);

            if (direction == new Vector2Int(1, 0))
            {
                // Back animation
                if (currentCharacterState != CurrentCharacterState.Move && moving)
                {
                    currentCharacterDirection = CurrentCharacterDirection.Back;
                    currentCharacterState = CurrentCharacterState.Move;
                    MoveBack();
                }
            }
            else if (direction == new Vector2Int(-1, 0))
            {
                // Forward animation
                if (currentCharacterState != CurrentCharacterState.Move && moving)
                {
                    currentCharacterDirection = CurrentCharacterDirection.Forward;
                    currentCharacterState = CurrentCharacterState.Move;
                    MoveForward();
                }
            }
            else if (direction == new Vector2Int(0, 1))
            {
                // Left animation
                if (currentCharacterState != CurrentCharacterState.Move && moving)
                {
                    currentCharacterDirection = CurrentCharacterDirection.Left;
                    currentCharacterState = CurrentCharacterState.Move;
                    MoveLeft();
                }
            }
            else if (direction == new Vector2Int(0, -1))
            {
                // Right animation
                if (currentCharacterState != CurrentCharacterState.Move && moving)
                {
                    currentCharacterDirection = CurrentCharacterDirection.Right;
                    currentCharacterState = CurrentCharacterState.Move;
                    MoveRight();
                }
            }
            else if (!moving)
            {
                if (currentCharacterState != CurrentCharacterState.Idle)
                {
                    currentCharacterState = CurrentCharacterState.Idle;
                    switch (currentCharacterDirection)
                    {
                        case CurrentCharacterDirection.Left:
                            IdleLeft();
                            break;
                        case CurrentCharacterDirection.Right:
                            IdleRight();
                            break;
                        case CurrentCharacterDirection.Back:
                            IdleBack();
                            break;
                        case CurrentCharacterDirection.Forward:
                            IdleForward();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //* Forward

        public void IdleForward()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = 1;
            frontState.SetAnimation(0, frontIdleAnimation, true);
        }

        public void MoveForward()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = 1;
            frontState.SetAnimation(0, frontWalkAnimation, true);
        }

        public void InteractForward()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = 1;
            frontState.SetAnimation(0, frontInteractAnimation, false);
            frontState.AddAnimation(0, frontIdleAnimation, true, 0f);
        }

        //* Right

        public void IdleRight()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = -1;
            frontState.SetAnimation(0, frontIdleAnimation, true);
        }
        public void MoveRight()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = -1;
            frontState.SetAnimation(0, frontWalkAnimation, true);
        }

        public void InteractRight()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = -1;
            frontState.SetAnimation(0, frontInteractAnimation, false);
            frontState.AddAnimation(0, frontIdleAnimation, true, 0f);
        }

        //* Left
        public void IdleLeft()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = -1;
            backState.SetAnimation(0, backIdleAnimation, true);
        }
        public void MoveLeft()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = -1;
            backState.SetAnimation(0, backWalkAnimation, true);
        }

        public void InteractLeft()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = -1;
            backState.SetAnimation(0, backInteractAnimation, false);
            backState.AddAnimation(0, backIdleAnimation, true, 0f);
        }

        //* Back
        public void IdleBack()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = 1;
            backState.SetAnimation(0, backIdleAnimation, true);
        }
        public void MoveBack()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = 1;
            backState.SetAnimation(0, backWalkAnimation, true);
        }

        public void InteractBack()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = 1;
            backState.SetAnimation(0, backInteractAnimation, false);
            backState.AddAnimation(0, backIdleAnimation, true, 0f);
        }

    }
}
