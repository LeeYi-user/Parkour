using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : NetworkBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [SerializeField]
    private float movementSpeed, rotationSpeed, jumpSpeed, gravity;

    private Vector3 movementDirection = Vector3.zero;
    private bool playerGrounded;

    [SerializeField]
    private CinemachineVirtualCamera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (IsOwner)
        {
            playerCam = FindObjectOfType<CinemachineVirtualCamera>();
            playerCam.LookAt = transform;
            playerCam.Follow = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        playerGrounded = characterController.isGrounded;

        //movement
        Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");

        characterController.Move(inputMovement * Time.deltaTime);
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime);

        //jumping
        if (Input.GetButton("Jump") && playerGrounded)
        {
            movementDirection.y = jumpSpeed;
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);

        //animations
        animator.SetBool("isRunning", Input.GetAxisRaw("Vertical") != 0);
        animator.SetBool("isJumping", !characterController.isGrounded);

    }
}
