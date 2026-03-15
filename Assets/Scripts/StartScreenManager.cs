using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField nameInputField;
    public TMP_InputField ageInputField;

    [Header("Buttons")]
    public Button startButton;
    public Button endGameButton;

    [Header("Music")]
    public Toggle musicToggle;
    public DynamicPlayer dynamicPlayer;

    [Header("Scene")]
    public string characterCreationSceneName = "CharacterCreation";

    void Start()
    {
        // button listeners
        startButton.onClick.AddListener(OnStartClicked);
        endGameButton.onClick.AddListener(OnEndGameClicked);
        musicToggle.onValueChanged.AddListener(OnMusicToggled);

        // initial music state
        musicToggle.isOn = true;
    }

    void OnStartClicked()
    {
        // name validation
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.LogWarning("Character name is required!");
            // need to add visible label
            return;
        }

        // age vvalidation
        if (string.IsNullOrWhiteSpace(ageInputField.text))
        {
            Debug.LogWarning("Character age is required!");
            // need to add visible label
            return;
        }

        // next scene data save
        PlayerPrefs.SetString("CharacterName", nameInputField.text);
        PlayerPrefs.SetInt("CharacterAge", int.Parse(ageInputField.text));
        PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        // lod next screen
        SceneManager.LoadScene(characterCreationSceneName);
    }

    void OnEndGameClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnMusicToggled(bool isOn)
    {
        if (dynamicPlayer == null) return;

        if (isOn)
            dynamicPlayer.PlayMusic();
        else
            dynamicPlayer.StopMusic();
    }
}
