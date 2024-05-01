using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;
using System.Collections;
using UnityEngine.UI;

public class HitPointSystem_V2 : MonoBehaviour
{
    [SerializeField,Header("プレイヤーのHP値")]
    private float hitPoint;
    public float HitPoint { get { return hitPoint; } }

    [SerializeField, Header("受けるダメージ量（エネミーの攻撃力）")]
    private float attackDamage;

    [SerializeField, Header("無敵時間を設定　(秒)")]
    private float invisibleTime;

    [SerializeField]
    private TextMeshProUGUI hitPointText;

    [SerializeField]
    private Slider hitPointSlider;

    [SerializeField]
    private MainSceneSoundEffectManager_V2 soundEffect;

    [Tooltip("無敵判定　true->無敵")]
    private bool isInvisible;

    private void Awake()
    {
        hitPointText.SetText("HP:" + HitPoint);
    }

    public void TakenDamage()
    {
        if (isInvisible)
            return;

        var playerData = PlayerData.Instance;

        hitPoint += -attackDamage;
        hitPointText.SetText("HP:" + HitPoint);
        hitPointSlider.value = playerData.ConvertPercentageValue(HitPoint);

        var playerScript = PlayerScript.Instance;

        StartCoroutine(playerScript.HitStopSystem.HitStop(playerData.DamageHitStopDuration, playerData.DamageHitStopStrengh, null));

        soundEffect.PlaySEPlayerDamaged();

        playerScript.CameraSystem.StartCoroutine(
            playerScript.CameraSystem.ShakeSystem(playerData.DamageShakeDuration, playerData.DamageShakeStrength)
            );

        StartCoroutine(InvisibleSystem());

        if (hitPoint <= 0)
            SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator InvisibleSystem()
    {
        isInvisible = true;

        yield return new WaitForSeconds(invisibleTime);

        isInvisible = false;
    }
}
