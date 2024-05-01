using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollNumberTest : MonoBehaviour
{
    [SerializeField]
    private int number;

    private int stageSequentalOrder;
    public int StageSequentalOrder { get { return stageSequentalOrder; } }

    private void Awake()
    {
        stageSequentalOrder = number;
    }

    public void SetNumber(int value)
    {
        number += value;
    }

    public int GetNumber()
    {
        return number;
    }
}
