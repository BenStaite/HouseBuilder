using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != gameObject)
        {
            Debug.Log("COL");
            GetComponent<BuildObject>().isValid = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            Debug.Log("COLL");
            GetComponent<BuildObject>().isValid = false;
        }
    }
}
