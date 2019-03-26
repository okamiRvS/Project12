using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePil : Pill {

    public int healAmount = 10;

    protected override void OnPicked(Collider other) {
        base.OnPicked(other);
        HealtManager healtManager = other.GetComponent<HealtManager>();
        if (!healtManager) { return; }

        healtManager.Heal(healAmount);
        Destroy(gameObject, 2);
    }
}
