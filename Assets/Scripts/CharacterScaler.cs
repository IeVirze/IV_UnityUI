using UnityEngine;
using UnityEngine.UI;

public class CharacterScaler : MonoBehaviour
{
    [Header("Characters")]
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    [Header("Sliders")]
    public Slider heightSlider;
    public Slider widthSlider;

    [Header("Scale Limits")]
    public float minHeight = 0.7f;
    public float maxHeight = 1.4f;
    public float minWidth  = 0.7f;
    public float maxWidth  = 1.4f;

    private GameObject activeCharacter => 
        maleCharacter.activeSelf ? maleCharacter : femaleCharacter;

    void Start()
    {
 
        heightSlider.minValue = minHeight;
        heightSlider.maxValue = maxHeight;
        heightSlider.value    = 1f;

        widthSlider.minValue  = minWidth;
        widthSlider.maxValue  = maxWidth;
        widthSlider.value     = 1f;


        heightSlider.onValueChanged.AddListener(OnHeightChanged);
        widthSlider.onValueChanged.AddListener(OnWidthChanged);
    }

    void OnHeightChanged(float value)
    {
        Vector3 scale = activeCharacter.transform.localScale;
        scale.y = value;
        activeCharacter.transform.localScale = scale;


        PlayerPrefs.SetFloat("CharacterHeight", value);
    }

    void OnWidthChanged(float value)
    {
        Vector3 scale = activeCharacter.transform.localScale;
        scale.x = value;
        scale.z = value; 
        activeCharacter.transform.localScale = scale;


        PlayerPrefs.SetFloat("CharacterWidth", value);
    }


    public void ResetSliders()
    {
        heightSlider.value = 1f;
        widthSlider.value  = 1f;
    }
}
