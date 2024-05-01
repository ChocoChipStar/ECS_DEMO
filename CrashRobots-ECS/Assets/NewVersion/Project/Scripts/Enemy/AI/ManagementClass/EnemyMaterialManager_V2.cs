using System.Collections;
using UnityEngine;

public class EnemyMaterialManager_V2 : EnemyParentClass_V2
{
    [SerializeField, Header("normalMaterialをアタッチしてください")]
    private MaterialStruct normal = new MaterialStruct();

    [SerializeField, Header("よろめいてる間のMaterialをアタッチしてください")]
    private MaterialStruct stan   = new MaterialStruct();

    [SerializeField, Header("攻撃間のMaterialをアタッチしてください")]
    private MaterialStruct attack = new MaterialStruct();

    [SerializeField, Header("ダメージを受けた時のMaterialをアタッチしてください")]
    private MaterialStruct damage = new MaterialStruct();

    [SerializeField, Header("damageMaterialを表示させておく時間を設定してください")]
    private float isDamageTime;
    // isDamageTime用タイマー
    private float isDamageTimeTimer = 0.0f;

    [System.Serializable]
    public struct MaterialStruct
    {
        [SerializeField, Header("Materialをアタッチしてください")]
        public Material material;
        // material挿入中はtrue
        public bool isFlag;
    }


    public override void Initialized()
    {
        NullCheckInsertMaterial(ref normal);
        isDamageTimeTimer   = 0.0f;
        stan        .isFlag = false;
        attack      .isFlag = false;
        damage      .isFlag = false;
    }


    private void OnEnable()
    {
        Initialized();
    }

    private void OnDisable()
    {
        NullCheckInsertMaterial(ref normal);
    }

    void Update()
    {
        if (IsMeshrendererNull()) return;
        SeNormalMaterial();
        SetFlagsFalse();
    }


    /// <summary>
    /// defaulMaterialに差し替え
    /// </summary>
    private void SeNormalMaterial()
    {
        if (IsMeshrendererNull()) return;
        if (GetFlagsTrue())     return;
        Data.SetMaterial(normal.material);
    }

    /// <summary>
    /// 各フラグが立っているか判定
    /// </summary>
    /// <returns></returns>
    private bool GetFlagsTrue()
    {
        if (stan.   isFlag) return true;
        if (attack. isFlag) return true;
        if (damage. isFlag) return true;
        return false;
    }

    /// <summary>
    /// 各フラグをfalse
    /// </summary>
    private void SetFlagsFalse()
    {
        stan    .isFlag = false;
        attack  .isFlag = false;        
    }


    /// <summary>
    /// StanMaterialに差し替え
    /// </summary>
    public void EnableStanMaterial() { NullCheckInsertMaterial(ref stan); }


    /// <summary>
    /// StanMaterialに差し替え
    /// </summary>
    public void EnableAttackMaterial() { NullCheckInsertMaterial(ref attack); }


    /// <summary>
    /// StanMaterialに差し替え
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnableDamageMaterial() {
        if (IsMeshrendererNull()) yield break;
        isDamageTimeTimer = 0.0f;
        while (isDamageTimeTimer < isDamageTime) 
        {
            InsertMaterial(ref damage);
            isDamageTimeTimer += Time.deltaTime;

            yield return null;
        }
        damage.isFlag = false;
    }

    /// <summary>
    /// Meshrendererがあるかチェックする
    /// </summary>
    /// <returns> true -> MeshRendererがnull </returns>
    private bool IsMeshrendererNull()
    {
        if (Data.MeshRenderer == null)  return true;
        else                            return false;
    }

    /// <summary>
    /// マテリアル差し替え
    /// </summary>
    /// <param name="materialStruct"> MaterialStruct型の構造体を入れてください </param>
    private void InsertMaterial(ref MaterialStruct materialStruct)
    {
        Data.SetMaterial(materialStruct.material);
        materialStruct.isFlag = true;
    }

    /// <summary>
    /// nullチェックも兼ねた差し替え
    /// </summary>
    /// <param name="materialStruct"> MaterialStruct型の構造体を入れてください </param>
    private void NullCheckInsertMaterial(ref MaterialStruct materialStruct)
    {
        if (IsMeshrendererNull()) return;
        InsertMaterial(ref materialStruct);
    }

}
