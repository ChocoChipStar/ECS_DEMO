using UnityEngine;
using UnityEngine.InputSystem;

public class InvinibleTest : MonoBehaviour
{
    EnemyScriptManager_V2 enemyManager;

    void Start()
    {
        enemyManager = GetComponent<EnemyScriptManager_V2>();
    }

    void Update()
    {
        var current = Keyboard.current;
        if (current.tKey.wasPressedThisFrame)
        {
            enemyManager.Enemy.StanSystem.EnabledStan();
        }
    }
}
