using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyCube : MonoBehaviour
{
    private static readonly string[] ALPHABETS = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    [SerializeField]
    TextMeshPro enemyNumber;
    //[SerializeField]
    //GameObject explosionPrefab;

    string alphabet;

    BossGenerator bossGenerator;
    void Start()
    {
        bossGenerator = GetComponent<BossGenerator>();
        //ALPHABETS‚Ì’†‚©‚çˆê‚Â‚ðƒ‰ƒ“ƒ_ƒ€‚ÅŽæ“¾‚·‚é
        alphabet = ALPHABETS.ElementAt(Random.Range(0, ALPHABETS.Count()));

        Debug.Log(alphabet);
        enemyNumber.text = alphabet;
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Input.GetKey(KeyCode.A + alphabet[0] - 'A'))
            {
                //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                //bossGenerator.StageActivate();
            }
        }
    }
}
