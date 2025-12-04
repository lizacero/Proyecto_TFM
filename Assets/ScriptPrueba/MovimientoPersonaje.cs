using System.Collections;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 3f;
    [SerializeField] private float distanciaMinima = 0.1f; // Distancia mínima para considerar que llegó al destino

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal; // La cámara principal de la escena
    [SerializeField] private GameObject pies;

    // Variables privadas
    private Vector3 destino;
    private bool estaMoviendose = false;
    private Coroutine movimientoCorrutina;

    void Start()
    {
        // Si no se asigna la cámara en el Inspector, buscar la cámara principal automáticamente
        if (camaraPrincipal == null)
        {
            camaraPrincipal = Camera.main;

            if (camaraPrincipal == null)
            {
                Debug.LogError("MovimientoPersonaje: No se encontró la cámara principal. Asegúrate de que la cámara tenga el tag 'MainCamera'.");
            }
        }

        // Guardar la posición inicial como primer destino
        destino = transform.position;
    }

    void Update()
    {
        // Detectar clic del mouse (botón izquierdo)
        if (Input.GetMouseButtonDown(0))
        {
            // Verificar que no se esté haciendo clic en un elemento de UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                MoverAlClic();
            }
        }
    }

    /// Convierte la posición del clic del mouse a posición del mundo y mueve al personaje
    private void MoverAlClic()
    {
        if (camaraPrincipal == null)
        {
            Debug.LogWarning("MovimientoPersonaje: No hay cámara asignada. No se puede mover el personaje.");
            return;
        }

        // Obtener la posición del mouse en la pantalla
        Vector3 posicionMouse = Input.mousePosition;

        // Convertir la posición del mouse a posición del mundo
        // Para una cámara ortográfica 2D, usamos ScreenToWorldPoint con z = 0 o la distancia de la cámara
        posicionMouse.z = camaraPrincipal.nearClipPlane + 1f; // Distancia desde la cámara
        destino = camaraPrincipal.ScreenToWorldPoint(posicionMouse);

        // En 2D, generalmente solo usamos X e Y, así que mantenemos Z en 0 o la posición actual
        destino.z = transform.position.z;

        // Iniciar el movimiento suave
        IniciarMovimiento();
    }

    /// Inicia el movimiento suave hacia el destino
    private void IniciarMovimiento()
    {
        // Si ya hay un movimiento en curso, detenerlo primero
        if (movimientoCorrutina != null)
        {
            StopCoroutine(movimientoCorrutina);
        }

        // Iniciar nueva corrutina de movimiento
        movimientoCorrutina = StartCoroutine(MoverSuavemente());
    }

    /// Corrutina que mueve al personaje suavemente hacia el destino
    private IEnumerator MoverSuavemente()
    {
        estaMoviendose = true;

        // Mover mientras no se haya llegado al destino
        while (Vector3.Distance(transform.position, destino) > distanciaMinima)
        {
            // Calcular la dirección hacia el destino
            Vector3 direccion = (destino - transform.position).normalized;

            // Mover hacia el destino usando MoveTowards para un movimiento más controlado
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);

            // Esperar al siguiente frame
            yield return null;
        }

        // Asegurarse de llegar exactamente al destino
        transform.position = destino;
        estaMoviendose = false;
        movimientoCorrutina = null;

        Debug.Log($"Personaje llegó al destino: {destino}");
    }
}
