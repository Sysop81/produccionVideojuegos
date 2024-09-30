using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("right"))
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(300.0f * Time.deltaTime, 0.0f));
        }else if (Input.GetKey("left"))
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300.0f * Time.deltaTime, 0.0f));
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
        }
    }
}
