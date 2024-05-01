using UnityEngine;

public class SwingHammerSystem : MonoBehaviour
{
    private bool isSwingHammer;
    private bool isTrigger;

    public GameObject trashObj { private get; set; }

    [SerializeField]
    private ScoreManager scoreManeger;

    private void Update()
    {
        if (!GetSwingHammerActive())
            return;

        if (trashObj != null)
        {
            SoundManager.Instance.HitSound();
            //trashObj.gameObject.GetComponent<Trash>().ExplosionTrash(transform.position);
            trashObj.gameObject.GetComponent<AssultTrush>().AssultTrash(transform.position);
        }

        PlayerScriptsManager.Instance.swingHammerSystem.SetSwingHammerActive(false);
    }

    public bool GetSwingHammerActive()
    {
        return isSwingHammer;
    }

    public void SetSwingHammerActive(bool isActive)
    {
        isSwingHammer = isActive;
    }
}
