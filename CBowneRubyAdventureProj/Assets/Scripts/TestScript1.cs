using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{

    public bool dynamic;
    public bool Addition;
    
    new Rigidbody2D rigidbody2D;




    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = dynamic ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    
        if(dynamic) rigidbody2D.velocity = new Vector2(1f * (Addition? 1.1f : 1f) , 0);
    }

    void Update()
    {
        //if(!dynamic) rigidbody2D.MovePosition(new Vector2(rigidbody2D.position.x + (0.1f * (Addition? 1.1f : 1f)) , rigidbody2D.position.y));
    }
    void FixedUpdate()
    {
        if(!dynamic) rigidbody2D.MovePosition(new Vector2(rigidbody2D.position.x + (1f * (Addition? 1.1f : 1f))*Time.deltaTime , rigidbody2D.position.y));
    }
}
