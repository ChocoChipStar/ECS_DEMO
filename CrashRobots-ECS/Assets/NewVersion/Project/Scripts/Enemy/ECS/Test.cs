using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;

    private float moveSpeed = 1.0f;

    void Update()
    {
        var keyCurrent = Keyboard.current;

        if (keyCurrent.wKey.isPressed)
            rigidBody.velocity += new Vector3(0.0f, 0.0f, moveSpeed) * Time.deltaTime;

        if (keyCurrent.aKey.isPressed)
            rigidBody.velocity += new Vector3(-moveSpeed, 0.0f, 0.0f) * Time.deltaTime;

        if (keyCurrent.sKey.isPressed)
            rigidBody.velocity += new Vector3(0.0f, 0.0f, -moveSpeed) * Time.deltaTime;

        if (keyCurrent.dKey.isPressed)
            rigidBody.velocity += new Vector3(moveSpeed, 0.0f, 0.0f) * Time.deltaTime;
    }
}
