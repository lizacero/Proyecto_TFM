using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Música")]
    [SerializeField] private AudioClip musicaMenu;
    [SerializeField] private AudioClip musicaJuego;
    [SerializeField] private AudioClip musicaCreditos;

    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";

    private AudioSource audioSource;
    private AudioClip clipActual;
    private AudioClip nuevoClip;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool esMenu = scene.name == nombreEscenaMenu;
        bool esCredito = scene.name == "Creditos";

        if (esMenu)
        {
            nuevoClip = musicaMenu;
        }
        else if (esCredito)
        {
            nuevoClip = musicaCreditos;
        }
        else
        {
            nuevoClip = musicaJuego;
        }


        if (nuevoClip == null) return;
        if (nuevoClip == clipActual && audioSource.isPlaying) return;

        clipActual = nuevoClip;
        audioSource.clip = nuevoClip;
        audioSource.Play();
    }

    /// <summary>
    /// Para que el slider de ajustes controle el volumen.
    /// </summary>
//    public void SetVolumen(float volumen)
//    {
//        if (audioSource != null)
//            audioSource.volume = Mathf.Clamp01(volumen);
//    }
}
