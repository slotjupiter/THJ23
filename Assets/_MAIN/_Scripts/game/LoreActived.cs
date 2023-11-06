using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THJ
{
    public class LoreActived : MonoBehaviour
    {
        public List<GameObject> loreBorder;
        public GameObject blockBG;
        public List<GameObject> cancleButton;


        public void ActiveBorder()
        {
            StartCoroutine(ActiveOBJ());
        }

        IEnumerator ActiveOBJ()
        {
            for (int i = 0; i < loreBorder.Count; i++)
            {
                loreBorder[i].SetActive(true);
                cancleButton[i].SetActive(true);
                blockBG.SetActive(true);
                AudioController.Instance.PlayFX("Popup");
                yield return new WaitForSeconds(0.75f);
            }
        }
        public void CloseBorder()
        {
            AudioController.Instance.PlayFX("Popup");

            foreach (var item in loreBorder)
            {
                item.SetActive(false);
            }

            foreach (var item in cancleButton)
            {
                item.SetActive(false);
            }

            blockBG.SetActive(false);
        }
    }

}
