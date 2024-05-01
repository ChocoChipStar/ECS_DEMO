using System.Collections;
using UnityEngine;

public class HitStopSystem : MonoBehaviour
{
    /// <summary>
    /// ヒットストップを実行します
    /// </summary>
    /// <param name="stopTime">ヒットストップの静止時間を代入</param>
    public void StartHitStop(float stopTime)
    {
        StartCoroutine(HitStopAction(stopTime));
    }

    /// <summary>
    /// ヒットストップの処理を行います
    /// </summary>
    /// <param name="stopTime">ヒットストップの静止時間を代入</param>
    /// <returns></returns>
    private IEnumerator HitStopAction(float stopTime)
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(stopTime);

        Time.timeScale = 1;
    }
}
