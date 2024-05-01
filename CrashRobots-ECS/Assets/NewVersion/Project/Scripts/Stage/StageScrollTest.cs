using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TotemParams
{
    [SerializeField, Header("生成オブジェクト")]
    private GameObject generateObj;
    public GameObject GenerateObj { get { return generateObj; } }

    [SerializeField, Header("トーテムの生成パネルナンバー")]
    private int generatePanelNumber;
    public int GeneratePanelNumber { get {  return generatePanelNumber; } }

    [SerializeField, Header("トーテム破壊に必要な雑魚敵の数")]
    private int enableKillCount;
    public int EnableKillCount { get { return enableKillCount; } }
}

public class StageScrollTest : MonoBehaviour
{
    [SerializeField,Header("ステージのスクロール速度")]
    private float scrollSpeed;
    public float ScrollSpeed { get { return scrollSpeed; } }

    [SerializeField,Header("スクロール回数")]
    private float scrollCounter;
    
    [SerializeField]
    private List<TotemParams> totemParams;

    private int generateCounter;

    private bool isScroll = true;

    private const int PARTS_COUNT = 3;

    private GameObject[] generateObj = new GameObject[100];
    private int destroyCounter;

    [SerializeField]
    private GameObject[] stageParts = new GameObject[PARTS_COUNT];

    private const float SCROLL_MIN_POS = -70.0f;
    private const float SCROLL_MAX_POS = 210.0f;

    private int? currentKillCount;
    private int[] savedKillCount = new int[100];

    private int totemCount;

    private void Awake()
    {
        totemCount = totemParams.Count;

        for(int i = 0; i < totemParams.Count; ++i)
        {
            savedKillCount[i] = totemParams[i].EnableKillCount;
        }

        for (int i = 0; i < stageParts.Length; ++i)
        {
            stageParts[i].GetComponent<ScrollNumberTest>().SetNumber(i + 1);
        }
    }

    private void Update()
    {
        ScrollSystem();
        ScrollStopSystem();
    }

    private void ScrollSystem()
    {
        if (!isScroll)
            return;

        for(int i = 0; i < stageParts.Length; ++i)
        {
            var stagePos = stageParts[i].transform.position;

            stagePos.z += -scrollSpeed * Time.deltaTime;

            if (stagePos.z < SCROLL_MIN_POS)
            {
                stagePos.z += SCROLL_MAX_POS;

                stageParts[i].GetComponent<ScrollNumberTest>().SetNumber(PARTS_COUNT);

                GenerateSystem();

                ++scrollCounter;
            }

            stageParts[i].transform.position = stagePos;
        }
    }

    private void ScrollStopSystem()
    {
        if (generateObj[destroyCounter] == null)
            return;

        var position = generateObj[destroyCounter].transform.parent.position;

        if (position.z >= 0.0f)
            return;

        if (!currentKillCount.HasValue)
            currentKillCount = ScoreManager_V2.Instance.GetEnemyKillCount();

        isScroll = false;

        if (currentKillCount + savedKillCount[destroyCounter] > ScoreManager_V2.Instance.GetEnemyKillCount())
            return;

        Destroy(generateObj[destroyCounter]);
        ++destroyCounter;

        currentKillCount = null;
        isScroll = true;

        if (destroyCounter == totemCount)
            SceneManager.LoadScene("ResultScene");
    }

    public void GenerateSystem()
    {
        for (int i = 0; i < PARTS_COUNT; ++i)
        {
            for (int j = 0; j < totemParams.Count; ++j)
            {
                if (stageParts[i].GetComponent<ScrollNumberTest>().GetNumber() == totemParams[j].GeneratePanelNumber)
                {
                    generateObj[generateCounter] = Instantiate(
                        totemParams[0].GenerateObj,
                        stageParts[i].transform.position,
                        Quaternion.identity
                    );

                    generateObj[generateCounter].transform.SetParent(stageParts[i].transform);
                    generateObj[generateCounter].gameObject.name = "Totem" + generateCounter;

                    ++generateCounter;

                    totemParams.RemoveAt(0);

                    break;
                }
            }
        }
    }
}
