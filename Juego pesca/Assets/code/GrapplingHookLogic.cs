using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Grapple {
    public class GrapplingHookLogic : MonoBehaviour {
        [Header("Propiedades fisicas del gancho")]
        public float SpringForce = 5.0f;
        public float SpringDamper = 7.0f;
        public float SpringMassScale = 4.5f;
        public float maxDistance = 30;
        public float airBoost = 1;

        [Header("Layer Mask")]
        public LayerMask whatIsGrappeable;

        [Header("Objetos")]
        public Transform gunTip;
        public Transform cam;
        public Transform player;
        public Camera camCamera;
        public GameObject crosshair;
        public GameObject pulsera;

        private LineRenderer lr;
        private Transform anchor;
        private SpringJoint joint;
        private Vector3 grapplingPoint = new(0, 0, 0);
        private float distance;
        private bool movil;
        private bool fishing;
        private float defaultAirMultiplier;
        private bool zoomOut = true;
        private AudioSource grappleSound;

        public PlayerController pl;
        bool hookDeplyed;
        float rt;
        bool controllerConnected;

        void Start() {
            lr = GetComponent<LineRenderer>();
            defaultAirMultiplier = pl.getAirMultiplier();
            grappleSound = GetComponent<AudioSource>();
            fishing = false;
        }

        void Update() {
            //cambiar color de la mira
            RaycastHit distancia;
            if (Physics.Raycast(cam.position, cam.forward, out distancia, whatIsGrappeable)) {
                if(distancia.distance <= maxDistance) {
                    if (hookDeplyed) {
                        crosshair.GetComponent<Image>().color = Color.green;
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.yellow);
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.yellow);
                    }
                    else if (distancia.collider.CompareTag("GrappableObject") || distancia.collider.CompareTag("MovablePlatform")) {
                        crosshair.GetComponent<Image>().color = Color.green;
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.green);
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.green);
                    }
                    else if (distancia.collider.CompareTag("NoGrappableZone") || distancia.collider.CompareTag("MovableNoGrappablePlatform")) {
                        crosshair.GetComponent<Image>().color = Color.white;
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.red);
                        pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.red);
                    }
                }
                else {
                    crosshair.GetComponent<Image>().color = Color.white;
                    pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.red);
                    pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.red);
                }
            }
            else if (hookDeplyed) {
                crosshair.GetComponent<Image>().color = Color.white;
                pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.yellow);
                pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.yellow);
            }
            else {
                crosshair.GetComponent<Image>().color = Color.white;
                pulsera.GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.red);
                pulsera.GetComponent<Renderer>().materials[2].SetColor("_Color", Color.red);
            }

            // activar el gancho
            // se verifica si hay un control conectado
            rt = Input.GetAxis("RT");
            if (controllerConnected) {
                if ((rt >= 0.5f && !hookDeplyed)) {
                    StartHook();
                }
                else if (rt <= 0.1f && hookDeplyed) {
                    StopHook();
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    StartHook();
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    StopHook();
                }
            }

            //detiene el gancho al morir
            if (player.position.y <= -30) {
                StopHook();
            }

            //cambia el FOV al usar el gancho
            if (zoomOut) {
                if (camCamera.fieldOfView > 75) {
                    camCamera.fieldOfView -= Time.deltaTime * 100f;
                } else {
                    camCamera.fieldOfView = 75;
                }
            }
            else {
                if (camCamera.fieldOfView < 95) {
                    camCamera.fieldOfView += Time.deltaTime * 100f;
                } else {
                    camCamera.fieldOfView = 95;
                }
            }
        }

        void LateUpdate() {
            DrawRope(movil);
        }

        IEnumerator CheckController() {
            while (true) {
                var controles = Input.GetJoystickNames();
                controllerConnected = controles.Length > 0 ? (controles[0] != "") : false;
                //if (controles.Length > 0) {
                //controllerConnected = controles[0] != "";
                //}
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void Awake() {
            StartCoroutine(CheckController());
        }

        public bool getControllerConnected() {
            return controllerConnected;
        }

        void StartHook() {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, maxDistance, whatIsGrappeable)) {

                hookDeplyed = true;
                grappleSound.Play();
                zoomOut = false;
                //para objeto estatico
                grapplingPoint = hitInfo.point;
                //para objeto en movimiento
                anchor = hitInfo.transform;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.enablePreprocessing = false;
                joint.enableCollision = true;
                if (hitInfo.collider.CompareTag("IsFish")){
                    Destroy(hitInfo.collider.gameObject);
                    joint.connectedBody = hitInfo.rigidbody;
                    distance = Vector3.Distance(player.position, anchor.position);
                    movil = true;
                    //fishing = true;
                }
                else{
                    //fishing = false;

                    if (hitInfo.collider.CompareTag("MovablePlatform"))
                    {
                        joint.connectedBody = hitInfo.rigidbody;
                        distance = Vector3.Distance(player.position, grapplingPoint);
                        movil = true;
                    }
                    else
                    {
                        joint.connectedAnchor = grapplingPoint;
                        distance = Vector3.Distance(player.position, anchor.position);
                        movil = false;
                    }

                    joint.minDistance = distance * 0.25f;
                    joint.spring = SpringForce;
                    joint.damper = SpringDamper;
                    joint.massScale = SpringMassScale;

                    pl.setAirMultiplier(airBoost);

                    lr.positionCount = 2;
                }
            }
        }

        void DrawRope(bool flag) {
            if (!joint || anchor == null) return;
            lr.SetPosition(0, gunTip.position);
            //distingue entre punto en movimiento o estatico
            if (flag) {
            lr.SetPosition(1, anchor.position);
            }
            else {
                lr.SetPosition(1, grapplingPoint);
            }
        }

        void StopHook() {
            Destroy(joint);
            pl.setAirMultiplier(defaultAirMultiplier);
            zoomOut = true;
            hookDeplyed = false;
            lr.positionCount = 0;
        }

    }
}