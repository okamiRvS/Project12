using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class HealtManager : MonoBehaviour {

    public int maxHealth;
    public int health;
    public Text lbHealth;
    public Slider sliderHealth;
    public GameObject pnlDeath;
    public Button btnRespawn;
    public GameObject menuPause;

    Vector3 firstTransform;
    Quaternion orentation;

    // create an attribute with other attribute information
    float currentHealthPercentuage {
        get {
            return (float)(health) / (float)(maxHealth);
        }
    }

    void Start() {
        pnlDeath.SetActive(false);
        health = maxHealth - 20;
        lbHealth.text = "Salute: " + health;
        sliderHealth.value = currentHealthPercentuage;
        UpdateColorSlidebar();
        btnRespawn.onClick.AddListener(Respawn);

        // if we want we could remove the listener before in the follow way
        // btnRespawn.onClick.AddListener(Respawn);

        MenuPause info = menuPause.GetComponent<MenuPause>();
        info.IsAlive = true;
        firstTransform = this.transform.position;
        orentation = this.transform.rotation;

    }

    public void Heal(int amount) {
        Damage(-amount);
    }

    public void Damage(int damageTaken) {
        health -= damageTaken;
        if (health < 1) {
            Die();
        }
        if (health > maxHealth) {
            health = maxHealth;
        }
        lbHealth.text = "Salute: " + health;
        sliderHealth.value = currentHealthPercentuage;

        // change fill color bar 
        UpdateColorSlidebar();
    }

    public void UpdateColorSlidebar() {
        if (currentHealthPercentuage >= 0.75f) {
            sliderHealth.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (currentHealthPercentuage >= 0.25) {
            sliderHealth.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else {
            sliderHealth.fillRect.GetComponent<Image>().color = Color.red;
        }
    }

    public void Die() {
        // to block movement after dead, you need add follow code as new name space up to this code 
        // using UnityStandardAssets.Characters.FirstPerson;
        MenuPause info = menuPause.GetComponent<MenuPause>();
        info.IsAlive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pnlDeath.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn() {
        MenuPause info = menuPause.GetComponent<MenuPause>();
        info.IsAlive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pnlDeath.SetActive(false);
        Time.timeScale = 1f;

        health = maxHealth;
        lbHealth.text = "Salute: " + health;
        sliderHealth.value = currentHealthPercentuage;
        UpdateColorSlidebar();


        this.transform.position = firstTransform;
        this.transform.rotation = orentation;

    }

}
