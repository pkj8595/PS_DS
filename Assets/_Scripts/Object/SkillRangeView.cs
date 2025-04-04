using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeView : MonoBehaviour
{
    public Transform _ableRange;
    public Transform _noneAbleRange;

    public void SetRange(float min, float max)
    {
        _ableRange.localScale = Vector3.one * 2 * max;
        _noneAbleRange.localScale = Vector3.one * 2 * min;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
