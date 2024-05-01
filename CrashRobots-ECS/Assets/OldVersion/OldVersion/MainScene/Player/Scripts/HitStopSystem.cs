using System.Collections;
using UnityEngine;

public class HitStopSystem : MonoBehaviour
{
    /// <summary>
    /// �q�b�g�X�g�b�v�����s���܂�
    /// </summary>
    /// <param name="stopTime">�q�b�g�X�g�b�v�̐Î~���Ԃ���</param>
    public void StartHitStop(float stopTime)
    {
        StartCoroutine(HitStopAction(stopTime));
    }

    /// <summary>
    /// �q�b�g�X�g�b�v�̏������s���܂�
    /// </summary>
    /// <param name="stopTime">�q�b�g�X�g�b�v�̐Î~���Ԃ���</param>
    /// <returns></returns>
    private IEnumerator HitStopAction(float stopTime)
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(stopTime);

        Time.timeScale = 1;
    }
}
