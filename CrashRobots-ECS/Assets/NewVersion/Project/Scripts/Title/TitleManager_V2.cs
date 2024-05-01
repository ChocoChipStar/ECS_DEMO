using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager_V2 : MonoBehaviour
{
    [SerializeField]
    private Image descriptionImage;

    private bool isDescription = false;

    private bool isWait = false;

    // Start is called before the first frame update
    void Start()
    {
        isDescription = false;
        isWait = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        isWait = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWait)
            return;

        if(isDescription)
        {
            descriptionImage.enabled = true;

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
                LoadScene();

            if (Gamepad.current == null)
                return;

            if (Gamepad.current.bButton.wasReleasedThisFrame)
                LoadScene();

            return;
        }


        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            isDescription = true;

        if (Gamepad.current == null)
            return;

        if (Gamepad.current.bButton.wasReleasedThisFrame)
            isDescription = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
