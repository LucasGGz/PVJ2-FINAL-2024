using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Netcode;
public class PlayerController : NetworkBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float range = 5f;
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

    [SerializeField] private Transform spawnObjectPrefab;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        playerInput = new Vector3(horizontal, 0, vertical).normalized;
        // playerInput = Vector3.ClampMagnitude(playerInput, 1);
        camDirection();

        moveDir = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = moveDir * speed;

        if (Input.GetKeyDown(KeyCode.T))
        {
            InstantiaServerRpc();
        }

        // Verificar si moveDir tiene una magnitud suficiente antes de usar LookRotation
        if (moveDir.magnitude > 0.1f)
        {
            //  Debug.Log("En movimiento");
            // Interpolar la rotación de manera suave
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            //Debug.Log("Quieto");
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

    [ServerRpc]
    private void InstantiaServerRpc()
    {
        Transform spawnObjectTransform = Instantiate(spawnObjectPrefab);
        spawnObjectTransform.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    {
        UpdatePositionServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePositionServerRpc()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(range, -range), 0, UnityEngine.Random.Range(range, -range));
        transform.rotation = new Quaternion(0, 180, 0, 0);
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
