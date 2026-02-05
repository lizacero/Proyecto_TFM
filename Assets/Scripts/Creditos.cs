using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    [SerializeField] private GameObject btnMenu;
    [SerializeField] private GameObject btnSalir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnMenu.SetActive(false);
        btnSalir.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Se llama al terminar la animación de los créditos
    private void ActivarBotones()
    {
        btnMenu.SetActive(true);
        btnSalir.SetActive(true);
    }

    //Cambia a la escena del menú principal al seleccionar el botón.
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    //Cierra la aplicación al seleccionar el botón.
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }
}
