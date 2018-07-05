using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    Start, Afternoon, Dusk, Night, Latenight, Latenight2, Latenight3, Dawn, End
}

public class GameManager : ManagerBase
{
    public static GameManager Instance;

    public event Action<GamePhase> PhaseLoaded;
    public event Action<GamePhase> PhaseUnloaded;

    public event Action DeathByEnemy;
    public event Action GameOverEvent;

    public GamePhase CurrPhase { get; private set; }
    public float CurrPhaseTime { get; private set; }
    public float[] PhaseLengths;

    public bool debugMode;
    public float minPhaseLengthInSeconds;
    public float maxPhaseLengthInSeconds;

    public Transform leftHandAnchor;
    public Transform rightHandAnchor;
    public GameObject trackedRemote;
    public GameObject ovrPlatformMenuPrefab;

    bool gameOver;
    bool isRightHanded;

    void Awake()
    {
        Instance = this;

        RunAssertions();
        InitOVRSettings();
        InitPhaseLengths();
    }

    void Start()
    {
        // Fire events for loading Start
        PhaseLoaded(CurrPhase);

        if (debugMode)
        {
            InitDebugMode();
        }
    }

    void Update()
    {
#if UNITY_ANDROID
        // Switch to correct hand
        if (isRightHanded && OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            isRightHanded = false;
            trackedRemote.transform.parent = leftHandAnchor;
        }
        else if (!isRightHanded && OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            isRightHanded = true;
            trackedRemote.transform.parent = rightHandAnchor;
        }
#endif

        if (gameOver)
        {
            return;
        }

        // Advance time
        CurrPhaseTime += Time.deltaTime;
        if (CurrPhaseTime >= PhaseLengths[(int)CurrPhase])
        {
            PhaseTransition();
        }
    }

    void RunAssertions()
    {
        Assert.IsTrue(PhaseLengths.Length == Enum.GetValues(typeof(GamePhase)).Length);
    }

    void InitOVRSettings()
    {
#if UNITY_ANDROID
        // Enable fixed foveated rendering
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;

        // Register back button as Pause
        Instantiate(ovrPlatformMenuPrefab, transform);

        // Unmounted HMD should also Pause
        OVRManager.HMDUnmounted -= OVRManager.PlatformUIConfirmQuit; // Makes sure no re-add
        OVRManager.HMDUnmounted += OVRManager.PlatformUIConfirmQuit;

        // Initialize hand
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            isRightHanded = false;
        }
        else
        {
            isRightHanded = true;
        }
#endif
    }

    void InitDebugMode()
    {
        //PhaseLengths[(int)GamePhase.Dawn] = Mathf.Infinity;
    }

    void InitPhaseLengths()
    {
        if (debugMode)
        {
            for (int i = 0; i < PhaseLengths.Length; ++i)
            {
                PhaseLengths[i] = UnityEngine.Random.Range(minPhaseLengthInSeconds, maxPhaseLengthInSeconds);
            }
        }

        // No limit for Start or End
        PhaseLengths[(int)GamePhase.Start] = Mathf.Infinity;
        PhaseLengths[(int)GamePhase.End] = Mathf.Infinity;
    }

    public void PressedStart()
    {
        if (CurrPhase == GamePhase.Start)
        {
            PhaseTransition();
        }
    }

    public void GameOver(bool win = false)
    {
        Debug.LogError("GAME OVER!!!");
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        if (win)
        {
            RestartGame(8f, 26f, 4f);
        }
        else
        {
            DeathByEnemy();
            if (GameOverEvent != null)
            {
                GameOverEvent();
            }
            RestartGame(10f, 10f, 4f);
        }
    }

    void RestartGame(float eyeCloseTime, float delay, float afterEyeCloseDelay)
    {
        // TODO: Clean this up
        Eye e = Camera.main.gameObject.GetComponent<Eye>();

        e.EyeClose(eyeCloseTime, true, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }, delay, afterEyeCloseDelay);
    }

    void PhaseTransition()
    {
        Debug.Log("Transition to: " + (CurrPhase + 1).ToString());
        CurrPhaseTime = 0;
        PhaseUnloaded(CurrPhase);
        PhaseLoaded(++CurrPhase);
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Start:
                Eye e = Camera.main.gameObject.GetComponent<Eye>();
                e.EyeOpen(0.3f, false, () =>
                {
                    e.EyeOpen(0.8f, true, null, 0.1f);
                });
                break;
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
                GameOver(true);
                break;
        }
    }

    protected override void OnPhaseUnload(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Start:
                break;
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
