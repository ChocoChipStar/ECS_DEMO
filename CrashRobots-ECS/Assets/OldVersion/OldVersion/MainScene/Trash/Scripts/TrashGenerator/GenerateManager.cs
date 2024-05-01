using UnityEngine;

public class GenerateManager : GenericSingleton<GenerateManager>
{

    [SerializeField, Header("ゴミ生成の上限値を設定")]
    private int assignGenerateLimit;

    private const int TRASH_COUNT = 3;

    [SerializeField]
    private GameObject[] trashPrefab = new GameObject[TRASH_COUNT];

    public int GetGenerateLimit()
    {
        return assignGenerateLimit;
    }

    public GameObject GetTrashPrefab(int number)
    {
        return trashPrefab[number].gameObject;
    }

    public int GetRandomValue()
    {
        return Random.Range(0, TRASH_COUNT);
    }
}