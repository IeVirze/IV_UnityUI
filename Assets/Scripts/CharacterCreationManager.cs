using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterCreationManager : MonoBehaviour
{
    [Header("3D Characters")]
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    [Header("Gender Dropdown")]
    public TMP_Dropdown genderDropdown;

    [Header("Male Clothing")]
    public GameObject[] maleHelmets;
    public GameObject[] maleGloves;
    public GameObject[] maleTops;
    public GameObject[] malePants;
    public GameObject[] maleBoots;
    public GameObject[] maleNecklaces;

    [Header("Female Clothing")]
    public GameObject[] femaleHelmets;
    public GameObject[] femaleGloves;
    public GameObject[] femaleTops;
    public GameObject[] femalePants;
    public GameObject[] femaleBoots;
    public GameObject[] femaleNecklaces;

    [Header("Arrow Buttons")]
    public Button leftArrow;
    public Button rightArrow;

    [Header("Category Toggles")]
    public Toggle helmetToggle;
    public Toggle gloveToggle;
    public Toggle topToggle;
    public Toggle pantToggle;
    public Toggle bootToggle;
    public Toggle necklaceToggle;

    [Header("Clothing Grid")]
    public ClothingGridDisplay clothingGrid;

    [Header("Sliders")]
    public Slider heightSlider;
    public Slider widthSlider;

    [Header("Scale Ranges")]
    public float minHeight = 0.85f, maxHeight = 1.15f;
    public float minWidth  = 0.85f, maxWidth  = 1.15f;

    [Header("Navigation Buttons")]
    public Button backButton;
    public Button continueButton;

    public enum ClothingCategory { None, Helmet, Glove, Top, Pant, Boot, Necklace }
    private ClothingCategory activeCategory = ClothingCategory.None;

    private int helmetIdx, gloveIdx, topIdx, pantIdx, bootIdx, necklaceIdx;
    private bool isMale = true;

    private GameObject currentHelmet, currentGlove, currentTop,
                       currentPant,   currentBoot,  currentNecklace;

    void Start()
    {
        genderDropdown.onValueChanged.AddListener(OnGenderChanged);

        leftArrow .onClick.AddListener(() => CycleActiveCategory(-1));
        rightArrow.onClick.AddListener(() => CycleActiveCategory(+1));

        helmetToggle  .onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Helmet);   });
        gloveToggle   .onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Glove);    });
        topToggle     .onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Top);      });
        pantToggle    .onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Pant);     });
        bootToggle    .onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Boot);     });
        necklaceToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Necklace); });

        heightSlider.onValueChanged.AddListener(_ => ApplyScale());
        widthSlider .onValueChanged.AddListener(_ => ApplyScale());

        backButton    .onClick.AddListener(OnBack);
        continueButton.onClick.AddListener(OnContinue);

        SetGender(true);
    }

    void SetActiveCategory(ClothingCategory category)
    {
    activeCategory = category;
    leftArrow .interactable = true;
    rightArrow.interactable = true;
    clothingGrid.LoadCategory(GetCategoryData(category));
    }
    
    GameObject[] GetCategoryData(ClothingCategory category)
    { switch (category)
    {
        case ClothingCategory.Helmet:   return isMale ? maleHelmets   : femaleHelmets;
        case ClothingCategory.Glove:    return isMale ? maleGloves    : femaleGloves;
        case ClothingCategory.Top:      return isMale ? maleTops      : femaleTops;
        case ClothingCategory.Pant:     return isMale ? malePants     : femalePants;
        case ClothingCategory.Boot:     return isMale ? maleBoots     : femaleBoots;
        case ClothingCategory.Necklace: return isMale ? maleNecklaces : femaleNecklaces;
        default:                        return null;
    }
    }

    void CycleActiveCategory(int dir)
    {
        clothingGrid.TurnPage(dir);
    }

    public void EquipFromGrid(GameObject item)
    {
        GameObject[] pool = isMale ? GetMalePool(activeCategory)
                                : GetFemalePool(activeCategory);
        HideAll(pool);

        if (item != null)
            item.SetActive(true);
    }

    GameObject[] GetMalePool(ClothingCategory category)
    {
        switch (category)
        {
            case ClothingCategory.Helmet:   return maleHelmets;
            case ClothingCategory.Glove:    return maleGloves;
            case ClothingCategory.Top:      return maleTops;
            case ClothingCategory.Pant:     return malePants;
            case ClothingCategory.Boot:     return maleBoots;
            case ClothingCategory.Necklace: return maleNecklaces;
            default:                        return null;
        }
    }

    GameObject[] GetFemalePool(ClothingCategory category)
    {
        switch (category)
        {
            case ClothingCategory.Helmet:   return femaleHelmets;
            case ClothingCategory.Glove:    return femaleGloves;
            case ClothingCategory.Top:      return femaleTops;
            case ClothingCategory.Pant:     return femalePants;
            case ClothingCategory.Boot:     return femaleBoots;
            case ClothingCategory.Necklace: return femaleNecklaces;
            default:                        return null;
        }
    }

    void OnGenderChanged(int value)
    {
        SetGender(value == 0);
    }

    void SetGender(bool male)
    {
        isMale = male;
        maleCharacter  .SetActive(male);
        femaleCharacter.SetActive(!male);
        ResetAllClothing();

        helmetIdx = gloveIdx = topIdx = pantIdx = bootIdx = necklaceIdx = 0;
        currentHelmet = currentGlove = currentTop =
        currentPant   = currentBoot  = currentNecklace = null;

        activeCategory = ClothingCategory.None;

        helmetToggle  .SetIsOnWithoutNotify(false);
        gloveToggle   .SetIsOnWithoutNotify(false);
        topToggle     .SetIsOnWithoutNotify(false);
        pantToggle    .SetIsOnWithoutNotify(false);
        bootToggle    .SetIsOnWithoutNotify(false);
        necklaceToggle.SetIsOnWithoutNotify(false);

        clothingGrid.Clear();

        heightSlider.value = 0.5f;
        widthSlider .value = 0.5f;
        ApplyScale();
    }

    void ApplyScale()
    {
        Transform root = isMale ? maleCharacter.transform : femaleCharacter.transform;
        float h = Mathf.Lerp(minHeight, maxHeight, heightSlider.value);
        float w = Mathf.Lerp(minWidth,  maxWidth,  widthSlider.value);
        root.localScale = new Vector3(w, h, w);
    }

    void Cycle(ref int index, GameObject[] items, int dir, ref GameObject current)
    {
        if (items == null || items.Length == 0) return;

        if (current != null)
        {
            current.SetActive(false);
            current = null;
        }

        index += dir;
        if (index < 0)            index = items.Length;
        if (index > items.Length) index = 0;

        if (index > 0 && items[index - 1] != null)
        {
            current = items[index - 1];
            current.SetActive(true);
        }
    }

    GameObject[] GetHelmets()   => isMale ? maleHelmets   : femaleHelmets;
    GameObject[] GetGloves()    => isMale ? maleGloves    : femaleGloves;
    GameObject[] GetTops()      => isMale ? maleTops      : femaleTops;
    GameObject[] GetPants()     => isMale ? malePants     : femalePants;
    GameObject[] GetBoots()     => isMale ? maleBoots     : femaleBoots;
    GameObject[] GetNecklaces() => isMale ? maleNecklaces : femaleNecklaces;

    void ResetAllClothing()
    {
        HideAll(maleHelmets);    HideAll(femaleHelmets);
        HideAll(maleGloves);     HideAll(femaleGloves);
        HideAll(maleTops);       HideAll(femaleTops);
        HideAll(malePants);      HideAll(femalePants);
        HideAll(maleBoots);      HideAll(femaleBoots);
        HideAll(maleNecklaces);  HideAll(femaleNecklaces);
    }

    void HideAll(GameObject[] items)
    {
        if (items == null) return;
        foreach (var item in items)
            if (item != null) item.SetActive(false);
    }

    void OnBack()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current - 1);
    }

    void OnContinue()
    {
        PlayerPrefs.SetString("Gender",      isMale ? "Male" : "Female");
        PlayerPrefs.SetInt   ("HelmetIdx",   helmetIdx);
        PlayerPrefs.SetInt   ("GloveIdx",    gloveIdx);
        PlayerPrefs.SetInt   ("TopIdx",      topIdx);
        PlayerPrefs.SetInt   ("PantIdx",     pantIdx);
        PlayerPrefs.SetInt   ("BootIdx",     bootIdx);
        PlayerPrefs.SetInt   ("NecklaceIdx", necklaceIdx);
        PlayerPrefs.SetFloat ("Height",      heightSlider.value);
        PlayerPrefs.SetFloat ("Width",       widthSlider.value);
        PlayerPrefs.Save();

        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1);
    }
}