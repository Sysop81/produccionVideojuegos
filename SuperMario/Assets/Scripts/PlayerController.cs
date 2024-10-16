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

    // Trigger OnCollisionStay2D
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") && rb2D.velocity.y < 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            canJump = true;
        }
    }

    // Trigger OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Verify the normal collision
        ContactPoint2D contact = collision.contacts[0];  
        Vector2 normal = contact.normal;
        
        // If player collision with ground change var canJump and reset animation
        if (collision.gameObject.CompareTag("ground"))
        {
            gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            canJump = true;
            return;
        }
        
        // Brick collision
        if (collision.gameObject.CompareTag("Brick"))
        {
            
            if (normal.y >= 0.0f)
            {
                gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
                canJump = normal.y != 0;
                return;
            }

            // If Mario is big -> destroy the brick
            if (!isSmall)
            {
                Destroy(collision.gameObject);
                return;
            }

            // If Mario is small. Load corrutine to set animation to move bricks
            StartCoroutine(MoveBrick(collision.gameObject,0.2f,5f,0.1f));
        }
        
        // SurpriseBox collisions
        if (collision.gameObject.CompareTag("SurpriseBox"))
        {
            
            if (normal.y >= 0.0f)
            {
                gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
                canJump = normal.y != 0;
                return;
            }
            
            // Mario hit the surprise box
            // Set animation to move bricks
            StartCoroutine(MoveBrick(collision.gameObject,0.2f,5f,0.1f));
            
            // Change animation to surpriseBox
            collision.gameObject.GetComponent<Animator>().SetBool("isHitted", true);

            // Instanciate a mushrrom
            if (!_isMushroomout && collision.gameObject.transform.position.x > 7.5 && collision.gameObject.transform.position.x < 7.52)
            {
                Instantiate(mushroom, new Vector3(collision.gameObject.transform.position.x + 0.5f,
                    collision.gameObject.transform.position.y + 1f, collision.gameObject.transform.position.z), Quaternion.identity);
                _isMushroomout = true;
            }
        }
        
        // Collision with mushroom object
        if (collision.gameObject.CompareTag("mushroom"))
        {
            Destroy(collision.gameObject);
            isSmall = false;
            jumpForce = 700f;
            transform.localScale = new Vector3(10f, 9f, 1f);
        }
    }
    

    // Coroutine to move any brick when Mario hit hits from below
    private IEnumerator MoveBrick(GameObject brick, float moveDistance, float moveSpeed, float moveDuration)
    {
        // Get original brik position
        var originalPosition = brick.transform.position;
        
        // Save initial time 
        float elapsedTime = 0f;

        // Move up for half the total time
        while (elapsedTime < moveDuration / 2f)
        {
            brick.transform.position = Vector3.Lerp(originalPosition, originalPosition + Vector3.up * moveDistance, elapsedTime / (moveDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restar counter for the descent
        elapsedTime = 0f;

        // Move down for the rest of total time
        while (elapsedTime < moveDuration / 2f)
        {
            brick.transform.position = Vector3.Lerp(originalPosition + Vector3.up * moveDistance, originalPosition, elapsedTime / (moveDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set brick transform to original position
        brick.transform.position = originalPosition;
    }
}
