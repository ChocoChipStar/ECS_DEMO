using UnityEngine;
using PlayerScripts = PlayerScriptsManager;

public class TrashCollisionChecker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CanBreakTrash"))
            PlayerScripts.Instance.swingHammerSystem.trashObj = other.gameObject.transform.root.gameObject;
        else
            PlayerScripts.Instance.swingHammerSystem.SetSwingHammerActive(false);
    }
}
