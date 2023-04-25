using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _myBody; 
    public float speed = 5f; 
    // Start is called before the first frame update
    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A D 
        float v = Input.GetAxis("Vertical"); // W S 
        var pos = transform.position;

        pos.x += h * Time.deltaTime * speed;  
        pos.y += v * Time.deltaTime * speed;

        transform.position = pos;
    }
}
