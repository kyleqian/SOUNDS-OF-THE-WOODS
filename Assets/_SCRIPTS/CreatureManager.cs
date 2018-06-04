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
    [SerializeField] Dictionary<CreatureType, int> creatureCounts;
    [SerializeField] Dictionary<CreatureType, GameObject> creaturePrefabs;
    Dictionary<CreatureType, GameObject[]> creaturePools;

    void Awake()
    {
        Array types = Enum.GetValues(typeof(CreatureType));

        // Check that all types are accounted for
        // TODO: Could also check if keys are the same
        Assert.IsTrue(creatureCounts.Count == types.Length);
        Assert.IsTrue(creaturePrefabs.Count == types.Length);

        // Set up pools
        creaturePools = new Dictionary<CreatureType, GameObject[]>();
        foreach (CreatureType type in types)
        {
            creaturePools[type] = new GameObject[creatureCounts[type]];
            for (int i = 0; i < creatureCounts[type]; ++i)
            {
                creaturePools[type][i] = Instantiate(creaturePrefabs[type]);
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
