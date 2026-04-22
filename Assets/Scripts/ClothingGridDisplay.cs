using UnityEngine;
using UnityEngine.UI;

public class ClothingGridDisplay : MonoBehaviour
{
    [Header("Slot Images")]
    public Image[] slotImages;

    [Header("Slot Buttons")]
    public Button[] slotButtons;

    private ClothingItemData[] currentItems;
    private int page = 0;
    const int PAGE_SIZE = 4;

    void Awake()
    {
        for (int i = 0; i < PAGE_SIZE; i++)
        {
            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
            {
                slotImages[i].enabled = false;
                slotImages[i].raycastTarget = false; // IMAGE MUST NOT BLOCK THE BUTTON
            }

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
                slotButtons[i].interactable = false;
        }
    }

    public void LoadCategory(ClothingItemData[] items)
    {
        currentItems = items;
        page = 0;
        Refresh();
    }

    public void TurnPage(int dir)
    {
        if (currentItems == null) return;
        int maxPage = Mathf.Max(0, (currentItems.Length - 1) / PAGE_SIZE);
        page = Mathf.Clamp(page + dir, 0, maxPage);
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < PAGE_SIZE; i++)
        {
            int dataIndex = page * PAGE_SIZE + i;
            bool hasItem = currentItems != null && dataIndex < currentItems.Length;

            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
            {
                slotImages[i].raycastTarget = false; // always off - button handles clicks
                slotImages[i].enabled = hasItem;
                slotImages[i].sprite = hasItem ? currentItems[dataIndex].previewSprite : null;
                if (hasItem) slotImages[i].preserveAspect = true;
            }

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
            {
                slotButtons[i].interactable = hasItem;
                slotButtons[i].onClick.RemoveAllListeners();

                if (hasItem)
                {
                    int captured = dataIndex;
                    slotButtons[i].onClick.AddListener(() => OnItemClicked(captured));
                }
            }
        }
    }

    void OnItemClicked(int dataIndex)
    {
        if (currentItems == null || dataIndex >= currentItems.Length) return;
        FindObjectOfType<CharacterCreationManager>().EquipByName(currentItems[dataIndex].rigMeshName);
    }

    public void Clear()
    {
        currentItems = null;
        page = 0;

        for (int i = 0; i < PAGE_SIZE; i++)
        {
            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
            {
                slotImages[i].enabled = false;
                slotImages[i].sprite = null;
            }

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
            {
                slotButtons[i].interactable = false;
                slotButtons[i].onClick.RemoveAllListeners();
            }
        }
    }
}