using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform player;

    [Header("Layout")]
    [SerializeField, Range(2f, 5f)] private float verticalSpacing = 3f;
    [SerializeField] private int totalPlatforms = 20;
    [SerializeField] private int poolSize = 12;

    public PlatformBehavior CurrentPlatform { get; private set; }
    public PlatformBehavior NextPlatform { get; private set; }

    public static PlatformSpawner Instance { get; private set; }

    private readonly Queue<GameObject> _pool = new Queue<GameObject>();
    private readonly List<PlatformBehavior> _active = new List<PlatformBehavior>();
    private int _spawnedCount = 0;
    private float _nextSpawnY;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private void Start()
    {
        _nextSpawnY = player.position.y - verticalSpacing;

        for (int i = 0; i < 8 && _spawnedCount < totalPlatforms; i++)
            SpawnNext();

        if (_active.Count > 0) CurrentPlatform = _active[0];
        if (_active.Count > 1) NextPlatform = _active[1];
    }

    private void Update()
    {
        if (_active.Count > 0)
        {
            float highestY = _active[_active.Count - 1].transform.position.y;
            if (player.position.y + 15f > highestY && _spawnedCount < totalPlatforms)
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
        pb.Init(_spawnedCount, totalPlatforms);
        _active.Add(pb);

        _nextSpawnY += verticalSpacing;
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
        NextPlatform = (idx >= 0 && idx + 1 < _active.Count) ? _active[idx + 1] : null;
    }

    public bool IsFinalPlatform(PlatformBehavior pb) =>
        pb != null && pb.PlatformIndex >= totalPlatforms - 1;
}