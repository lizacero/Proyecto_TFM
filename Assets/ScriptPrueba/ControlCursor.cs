using TMPro;
using UnityEngine;

public class ControlCursor : MonoBehaviour
{
    public int tamanhioCursor = 32;
    public Texture2D cursorMano;
    public Texture2D cursorNormal;
    private Texture2D cursorActivo;
    public TextMeshProUGUI texto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Cursor.visible = false;
        
    }
    void Start()
    {
        CambiarCursor("Normal");
        // Inicializar el texto
        if (texto != null)
        {
            texto.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CambiarCursor(string tipoCursor)
    {
        if (tipoCursor == "Normal")
        {
            cursorActivo = cursorNormal;
        }
        if (tipoCursor == "Mano")
        {
            cursorActivo = cursorMano;
        }
    }

    private void OnGUI() // se pone la imagen en la posición del cursor.
    {
        GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, tamanhioCursor, tamanhioCursor), cursorActivo);
    }

    //Acciones interactuables
    public void Ventana()
    {
        if (texto != null)
        {
            texto.text = "Clic en ventana";
            Debug.Log(texto.text);
        }
        else
        {
            Debug.LogWarning("ControlCursor: No se ha asignado el componente TextMeshProUGUI.");
        }
    }

    public void Mesa()
    {
        if (texto != null)
        {
            texto.text = "Clic en mesita";
            Debug.Log(texto.text);
        }
        else
        {
            Debug.LogWarning("ControlCursor: No se ha asignado el componente TextMeshProUGUI.");
        }
    }

    public void Cama()
    {
        if (texto != null)
        {
            texto.text = "Clic en cama";
            Debug.Log(texto.text);
        }
        else
        {
            Debug.LogWarning("ControlCursor: No se ha asignado el componente TextMeshProUGUI.");
        }
    }


}
