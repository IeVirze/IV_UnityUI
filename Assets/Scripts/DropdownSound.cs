using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(_ =>
        {
            if (UISoundManager.Instance != null)
                UISoundManager.Instance.PlayDropdown();
        });
    }
}