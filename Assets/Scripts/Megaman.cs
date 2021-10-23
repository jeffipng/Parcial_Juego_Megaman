using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float dashForce;

    float pasoAni = 0;

    Animator myAnimator;
    Rigidbody2D rb;
    [SerializeField] float jumpSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float dashRate;

    BoxCollider2D myCollider;

    [SerializeField] BoxCollider2D misPies;
    [SerializeField] GameObject disparo;

    bool enSuelo=false;

    float nextFire = 0;
    float nextDash = 0;

   
    bool enDash = false;
    bool Doblesalto;
    public bool mirandoIzquierda = false;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("IsRunning", false);
        myAnimator.SetBool("Falling", false);
        myAnimator.SetBool("Dashing", false);
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Correr();
        Cayendo();

        Suelo();
        if (enSuelo == true)
        {
            Saltar();
        }else if (Doblesalto)
        {
            DObleSaltar();
        }

        if(enSuelo)
            {
            Doblesalto = true;
        }

        Disparar();
        Dash();
        
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)  && enSuelo==true && Time.time >= nextDash && mirandoIzquierda==false)
        {

            rb.velocity = new Vector2(dashForce,0);
            
            enDash = true;
            nextDash = Time.time + dashRate;
            myAnimator.SetBool("Dashing", true);

            
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift) && enSuelo == true && Time.time >= nextDash && mirandoIzquierda == true)
        {

            myAnimator.SetBool("Dashing", true);
            rb.velocity = new Vector2(-dashForce, 0); ;
            
           
            enDash = true;
            nextDash = Time.time + dashRate;

            

        }

        else
        {
            myAnimator.SetBool("Dashing", false);
            enDash = false;
        }
    }

   

    void Disparar()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetLayerWeight(1, 1);
            
        }
        else if ( Time.time >= pasoAni)
        {
            myAnimator.SetLayerWeight(1, 0);
            pasoAni = Time.time + 1.8f;
        }

        if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextFire && transform.localScale.x<0)
        {
            Instantiate(disparo, transform.position - new Vector3(1f, 0, 0), transform.rotation);
            nextFire = Time.time + fireRate;
        }

        else if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextFire && transform.localScale.x > 0)
        {
            Instantiate(disparo, transform.position - new Vector3(-1f, 0, 0), transform.rotation);
            nextFire = Time.time + fireRate;
        }
    }

    void FinishJump()
    {
        myAnimator.SetBool("Falling", true);
    }

    void Correr()
    {
        float movH = Input.GetAxis("Horizontal");
        if (enDash == false)
        {
            if (movH != 0)
            {
                
                if (movH < 0)
                {
                    transform.localScale = new Vector2(-1, 1);
                    mirandoIzquierda = true;
                }

                else
                {
                    transform.localScale = new Vector2(1, 1);
                    mirandoIzquierda = false;
                }

                myAnimator.SetBool("IsRunning", true);
                transform.Translate(new Vector2(movH * Time.deltaTime * speed, 0));
            }

            else
            {
                myAnimator.SetBool("IsRunning", false);
                
            }
        }
       

        
    }

    void Saltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpSpeed));
            myAnimator.SetTrigger("Jumping");
            
        }

        
    }

    void DObleSaltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpSpeed));
            myAnimator.SetTrigger("Jumping");
            Doblesalto = false;

        }


    }

    void Suelo()
    {
        if (misPies.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            enSuelo=true;
        }

        else
        {
            enSuelo = false;
        }
    }

    void Cayendo()
    {
        if (rb.velocity.y<0)
        {
            myAnimator.SetBool("Falling", true);
        }

        else if (enSuelo == true)
        {
            myAnimator.SetBool("Falling", false);
        }
        
    }

}
