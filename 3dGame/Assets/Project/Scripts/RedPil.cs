using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPil : Pill {

    public int damage = 10;

    protected override void OnPicked(Collider other) {
        base.OnPicked(other);
        HealtManager healtManager = other.GetComponent<HealtManager>();
        if(!healtManager) { return; }

        healtManager.Damage(damage);
        Destroy(gameObject, 2);
    }
}
