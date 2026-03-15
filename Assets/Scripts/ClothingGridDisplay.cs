using UnityEngine;
using UnityEngine.UI;

public class ClothingGridDisplay : MonoBehaviour
{
    [Header("Slot Raw Images")]
    public RawImage[] slotImages;

    [Header("Slot Buttons")]
    public Button[] slotButtons;

    [Header("Preview Cameras")]
    public Camera[] previewCameras;

    [Header("Render Textures")]
    public RenderTexture[] previewTextures;

    [Header("Preview Layer")]
    public string previewLayer = "ClothingPreview";

    private GameObject[] currentItems;
    private GameObject[] stagedMeshes;
    private int page = 0;
    const int PAGE_SIZE = 4;

    void Awake()
    {
        stagedMeshes = new GameObject[PAGE_SIZE];

        for (int i = 0; i < PAGE_SIZE; i++)
        {
            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
            {
                slotImages[i].texture = previewTextures != null && i < previewTextures.Length
                    ? previewTextures[i] : null;
                slotImages[i].enabled = false;
            }

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
                slotButtons[i].interactable = false;

            if (previewCameras != null && i < previewCameras.Length && previewCameras[i] != null)
                previewCameras[i].enabled = false;
        }
    }

    public void LoadCategory(GameObject[] items)
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
        ClearStage();

        for (int i = 0; i < PAGE_SIZE; i++)
        {
            int dataIndex = page * PAGE_SIZE + i;
            bool hasItem  = currentItems != null && dataIndex < currentItems.Length;

            if (slotImages  != null && i < slotImages.Length  && slotImages[i]  != null)
                slotImages[i].enabled = hasItem;

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
            {
                slotButtons[i].interactable = hasItem;
                slotButtons[i].onClick.RemoveAllListeners();
            }

            if (hasItem)
            {
                StageItem(i, currentItems[dataIndex]);

                int captured = dataIndex;
                slotButtons[i].onClick.AddListener(() => OnItemClicked(captured));
            }
        }
    }

    void StageItem(int slot, GameObject source)
    {
        if (source == null) return;
        if (previewCameras == null || slot >= previewCameras.Length || previewCameras[slot] == null) return;

        Vector3 stagePos = new Vector3(2000 + slot * 5, 0, 0);
        previewCameras[slot].transform.position = new Vector3(stagePos.x, stagePos.y, stagePos.z - 3f);
        previewCameras[slot].enabled = true;

        GameObject copy = Instantiate(source, stagePos, Quaternion.identity);
        copy.SetActive(true);
        SetLayerRecursively(copy, LayerMask.NameToLayer(previewLayer));
        stagedMeshes[slot] = copy;
    }

    void ClearStage()
    {
        if (stagedMeshes == null) return;

        for (int i = 0; i < stagedMeshes.Length; i++)
        {
            if (stagedMeshes[i] != null)
            {
                Destroy(stagedMeshes[i]);
                stagedMeshes[i] = null;
            }

            if (previewCameras != null && i < previewCameras.Length && previewCameras[i] != null)
                previewCameras[i].enabled = false;
        }
    }

    void OnItemClicked(int dataIndex)
    {
        if (currentItems == null || dataIndex >= currentItems.Length) return;
        FindObjectOfType<CharacterCreationManager>().EquipFromGrid(currentItems[dataIndex]);
    }

    public void Clear()
    {
        ClearStage();
        currentItems = null;
        page = 0;

        for (int i = 0; i < PAGE_SIZE; i++)
        {
            if (slotImages  != null && i < slotImages.Length  && slotImages[i]  != null)
            {
                slotImages[i].enabled = false;
                slotImages[i].texture = null;
            }

            if (slotButtons != null && i < slotButtons.Length && slotButtons[i] != null)
            {
                slotButtons[i].interactable = false;
                slotButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}