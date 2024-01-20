using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPrueba : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Choco");
            Destroy(gameObject);
        }
    }
}
