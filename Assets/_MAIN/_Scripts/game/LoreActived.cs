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
            blockBG.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            for (int i = 0; i < loreBorder.Count; i++)
            {
                loreBorder[i].SetActive(true);
                AudioController.Instance.PlayFX("Popup");
                yield return new WaitForSeconds(0.75f);
            }
            foreach (var btn in cancleButton)
            {
                btn.SetActive(true);
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
