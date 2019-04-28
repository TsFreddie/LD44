using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericBulletFirer))]
public class Pistol : Upgradable
{
    public int[] PiercingCount;
    private GenericBulletFirer bulletFirer;
    public override void UpgradeTo(int level)
    {
        bulletFirer.PiercingCount = PiercingCount[level];
    }

    void Start()
    {
        bulletFirer = GetComponent<GenericBulletFirer>();
    }
}
