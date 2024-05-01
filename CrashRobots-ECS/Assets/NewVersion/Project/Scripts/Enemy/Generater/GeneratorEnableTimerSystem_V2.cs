using UnityEngine;

public class GeneratorEnableTimerSystem_V2 : MonoBehaviour
{
    // EnemyGenerateSystem_V2�擾
    private EnemyGenerateSystem_V2 enemyGenerateSystem;

    [SerializeField, Header("�N�����鎞�Ԃ�ݒ肵�ĉ�����")]
    private float enableTime = 0.0f;
    // �N�����ԃ^�C�}�[
    private float enableTimeTimer = 0.0f;

    [SerializeField, Header("��񂾂��N���������Ƃ��̓`�F�b�N�����Ă�������")]
    private bool enableOneTime = false;

    void Update()
    {
        // ���Ԍo�߂��ĂȂ�������GenerateSystem�I�t�ɂ���return
        if (!ElapsedTimer()) { enemyGenerateSystem.enabled = false; return; }
        // ���Ԍo�߂�����N��
        enemyGenerateSystem.enabled = true;
        // �ŏ��̐��������Ă��Ȃ�������return
        if (!enemyGenerateSystem.firstGanarate) return;
        // �N����ɂ��̃V�X�e�����I�t�ɂ���
        if (enableOneTime) GetComponent<GeneratorEnableTimerSystem_V2>().enabled = false;
    }
    
    // ���Ԍo�߃t���O
    private bool ElapsedTimer()
    {
        enableTimeTimer += Time.deltaTime;
        var elpsedTimer = enableTimeTimer >= enableTime;
        return elpsedTimer;
    }
}
