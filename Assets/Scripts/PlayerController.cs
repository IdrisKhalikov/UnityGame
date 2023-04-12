using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;


    public float moveSpeed = 10f;
  
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(horizontal, vertical);
        body.AddForce(movementVector * moveSpeed * 10000 * Time.deltaTime);
    }
}
