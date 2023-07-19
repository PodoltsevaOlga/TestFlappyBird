using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubesController : MonoBehaviour
{
    [SerializeField] private GameObject m_CubePrefab;
    
    [SerializeField] public float m_DistanceBetween = 10.0f;
    [SerializeField] private int m_MaxValue = 10;
    [SerializeField] public Rect m_SpawnRect;

    private float m_RightBorderX;

    private EntityPool m_CubePool;
    private List<GameObject> m_ActiveCubes = new();

    private GameObject m_LastSpawnedCube;

    private void Start()
    {
        m_CubePool = new EntityPool(m_CubePrefab);
        var cam = Camera.main;
        if (cam == null)
            return;

        m_RightBorderX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0.0f, 0.0f)).x + 2.0f;
    }

    private void OnEnable()
    {
        GameManager.OnEndGame += Clear;
    }
    
    private void OnDisable()
    {
        GameManager.OnEndGame -= Clear;
    }

    private void Update()
    {
        foreach (var cube in m_ActiveCubes)
        {
            if (!cube.activeSelf)
            {
                m_CubePool.ReleaseEntity(cube);
                if (m_LastSpawnedCube == cube)
                    m_LastSpawnedCube = null;
            }
        }

        m_ActiveCubes.RemoveAll(cube => cube.activeSelf == false);

        if (m_LastSpawnedCube == null)
            m_LastSpawnedCube = SpawnNewCubes();
        
        else if (Mathf.Abs(m_LastSpawnedCube.transform.position.x - m_RightBorderX) > m_DistanceBetween)
        {
            m_LastSpawnedCube = SpawnNewCubes();
        }
    }

    private GameObject SpawnNewCubes()
    {
        int points = GameManager.Instance.Points;
        int count = (points == 0) ? 1 : (int)((points + 3) / 2);
        count = Mathf.Min(count, m_MaxValue);

        for (int i = 0; i < count; ++i)
        {
            var cube = m_CubePool.GetEntity();
            m_ActiveCubes.Add(cube);
            cube.transform.position = new Vector3(m_RightBorderX + Random.Range(m_SpawnRect.xMin, m_SpawnRect.xMax),
                Random.Range(m_SpawnRect.yMin, m_SpawnRect.yMax));
            cube.transform.parent = gameObject.transform;
            cube.SetActive(true);
        }

        m_ActiveCubes.Sort((cube1, cube2) => cube1.transform.position.x < cube2.transform.position.x ? -1 : 1);
        return m_ActiveCubes[^1];
    }

    private void Clear()
    {
        foreach (var cube in m_ActiveCubes)
        {
            cube.SetActive(false);
            m_CubePool.ReleaseEntity(cube);
        }
        
        m_ActiveCubes.Clear();
        m_LastSpawnedCube = null;
    }
}
