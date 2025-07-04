using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class MovimientoStrike : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("FuerzaGolpe")]
    public float fuerzaMin = 50f;
    public float fuerzaMax = 150f;

    [Header("ConfiguracionFisica")]
    public float gravedadMultiplicador = 2f;
    public float angularDragValor = 5f;

    [Header("Input")]
    public InputAction testForceAction;

    [Header("Referencias")]
    private Rigidbody rb;
    private HingeJoint hinge;
    private Camera mainCamera;
    #endregion

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
        rb.angularDamping = angularDragValor;
        rb.useGravity = false;

        hinge.useSpring = true;
        JointSpring spring = hinge.spring;
        spring.spring = 40f;
        spring.damper = 5f;
        spring.targetPosition = 0f;
        hinge.spring = spring;

        hinge.useLimits = true;
        JointLimits limits = hinge.limits;
        limits.min = -90f;
        limits.max = 90f;
        hinge.limits = limits;
    }

    private void FixedUpdate()
    {
        Vector3 gravedadPersonalizada = Physics.gravity * gravedadMultiplicador;
        rb.AddForce(gravedadPersonalizada * rb.mass);
    }

    public void AplicarFuerzaAleatoria()
    {
        float fuerzaZ = Random.Range(fuerzaMin, fuerzaMax);
        float fuerzaX = fuerzaZ / 3f * (Random.value < 0.5f ? -1f : 1f);

        Vector3 fuerzaVector = transform.forward * fuerzaZ + transform.right * fuerzaX;

        rb.AddForce(fuerzaVector, ForceMode.Impulse);

        if (depuracion)
            Debug.Log($"{gameObject.name}: Fuerza aplicada: {fuerzaVector}");
    }

    private void OnTestForcePerformed(InputAction.CallbackContext context)
    {
        AplicarFuerzaAleatoria();
    }
}