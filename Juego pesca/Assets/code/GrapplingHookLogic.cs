using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        public LayerMask fishMask;
        public LayerMask mision;

        [Header("Objetos")]
        public Transform gunTip;
        public Transform cam;
        public Transform player;
        public Camera camCamera;
        public GameObject crosshair;
        public GameObject pulsera;
        public GameObject fishPrefab;
        public FishGenerator generator;

        [Header("Rarezas")]
        public TextMeshProUGUI[] rarezas;
        private List<int> rarezasCounter = new List<int> { 0, 0, 0, 0, 0 };
        List<string> rarezasList = new List<string> { "comunes", "raros", "peculiares", "legendarios", "exóticos" };
        private int totalFishes = 0;

        [Header("UI")]
        public bool inStore;
        public TextMeshProUGUI winLabel;
        public TextMeshProUGUI misionDescription;
        public TextMeshProUGUI economiaPlateada;
        public Timer timer;

        private LineRenderer lr;
        private Transform anchor;
        private SpringJoint joint;
        private Vector3 grapplingPoint = new(0, 0, 0);
        private float distance;
        private bool movil;
        private float defaultAirMultiplier;
        private bool zoomOut = true;
        private AudioSource grappleSound;
        private int money;

        public PlayerController pl;
        bool hookDeplyed;
        float rt;
        bool controllerConnected;
        string rareza;

        string rarezasData;
        int cantidad;
        int tiempo;
        bool startMission;

        void Start() {
            startMission = false;
            money = 0;
            economiaPlateada.text = "Alhajas: " + money;
            ResetCounters();
            lr = GetComponent<LineRenderer>();
            defaultAirMultiplier = pl.getAirMultiplier();
            grappleSound = GetComponent<AudioSource>();

            hookDeplyed = false;
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

            if (Input.GetKeyDown(KeyCode.E) && inStore)
            {
                //open store panel
            }

            //detiene el gancho al morir
            if (player.position.y <= 43.5f) {
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
            else if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo2, maxDistance, fishMask)){
                hookDeplyed = true;
                grappleSound.Play();
                //para objeto estatico
                grapplingPoint = hitInfo2.point;
                //para objeto en movimiento
                anchor = hitInfo2.transform;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.enablePreprocessing = false;
                joint.enableCollision = true;

                joint.connectedBody = hitInfo2.rigidbody;
                distance = Vector3.Distance(player.position, grapplingPoint);
                movil = true;

                joint.minDistance = distance * 0.25f;
                joint.spring = 1;
                joint.damper = 1;
                joint.massScale = 1;

                lr.positionCount = 2;

                rareza = hitInfo2.collider.gameObject.GetComponent<Renderer>().material.ToString();
                GetRareza(rareza);

                Invoke("StopHook", 0.2f);
                Destroy(hitInfo2.collider.gameObject,0.2f);
                Instantiate(fishPrefab, generator.SetRandomPosition(), Quaternion.identity);

            } else if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo3, maxDistance, mision)) {
                hookDeplyed = true;
                grappleSound.Play();
                //para objeto estatico
                grapplingPoint = hitInfo3.point;
                //para objeto en movimiento
                anchor = hitInfo3.transform;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.enablePreprocessing = false;
                joint.enableCollision = true;

                joint.connectedBody = hitInfo3.rigidbody;
                distance = Vector3.Distance(player.position, grapplingPoint);
                movil = true;

                joint.minDistance = distance * 0.25f;
                joint.spring = 1;
                joint.damper = 1;
                joint.massScale = 1;

                lr.positionCount = 2;

                GameObject missionObject = hitInfo3.collider.gameObject;
                Missions missionInfo = missionObject.GetComponentInChildren<Missions>();

                GetMissionData(missionInfo);
                missionInfo.SetRandomMission();

                Invoke("StopHook", 0.2f);
                ResetCounters();
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

        void SetContadores(int index, string rareza, int value) {
            rarezas[index].text = rareza+ ": " + value;
        }

        void GetRareza(string rareza) {
            totalFishes++;
            SetContadores(5, "Total", totalFishes);
            switch (rareza) {
                case "fish mat_white (Instance) (UnityEngine.Material)":
                    rarezasCounter[0]++;
                    SetContadores(0, "Comunes", rarezasCounter[0]);
                    break;
                case "fish mat_green (Instance) (UnityEngine.Material)":
                    rarezasCounter[1]++;
                    SetContadores(1, "Raros", rarezasCounter[1]);
                    break;
                case "fish mat_blue (Instance) (UnityEngine.Material)":
                    rarezasCounter[2]++;
                    SetContadores(2, "Peculiares", rarezasCounter[2]);
                    break;
                case "fish mat_purple (Instance) (UnityEngine.Material)":
                    rarezasCounter[3]++;
                    SetContadores(3, "Legendarios", rarezasCounter[3]);
                    break;
                default:
                    rarezasCounter[4]++;
                    SetContadores(4, "Exóticos", rarezasCounter[4]);
                    break;
            }
            WinChecker();
        }

        void GetMissionData(Missions mision) {
            cantidad = mision.cantidad;
            rarezasData = mision.rareza;

            tiempo = mision.tiempo;
            misionDescription.text = mision.GetText();

            startMission = true;
            rarezasCounter = new List<int> { 0, 0, 0, 0, 0 };
            totalFishes = 0;
            timer.SetPause(false);
            winLabel.text = "Misión actual:";
        }

        public bool GetStartMission() {
            return startMission;
        }

        public void SetStartMission(bool flag) {
            startMission = flag;
        }

        public float GetTime() {
            return tiempo;
        }

        void ResetCounters() {
            rarezasCounter = new List<int> { 0, 0, 0, 0, 0 };
            totalFishes = 0;
            SetContadores(0, "Comunes", rarezasCounter[0]);
            SetContadores(1, "Raros", rarezasCounter[1]);
            SetContadores(2, "Peculiares", rarezasCounter[2]);
            SetContadores(3, "Legendarios", rarezasCounter[3]);
            SetContadores(4, "Exóticos", rarezasCounter[4]);
            SetContadores(5, "Total", totalFishes);
        }

        void WinChecker(){
            Debug.Log(rarezasList.IndexOf(rarezasData));
            float tiempoActual = timer.GetTimer();
            if (rarezasList.IndexOf(rarezasData) != -1){
                if ((rarezasCounter[rarezasList.IndexOf(rarezasData)] == cantidad) && tiempoActual > 0){
                    winLabel.text = "Misión cumplida";
                    // economia plateada
                    money += 100;
                    economiaPlateada.text = "Alhajas: " + money;
                    misionDescription.text = "";
                    ResetCounters();
                    timer.SetPause(true);
                }
                else if (tiempoActual <= 0){
                    winLabel.text = "Misión fallida";
                    misionDescription.text = "";
                }
            }
            
        }
    }
}