using UnityEngine;

public class ChangeFromPeaceToScrap : MonoBehaviour
{
    // Peaceの親取得
    [SerializeField, Header("Peaceの親をアタッチしてください")]
    private Transform Peace;

    // スクラップに変わるまでの時間
    private float changeTime = 2;

    private void Update()
    {
        if (!ElapsedChangeTime(changeTime)) return;
        ChangeSystem();
    }

    // スクラップに変わるまでのタイマー
    private float ChangeTimeTimer = 0;
    /// <summary>
    /// 壊れた後セットした時間が経過するとtrueを示す
    /// </summary>
    /// <returns></returns>
    private bool ElapsedChangeTime(float destroyTime)
    {
        if (!Peace.gameObject.activeSelf) return false;

        ChangeTimeTimer += Time.deltaTime;
        // destroyTime経過フラグ
        bool elapsedDestroyTime = destroyTime <= ChangeTimeTimer;

        return elapsedDestroyTime;
    }

    /// <summary>
    /// スクラップをオブジェクト分生成した後自壊
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
