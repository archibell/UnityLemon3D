using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement; // Variable to store the movement for the character.
    // Quaternion are mathematical operators that express rotation.
        // Set m_Rotation to a default value of no rotation.
        // Quaternion.identity == no rotation.
    Quaternion m_Rotation = Quaternion.identity;

    Animator m_Animator; // Interface to control the Mecanim animation system.
    Rigidbody m_Rigidbody; // This component allows Ruby to use physics. The physics in this case are useful for colliding, etc.

    // turnSpeed >> is the angle in radians you want the character to turn per second. 
    public float turnSpeed = 20f; // To control how fast J. Lemon should turn.

    AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing the created variables to something so it's not null.
            // The main components you access a lot during the game, you grab during start &
            // store them in a private field for use later.
        // GetComponent >> 	Gets a reference to a component of type T on the same GameObject as the component specified.
            // myResults = GetComponent<ComponentType>()
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

    }

    // Because OnAnimatorMove is called ontime with physics, change the below method from Update() to FixedUpdate().
    void FixedUpdate() // Physics update always happens 50x per second.
    {
        // The direction in which the keyboard is telling Ruby to move in this frame.
        float horizontal = -Input.GetAxis("Horizontal"); // "Horizontal" is tied to the Project Settings >> Input Manager settings.
        float vertical = -Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical); // Vector3.Set(float newX, float newY, float newZ);
        m_Movement.Normalize(); // Call Normalize to make the vector's length equal to 1.

        // Approximately takes 2 float parameters & returns a bool -
            // true if the 2 floats are approximately equal & false otherwise.
            // Approximately will return true if the horizontal variable is approximately zero.
            // However because of the ! (logical negation operator), hasHorizontal is true when horizontal is non-zero.
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        // We also care for hasVerticalInput.
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        // Now combine both horizontal & vertical:
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        // Use Animator.SetBool to pass Boolean values to an Animator Controller via script.
            // The first parameter is the name of the Animator Parameter that you want to set the value of,
            // and the second is the value you want to set it to.
        m_Animator.SetBool("IsWalking", isWalking); //  Animator.SetBool(string name, bool value)
        if (isWalking) // Play the Audio Source if isWalking is true.
        {
            if (!m_AudioSource.isPlaying) // We don't want to call Play() every frame, only if the Audio Source isn't already playing.
            {
                m_AudioSource.Play();
            }
        }
        else // Stop the Audio Source if isWalking is false.
        {
            m_AudioSource.Stop();
        }

        // RotateTowards >> Rotates a vector current towards target.
            // Vector3.RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta (change in angle in radians), float maxMagnitudeDelta (change in magnitude or length);
        // transform.forward >> Returns a normalized vector representing the Z axis (blue axis) of the transform in world space.
            //  Unlike Vector3.forward, Transform.forward moves the GameObject while also considering its rotation.
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        // Quaternion.LookRotation >> creates a rotation looking in the direction of the given parameter.  
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    // OnAnimatorMove is a special MonoBehavior method that can be used to change how root motion is applied from the Animator.
        // This method allows you to apply root motion as you want, which means that movement and rotation can be applied separately.
        // OnAnimatorMove is a callback for processing animation movements for modifying root motion.
        // This callback will be invoked at each frame after the state machines and the animations have been evaluated.
    private void OnAnimatorMove()
    {
        // Rigidbody.MovePosition(Vector3 position) >> moves the Rigidbody towards position.
            // position argument >> provides the new position for the Rigidbody object.
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
            // The new position starts off at the Rigidbody’s current position,and then you’ve add a change to that —
            // the movement vector multiplied by the magnitude of the Animator’s deltaPosition:
                // Animator.deltaPosition >> is the change in position due to root motion that would have been applied to this frame.
                // We are taking the magnitude (the length) & multiplying it by the movement vector,
                // which is the direction we want the character to move.
        // Apply the rotation:
        m_Rigidbody.MoveRotation(m_Rotation);

    }


}