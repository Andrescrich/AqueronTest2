using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool jumpedAlready = false;
    private Rigidbody2D rb; 
    public int health = 3;
    public bool hited = false;
    private bool invencible = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(8, 12);
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            if (!jumpedAlready) FindObjectOfType<AudioManager>().Play("Salta");
            jump = true;
            jumpedAlready = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        jumpedAlready = false;

    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!invencible)
        {
            if (collision.gameObject.layer == 14)
            {
                //Elimina velocidad anterior al golpe
                rb.velocity = Vector3.zero;
                //Depende de que direccion te toque el enemigo, haces knockback hacia la otra direccion
                if (collision.transform.rotation.y > 0)
                {
                   rb.AddForce(new Vector2(-350f, 600f));
                }
                else
                {
                   rb.AddForce(new Vector2(350f, 600f));
                }

                //Disminucion en el canvas de la vida
                health--;
                if (health == 2) FindObjectOfType<GameManager>().Health3();
                if (health == 1) FindObjectOfType<GameManager>().Health2();
                if (health == 0)
                {
                    FindObjectOfType<GameManager>().Health1();
                    FindObjectOfType<GameManager>().EndGame();
                }

                //Inmobilidad durante knockback
                gameObject.GetComponent<PlayerMovement>().enabled = false;
                Invoke("enableScript", 0.5f);
                //Animacion golpe
                FindObjectOfType<AudioManager>().Play("Hit");
                animator.SetBool("isHurt", true);
                Invoke("notHurt", 0.5f);
                //Invencivilidad durante 0.7s
                invencible = true;
                Invoke("resetInvencible", 0.7f);               
            }
        }

        if (collision.gameObject.CompareTag("FinMapa"))
        {
            animator.SetBool("isHurt", true);
            FindObjectOfType<AudioManager>().Play("Hit");
            FindObjectOfType<GameManager>().EndGame();
        }
        
     /*   if (collision.gameObject.CompareTag("Middle"))
        {
            jump = true;
        } */
    }



    void notHurt()
    {
        animator.SetBool("isHurt", false);
    }

    void resetInvencible()
    {
        invencible = false;
    }

    void enableScript()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }
}