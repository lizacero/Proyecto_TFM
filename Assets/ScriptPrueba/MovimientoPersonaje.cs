using System.Collections;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 3f;
    [SerializeField] private float distanciaMinima = 0.1f; // Distancia mínima para considerar que llegó al destino

    [Header("Zona Caminable (Piso)")]
    [SerializeField] private bool usarZonaCaminable = true;
    [SerializeField] private float limiteMinX = -5f; // Límite izquierdo del área caminable
    [SerializeField] private float limiteMaxX = 5f;  // Límite derecho del área caminable
    [SerializeField] private float limiteMinY = -3f; // Límite inferior del área caminable
    [SerializeField] private float limiteMaxY = 3f;  // Límite superior del área caminable

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal; // La cámara principal de la escena

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

        // Validar que la posición inicial esté dentro de la zona caminable
        if (usarZonaCaminable)
        {
            Vector3 posicionInicialAjustada = AjustarPuntoALaZonaCaminable(transform.position);
            if (posicionInicialAjustada != transform.position)
            {
                Debug.LogWarning($"MovimientoPersonaje: La posición inicial del personaje está fuera de la zona caminable. Se ajustará automáticamente.");
                transform.position = posicionInicialAjustada;
            }
        }
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

    /// <summary>
    /// Convierte la posición del clic del mouse a posición del mundo y mueve al personaje
    /// </summary>
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
        Vector3 destinoClic = camaraPrincipal.ScreenToWorldPoint(posicionMouse);

        // En 2D, generalmente solo usamos X e Y, así que mantenemos Z en 0 o la posición actual
        destinoClic.z = transform.position.z;

        // Si se usa zona caminable, validar y ajustar el destino
        if (usarZonaCaminable)
        {
            destino = AjustarPuntoALaZonaCaminable(destinoClic);
        }
        else
        {
            destino = destinoClic;
        }

        // Iniciar el movimiento suave
        IniciarMovimiento();
    }

    /// <summary>
    /// Verifica si un punto está dentro de la zona caminable
    /// </summary>
    /// <param name="punto">Punto a verificar</param>
    /// <returns>True si el punto está dentro de la zona caminable</returns>
    private bool EstaDentroDeLaZonaCaminable(Vector3 punto)
    {
        return punto.x >= limiteMinX && punto.x <= limiteMaxX &&
               punto.y >= limiteMinY && punto.y <= limiteMaxY;
    }

    /// <summary>
    /// Ajusta un punto para que esté dentro de la zona caminable.
    /// Si el punto está fuera, encuentra el punto más cercano dentro de la zona.
    /// </summary>
    /// <param name="punto">Punto a ajustar</param>
    /// <returns>Punto ajustado dentro de la zona caminable</returns>
    private Vector3 AjustarPuntoALaZonaCaminable(Vector3 punto)
    {
        // Si el punto ya está dentro de la zona, retornarlo sin cambios
        if (EstaDentroDeLaZonaCaminable(punto))
        {
            return punto;
        }

        // Si está fuera, encontrar el punto más cercano dentro de la zona
        // Esto se hace limitando cada coordenada a los límites del área
        Vector3 puntoAjustado = punto;

        // Limitar X dentro del rango [limiteMinX, limiteMaxX]
        puntoAjustado.x = Mathf.Clamp(punto.x, limiteMinX, limiteMaxX);

        // Limitar Y dentro del rango [limiteMinY, limiteMaxY]
        puntoAjustado.y = Mathf.Clamp(punto.y, limiteMinY, limiteMaxY);

        // Mantener Z igual
        puntoAjustado.z = punto.z;

        Debug.Log($"MovimientoPersonaje: El destino estaba fuera de la zona caminable. Punto original: {punto}, Punto ajustado: {puntoAjustado}");

        return puntoAjustado;
    }

    /// <summary>
    /// Inicia el movimiento suave hacia el destino
    /// </summary>
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

    /// <summary>
    /// Corrutina que mueve al personaje suavemente hacia el destino
    /// </summary>
    private IEnumerator MoverSuavemente()
    {
        estaMoviendose = true;

        // Mover mientras no se haya llegado al destino
        while (Vector3.Distance(transform.position, destino) > distanciaMinima)
        {
            // Calcular la dirección hacia el destino
            Vector3 direccion = (destino - transform.position).normalized;

            // Calcular la nueva posición
            Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);

            // Si se usa zona caminable, asegurar que la nueva posición esté dentro de la zona
            if (usarZonaCaminable)
            {
                nuevaPosicion = AjustarPuntoALaZonaCaminable(nuevaPosicion);
            }

            // Aplicar el movimiento
            transform.position = nuevaPosicion;

            // Esperar al siguiente frame
            yield return null;
        }

        // Asegurarse de llegar exactamente al destino (dentro de la zona caminable)
        if (usarZonaCaminable)
        {
            destino = AjustarPuntoALaZonaCaminable(destino);
        }
        transform.position = destino;
        estaMoviendose = false;
        movimientoCorrutina = null;

        Debug.Log($"Personaje llegó al destino: {destino}");
    }

    /// <summary>
    /// Método para visualizar la zona caminable en el Editor (Gizmos)
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (usarZonaCaminable)
        {
            // Dibujar un rectángulo que representa la zona caminable
            Gizmos.color = Color.green;

            // Dibujar las líneas del rectángulo
            Vector3 esquinaInferiorIzquierda = new Vector3(limiteMinX, limiteMinY, transform.position.z);
            Vector3 esquinaInferiorDerecha = new Vector3(limiteMaxX, limiteMinY, transform.position.z);
            Vector3 esquinaSuperiorIzquierda = new Vector3(limiteMinX, limiteMaxY, transform.position.z);
            Vector3 esquinaSuperiorDerecha = new Vector3(limiteMaxX, limiteMaxY, transform.position.z);

            // Dibujar las 4 líneas del rectángulo
            Gizmos.DrawLine(esquinaInferiorIzquierda, esquinaInferiorDerecha);   // Línea inferior
            Gizmos.DrawLine(esquinaInferiorDerecha, esquinaSuperiorDerecha);     // Línea derecha
            Gizmos.DrawLine(esquinaSuperiorDerecha, esquinaSuperiorIzquierda);   // Línea superior
            Gizmos.DrawLine(esquinaSuperiorIzquierda, esquinaInferiorIzquierda);  // Línea izquierda

            // Dibujar las diagonales para mejor visualización
            Gizmos.color = Color.green * 0.5f; // Más transparente
            Gizmos.DrawLine(esquinaInferiorIzquierda, esquinaSuperiorDerecha);
            Gizmos.DrawLine(esquinaInferiorDerecha, esquinaSuperiorIzquierda);
        }
    }
}
