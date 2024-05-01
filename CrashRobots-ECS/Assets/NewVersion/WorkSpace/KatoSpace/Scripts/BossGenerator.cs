using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Panel
{
    public GameObject Object;
    public int panel;
}
public class BossGenerator : MonoBehaviour
{
    public static BossGenerator instance;

    public GameObject BossObj;
    public int BossPanel;

    public List<Panel> Totems;

    public GameObject[] parentObj = new GameObject[3];

    StageScroller_V2 stagescript;

    Vector3 totemPos;

    GameObject[] tagObj;

    GameObject bossObj;

    public bool isStopped;
    public static bool bossOn = false;
    private void Awake()
    {
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

    // Start is called before the first frame update
    void Start()
    {
        stagescript = GetComponent<StageScroller_V2>();
        bossObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        isStopped = false;
        tagObj = GameObject.FindGameObjectsWithTag("Totem");
        for(int i = 0; i < tagObj.Length; ++i)
        {
            totemPos = tagObj[i].transform.position;

            if (totemPos.z <= 0)
            {
                stagescript.enabled = false;
                isStopped = true;
                break;
            }

        }
        if (bossObj != null)
        {
            if (bossObj.transform.position.z <= 0)
            {
                stagescript.enabled = false;
                isStopped = true;
            }
        }
        if (!isStopped)
            StageActivate();

        if(Totems.Count == 0)
        {
            bossOn = true;
        }

            //if (tagObj.Length >= 1)
            //{
            //    enemyPos = enemyObj.transform.position;

            //    if (enemyPos.z <= 0)
            //    {
            //        stagescript.enabled = false;
            //    }

            //}
            //else
            //{
            //    StageActivate();
            //}
        }

    public void EnemyGenerate(int parts, int groundNo)
    {
        GameObject totemObj;
        totemObj = Instantiate(Totems[parts].Object);
        totemObj.transform.parent = parentObj[parts].transform;
        totemObj.transform.localPosition = Vector3.zero;
        totemObj.GetComponent<Totem>().groundNo = groundNo;
    }
    public void BossGenerate(int parts)
    {
        bossObj = Instantiate(BossObj);
        bossObj.transform.parent = parentObj[parts].transform;
        bossObj.transform.localPosition = Vector3.zero;
    }

    public void StageActivate()
    {
        stagescript.enabled = true;
    }
}
