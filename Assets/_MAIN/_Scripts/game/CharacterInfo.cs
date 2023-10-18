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

        [TabGroup("Layer")] public int defaultOrder;
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

            SetSortingOrder(defaultOrder);
        }

        public void SetSortingOrder(int _order)
        {
            playerFront.gameObject.GetComponent<MeshRenderer>().sortingOrder = _order;
            playerBack.gameObject.GetComponent<MeshRenderer>().sortingOrder = _order;
        }

        public void MoveAnimation(Vector2Int from, Vector2Int to, bool moving)
        {
            Vector2Int direction = new Vector2Int(to.x - from.x, to.y - from.y);

            if (moving)
            {
                if (direction == new Vector2Int(1, 0))
                {
                    // Back animation
                    if (currentCharacterDirection != CurrentCharacterDirection.Back || currentCharacterState != CurrentCharacterState.Move)
                    {
                        currentCharacterState = CurrentCharacterState.Move;
                        currentCharacterDirection = CurrentCharacterDirection.Back;
                        MoveBack();
                    }
                }
                else if (direction == new Vector2Int(-1, 0))
                {
                    // Forward animation
                    if (currentCharacterDirection != CurrentCharacterDirection.Forward || currentCharacterState != CurrentCharacterState.Move)
                    {
                        currentCharacterDirection = CurrentCharacterDirection.Forward;
                        currentCharacterState = CurrentCharacterState.Move;
                        MoveForward();
                    }
                }
                else if (direction == new Vector2Int(0, 1))
                {
                    // Left animation
                    if (currentCharacterDirection != CurrentCharacterDirection.Left || currentCharacterState != CurrentCharacterState.Move)
                    {
                        currentCharacterState = CurrentCharacterState.Move;
                        currentCharacterDirection = CurrentCharacterDirection.Left;
                        MoveLeft();
                    }
                }
                else if (direction == new Vector2Int(0, -1))
                {
                    // Right animation
                    if (currentCharacterDirection != CurrentCharacterDirection.Right || currentCharacterState != CurrentCharacterState.Move)
                    {
                        currentCharacterState = CurrentCharacterState.Move;
                        currentCharacterDirection = CurrentCharacterDirection.Right;
                        MoveRight();
                    }
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

        private void IdleForward()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = 1;
            frontState.SetAnimation(0, frontIdleAnimation, true);
        }

        private void MoveForward()
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

        private void IdleRight()
        {
            if (!playerFront.activeSelf)
                playerFront.SetActive(true);
            playerBack.SetActive(false);

            frontAnimation.skeleton.ScaleX = -1;
            frontState.SetAnimation(0, frontIdleAnimation, true);
        }
        private void MoveRight()
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
        private void IdleLeft()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = -1;
            backState.SetAnimation(0, backIdleAnimation, true);
        }
        private void MoveLeft()
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
        private void IdleBack()
        {
            if (!playerBack.activeSelf)
                playerBack.SetActive(true);
            playerFront.SetActive(false);

            backAnimation.skeleton.ScaleX = 1;
            backState.SetAnimation(0, backIdleAnimation, true);
        }
        private void MoveBack()
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
