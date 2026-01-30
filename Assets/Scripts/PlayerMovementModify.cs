// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// #if ENABLE_INPUT_SYSTEM
// using UnityEngine.InputSystem;
// #endif

// public class PlayerMovementModify : MonoBehaviour
// {
//     public CharacterController controller;
    
//     // This is now unused for rotation, but we keep it
//     public Transform modelTransform; 
//     public Transform foamGeneratorParent;
    
//     // *** NEW: Add reference to your first-person camera's Transform ***
//     public Transform firstPersonCam; 
    
//     // rotationSpeed is no longer needed for movement
//     public float rotationSpeed = 5f;

//     public float speed = 12f;
//     public float gravity = -10f;
//     public float jumpHeight = 2f;

//     public Transform groundCheck;
//     public float groundDistance = 0.4f;
//     public LayerMask groundMask;

//     Vector3 velocity;
//     bool isGrounded;

// #if ENABLE_INPUT_SYSTEM
//     InputAction movement;
//     InputAction jump;

//     void Start()
//     {
//         // Your existing Input System setup:
//         movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
//         movement.AddCompositeBinding("Dpad")
//             .With("Up", "<Keyboard>/s")
//             .With("Up", "<Keyboard>/downArrow")
//             .With("Down", "<Keyboard>/w")
//             .With("Down", "<Keyboard>/upArrow")
//             .With("Right", "<Keyboard>/a")
//             .With("Right", "<Keyboard>/leftArrow")
//             .With("Left", "<Keyboard>/d")
//             .With("Left", "<Keyboard>/rightArrow");

//         jump = new InputAction("PlayerJump", binding: "<Gamepad>/a");
//         jump.AddBinding("<Keyboard>/space");

//         movement.Enable();
//         jump.Enable();
//     }

// #endif

//     // Update is called once per frame
//     void Update()
//     {
//         float x;
//         float z;
//         bool jumpPressed = false;

// #if ENABLE_INPUT_SYSTEM
//         var delta = movement.ReadValue<Vector2>();
        
//         // Note: The negation on x and z from your previous script is retained.
//         // If your controls feel backward (W moves you back), remove the negation.
//         x = -delta.x;
//         z = -delta.y;
//         jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);
// #else
//         x = -Input.GetAxis("Horizontal");
//         z = -Input.GetAxis("Vertical");
//         jumpPressed = Input.GetButtonDown("Jump");
// #endif

//         // --- Ground Check ---
//         isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

//         if (isGrounded && velocity.y < 0)
//         {
//             velocity.y = -2f;
//         }
        
//         // *** FPS MOVEMENT LOGIC (Camera-Relative) ***
        
//         // 1. Get the camera's forward vector and flatten it to the XZ plane.
//         Vector3 forward = firstPersonCam.forward;
//         forward.y = 0; // Ensures player moves along the ground
//         forward.Normalize();
        
//         // 2. Get the camera's right vector and flatten it.
//         Vector3 right = firstPersonCam.right;
//         right.y = 0;
//         right.Normalize();
        
//         // 3. Calculate the final movement vector based on camera direction
//         Vector3 move = right * x + forward * z; 
        
//         // Normalizing prevents faster diagonal movement
//         move = Vector3.Normalize(move);

//         controller.Move(move * speed * Time.deltaTime);

//         // --- Jumping and Gravity ---
//         if (jumpPressed && isGrounded)
//         {
//             velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//         }

//         velocity.y += gravity * Time.deltaTime;

//         controller.Move(velocity * Time.deltaTime);
        
//         // *** REMOVED: Player body rotation logic ***
//         // This logic is handled by the MouseLook script now.
//         /*
//         if (move != Vector3.zero)
//         {
//              Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
//              modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//         }
//         */
        
//         foamGeneratorParent.localScale = Vector3.one * move.magnitude;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementModify : MonoBehaviour
{
    public CharacterController controller;
    
    // These are now for visualization/effects, not core rotation
    public Transform modelTransform; 
    public Transform foamGeneratorParent;
    
    // Reference to your first-person camera's Transform (REQUIRED in Inspector)
    public Transform firstPersonCam; 
    
    // Rotation speed is kept but unused for movement
    public float rotationSpeed = 5f;

    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    InputAction movement;
    InputAction jump;

    void Start()
    {
        // *** CLEANED UP INPUT BINDINGS ***
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")      // W = Forward (Positive Z)
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")    // S = Backward (Negative Z)
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")     // A = Left (Negative X)
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")    // D = Right (Positive X)
            .With("Right", "<Keyboard>/rightArrow");

        jump = new InputAction("PlayerJump", binding: "<Gamepad>/a");
        jump.AddBinding("<Keyboard>/space");

        movement.Enable();
        jump.Enable();
    }

    void Update()
    {
        float x;
        float z;
        bool jumpPressed = false;

        // --- Read Input ---
        var delta = movement.ReadValue<Vector2>();
        
        // Removed negation for correct WASD direction (W is now positive Z, D is positive X)
        x = delta.x;
        z = delta.y;
        jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);

        // --- Ground Check ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // *** FPS MOVEMENT LOGIC: Use Player Root's direction (which is rotated by CameraOnlyLook) ***
        
        // 1. Get the Player Root's forward vector and flatten it to the XZ plane.
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        
        // 2. Get the Player Root's right vector and flatten it.
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();
        
        // 3. Calculate the final movement vector based on the rotated Player Root direction
        Vector3 move = right * x + forward * z; 
        
        // Normalizing prevents faster diagonal movement
        move = Vector3.Normalize(move);

        controller.Move(move * speed * Time.deltaTime);

        // --- Jumping and Gravity ---
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        // *** IMPORTANT: The original body rotation logic remains commented out ***
        // Player rotation is handled entirely by CameraOnlyLook.cs
        
        foamGeneratorParent.localScale = Vector3.one * move.magnitude;
    }
}