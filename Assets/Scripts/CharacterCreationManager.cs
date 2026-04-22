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

    [Header("Category Toggles")]
    public Toggle helmetToggle;
    public Toggle gloveToggle;
    public Toggle topToggle;
    public Toggle pantToggle;
    public Toggle bootToggle;
    public Toggle necklaceToggle;


    [Header("Clothing Grid")]
    public ClothingGridDisplay clothingGrid;

    [Header("Clothing Data")]
    public ClothingItemData[] helmetData;
    public ClothingItemData[] gloveData;
    public ClothingItemData[] topData;
    public ClothingItemData[] pantData;
    public ClothingItemData[] bootData;
    public ClothingItemData[] necklaceData;

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

        helmetToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Helmet);   });
        gloveToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Glove);    });
        topToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Top);      });
        pantToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Pant);     });
        bootToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Boot);     });
        necklaceToggle.onValueChanged.AddListener(isOn => { if (isOn) SetActiveCategory(ClothingCategory.Necklace); });

        heightSlider.onValueChanged.AddListener(_ => ApplyScale());
        widthSlider.onValueChanged.AddListener(_ => ApplyScale());

        backButton.onClick.AddListener(OnBack);
        continueButton.onClick.AddListener(OnContinue);

        SetGender(true);
    }

    void SetActiveCategory(ClothingCategory category)
    {
    activeCategory = category;
    clothingGrid.LoadCategory(GetCategoryData(category));
    }


    ClothingItemData[] GetCategoryData(ClothingCategory category)
    {
        switch (category)
        {
            case ClothingCategory.Helmet:   return helmetData;
            case ClothingCategory.Glove:    return gloveData;
            case ClothingCategory.Top:      return topData;
            case ClothingCategory.Pant:     return pantData;
            case ClothingCategory.Boot:     return bootData;
            case ClothingCategory.Necklace: return necklaceData;
            default:                        return null;
        }
    }


    public void EquipByName(string meshName)
    {
        GameObject[] pool = isMale ? GetMalePool(activeCategory)
                                : GetFemalePool(activeCategory);
        if (pool == null) return;  
        HideAll(pool);
        foreach (GameObject item in pool)
        {
            if (item != null && item.name == meshName)
            {
                item.SetActive(true);
                break;
            }
        }
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
            Application.Quit();
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