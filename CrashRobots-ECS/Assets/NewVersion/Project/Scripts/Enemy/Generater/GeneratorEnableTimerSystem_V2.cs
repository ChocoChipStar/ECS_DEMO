using UnityEngine;

public class GeneratorEnableTimerSystem_V2 : MonoBehaviour
{
    // EnemyGenerateSystem_V2取得
    private EnemyGenerateSystem_V2 enemyGenerateSystem;

    [SerializeField, Header("起動する時間を設定して下さい")]
    private float enableTime = 0.0f;
    // 起動時間タイマー
    private float enableTimeTimer = 0.0f;

    [SerializeField, Header("一回だけ起動したいときはチェックを入れてください")]
    private bool enableOneTime = false;

    void Update()
    {
        // 時間経過してなかったらGenerateSystemオフにしてreturn
        if (!ElapsedTimer()) { enemyGenerateSystem.enabled = false; return; }
        // 時間経過したら起動
        enemyGenerateSystem.enabled = true;
        // 最初の生成をしていなかったらreturn
        if (!enemyGenerateSystem.firstGanarate) return;
        // 起動後にこのシステムをオフにする
        if (enableOneTime) GetComponent<GeneratorEnableTimerSystem_V2>().enabled = false;
    }
    
    // 時間経過フラグ
    private bool ElapsedTimer()
    {
        enableTimeTimer += Time.deltaTime;
        var elpsedTimer = enableTimeTimer >= enableTime;
        return elpsedTimer;
    }
}
