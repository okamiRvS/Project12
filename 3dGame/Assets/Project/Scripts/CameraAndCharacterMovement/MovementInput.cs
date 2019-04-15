﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{

    public float InputX;
    public float InputZ;
    public bool SHIFTClicked;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desireRotationSpeed;
    public Animator anim;
    public float Speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector;
    public GameObject particle;

    private bool tapOnTheScreen = false;
    private RaycastHit hit;
    LayerMask layerMask = ~(1 << 10);

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        desireRotationSpeed = 0.1f;
        allowPlayerRotation = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {

        // MOBILE

        // Input.touchCount count how much fingers hit the screen (max 5)
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended &&
                touch.position.x < Screen.width * 5/6 && touch.position.x > Screen.width/6)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 200, layerMask))
                {
                    tapOnTheScreen = true;
                    Speed = 1;
                }
            }
        }

        float distOnTheTap = Vector3.Distance(transform.position, hit.point);
        if (tapOnTheScreen && distOnTheTap >= 0)
        {
            if (distOnTheTap < 0.5f)
            {
                Speed = 0;
                return;
            }
            else if (distOnTheTap < 1f)
            {
                Speed = -0.1f;
                transform.position = Vector3.MoveTowards(transform.position, hit.point, Speed * Time.deltaTime);
            }

            Vector3 heading = hit.point - transform.position;
            float distance = heading.magnitude;
            Vector3 desiredMoveDirection = heading / distance; // This is now the normalized direction.

            // Set animator parameters
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);

            if (blockRotationPlayer == false)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desireRotationSpeed);
            }

            characterGrounded();
            return;
        }

        // END MOBILE>

        InputMagnitude();
        characterGrounded();

    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desireRotationSpeed);
        }

        float inp = anim.GetFloat("InputMagnitude");
        float acceleration = inp - 1;   // [0, 1] it enables to accelerate, so character doesn't move faster immediately
        if (inp > 0 && SHIFTClicked)
        {
            // move forward at speed "Time.deltaTime * acceleration * 10"
            transform.Translate(new Vector3(0, 0, Time.deltaTime * acceleration * 5));

        }
        else if (inp > 0)
        {
            // // move forward at speed "Time.deltaTime * acceleration * 2"
            transform.Translate(new Vector3(0, 0, Time.deltaTime * acceleration * 2));

        }

    }

    void InputMagnitude()
    {
        // Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        // GetKey       if hold
        // GetKeyDown   if clicked
        SHIFTClicked = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Set animator parameters
        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);
        anim.SetBool("SHIFT", SHIFTClicked);

        // Calculate the Input Magnitude, is the sum of the positive values of InputX and InputZ
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        // if InputX + InputZ is equal to 2 we convert speed to 1 because we don't running if user does diagonal movements
        if (Speed > 1)
        {
            Speed = 1;
        }

        // Physically move player
        if (Speed > allowPlayerRotation)
        {
            if (SHIFTClicked)
            {
                anim.SetFloat("InputMagnitude", Speed + 1, 0.5f, Time.deltaTime);
            }
            else
            {
                anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            }
            PlayerMoveAndRotation();

        }
        else if (Speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);

        }

    }

    void characterGrounded()
    {
        // If you don't need the character grounded then get rid this part
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 0.001f;
        }

        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);
    }
}
