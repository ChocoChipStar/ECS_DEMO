using UnityEngine;

public class ScrapGenerate : MonoBehaviour
{
    // �V���O���g����
    public static ScrapGenerate instance;

    // Scrap�v���n�u�擾
    [SerializeField, Tooltip("Scrap�v���n�u���A�^�b�`���Ă�������")]
    private Transform scrapPrefab;
    // ScoreUI�擾
    [SerializeField, Tooltip("ScoreUI���A�^�b�`���ĉ�����")]
    private RectTransform scoreUI;
    // Canvas�擾
    [SerializeField, Tooltip("Canvas���A�^�b�`���Ă�������")]
    private RectTransform canvas;
    // ScrapUIDisplay�擾
    [SerializeField, Tooltip("ScrapUIDisplay���A�^�b�`���Ă�������")]
    private ScrapUIDisplay scrapUIDisplay;

    private void Awake()
    {
        // �e�q�֌W����
        transform.parent = null;
        // Canvas�̎q���ł͂Ȃ��Ȃ邽��Transform�ɐ؂�ւ�
        gameObject.AddComponent<Transform>();
        Destroy(GetComponent<RectTransform>());

        // �V���O���g����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �K�v�I�u�W�F�N�g����
    public Transform ScrapUIGenerateSystem(Vector3 position)
    {
        // Scrap����
        var scrapClone = Instantiate(scrapPrefab, position, Quaternion.identity);
        var scrapCloneScript = scrapClone.GetComponent<Scrap>();
        scrapCloneScript.Initialize(scoreUI);
        // ScrapUI�����A������
        var marker = Instantiate(scrapUIDisplay, canvas);
        marker.Initialize(scrapClone);

        return scrapClone;
    }
}
