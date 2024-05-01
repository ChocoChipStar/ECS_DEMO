using UnityEngine;

public class ChangeFromPeaceToScrap : MonoBehaviour
{
    // Peace�̐e�擾
    [SerializeField, Header("Peace�̐e���A�^�b�`���Ă�������")]
    private Transform Peace;

    // �X�N���b�v�ɕς��܂ł̎���
    private float changeTime = 2;

    private void Update()
    {
        if (!ElapsedChangeTime(changeTime)) return;
        ChangeSystem();
    }

    // �X�N���b�v�ɕς��܂ł̃^�C�}�[
    private float ChangeTimeTimer = 0;
    /// <summary>
    /// ��ꂽ��Z�b�g�������Ԃ��o�߂����true������
    /// </summary>
    /// <returns></returns>
    private bool ElapsedChangeTime(float destroyTime)
    {
        if (!Peace.gameObject.activeSelf) return false;

        ChangeTimeTimer += Time.deltaTime;
        // destroyTime�o�߃t���O
        bool elapsedDestroyTime = destroyTime <= ChangeTimeTimer;

        return elapsedDestroyTime;
    }

    /// <summary>
    /// �X�N���b�v���I�u�W�F�N�g�����������㎩��
    /// </summary>
    private void ChangeSystem()
    {
        if (!Peace.gameObject.activeSelf) return;

        for (int i = 0; i < Peace.childCount; ++i)
        {
            var PeaceChild = Peace.GetChild(i);
            ScrapGenerate.instance.ScrapUIGenerateSystem(PeaceChild.position);
        }

        Destroy(gameObject);
    }
}
