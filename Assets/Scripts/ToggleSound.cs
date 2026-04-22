using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(_ =>
        {
            if (UISoundManager.Instance != null)
                UISoundManager.Instance.PlayToggle();
        });
    }
}
