using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    private float ySpeed;
    private CharacterController controller;
    private Vector3 camForward;
    private Vector3 camRight;
    public Camera mainCamera;
    private Vector3 movePlayer;
    private float horizontal;
    private float vertical;
    private Vector3 playerInput;
    private Vector3 moveDir;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        playerInput = new Vector3(horizontal, 0, vertical).normalized;
        // playerInput = Vector3.ClampMagnitude(playerInput, 1);
        camDirection();

        moveDir = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = moveDir * speed;

        // Verificar si moveDir tiene una magnitud suficiente antes de usar LookRotation
        if (moveDir.magnitude > 0.1f)
        {
            Debug.Log("En movimiento");
            // Interpolar la rotación de manera suave
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Quieto");
        }

        // Aplicar la gravedad solo si no está en el suelo
        if (!controller.isGrounded)
        {
            float gravedad = 9.8f;
            ySpeed += Physics.gravity.y * gravedad * Time.deltaTime;
            
        }
        else
        {
            // Asegurarse de que la velocidad vertical sea negativa cuando está en el suelo
            ySpeed = -0.5f;
  
            // Manejar el salto mientras está en el suelo
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
                Debug.Log("salte PA");
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }
        // Aplicar movimiento vertical
        movePlayer.y = ySpeed;

        // Aplicar movimiento
        controller.Move(movePlayer * Time.deltaTime);
    }

    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
}
