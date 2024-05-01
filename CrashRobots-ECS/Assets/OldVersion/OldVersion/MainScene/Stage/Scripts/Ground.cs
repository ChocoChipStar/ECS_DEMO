using UnityEngine;

public class Ground_V2 : MonoBehaviour
{
    // Wall取得
    [SerializeField, Tooltip("Wallをアタッチしてください")]
    GameObject wallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 wallPos = Vector3.zero;
            Vector3 wallScale = Vector3.one;

            GameObject wall = Instantiate(wallPrefab, transform.position, Quaternion.identity);

            if (i % 2 == 0)
            {
                wallPos.z = transform.lossyScale.z / 2 + 0.5f;
                wallScale.x = transform.lossyScale.x;
                if (i >= 2) wallPos.z *= -1;
            }
            else
            {
                wallPos.x = transform.lossyScale.x / 2 + 0.5f;
                wallScale.z = transform.lossyScale.z;
                if (i >= 2) wallPos.x *= -1;
            }
            wallScale.y = 10.0f;
            wall.transform.position = wallPos; 
            wall.transform.localScale = wallScale;
        }
    }
}
