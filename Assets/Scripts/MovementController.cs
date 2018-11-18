using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    CharacterController controller;
    Camera camera;

    public float speed = 3.0f;
    public float jumpSpeed = 5.0f;
    public float gravity = 20.0f;
    public float sensitivity = 2f;

    Vector3 moveDirection = Vector3.zero;
    float rotate = 0;
    bool active = true;

    // Use this for initialization
    void Start() {
        camera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
    }

    void UpdateMovement() {
        if (controller.isGrounded && active) {
            // We are grounded, so recalculate
            // move direction directly from axes
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float amount = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(horizontal, 2) + Mathf.Pow(vertical, 2)));
            float damping = 1.0f / amount;
            damping = damping > 1 ? 1 : damping;

            moveDirection = new Vector3(horizontal * damping, 0.0f, vertical * damping);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void UpdateMouse() {
        float horizontal = Input.GetAxis("Mouse X") * sensitivity;
        float vertical = -Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(0, horizontal, 0);

        rotate += vertical * 45 * Time.deltaTime;
        rotate = Mathf.Clamp(rotate, -80, 80);
        camera.transform.localEulerAngles = new Vector3(rotate, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update() {
        UpdateMovement();
        if (active) {
            UpdateMouse();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape)) {
            active = !active;
            Debug.Log("escaped = " + active);
        }
    }
}
