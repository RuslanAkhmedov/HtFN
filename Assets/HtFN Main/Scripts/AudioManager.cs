using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip shotSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.1f;
            audioSource.playOnAwake = false;  // Убедись, что звук не начинает проигрываться сразу при старте
            audioSource.spatialBlend = 0;     // 2D звук
            audioSource.pitch = 1f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSound()
    {
        if (hoverSound != null) ;
          //  audioSource.PlayOneShot(hoverSound);
    }



    public void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void PlayShotSound()
    {

        audioSource.PlayOneShot(shotSound);
    }
}
