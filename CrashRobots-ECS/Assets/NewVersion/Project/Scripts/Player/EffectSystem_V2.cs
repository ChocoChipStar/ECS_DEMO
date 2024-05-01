using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectSystem_V2 : MonoBehaviour
{
    [SerializeField]
    private VisualEffect rotationEffect;

    public void Rotation()
    {
        rotationEffect.SendEvent("OnPlay");
    }
}
