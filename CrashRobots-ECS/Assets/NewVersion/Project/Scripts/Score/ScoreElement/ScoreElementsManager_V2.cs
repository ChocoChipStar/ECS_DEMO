using UnityEngine;

public class ScoreElementsManager_V2 : MonoBehaviour
{
    [SerializeField, Header("scoreElementObjectプレハブをアタッチしてください")]
    private Transform scoreElementObject;
    [SerializeField, Header("ScrapUIDisplayをアタッチしてください")]
    private ScoreElementUIDisplaySysem_V2 scoreElementUIDisplay;

    // 必要オブジェクト生成
    public Transform ScoreElementGenerateSystem(Vector3 position)
    {
        // ScoreElement生成
        var scrapClone = Instantiate(scoreElementObject, position, Quaternion.identity);
        // ScoreElement初期化
        var scrapCloneScript = scrapClone.GetComponent<ScoreElementObject_V2>();
        var scoreUI = ScoreManager_V2.Instance.scoreUI.rectTransform;
        scrapCloneScript.Initialize(scoreUI);
        // scoreElementUI生成、初期化
        var marker = Instantiate(scoreElementUIDisplay, ScoreManager_V2.Instance.canvas);
        marker.Initialize(scrapClone);

        return scrapClone;
    }
}
