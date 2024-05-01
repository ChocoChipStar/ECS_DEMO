using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public int groundNo { get; set; }

    GameObject[] tagObjects;

    BossGenerator bossGenerator;
    //[SerializeField]
    //GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { // éGãõìGÇ™è¡ñ≈ÇµÇΩÇÁ
            Destroy();
        }
    }

    public void Destroy()
    {
        for(int i = 0; i < BossGenerator.instance.Totems.Count; ++i)
        {
            if (BossGenerator.instance.Totems[i].panel == groundNo)
            {
                BossGenerator.instance.Totems.RemoveAt(i);
                break;
            }
        }
        Destroy(gameObject);
    }
}
