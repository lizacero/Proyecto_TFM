using TMPro;
using UnityEngine;

public class ControlCursor : MonoBehaviour
{
    public int tamanhioCursor = 32;
    public Texture2D cursorMano;
    public Texture2D cursorNormal;
    private Texture2D cursorActivo;

    private void Awake()
    {
        Cursor.visible = false;   
    }

    void Start()
    {
        CambiarCursor("Normal");
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
}
