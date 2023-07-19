using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] 
    private float m_DistanceBetweenPoints;

    [SerializeField] 
    private float m_HorizontalSpeed;

    [SerializeField] 
    private float m_MaxWidth;

    [SerializeField] 
    private float m_MinWidth;

    private LineRenderer m_LineRenderer;

    private List<Vector3> m_LinePoints = new();
    private List<Vector3> m_PrevPoints = new();

    private float m_MinX;

    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_MinX = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 0.1f;
        Initialize();
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += Initialize;
        GameManager.OnEndGame += Clear;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= Initialize;
        GameManager.OnEndGame -= Clear;
    }

    void Update()
    {
        if (m_LinePoints.Count == 0)
        {
            return;
        }
        
        m_PrevPoints.Clear();
        foreach (var point in m_LinePoints)
        {
            m_PrevPoints.Add(point + Vector3.left * (m_HorizontalSpeed * Time.deltaTime));
        }

        (m_PrevPoints, m_LinePoints) = (m_LinePoints, m_PrevPoints);
        
        var playerPosition = GameManager.Instance.Player.transform.position;
        if (Mathf.Abs(m_LinePoints[^1].x - playerPosition.x) > m_DistanceBetweenPoints)
        {
            m_LinePoints.Add(new Vector3(playerPosition.x, playerPosition.y, transform.position.z));
        }

        if (m_LinePoints[0].x < m_MinX)
        {
            m_LinePoints.RemoveAt(0);
        }

        m_LineRenderer.positionCount = m_LinePoints.Count;
        m_LineRenderer.SetPositions(m_LinePoints.ToArray());
    }

    private void Clear()
    {
        m_LinePoints.Clear();
        m_LineRenderer.enabled = false;
    }

    private void Initialize()
    {
        var playerPosition = GameManager.Instance.Player.transform.position;
        m_LinePoints.Add(new Vector3(playerPosition.x, playerPosition.y, transform.position.z));
        m_LinePoints.Add(m_LinePoints[^1]);
        m_LineRenderer.startWidth = m_MinWidth;
        m_LineRenderer.endWidth = m_MaxWidth;
        m_LineRenderer.enabled = true;
        m_LineRenderer.SetPositions(m_LinePoints.ToArray());
        
    }
}
