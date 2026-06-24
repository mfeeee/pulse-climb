using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Referências")]
    [SerializeField] private GameObject      platformPrefab;
    [SerializeField] private Transform       player;
    [SerializeField] private PlatformBehavior startPlatform;

    public PlatformBehavior CurrentPlatform  { get; private set; }
    public PlatformBehavior NextPlatform     { get; private set; }
    public PlatformBehavior PreviousPlatform { get; private set; }

    private readonly Queue<GameObject>       _pool   = new Queue<GameObject>();
    private readonly List<PlatformBehavior>  _active = new List<PlatformBehavior>();
    private int   _spawnedCount;
    private float _nextSpawnY;
    private int   _totalPlatforms; // calculado do LevelData em Start

    private void Awake()
    {
        Instance = this;
        if (LevelSelector.Selected != null)
            levelData = LevelSelector.Selected;

        for (int i = 0; i < levelData.poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private void Start()
    {
        _totalPlatforms = levelData.TotalPlatforms;

        if (startPlatform != null)
        {
            startPlatform.Init(0, _totalPlatforms);
            _active.Add(startPlatform);
            CurrentPlatform = startPlatform;
            _spawnedCount = 1;
        }
        else
        {
            Debug.LogError("[PlatformSpawner] startPlatform não atribuída no Inspector!");
        }

        _nextSpawnY = player.position.y - 0.5f + levelData.verticalSpacing;

        for (int i = 0; i < 8 && _spawnedCount < _totalPlatforms; i++)
            SpawnNext();

        if (_active.Count > 1) NextPlatform = _active[1];
        PreviousPlatform = null;
    }

    private void Update()
    {
        if (_active.Count > 0)
        {
            float highestY = _active[_active.Count - 1].transform.position.y;
            if (player.position.y + 15f > highestY && _spawnedCount < _totalPlatforms)
                SpawnNext();
        }

        if (_active.Count > 0 && _active[0].transform.position.y < player.position.y - 10f)
            Recycle(_active[0]);
    }

    private void SpawnNext()
    {
        if (_pool.Count == 0) return;

        GameObject obj = _pool.Dequeue();
        obj.transform.position = new Vector3(0f, _nextSpawnY, 0f);
        obj.SetActive(true);

        PlatformBehavior pb = obj.GetComponent<PlatformBehavior>();
        pb.Init(_spawnedCount, _totalPlatforms);
        _active.Add(pb);

        _nextSpawnY += levelData.verticalSpacing;
        _spawnedCount++;
    }

    private void Recycle(PlatformBehavior pb)
    {
        _active.Remove(pb);
        pb.gameObject.SetActive(false);
        _pool.Enqueue(pb.gameObject);
    }

    public void OnPlayerLanded(PlatformBehavior landed)
    {
        CurrentPlatform = landed;
        int idx = _active.IndexOf(landed);

        NextPlatform     = (idx >= 0 && idx + 1 < _active.Count) ? _active[idx + 1] : null;
        PreviousPlatform = (idx > 0)                              ? _active[idx - 1] : null;
    }

    public bool IsFinalPlatform(PlatformBehavior pb) =>
        pb != null && pb.PlatformIndex >= _totalPlatforms - 1;

    public PlatformBehavior GetPlatformAhead(int steps)
    {
        if (CurrentPlatform == null) return null;
        int idx    = _active.IndexOf(CurrentPlatform);
        int target = idx + steps;
        return (target < _active.Count) ? _active[target] : null;
    }

    public int GetCurrentActiveIndex()
    {
        if (CurrentPlatform == null) return -1;
        return _active.IndexOf(CurrentPlatform);
    }
}