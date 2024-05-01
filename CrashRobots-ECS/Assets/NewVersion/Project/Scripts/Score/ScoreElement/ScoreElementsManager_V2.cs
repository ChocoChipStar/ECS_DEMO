using UnityEngine;

public class ScoreElementsManager_V2 : MonoBehaviour
{
    [SerializeField, Header("scoreElementObject�v���n�u���A�^�b�`���Ă�������")]
    private Transform scoreElementObject;
    [SerializeField, Header("ScrapUIDisplay���A�^�b�`���Ă�������")]
    private ScoreElementUIDisplaySysem_V2 scoreElementUIDisplay;

    // �K�v�I�u�W�F�N�g����
    public Transform ScoreElementGenerateSystem(Vector3 position)
    {
        // ScoreElement����
        var scrapClone = Instantiate(scoreElementObject, position, Quaternion.identity);
        // ScoreElement������
        var scrapCloneScript = scrapClone.GetComponent<ScoreElementObject_V2>();
        var scoreUI = ScoreManager_V2.Instance.scoreUI.rectTransform;
        scrapCloneScript.Initialize(scoreUI);
        // scoreElementUI�����A������
        var marker = Instantiate(scoreElementUIDisplay, ScoreManager_V2.Instance.canvas);
        marker.Initialize(scrapClone);

        return scrapClone;
    }
}
