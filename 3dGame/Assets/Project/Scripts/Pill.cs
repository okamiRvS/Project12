using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour {

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickSound;
    public bool goUp;
    public float speed = 0.25f;
    public float rotationSpeed = 100f;


    void Start() {
        StartCoroutine(SwitchDirection());
    }

    void Update() {

        if (goUp) {
            transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
        }
        else {
            transform.position = transform.position + new Vector3(0, -speed * Time.deltaTime, 0);
        }

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    IEnumerator SwitchDirection() {

        while (gameObject.activeSelf) {

            yield return new WaitForSeconds(0.5f);
            goUp = !goUp;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            OnPicked(other);
        }
    }

    protected virtual void OnPicked(Collider other) {
        // to pick sound, but we have already set from inspector the audio
        // audioSource.clip = pickSound;
        audioSource.Play();

        // to hide the pill when player pass over him
        HidePill();
        GetComponent<Collider>().enabled = false;
        Debug.Log("Hai preso " + gameObject.name);
    }

    void HidePill() {
        GetComponent<Renderer>().enabled = false;
    }
}
