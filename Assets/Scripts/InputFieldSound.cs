using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_InputField>().onSelect.AddListener(_ =>
        {
            if (UISoundManager.Instance != null)
                UISoundManager.Instance.PlayInputField();
        });
    }
}