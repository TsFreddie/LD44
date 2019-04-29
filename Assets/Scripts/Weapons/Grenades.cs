using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericProjectileFirer))]
public class Grenades : Upgradable
{
    public bool[] ExplodeOnGround;
    public float[] ExplosionRadius;
    private GenericProjectileFirer bulletFirer;
    public override void UpgradeTo(int level)
    {
        bulletFirer.ExplodeOnGround = ExplodeOnGround[level];
        bulletFirer.ExplosionRadius = ExplosionRadius[level];
    }

    void Start()
    {
        bulletFirer = GetComponent<GenericProjectileFirer>();
    } 
}
