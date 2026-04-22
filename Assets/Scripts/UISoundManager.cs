using UnityEngine;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public AudioClip toggleSound;
    public AudioClip dropdownSound;
    public AudioClip inputFieldSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlayClick()      => PlaySound(clickSound);
    public void PlayHover()      => PlaySound(hoverSound);
    public void PlayToggle()     => PlaySound(toggleSound);
    public void PlayDropdown()   => PlaySound(dropdownSound);
    public void PlayInputField() => PlaySound(inputFieldSound);

    void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}

