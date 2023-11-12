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

    [TabGroup("Head")] public HeadMinigame headMinigame;
    [TabGroup("Head")] public ItemSO headKey;
    public bool headWin { get; set; } = false;

    [TabGroup("Legs")] public LegsMinigame legsMinigame;
    [TabGroup("Legs")] public ItemSO legsKey;
    public bool legsWin { get; set; } = false;

    [TabGroup("Arms")] public ArmsMinigame armsMinigame;
    [TabGroup("Arms")] public ItemSO armsKey;
    public bool armsWin { get; set; } = false;


}
