using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPerkTester : MonoBehaviour
{
    public Character character;
    public GameObject perkPrefab;
    void Start()
    {
        character.GivePerk(perkPrefab);
    }
}
