using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCube : MonoBehaviour
{
    GameObject[] tagObjects;

    //[SerializeField]
    //GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (tagObjects.Length == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
