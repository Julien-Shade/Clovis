using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Rigidbody2D rb;
    Controler inputActions;

    [Header("Movement")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] private float speed;
    [SerializeField] private float baseSpeed;

    [Header("Run")]
    [SerializeField] private float runMultiplier;
    [SerializeField] private bool isRunning;

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
        set { endurence = Mathf.Clamp(value, 0, maxEndurence); slider.value = endurence; slider.maxValue = maxEndurence; } // clamp la valeur entre 0 et maxEndurence (100 pour le moment) et est aussi synchroniser pour le slider (uniquement à l'écriture)
    }
    public float Speed
    {
        get
        {
            if (isRunning && Endurence > 0)
            {
                speed = runMultiplier * baseSpeed;
                //Debug.Log(speed + " is running");
                return speed;
            }
            else
            {
                //Debug.Log(speed + " !running");
                speed = baseSpeed;
                return speed;
            }
        }

        set => speed = value;
    }
    #endregion
    #region Input system
    private void OnEnable()
    {
        inputActions.Player.Jump.performed += ctx => Jump();

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Move.canceled += ctx => rb.velocity = Vector2.zero;

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

        slider = GameObject.Find("Slider_Endurence").GetComponent<Slider>(); // à modifier si nécessaire (se refaire au nom EXACTE du gameobject pour avoir le Slider)
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Run();
    }
    private void FixedUpdate()
    {
        Move();

    }
    private void Move()
    {
        rb.AddForce(moveInput * Speed);
    }
    private void Run()
    {
        if (!isRunning) return;

        if (Endurence > 0)
        {
            Endurence -= consomationEndurence * Time.deltaTime; // 20endurence/s
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
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isgrounded = true;
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
