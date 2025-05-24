using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPScontroller : MonoBehaviour
{
    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder; //Ref al objeto que almacena la camara
    public float speed;
    public float sprintSpeed;
    public float crounchSpeed;
    public float maxForce = 1; //Limitador de la aceleracion del personaje
    public float sensitivity = 0.1f; //Sensibilidad aplicada a la rotacion a la cámara

    [Header("Jumping stats")]
    public float jumpForce;
    //variables del GroundChek
    [SerializeField] GameObject groundcheck;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    [Header("Player states bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    
    
    //Referencias privadas

    Rigidbody Playerrb;
    Animator anim;
    //Valores de almacenaje de input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation; //Valor de rotación que puede ser utilizado para la dirección de movimiento


    private void Awake()
    {
        Playerrb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        camHolder = GameObject.Find("CameraHolder");
        groundcheck = GameObject.Find("GroundCheck");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Restringir el cursor para siempre esté en el centro de la pantalla
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundcheck.transform.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        cameraLookMove();
    }

    void Movement()
    {
        Vector3 currentVelocity = Playerrb.velocity; //Define la velocidad actual, siempre es igual a la velocidad del rigidbody
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); //La velocidad hacia la que nos queremos mover definida por la dirección
        targetVelocity *= isCrouching ? crounchSpeed : (isSprinting ? sprintSpeed : speed);

        //Alinear la dirección con la orientación correcta (cámara)
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        //Limitar la fuerza máxima de aceleración
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //Aplicamos el movimiento en sí mismo
        Playerrb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    
    void cameraLookMove()
    {
        //Girar 
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Mirar
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90); //Limite para mirar de arriba a bajo y que el personaje no de la vuelta de forma inatural
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }
    void Jump()
    {
        Playerrb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
        #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            isCrouching = !isCrouching; //Cambia el bool al valor contrario que tenga en ese momento
            anim.SetTrigger("CrounchState");
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isCrouching) isSprinting = true;

        }
        if (context.canceled)
        {
            isSprinting = false;
        }
    }
    #endregion

}
