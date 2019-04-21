using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityChan
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class movement : MonoBehaviour
    {
        private CapsuleCollider col;
        private Rigidbody rb;
        private Animator anim;
        private AnimatorStateInfo currentBaseState;

        private Vector3 velocity;
        private float orgColHight;
        private Vector3 orgVectColCenter;


        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int walk = Animator.StringToHash("Base Layer.Walk");

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();

            orgColHight = col.height;
            orgVectColCenter = col.center;
        }

        private void Update()
        {

        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                int n = (int)Random.Range(0, 2);
                anim.Play("DM" + n, -1, 0.0f);
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool run = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            anim.SetFloat("Speed", v);
            anim.SetFloat("H", h);
            anim.SetFloat("V", v);
            anim.SetBool("Run", run);

            velocity = new Vector3(0, 0, v);
            velocity = transform.TransformDirection(velocity);

            float moveX = h * 20f * Time.deltaTime;

            float moveZ = 0;

            if (run)
            {
                moveZ = v * 350f * Time.deltaTime;
            }
            else
            {
                moveZ = v * 100f * Time.deltaTime;
            }

            Vector3 vtemp = new Vector3(moveX, 0f, moveZ);
            vtemp = transform.TransformVector(vtemp);
            rb.velocity = vtemp;

            transform.Rotate(0, h * 2, 0);

            resetCollider();
        }

        void resetCollider()
        {
            col.height = orgColHight;
            col.center = orgVectColCenter;
            Debug.Log("ok");
        }
    }
}