using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (UISoundManager.Instance != null)
                UISoundManager.Instance.PlayClick();
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayHover();
    }
}
