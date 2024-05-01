using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemySpinSystem_V2 : EnemyParentClass_V2
{
    [SerializeField, Header("スピンの秒間回数を設定してください[t/s]")]
    private float oneSpinSpeed = 2;

    public override void Initialized()
    {
        enableSpinSystem = false;
    }

    private void OnEnable()
    {
        Initialized();
    }

    /// <summary>
    /// SpinSystem中断
    /// </summary>
    public void BreakeSpinSystem() { enableSpinSystem = false; }
    // enableOneRotationオフ用フラグ
    private bool enableSpinSystem = false;


    /// <summary>
    /// １回転させる
    /// </summary>
    public IEnumerator SpinSystem()
    {
        enableSpinSystem = true;
        var ratation = Data.Rigidbody.rotation.eulerAngles;
        while (enableSpinSystem)
        {
            Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(true);

            var spinSpeed = 360.0f * oneSpinSpeed * Time.deltaTime;
            ratation.x = 0.0f; ratation.z = 0.0f;
            ratation.y += spinSpeed;
            Data.Rigidbody.rotation = Quaternion.Euler(ratation);

            yield return null;
        }
        Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(false);
    }
}