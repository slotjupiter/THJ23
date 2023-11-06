using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using THJ;
using UnityEngine;

public class MinigamesSystem : MonoBehaviour
{
    [TabGroup("Organs")] public OrgansMinigame organsMinigame;
    [TabGroup("Organs")] public ItemSO organsKey;

    public bool organsWin { get; set; } = false;

}
