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
    [SerializeField] int[] creatureCounts; // Determines initial Instantiation numbers
    [SerializeField] GameObject[] creaturePrefabs;

    Dictionary<CreatureType, List<CreatureBase>> creaturePools;

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
        creaturePools = new Dictionary<CreatureType, List<CreatureBase>>();
        foreach (CreatureType type in creatureTypes)
        {
            creaturePools[type] = new List<CreatureBase>();
            for (int i = 0; i < creatureCounts[(int)type]; ++i)
            {
                InstantiateCreature(type);
            }
        }
    }

    CreatureBase InstantiateCreature(CreatureType type)
    {
        var newCreature = Instantiate(creaturePrefabs[(int)type]);
        var newCreatureComponent = newCreature.GetComponent<CreatureBase>();
        newCreature.name = type.ToString() + creaturePools[type].Count;

        creaturePools[type].Add(newCreatureComponent);
        newCreature.SetActive(false);
        return newCreatureComponent;
    }

    public void SpawnCreatures(CreatureType type, int count)
    {
        // Max count if negative number
        if (count < 0)
        {
            count = creatureCounts[(int)type];
        }

        for (int i = 0; i < count; ++i)
        {
            // Find an unspawned creature of that type
            var unspawnedCreature = creaturePools[type].Find(c => !c.Spawned);

            // Add to pool if not enough creatures in reserve
            if (unspawnedCreature == null)
            {
                unspawnedCreature = InstantiateCreature(type);
                creatureCounts[(int)type]++;
            }

            // Spawn it
            unspawnedCreature.Spawn();
        }
    }

    // TODO: Possibly do a proportion instead of raw count?
    // TODO: Right now ignores count completed and despawns everything no matter what
    public void DespawnCreatures(CreatureType type, int count)
    {
        // Max count if negative number
        if (count < 0)
        {
            count = creatureCounts[(int)type];
        }

        // TODO: Fix this; right now ignoring count
        List<CreatureBase> spawnedCreatures = new List<CreatureBase>();
        foreach (CreatureBase cb in creaturePools[type])
        {
            if (cb.Spawned)
            {
                spawnedCreatures.Add(cb);
            }
        }

        foreach (CreatureBase spawnedCreature in spawnedCreatures)
        {
            //if (spawnedCreature == null)
            //{
            //    break;
            //}
            spawnedCreature.Despawn();
        }
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                SpawnCreatures(CreatureType.Squirrel, 1);
                SpawnCreatures(CreatureType.Deer, 1);
                SpawnCreatures(CreatureType.Raccoon, 1);
                SpawnCreatures(CreatureType.Crow, 1);
                SpawnCreatures(CreatureType.Wolf, 1);
                SpawnCreatures(CreatureType.Bunny, 1);

                //SpawnCreatures(CreatureType.Shapeshifter, 1);
                //SpawnCreatures(CreatureType.Decoy, 1);
                //SpawnCreatures(CreatureType.Reverse, 1);
                break;
            case GamePhase.Dusk:
                SpawnCreatures(CreatureType.Bunny, 1);
                SpawnCreatures(CreatureType.Raccoon, 1);
                SpawnCreatures(CreatureType.Wolf, 1);
                break;
            case GamePhase.Night:
                SpawnCreatures(CreatureType.Crow, 1);
                break;
            case GamePhase.Latenight:
                SpawnCreatures(CreatureType.Shapeshifter, 1);
                SpawnCreatures(CreatureType.Decoy, 1);
                SpawnCreatures(CreatureType.Reverse, 1);
                break;
            case GamePhase.Dawn:
                SpawnCreatures(CreatureType.Squirrel, 1);
                SpawnCreatures(CreatureType.Deer, 1);
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
                //DespawnCreatures(CreatureType.Squirrel, -1);
                //DespawnCreatures(CreatureType.Deer, -1);
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                DespawnCreatures(CreatureType.Crow, -1);
                DespawnCreatures(CreatureType.Wolf, -1);
                DespawnCreatures(CreatureType.Shapeshifter, -1);
                DespawnCreatures(CreatureType.Reverse, -1);
                DespawnCreatures(CreatureType.Decoy, -1);
                break;
            case GamePhase.Dawn:
                DespawnCreatures(CreatureType.Raccoon, -1);
                DespawnCreatures(CreatureType.Bunny, -1);
                break;
            case GamePhase.End:
                break;
        }
    }
}
