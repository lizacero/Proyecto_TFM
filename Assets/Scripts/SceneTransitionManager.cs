using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [SerializeField] private float duracionFade = 0.5f;
    private const string tagPanel = "PanelFade";

    /// <summary>
    /// Busca un GameObject por tag incluyendo objetos inactivos.
    /// FindWithTag solo encuentra activos; si el panel est? SetActive(false), no lo encuentra.
    /// </summary>
    private static GameObject FindWithTagIncluyendoInactivos(string tag)
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (var t in transforms)
            {
                if (t.CompareTag(tag))
                    return t.gameObject;
            }
        }
        return null;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadSceneConFade(int sceneBuildIndex)
    {
        StartCoroutine(FadeOutYLoad(sceneBuildIndex));
    }

    private IEnumerator FadeOutYLoad(int sceneBuildIndex)
    {
        var panelActual = FindWithTagIncluyendoInactivos(tagPanel);
        CanvasGroup cg = panelActual != null ? panelActual.GetComponent<CanvasGroup>() : null;
        if (cg == null && panelActual != null)
            cg = panelActual.AddComponent<CanvasGroup>();

        if (cg != null && panelActual != null)
        {
            panelActual.SetActive(true);           // Activar antes del fade
            cg.alpha = 0f;
            cg.blocksRaycasts = true;

            float tiempo = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
                yield return null;
            }
            cg.alpha = 1f;
            // No desactivar aquí: la escena se va a destruir al cargar la siguiente
        }

        SceneManager.LoadScene(sceneBuildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeInEscenaActual());
    }

    private IEnumerator FadeInEscenaActual()
    {
        yield return null;  // Esperar un frame para que la escena esté lista

        var panelNuevo = FindWithTagIncluyendoInactivos(tagPanel);
        CanvasGroup cg = panelNuevo != null ? panelNuevo.GetComponent<CanvasGroup>() : null;
        if (cg == null && panelNuevo != null)
            cg = panelNuevo.AddComponent<CanvasGroup>();

        if (cg == null) yield break;

        cg.alpha = 1f;
        cg.blocksRaycasts = true;
        panelNuevo.SetActive(true);

        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            yield return null;
        }
        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        panelNuevo.SetActive(false);
    }
}
