using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponTester : MonoBehaviour
{
    public Character character;
    public GameObject weaponPrefab;
    void Start()
    {
        character.GiveWeapon(weaponPrefab);
    }
}
