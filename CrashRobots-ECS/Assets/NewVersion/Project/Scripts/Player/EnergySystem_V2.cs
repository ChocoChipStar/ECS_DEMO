using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class EnergySystem_V2 : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのエネルギー値")]
    private float energyPoint;
    public float EnergyPoint {  get { return energyPoint; } }

    [SerializeField]
    private float maxEnergyPoint;

    [SerializeField]
    private TextMeshProUGUI energyText;

    [SerializeField]
    private Slider energySlider;

    private const float CONSUME_ATTACK_VALUE = 10.0f;

    private const float RESTORE_TIME = 1.0f;
    private const float ONE_RESTORE_VALUE = 5.0f;
    private const float CHARGE_PERCANTAGE = 33.3f;

    private PlayerDataManager_V2 playerData;

    private void Awake()
    {
        playerData = PlayerDataManager_V2.Instance;

        StartCoroutine(RestoreEnergy());    
    }

    public bool AttackConsumeEnergy()
    {
        if(EnergyPoint - CONSUME_ATTACK_VALUE < 0.0f)
            return false;

        energyPoint += -CONSUME_ATTACK_VALUE;
        energyText.SetText("EN:" + (int)EnergyPoint);
        energySlider.value = playerData.ConvertPercentageValue(EnergyPoint);

        return true;
    }

    public bool ChargeAttackConsumeEnergy(float chargeTime)
    {
        if (EnergyPoint + ConsumeValue(chargeTime) < 0.0f)
            return false;

        energyPoint += ConsumeValue(chargeTime);
        energyText.SetText("EN:" + (int)EnergyPoint);
        energySlider.value = playerData.ConvertPercentageValue(EnergyPoint);

        return true;
    }

    private float ConsumeValue(float chargeValue)
    {
        return -chargeValue * CHARGE_PERCANTAGE;
    }

    private IEnumerator RestoreEnergy()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(RESTORE_TIME);

            if (EnergyPoint + ONE_RESTORE_VALUE < maxEnergyPoint)
                energyPoint += ONE_RESTORE_VALUE;
            else
                energyPoint = maxEnergyPoint;

            energyText.SetText("EN:" + (int)EnergyPoint);
            energySlider.value = playerData.ConvertPercentageValue(EnergyPoint);
        }
    }
}
