using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkinProperties : MonoBehaviour
{
    [TabGroup("Face")][SpineSkin] public string aliveSkin;
    [TabGroup("Face")][SpineSkin] public string halfSkin;
    [TabGroup("Face")][SpineSkin] public string deceaseSkin;
    [TabGroup("Face")][SerializeField] private SkeletonGraphic spineObject;

    public void SetSkin(string skinName)
    {
        if (spineObject != null)
        {
            spineObject.Skeleton.SetSkin(skinName);
            spineObject.Skeleton.SetSlotsToSetupPose();
            spineObject.LateUpdate();
        }

    }
}
