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
    [SerializeField] CreatureType[] creatureTypes;
    [SerializeField] int[] creatureCounts;
    [SerializeField] GameObject[] creaturePrefabs;

    Dictionary<CreatureType, CreatureBase[]> creaturePools;

    void Awake()
    {
        // Check that all types are in the right order
        // A lil hacky yes
        Array _creatureTypes = Enum.GetValues(typeof(CreatureType));

        Assert.IsTrue(creatureTypes.Length == _creatureTypes.Length);
        for (int i = 0; i < creatureTypes.Length; ++i)
        {
            Assert.IsTrue(creatureTypes[i] == (CreatureType)_creatureTypes.GetValue(i));
        }
        Assert.IsTrue(creatureCounts.Length == _creatureTypes.Length);
        Assert.IsTrue(creaturePrefabs.Length == _creatureTypes.Length);

        InitializeCreaturePools();
    }

    void InitializeCreaturePools()
    {
        creaturePools = new Dictionary<CreatureType, CreatureBase[]>();
        foreach (CreatureType type in creatureTypes)
        {
            creaturePools[type] = new CreatureBase[creatureCounts[(int)type]];
            for (int i = 0; i < creatureCounts[(int)type]; ++i)
            {
                GameObject newCreature = Instantiate(creaturePrefabs[(int)type]);
                creaturePools[type][i] = newCreature.GetComponent<CreatureBase>();
                newCreature.SetActive(false);
            }
        }
    }

    // TODO: Deal with spawning more than there are in reserve
    public void SpawnCreatures(CreatureType type, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            // Find an unspawned creature of that type
            var unspawnedCreature = Array.Find(creaturePools[type], c => !c.Spawned);

            // Break early if found none
            if (unspawnedCreature == null)
            {
                break;
            }

            // Spawn it
            unspawnedCreature.Spawn();
        }
    }

    // TODO: Possibly do a proportion instead of raw count?
    public void DespawnCreatures(CreatureType type, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            // Find a spawned creature of that type
            var spawnedCreature = Array.Find(creaturePools[type], c => c.Spawned);

            // Break early if found none
            if (spawnedCreature == null)
            {
                break;
            }

            // Despawn it
            spawnedCreature.Despawn();
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
