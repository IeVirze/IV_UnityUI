using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterCreationManager : MonoBehaviour
{
    [Header("Characters")]
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    [Header("Gender")]
    public TMP_Dropdown genderDropdown;

    [Header("Clothing Arrays - Male")]
    public GameObject[] maleAccessories;
    public GameObject[] maleTops;
    public GameObject[] malePants;
    public GameObject[] maleHair;

    [Header("Clothing Arrays - Female")]
    public GameObject[] femaleAccessories;
    public GameObject[] femaleTops;
    public GameObject[] femaleSkirts;
    public GameObject[] femaleHair;

    [Header("Arrow Buttons")]
    public Button accessoriesLeft, accessoriesRight;
    public Button topsLeft, topsRight;
    public Button pantsLeft, pantsRight;
    public Button hairLeft, hairRight;

    [Header("Item Display Images")]
    public Image accessoryDisplay;
    public Image topsDisplay;
    public Image pantsDisplay;
    public Image hairDisplay;

    [Header("Buttons")]
    public Button resetButton;
    public Button finishButton;
    public Button quitButton;

    private int accIndex = 0, topIndex = 0, pantIndex = 0, hairIndex = 0;
    private bool isMale = true;

    private GameObject currentAcc, currentTop, currentPant, currentHair;

    void Start()
    {
        // gender dropdown
        genderDropdown.onValueChanged.AddListener(OnGenderChanged);

        // arrow buttons
        accessoriesLeft.onClick.AddListener(() => CycleItem(ref accIndex, GetAccessories(), -1, ref currentAcc));
        accessoriesRight.onClick.AddListener(() => CycleItem(ref accIndex, GetAccessories(), 1, ref currentAcc));
        topsLeft.onClick.AddListener(() => CycleItem(ref topIndex, GetTops(), -1, ref currentTop));
        topsRight.onClick.AddListener(() => CycleItem(ref topIndex, GetTops(), 1, ref currentTop));
        pantsLeft.onClick.AddListener(() => CycleItem(ref pantIndex, GetPants(), -1, ref currentPant));
        pantsRight.onClick.AddListener(() => CycleItem(ref pantIndex, GetPants(), 1, ref currentPant));
        hairLeft.onClick.AddListener(() => CycleItem(ref hairIndex, GetHair(), -1, ref currentHair));
        hairRight.onClick.AddListener(() => CycleItem(ref hairIndex, GetHair(), 1, ref currentHair));

        resetButton.onClick.AddListener(ResetCharacter);
        finishButton.onClick.AddListener(OnFinish);
        quitButton.onClick.AddListener(OnQuit);

        SetGender(true);
    }

    void OnGenderChanged(int value)
    {
        // 0 = female, 1 = male
        SetGender(value == 1);
    }

    void SetGender(bool male)
    {
        isMale = male;
        maleCharacter.SetActive(male);
        femaleCharacter.SetActive(!male);
        ResetCharacter();
    }

    void ResetCharacter()
    {
        // deactivate all clothing
        DeactivateAll(maleAccessories); DeactivateAll(femaleAccessories);
        DeactivateAll(maleTops);        DeactivateAll(femaleTops);
        DeactivateAll(malePants);       DeactivateAll(femaleSkirts);
        DeactivateAll(maleHair);        DeactivateAll(femaleHair);

        currentAcc = null; currentTop = null;
        currentPant = null; currentHair = null;
        accIndex = 0; topIndex = 0;
        pantIndex = 0; hairIndex = 0;
    }

    void DeactivateAll(GameObject[] items)
    {
        foreach (var item in items)
            if (item != null) item.SetActive(false);
    }

    void CycleItem(ref int index, GameObject[] items, int direction, ref GameObject current)
    {
        if (items == null || items.Length == 0) return;

        if (current != null) current.SetActive(false);

        //  0 = no item equipped
        index += direction;
        if (index < 0) index = items.Length; // 0 = none, 1-N = items
        if (index > items.Length) index = 0;

        if (index == 0)
        {
            current = null; // naked slot
        }
        else
        {
            current = items[index - 1];
            if (current != null) current.SetActive(true);
        }
    }

    // return correct gender array
    GameObject[] GetAccessories() => isMale ? maleAccessories : femaleAccessories;
    GameObject[] GetTops()        => isMale ? maleTops : femaleTops;
    GameObject[] GetPants()       => isMale ? malePants : femaleSkirts;
    GameObject[] GetHair()        => isMale ? maleHair : femaleHair;

    void OnFinish()
    {
        // save selections for review screen
        PlayerPrefs.SetString("Gender", isMale ? "Male" : "Female");
        PlayerPrefs.SetInt("AccIndex", accIndex);
        PlayerPrefs.SetInt("TopIndex", topIndex);
        PlayerPrefs.SetInt("PantIndex", pantIndex);
        PlayerPrefs.SetInt("HairIndex", hairIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("ReviewScreen");
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
