using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private bool canJump;
    [SerializeField] private bool isSmall;
    [SerializeField] private float xForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject mushroom;
    private Rigidbody2D rb2D;
    private bool _isMushroomout = false;
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        isSmall = true;
        canJump = true;
        xForce = 800.0f;
        jumpForce = 660.0f;
        _camera = Camera.main;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        // Jumping
        if (Input.GetKey("up") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
            gameObject.GetComponent<Animator>().SetBool("isSaltando", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            canJump = false;
        }
        
        // Right, left, wait moves
        if(Input.GetKey("right") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce * Time.deltaTime, 0.0f));
        }else if (Input.GetKey("left") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xForce * Time.deltaTime, 0.0f));
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
        }        
    }

    // Se ejecuta mientras sigue colisionando (por ejemplo, con el suelo)
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" && rb2D.velocity.y < 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            
            canJump = true;
            Debug.Log(rb2D.velocity.y);
            return;
        }
    }

    // Trigger OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Verificar la normal de colisión
        ContactPoint2D contact = collision.contacts[0];  // Obtenemos el primer punto de contacto
        Vector2 normal = contact.normal;
        
        // If player collision with ground change var canJump and reset animation
        if (collision.gameObject.tag == "ground")
        {
            gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            //canJump = normal.y != 0;
            canJump = true;
            Debug.Log(rb2D.velocity.y);
            return;
        }

        if (collision.gameObject.tag == "Brick")
        {
            
            // Verificar la normal de colisión
            //ContactPoint2D contact = collision.contacts[0];  // Obtenemos el primer punto de contacto
            //Vector2 normal = contact.normal;

            if (normal.y >= 0.0f)
            {
                gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
                canJump = normal.y != 0;
                Debug.Log("peta...................................................... brick");
                return;
            }

            
            
            if (!isSmall)
            {
                Destroy(collision.gameObject);

                return;
            }

            // Set animation to move bricks
            StartCoroutine(MoveBrick(collision.gameObject,0.2f,5f,0.1f));
        }

        if (collision.gameObject.tag == "SurpriseBox")
        {
            
            // Verificar la normal de colisión
            //ContactPoint2D contact = collision.contacts[0];  // Obtenemos el primer punto de contacto
            //Vector2 normal = contact.normal;

            if (normal.y >= 0.0f)
            {
                gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
                canJump = normal.y != 0;
                
                Debug.Log("peta...................................................... surprise");
                return;
            }
            
            // Mario hit the surprise box
            // Set animation to move bricks
            StartCoroutine(MoveBrick(collision.gameObject,0.2f,5f,0.1f));
            
            // Change animation
            collision.gameObject.GetComponent<Animator>().SetBool("isHitted", true);

            if (!_isMushroomout && collision.gameObject.transform.position.x > 7.5 && collision.gameObject.transform.position.x < 7.52)
            {
                Instantiate(mushroom, new Vector3(collision.gameObject.transform.position.x + 0.5f,
                    collision.gameObject.transform.position.y + 1f, collision.gameObject.transform.position.z), Quaternion.identity);
                _isMushroomout = true;
            }
        }

        if (collision.gameObject.tag == "mushroom")
        {
            Destroy(collision.gameObject);
            isSmall = false;
            jumpForce = 700f;
            transform.localScale = new Vector3(10f, 9f, 1f);
        }
    }

    /*void OnCollisionExit2D(Collision2D collision)
    {
        // If player collision with ground change var canJump and reset animation
        if (collision.gameObject.tag == "ground")
        {
            //gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            //canJump = normal.y != 0;
            canJump = false;
            //Debug.Log("peta......................................................");
            
        }
    }*/

    // Coroutine para mover el ladrillo hde arriba a abajo
    private IEnumerator MoveBrick(GameObject brick, float moveDistance, float moveSpeed, float moveDuration)
    {
        var originalPosition = brick.transform.position;
        
        // Guardar el tiempo inicial
        float elapsedTime = 0f;

        // Mover hacia arriba durante la mitad del tiempo total
        while (elapsedTime < moveDuration / 2f)
        {
            brick.transform.position = Vector3.Lerp(originalPosition, originalPosition + Vector3.up * moveDistance, elapsedTime / (moveDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restablecer el temporizador para la bajada
        elapsedTime = 0f;

        // Mover hacia abajo durante la otra mitad del tiempo total
        while (elapsedTime < moveDuration / 2f)
        {
            brick.transform.position = Vector3.Lerp(originalPosition + Vector3.up * moveDistance, originalPosition, elapsedTime / (moveDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Al final de la animación, asegúrate de que el ladrillo esté en su posición original
        brick.transform.position = originalPosition;
        
    }
}
