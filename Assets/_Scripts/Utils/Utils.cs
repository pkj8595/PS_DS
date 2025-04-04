using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public static class Utils
{

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go">최상위 부모</param>
    /// <param name="name">이름 없으면 비교하지 않음.</param>
    /// <param name="recursive">TRUE = 재귀적으로 찾을것인가 / FALSE = 하위만 찾을것인가</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive is false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }

    public static int CalculateTableNum(int tableNum)
    {
        return tableNum / 1000000;
    }

    public static int CalculateCategory(int tableNum)
    {
        return (tableNum / 1000) % 1000;
    }

    public static int CalculateTableBaseNumber(int tableNum)
    {
        return tableNum - (tableNum % 1000);
    }
    public static int CalculateTable(int tableNum, int category, int index)
    {
        return (tableNum * 1000000) + (category * 1000) + index;
    }

    /// <summary>
    /// 비트 연산자를 사용해 자리값이 1이 맞는지 확인한다.
    /// <param name="x">비교할 값</param>
    /// <param name="y">비교할 값</param>
    /// </returns>
    public static bool HasFlag(int x ,int y)
    {
        return (x & y) != 0;
    }

    /// <summary>
    /// 카메라 마우스 포지션에서 Ray를 쏘고 제일 먼저 맞는 worldPosition 반환
    /// </summary>
    /// <param name="action">Action<Vector3> </param>
    public static void GetMouseWorldPositionToRay(Action<Vector3> action)
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            action?.Invoke(hit.point);
        }
    }


    /// <summary>
    /// 반올림
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Round(float value)
    {
        if (value < 0)
        {
            return (int)(value - 0.5f); // 음수일 때는 -0.5를 더해줌
        }
        else
        {
            return (int)(value + 0.5f); // 양수일 때는 +0.5로 처리
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    /// <returns>float 범위 0f ~ 1f </returns>
    public static float Percent(float current, float max)
    {
        return (current != 0 && max != 0) ? current / max : 0;
    }

    public static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color ret))
        {
            return ret;
        }
        Debug.Log($"문자열 {hex}가 color로 치환될 수 없습니다.");
        return Color.white;
    }

    public static int[] StringToIntArray(string input)
    {
        string[] stringValues = input.Split(',');
        int[] intValues = new int[stringValues.Length];

        for (int i = 0; i < stringValues.Length; i++)
        {
            // 앞뒤 공백 제거 후 int로 변환하여 배열에 저장
            intValues[i] = int.Parse(stringValues[i].Trim());
        }
        return intValues;
    }

    private static string GetStatColor(int value, string str)
    {
        string ret;
        if (0 < value)
            ret = $"<color=#85b576>{str}</color>";
        else
            ret = $"<color=#eb2f3d>{str}</color>";
        return ret;
    }

    //두 벡터간 각도 산출
    public static float GetAngle(Vector3 a, Vector3 b)
    {
        float dot = Vector3.Dot(a.normalized, b.normalized); // 내적 계산
        return Mathf.Acos(dot) * Mathf.Rad2Deg; // 라디안을 도(degree)로 변환
    }
}
