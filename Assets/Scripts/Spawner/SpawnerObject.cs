using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    private Rigidbody rb;
    public float minY =0.6f;
    public float maxY = 10.0f;
    public float z =-2;
    private bool freeze =false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (freeze)
        {
            freeze = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.SetActive(false);
        }

        if ( transform.position.y <= minY)
        {
           
            print("Move object");
            rb.useGravity = false;
            rb.position = new Vector3(transform.position.x, maxY, z);
            //rb.constraints = RigidbodyConstraints.FreezeAll;
           // gameObject.SetActive(false);
           freeze = true;

        }
 
    }
}
