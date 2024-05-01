using System;
using UnityEngine;

public class EnemyFanction_V2 : MonoBehaviour
{
    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialized() { }


    /// <summary>
    /// GetConponentしたときにnullかどうかチェックしてしてくれます
    /// </summary>
    /// <typeparam name="T">クラス名</typeparam>
    /// <returns> true -> null || false -> not null </returns>
    protected bool IsComponentNull<T>() { return GetComponent<T>() == null; }


    /// <summary>
    /// refを使用して変数内に自動的にGetComponentしてくれる関数
    /// </summary>
    /// <typeparam name="T">取得したいクラス名</typeparam>
    /// <param name="script">Tと同型のクラス変数</param>
    protected void AutoGetEnemyCompornent<T>(ref T script)
    {
        if (IsComponentNull<T>()) return;
        script = GetComponent<T>();
    }


    /// <summary>
    /// EnemySystemuに限り自動的にシステムのオンオフを行う関数
    /// </summary>
    /// <typeparam name="T">EnemyParentClass_V2を基底クラスに持つクラス名</typeparam>
    /// <param name="script">T型の変数</param>
    /// <param name="enabled"> true -> System.enablde = true </param>
    protected void EnemySytemEnabled<T>(ref T script, bool enabled) where T : EnemyParentClass_V2
    {
        if (IsComponentNull<T>()) return;
        script.enabled = enabled;
    }


    /// <summary>
    /// Vector3を角度に直す関数
    /// </summary>
    /// <param name="vec">角度に直したいベクトル</param>
    /// <returns>角度(float)</returns>
    protected float Vec3ToAngle(Vector3 vec) { float angle = Mathf.Atan2(vec.x, vec.z) * Mathf.Rad2Deg; return angle; }


    /// <summary>
    /// 時間を計測します
    /// </summary>
    /// <param name="timer">timer変数</param>
    protected void Timer(ref float timer) { timer += Time.deltaTime; }


    /// <summary>
    /// 時間計測関数
    /// </summary>
    /// <param name="goalTime">目標到達時間</param>
    /// <param name="timer">timer変数</param>
    /// <returns>(bool) true -> 目標時間到達 || false -> 目標時間未到達</returns>
    protected bool TimerCount(float goalTime, ref float timer)
    {
        Timer(ref timer);

        if (goalTime >= timer) return false;

        return true;
    }


    /// <summary>
    /// 時間計測関数
    /// </summary>
    /// <param name="goalTime">目標到達時間</param>
    /// <param name="timer">timer変数(目標到達後0にリセットされる)</param>
    /// <returns>(bool) true -> 目標時間到達 || false -> 目標時間未到達</returns>
    protected bool TimerCountAndReset(float goalTime, ref float timer)
    {
        var timeCount = TimerCount(goalTime, ref timer);

        if(!timeCount) return timeCount;

        timer = 0;
        return timeCount;
    }


    /// <summary>
    /// 列挙型の変数の値を変更することができます
    /// </summary>
    /// <typeparam name="State"> enum型を入力してください </typeparam>
    /// <param name="i"> 取得したい配列の数字を入れてください </param>
    /// <returns> 指定された列挙体配列の数字 </returns>
    protected void ChangeState<State>(ref State state, int i) where State : Enum
    {
        state = (State)Enum.ToObject(typeof(State), i);
    }


    /// <summary>
    /// 配列の数字とStetaのValueの数字が同じであれば次の数字のState(最後のStetaならば最初)へ移行する
    /// </summary>
    /// <typeparam name="State"> Enum型のStateを入力してください </typeparam>
    /// <param name="state"> State型の変数を入力してください </param>
    protected void JumpNextState<State>(ref State state) where State : Enum
    {
        var stateLength = Enum.GetValues(typeof(State)).Length;

        var lastState = (State)Enum.ToObject(typeof(State), stateLength - 1);

        if (state.ToString() == lastState.ToString())
            state = (State)Enum.ToObject(typeof(State), 0);
        else
            state = (State)Enum.ToObject(typeof(State), +1);
    }
}