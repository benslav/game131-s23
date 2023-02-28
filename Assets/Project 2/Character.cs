using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    public enum CharacterClass
    {
        Fighter,
        Cleric,
        Archer,
        Mage
    }
    abstract public CharacterClass characterClass { get; }

    // Clamp between 1 and 10
    public uint level = 1;
    public uint BaseHp { get { return level * 10 + power; } }
    public uint BaseTp { get { return level * 5 + (skill + mind) / 2; } }

    public uint power, mobility, skill, mind;

}
