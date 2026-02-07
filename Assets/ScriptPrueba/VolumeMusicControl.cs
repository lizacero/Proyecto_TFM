using UnityEngine;
using UnityEngine.UI;

public class VolumeMusicControl : MonoBehaviour
{
    private const string KEY_VOLUMEN = "VolMusica";
    private const float VOLUMEN_DEFECTO = 0.7f;

    [SerializeField] private Slider slider;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    void Start()
    {
        if (slider == null) return;

        float valorGuardado = PlayerPrefs.GetFloat(KEY_VOLUMEN, VOLUMEN_DEFECTO);
        slider.value = valorGuardado;
        AplicarVolumen(valorGuardado);
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    void OnDestroy()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float valor)
    {
        AplicarVolumen(valor);
        PlayerPrefs.SetFloat(KEY_VOLUMEN, valor);
        PlayerPrefs.Save();
    }

    private void AplicarVolumen(float valor)
    {
        if (MusicManager.instance != null)
            MusicManager.instance.SetVolumen(valor);
    }
}
