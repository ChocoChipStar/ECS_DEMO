using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    private GameObject generateFieldObj;

    private const float ONE_ROTATION_VALUE = 360.0f;

    private void Start()
    {
        generateFieldObj = transform.GetChild(0).gameObject;
        GenerateSystem();
    }

    private void GenerateSystem()
    {
        var generateManager = GenerateManager.Instance;

        for (int generateCount = 0; generateCount < generateManager.GetGenerateLimit(); ++generateCount)
        {
            Instantiate (
                generateManager.GetTrashPrefab(generateManager.GetRandomValue()),
                new Vector3(GetRandomCirclePosition().x,0.0f, GetRandomCirclePosition().y),
                Quaternion.Euler(0.0f, GetRandomRotationY(),0.0f)
            );
        }
    }

    private Vector2 GetRandomCirclePosition()
    {
        return generateFieldObj.transform.localScale.x / 2 * Random.insideUnitCircle;
    }

    private float GetRandomRotationY()
    {
        return Random.Range(0.0f, ONE_ROTATION_VALUE);
    }
}
