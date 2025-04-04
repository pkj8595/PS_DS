using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColision : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
    }
    void Start()
    {

    }

    void Update()
    {
        

    }
}
