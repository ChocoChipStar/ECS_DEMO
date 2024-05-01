using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField]
    private Image buttonImage;

    private void Update()
    {
        var playerScript = PlayerScriptManager_V2.Instance;



        if (!playerScript.AttackSystem.IsAttack)
            buttonImage.color = new Vector4(40, 40, 40, 255);
        else
            buttonImage.color = new Vector4(245, 245, 245, 255);


    }
}
