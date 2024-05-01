using TMPro;
using UnityEngine;
using PlayerData = PlayerDataManager_V2;

public class StageScroller_V2 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stageParts = new GameObject[3];

    private GameObject enemyObject;

    [SerializeField]
    TextMeshProUGUI stageCountText;
    public float speed = 5.0f;

    public static int stagecount = 1;

    Vector3 enemyPos;

    [SerializeField]
    private BossGenerator bossGenerator;

    private void Start()
    {
        //Debug.Log("StageCount : " + stagecount);

        enemyObject = GameObject.FindWithTag("Totem");

        //for (int i = 0; i < stageParts.Length; ++i)
        //{
        //    stageParts[i].GetComponent<GroundNo>().no = i + 1;
        //    TotemGenerate(i, i + 1);
        //}
    }

    private void ScrollSystem()
    {
        //enemyPos = enemyObject.transform.position;

        //stageCountText.text = "Stagecount : " + stagecount.ToString();

        for (int i = 0; i < stageParts.Length; ++i)
        {
            Vector3 stagePos = stageParts[i].transform.position;
                stagePos.z -= speed * Time.deltaTime;
                stageParts[i].transform.position = stagePos;

            if (stagePos.z <= -112)
            {
                stagePos.z = 224f;
                stageParts[i].transform.position = stagePos;
                StageCount();

                // stageParts[i].GetComponent<GroundNo>().no += 3;

                // TotemGenerate(i, stageParts[i].GetComponent<GroundNo>().no);
            }
        }
    }

    void StageCount()
    {
        stagecount++;
        //Debug.Log("StageCount : " + stagecount);
    }

    private void Update()
    {
        ScrollSystem();
    }

    void TotemGenerate(int groundIndex, int no)
    {
        for (int n = 0; n < bossGenerator.Totems.Count; ++n)
        {
            if (no == bossGenerator.Totems[n].panel)
            {
                bossGenerator.EnemyGenerate(groundIndex, bossGenerator.Totems[n].panel);
                break;
            }
        }
        if (BossGenerator.bossOn)
        {
            if (no == bossGenerator.BossPanel)
            {
                bossGenerator.BossGenerate(groundIndex);
            }
        }

    }
}
