using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text characterNameText;
    public TMP_Text birthYearText;
    public TMP_Text descriptionText;
    public Button   quitButton;

    [Header("Descriptions")]
    [TextArea(5, 10)]
    public string maleDescription;
    [TextArea(5, 10)]
    public string femaleDescription;

    const int CURRENT_YEAR = 2026;

    void Start()
    {
        //read prefabs
        string name   = PlayerPrefs.GetString("CharacterName", "Unknown");
        int    age    = PlayerPrefs.GetInt   ("CharacterAge",  0);
        bool   isMale = PlayerPrefs.GetString("Gender", "Male") == "Male";
        characterNameText.text = name;
        birthYearText.text     = "Born: " + (CURRENT_YEAR - age);
        descriptionText.text   = isMale ? maleDescription : femaleDescription;

        quitButton.onClick.AddListener(OnQuit);
    }

    void ApplyClothing(GameObject[] pool, int idx)
    {
        if (pool == null) return;

        foreach (GameObject item in pool)
            if (item != null) item.SetActive(false);

        if (idx > 0 && idx <= pool.Length && pool[idx - 1] != null)
            pool[idx - 1].SetActive(true);
    }

    void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
