using Grapple;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Propiedades de la camara")]
    public Camera cam;
    public Transform orientation;
    private float sensitivity;

    private float mouseX = 0, mouseY = 0;
    private float rsX = 0, rsY = 0;
    private float xCamRotation;
    private float yCamRotation;

    [Header("Propiedades del jugador")]
    public float moveForce = 250;
    public float jumpForce = 60;
    public float jumpCooldown = 1;
    public float airMultiplier = 1;
    public float defaultSsensitivity = 10;
    public GrapplingHookLogic hookLogic;

    private bool jumpReady;

    private Rigidbody rb;
    private Vector3 moveDirection;
    Quaternion startRot;

    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    bool isGrounded;
    float sphereCastDist;

    private Ray rayo;
    private float horizontalInput, verticalInput;
    private AudioSource deathSound;
    bool controllerConnected;

    void Start() {
        sensitivity = PlayerPrefs.GetFloat("sensibilidad", defaultSsensitivity);
        deathSound = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGrounded = true;
        jumpReady = true;

        startRot = Quaternion.identity;
    }

    // Update is called once per frame
    private void Update() {
        //Verifica si hay un control conectado
        controllerConnected = hookLogic.getControllerConnected();
        if (controllerConnected) {
            //Control
            rsX = Input.GetAxisRaw("Right stick X") * Time.deltaTime * sensitivity*2;
            rsY = Input.GetAxis("Right stick Y") * Time.deltaTime * sensitivity*2;

            //Control de la camara
            yCamRotation += rsX;
            xCamRotation -= rsY;
            xCamRotation = Mathf.Clamp(xCamRotation, -90f, 90f);
            cam.transform.rotation = Quaternion.Euler(xCamRotation, yCamRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yCamRotation, 0);

            //Control del personaje
            horizontalInput = Input.GetAxisRaw("Left stick X");
            verticalInput = Input.GetAxisRaw("Left stick Y");

            if (Input.GetButtonDown("A") && isGrounded && jumpReady) {
                Saltar();
            }
        }
        else {
            //Mouse
            mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

            //Control de la camara
            yCamRotation += mouseX;
            xCamRotation -= mouseY;
            xCamRotation = Mathf.Clamp(xCamRotation, -90f, 90f);
            cam.transform.rotation = Quaternion.Euler(xCamRotation, yCamRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yCamRotation, 0);

            //Control del personaje
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpReady) {
                Saltar();
            }
        }

        //GroundCheck
        RaycastHit groundInfo;
        isGrounded = Physics.SphereCast(transform.position, 0.5f,Vector3.down, out groundInfo, playerHeight * 0.5f + 0.05f, whatIsGround);
        if (isGrounded) {
            rb.drag = groundDrag;
            sphereCastDist = groundInfo.distance;
            if (groundInfo.collider.CompareTag("MovablePlatform") || groundInfo.collider.CompareTag("MovableNoGrappablePlatform")) {
                transform.parent = groundInfo.transform;
                moveForce = 35;
            }
        }
        else {
            rb.drag = 0;
            transform.parent = null;
            moveForce = 20;

        }

        //Control de velocidad
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveForce) {
            Vector3 limitedVel = flatVel.normalized * moveForce;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        //resetea al jugador al morir
        if (transform.position.y <= 42.5f) {
            deathSound.Play();
            transform.position = new Vector3(-87f, 50f, 19f);
            mouseX = 0;
            mouseY = 0;
            xCamRotation = 0;
            yCamRotation = 90;
            orientation.rotation = Quaternion.identity;
            cam.transform.rotation = startRot;
        }
    }
    private void FixedUpdate() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (isGrounded) {
            rb.AddForce(moveDirection.normalized * moveForce * 10f);
        }
        else if (!isGrounded){
            rb.AddForce(moveDirection.normalized * moveForce * airMultiplier * 10f);
        }

    }

    void ResetJump() {
        jumpReady = true;
    }

    public void setAirMultiplier(float value) {
        airMultiplier = value;
    }

    public float getAirMultiplier() {
        return airMultiplier;
    }

    private void Saltar() {
        jumpReady = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.drag = 0;
        transform.parent = null;
        moveForce = 20;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    public float GetSpeed()
    {
        return moveForce;
    }

    public void SetSpeed(float speed)
    {
        this.moveForce = speed;
    }
}
