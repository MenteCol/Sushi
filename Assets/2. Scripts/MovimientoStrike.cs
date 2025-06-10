using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class MovimientoStrike : MonoBehaviour
{
    [Header("Fuerza de golpe")]
    public float fuerzaMin = 50f;
    public float fuerzaMax = 150f;

    [Header("Configuraci�n f�sica")]
    public float gravedadMultiplicador = 2f;  // Multiplica la gravedad solo para este objeto
    public float angularDragValor = 5f;        // Amortiguaci�n rotacional

    [Header("Input")]
    public InputAction testForceAction;        // Acci�n para probar la fuerza (ejemplo: tecla espacio)

    private Rigidbody rb;
    private HingeJoint hinge;
    private Camera mainCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        testForceAction.Enable();
        testForceAction.performed += OnTestForcePerformed;
    }

    private void OnDisable()
    {
        testForceAction.performed -= OnTestForcePerformed;
        testForceAction.Disable();
    }

    void Start()
    {
        // Configurar Rigidbody
        rb.angularDamping = angularDragValor;

        // Desactivar gravedad por defecto para aplicar personalizada
        rb.useGravity = false;

        // Configurar Hinge Joint con resorte y amortiguaci�n
        hinge.useSpring = true;
        JointSpring spring = hinge.spring;
        spring.spring = 40f;       // Fuerza del resorte (ajusta a tu gusto)
        spring.damper = 5f;       // Amortiguaci�n para reducir oscilaciones
        spring.targetPosition = 0f; // Posici�n de reposo
        hinge.spring = spring;

        // Limitar �ngulo de oscilaci�n
        hinge.useLimits = true;
        JointLimits limits = hinge.limits;
        limits.min = -90f;
        limits.max = 90f;
        hinge.limits = limits;
    }

    private void FixedUpdate()
    {
        // Aplicar gravedad personalizada solo a este Rigidbody
        Vector3 gravedadPersonalizada = Physics.gravity * gravedadMultiplicador;
        rb.AddForce(gravedadPersonalizada * rb.mass);
    }

    // M�todo p�blico para aplicar una fuerza aleatoria
    public void AplicarFuerzaAleatoria()
    {
        float fuerzaZ = Random.Range(fuerzaMin, fuerzaMax);
        float fuerzaX = fuerzaZ / 3f * (Random.value < 0.5f ? -1f : 1f);

        Vector3 fuerzaVector = transform.forward * fuerzaZ + transform.right * fuerzaX;

        rb.AddForce(fuerzaVector, ForceMode.Impulse);

        Debug.Log($"Fuerza aplicada: {fuerzaVector}");
    }

    // Evento para probar la fuerza con tecla configurada en testForceAction
    private void OnTestForcePerformed(InputAction.CallbackContext context)
    {
        AplicarFuerzaAleatoria();
    }
}
