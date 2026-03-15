using UnityEngine;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip buttonClick;
    public AudioClip buttonHover;

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

    public void PlayClick()
    {
        if (buttonClick != null)
            audioSource.PlayOneShot(buttonClick);
    }

    public void PlayHover()
    {
        if (buttonHover != null)
            audioSource.PlayOneShot(buttonHover);
    }
}

