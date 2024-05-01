using System.Collections;
using UnityEngine;

public class EnemyMaterialManager_V2 : EnemyParentClass_V2
{
    [SerializeField, Header("normalMaterial���A�^�b�`���Ă�������")]
    private MaterialStruct normal = new MaterialStruct();

    [SerializeField, Header("���߂��Ă�Ԃ�Material���A�^�b�`���Ă�������")]
    private MaterialStruct stan   = new MaterialStruct();

    [SerializeField, Header("�U���Ԃ�Material���A�^�b�`���Ă�������")]
    private MaterialStruct attack = new MaterialStruct();

    [SerializeField, Header("�_���[�W���󂯂�����Material���A�^�b�`���Ă�������")]
    private MaterialStruct damage = new MaterialStruct();

    [SerializeField, Header("damageMaterial��\�������Ă������Ԃ�ݒ肵�Ă�������")]
    private float isDamageTime;
    // isDamageTime�p�^�C�}�[
    private float isDamageTimeTimer = 0.0f;

    [System.Serializable]
    public struct MaterialStruct
    {
        [SerializeField, Header("Material���A�^�b�`���Ă�������")]
        public Material material;
        // material�}������true
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
    /// defaulMaterial�ɍ����ւ�
    /// </summary>
    private void SeNormalMaterial()
    {
        if (IsMeshrendererNull()) return;
        if (GetFlagsTrue())     return;
        Data.SetMaterial(normal.material);
    }

    /// <summary>
    /// �e�t���O�������Ă��邩����
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
    /// �e�t���O��false
    /// </summary>
    private void SetFlagsFalse()
    {
        stan    .isFlag = false;
        attack  .isFlag = false;        
    }


    /// <summary>
    /// StanMaterial�ɍ����ւ�
    /// </summary>
    public void EnableStanMaterial() { NullCheckInsertMaterial(ref stan); }


    /// <summary>
    /// StanMaterial�ɍ����ւ�
    /// </summary>
    public void EnableAttackMaterial() { NullCheckInsertMaterial(ref attack); }


    /// <summary>
    /// StanMaterial�ɍ����ւ�
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
    /// Meshrenderer�����邩�`�F�b�N����
    /// </summary>
    /// <returns> true -> MeshRenderer��null </returns>
    private bool IsMeshrendererNull()
    {
        if (Data.MeshRenderer == null)  return true;
        else                            return false;
    }

    /// <summary>
    /// �}�e���A�������ւ�
    /// </summary>
    /// <param name="materialStruct"> MaterialStruct�^�̍\���̂����Ă������� </param>
    private void InsertMaterial(ref MaterialStruct materialStruct)
    {
        Data.SetMaterial(materialStruct.material);
        materialStruct.isFlag = true;
    }

    /// <summary>
    /// null�`�F�b�N�����˂������ւ�
    /// </summary>
    /// <param name="materialStruct"> MaterialStruct�^�̍\���̂����Ă������� </param>
    private void NullCheckInsertMaterial(ref MaterialStruct materialStruct)
    {
        if (IsMeshrendererNull()) return;
        InsertMaterial(ref materialStruct);
    }

}
