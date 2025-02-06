using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variáveis de movimento
    public float runSpeed = 4f;
    public float rotationSpeed = 15f;
    public float accelerationTime = 0.2f;
    public float decelerationTime = 0.2f;
    public float postRotationAccelerationTime = 0.1f;
    private float lastDirection = 0f;
    private float currentSpeed = 0f;
    private bool isRotating = false;
    public float maxRunSpeed = 5.5f;

    // Variáveis de pulo
    public bool grounded = false;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    Collider[] groundCollisions;
    public float groundCheckRadius = 1f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpSpeed;
    private float ySpeed;

    // Variáveis de entrada
    public Vector2 move;
    public bool jump;
    public bool switchMode;

    Rigidbody rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("currentSpeed", Mathf.Abs(currentSpeed));

        // Calcula a velocidade alvo
        move.x = Mathf.Clamp(move.x, -2, 2);
        float targetSpeed = move.x * runSpeed;
        
        if (Mathf.Abs(move.x) > 0.01f) // Quando há movimento
        {
            // Se o personagem está mudando de direção, faz uma desaceleração rápida
            if ((Mathf.Sign(move.x) != Mathf.Sign(lastDirection)) && Mathf.Abs(currentSpeed) > 2)
            {
                // Desaceleração mais rápida ao mudar de direção
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / postRotationAccelerationTime);
            }
            else
            {
                // Suaviza a velocidade atual em direção à velocidade alvo normalmente
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);
            }
        }
        else // Quando não há movimento
        {
            // Suaviza a velocidade atual para 0 usando decelerationTime
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / decelerationTime);
        }

        // Limita a velocidade atual
        currentSpeed = Mathf.Clamp(currentSpeed, -maxRunSpeed, maxRunSpeed);

        // Verifica se há movimento na direção horizontal
        if (move.x != 0)
        {
            lastDirection = move.x; // Atualiza a última direção de movimento
            isRotating = true; // Define a flag de rotação como verdadeira
        }

        // Se o personagem está rotacionando
        if (isRotating)
        {
            float targetRotation = Mathf.Sign(lastDirection) * 90f; // Define a rotação alvo com base na última direção
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * rotationSpeed); // Interpola o ângulo de rotação
            transform.eulerAngles = new Vector3(0, angle, 0); // Aplica a nova rotação

            // Verifica se a rotação atual está próxima da rotação alvo
            if (Mathf.Abs(transform.eulerAngles.y - targetRotation) < 1f) 
            {
                isRotating = false; // Para a rotação quando próximo da rotação alvo
            }
        }

        ySpeed += Physics.gravity.y * Time.deltaTime;
        // Verifica se o personagem está no chão e se o botão de pulo foi pressionado
        if (grounded)
        {   
            anim.SetBool("grounded", grounded); // Atualiza o estado da animação
            isGrounded = true;
            anim.SetBool("isJumping", false);
            isJumping = false;
            anim.SetBool("isFalling", false);

            if (jump)
            {
                ySpeed = jumpSpeed;
                anim.SetBool("isJumping", true);
                isJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z); // Reseta a velocidade vertical
                grounded = false;
            }
            
        }
        else
        {
            anim.SetBool("grounded", false); // Atualiza o estado da animação
            grounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                anim.SetBool("isFalling", true);
            }
        }
    }

    void FixedUpdate()
    {
        // Verifica contato com o chão
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        grounded = groundCollisions.Length > 0; // Define o estado de "grounded" baseado nas colisões

        // Move o personagem na direção horizontal com a velocidade interpolada
        rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
    }
}
