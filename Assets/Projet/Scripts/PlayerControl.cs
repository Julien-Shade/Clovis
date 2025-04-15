using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Slider slider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    Controler inputActions;

    [Header("Movement")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float baseSpeed;

    [Header("Run")]
    [SerializeField] private float runMultiplier;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool cantrail;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isgrounded;

    [Header("Endurence")]
    [SerializeField] private float endurence;
    [SerializeField] private float maxEndurence;
    [SerializeField] private float consomationEndurence;

    #region Encasplulation
    public float Endurence
    {
        get => endurence; // read / lire
        set { endurence = Mathf.Clamp(value, 0, MaxEndurence); slider.value = endurence; slider.maxValue = MaxEndurence; } // clamp la valeur entre 0 et maxEndurence (100 pour le moment) et est aussi synchroniser pour le slider (uniquement à l'écriture)
    }
    public float CurrentSpeed
    {
        get
        {
            if (isRunning && Endurence > 0) // si cours et peut consomer endurence
            {
                currentSpeed = runMultiplier * baseSpeed;
                //Debug.Log(speed + " is running");
                return currentSpeed;
            }
            else
            {
                //Debug.Log(speed + " !running");
                currentSpeed = baseSpeed;
                return currentSpeed;
            }
        }

        set => currentSpeed = value;
    }

    public float MaxEndurence { get => maxEndurence; set => maxEndurence = value; }
    #endregion
    #region Input system
    private void OnEnable()
    {
        inputActions.Player.Jump.performed += ctx => Jump();

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.performed += ctx => animator.SetBool("Run", isRunning);
        inputActions.Player.Run.canceled += ctx => isRunning = false;
        inputActions.Player.Run.canceled += ctx => animator.SetBool("Run", isRunning);

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.performed += ctx => isWalking = true;
        inputActions.Player.Move.performed += ctx => animator.SetBool("Walk", isWalking);
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Move.canceled += ctx => rb.velocity = Vector2.zero;
        inputActions.Player.Move.canceled += ctx => isWalking = false;
        inputActions.Player.Move.canceled += ctx => animator.SetBool("Walk", isWalking);

        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
    private void Awake()
    {
        inputActions = new();
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        slider = GameObject.Find("Slider_Endurence").GetComponent<Slider>(); // à modifier si nécessaire (se refaire au nom EXACTE du gameobject pour avoir le Slider)
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -15 ,15), transform.position.y, transform.position.z ); //limite déplacement player
        Run();
    }
    private void FixedUpdate()
    {
        Move();

    }
    private void Move()
    {
        rb.AddForce(moveInput * CurrentSpeed);
    }
    private void Run()
    {
        if (!isRunning || Endurence == 0)
        {
            trail.emitting = false;
            return;
        }

        if (Endurence > 0 && moveInput != Vector2.zero) // si peut consomer endurence et qu'il bouge en gros
        {
            Endurence -= consomationEndurence * Time.deltaTime; // X endurence/s
            if (cantrail)
            {
                trail.emitting = true;
            }
        }
    }
    /// <summary>
    /// Ajoute 20 endurence de BASE (peut etre changer)
    /// </summary>
    /// <param name="amount"></param>
    public void AddEndurence(float amount = 20)
    {
        Endurence += amount; // ajouter la valeur initié
    }
    private void Jump()
    {
        if (!isgrounded) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isgrounded = true;
            animator.SetTrigger("End");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isgrounded = false;
        }
    }
}
