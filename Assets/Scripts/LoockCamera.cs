using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoockCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
