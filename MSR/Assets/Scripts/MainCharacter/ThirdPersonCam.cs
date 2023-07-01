using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using Cinemachine;
using EzySlice;
using UnityEngine.Rendering;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    [SerializeField]
    public GameObject mainPlayerCam;
    public GameObject ultraModeCam;
    public GameObject plane;
    public GameObject pauseMenu;
    public GameObject ultraModeEffects;

    public KeyCode attackMouse = KeyCode.Mouse0;
    public KeyCode attackJoystick = KeyCode.Joystick1Button2;

    public bool ultraMode;

    public Material crossMaterial;
    public LayerMask layerMask;

    public float rotationSpeed;

    public Animator animator;

    private AttributesControler atributesScript;

    private void Start()
    {
        animator = GameObject.Find("MainPlayer").GetComponent<Animator>();    
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject aux = GameObject.Find("GameManager");
        atributesScript = aux.GetComponent<AttributesControler>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("UltraMode") || Input.GetAxis("UltraMode") == 1f)
        {
            //Slow Motion Sound
            FindObjectOfType<AudioManager>().Play("SlowMotionFinish");
            ultraMode = true;
            atributesScript.ultraMode = true;

        }
        else 
        {
            //Slow Motion Sound
            FindObjectOfType<AudioManager>().Play("SlowMotionStart");
            plane.transform.localEulerAngles = Vector3.zero;
            ultraMode = false;
            atributesScript.ultraMode = false;
        }

        if (ultraMode)
        {
            animator.SetBool("ultraModeOn", true);
            
            plane.SetActive(true);
            ultraModeEffects.SetActive(true);

            //rotate the plane 
            rotatePlane();
            if (Time.timeScale > 0.3f) 
            {
                
                Time.timeScale -= 0.01f;
            }
            mainPlayerCam.SetActive(false);
            ultraModeCam.SetActive(true);

            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            // rotate player object 
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");
            Vector3 inputDir =  orientation.right * horizontalInput;
            

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed/2);

            if (Input.GetKeyDown(attackMouse) || Input.GetKeyDown(attackJoystick))
            {
               
                //plane.transform.GetChild(0).DOComplete();
                //plane.transform.GetChild(0).DOLocalMoveX(cutPlane.GetChild(0).localPosition.x * -1, .05f).SetEase(Ease.OutExpo);
                //ShakeCamera();
                Slice();
            }
        }
        else 
        {
            animator.SetBool("ultraModeOn", false);
            ultraModeEffects.SetActive(false);
            if (Time.timeScale < 1f && !pauseMenu.GetComponent<PauseMenu>().GameIsPaused)
            {
                Time.timeScale += 0.01f;
            }
            plane.SetActive(false);

            mainPlayerCam.SetActive(true);
            ultraModeCam.SetActive(false);

            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            // rotate player object 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

       
    }

    public void rotatePlane()
    {
        plane.transform.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse Y") / 2);
    }

    public void Slice()
    {
        FindObjectOfType<AudioManager>().Play("KatanaSlice");

        Collider[] hits = Physics.OverlapBox(plane.transform.position, new Vector3(3, 0.1f, 3), plane.transform.rotation, layerMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;
        go.layer = 8;
        go.AddComponent<destroyEnemies>();
        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(plane.transform.position, plane.transform.up, crossSectionMaterial);
    }
}
