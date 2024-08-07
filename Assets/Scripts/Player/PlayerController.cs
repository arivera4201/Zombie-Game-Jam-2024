using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public float speed = 5.0f;
    public float gravity = -20.0f;
    public float jumpHeight = 2.0f;
    public float sprintSpeed = 1.5f;
    private float sprintMultiplier = 5.0f;

    public GameObject arm;
    private Vector3 armOriginalPos;
    private float interpolationFrames = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //arm = GameObject.FindGameObjectWithTag("Arm");
        armOriginalPos = arm.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void Move(Vector2 input, bool isSprinting)
    {
        Vector3 movementDirection = Vector3.zero;
        movementDirection.x = input.x;
        movementDirection.z = input.y;
        if (isSprinting) sprintMultiplier = sprintSpeed;
        else sprintMultiplier = 1.0f;
        controller.Move(transform.TransformDirection(movementDirection) * speed * sprintMultiplier * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0) velocity.y = -1.0f;
        controller.Move(velocity * Time.deltaTime);
        ArmMovement(input);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1.0f * gravity);
        }
    }

    void ArmMovement(Vector2 input)
    {
        Vector3 armTargetPosition = new Vector3(armOriginalPos.x + input.x / 7.0f, armOriginalPos.y, armOriginalPos.z - input.y / 7.0f);
        arm.transform.localPosition = Vector3.Lerp(arm.transform.localPosition, armTargetPosition, Time.deltaTime * interpolationFrames);
    }
}
