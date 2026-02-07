using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    // Script de la escena de créditos.
    // Mantiene los botones ocultos al inicio y los muestra cuando termina la animación.
    // También gestiona el clic en Menú (volver) y Salir.
    [SerializeField] private GameObject btnMenu;
    [SerializeField] private GameObject btnSalir;

    // Al cargar la escena, los botones empiezan ocultos
    void Start()
    {
        btnMenu.SetActive(false);
        btnSalir.SetActive(false);
    }

    // Se llama con un evento al terminar la animación de los créditos
    // Activa los botones menú y salir
    private void ActivarBotones()
    {
        btnMenu.SetActive(true);
        btnSalir.SetActive(true);
    }

    // Cambia a la escena del menú principal
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    // Cierra el juego
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }
}
