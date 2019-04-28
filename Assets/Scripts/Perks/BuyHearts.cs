using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHearts : Upgradable
{
    public int[] Hearts;
    public override void UpgradeTo(int level)
    {
        Character.I.GainMaxHearts(Hearts[level]);
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.I.GainMaxHearts(Hearts[0]);
    }
}
