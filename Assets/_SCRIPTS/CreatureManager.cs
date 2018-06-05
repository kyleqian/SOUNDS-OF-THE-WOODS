using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum CreatureType
{
    Squirrel, Deer, Bunny, Raccoon, Crow, Wolf, Shapeshifter, Reverse, Decoy
}

public class CreatureManager : ManagerBase
{
    // Because Unity can't serialize Dictionaries...
    // `creatureTypes` is the key
    // `creatureCount` and `creaturePrefabs` are the values
    [SerializeField] CreatureType[] creatureTypes; // PUT IN ORDER!
    [SerializeField] int[] creatureCounts;
    [SerializeField] GameObject[] creaturePrefabs;

    Dictionary<CreatureType, GameObject[]> creaturePools;

    void Awake()
    {
        Array _creatureTypes = Enum.GetValues(typeof(CreatureType));

        // Check that all types are in the right order
        // A lil hacky yes
        Assert.IsTrue(creatureTypes.Length == _creatureTypes.Length);
        for (int i = 0; i < creatureTypes.Length; ++i)
        {
            Assert.IsTrue(creatureTypes[i] == (CreatureType)_creatureTypes.GetValue(i));
        }
        Assert.IsTrue(creatureCounts.Length == _creatureTypes.Length);
        Assert.IsTrue(creaturePrefabs.Length == _creatureTypes.Length);

        // Set up pools
        creaturePools = new Dictionary<CreatureType, GameObject[]>();
        foreach (CreatureType type in creatureTypes)
        {
            creaturePools[type] = new GameObject[creatureCounts[(int)type]];
            for (int i = 0; i < creatureCounts[(int)type]; ++i)
            {
                creaturePools[type][i] = Instantiate(creaturePrefabs[(int)type]);
                creaturePools[type][i].SetActive(false);
            }
        }
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }

    protected override void OnPhaseUnload(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }
}
