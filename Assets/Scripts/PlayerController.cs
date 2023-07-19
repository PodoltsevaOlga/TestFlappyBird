using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float m_Gravity = -9.8f;

    [SerializeField]
    private float m_StrengthUp = 1f;

    [SerializeField] private LayerMask m_ObstacleLayerMask;
    [SerializeField] private LayerMask m_PickUpLayerMask;

    private Vector3 m_Direction;

    private float m_MinY;
    private float m_MaxY;

    private float m_InitialY;

    private void Start()
    {
        var cam = Camera.main;
        if (cam == null)
            return;
        
        m_MinY = cam.ScreenToWorldPoint(Vector3.zero).y - 0.1f;
        m_MaxY = cam.ScreenToWorldPoint(new Vector3(0.0f, cam.pixelHeight, 0.0f)).y + 0.1f;

        m_InitialY = transform.position.y;
    }

    private void OnEnable()
    {
        GameManager.OnEndGame += Initialize;
    }
    
    private void OnDisable()
    {
        GameManager.OnEndGame -= Initialize;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (Utility.IsInput())
        {
            m_Direction += Vector3.up * (m_StrengthUp * deltaTime);
        }
        
        m_Direction.y += m_Gravity * deltaTime;

        var position = transform.position;
        position += m_Direction * deltaTime;
        float clampY = Mathf.Clamp(position.y, m_MinY, m_MaxY);
        m_Direction.y -= position.y - clampY;

        transform.position = new Vector3(position.x, clampY, position.z);
        
        if (Mathf.Abs(clampY - m_MinY) < Single.Epsilon || Mathf.Abs(clampY - m_MaxY) < Single.Epsilon)
            GameManager.Instance.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherLayer = other.GameObject().layer;
        if (m_ObstacleLayerMask == (m_ObstacleLayerMask | 1 << otherLayer))
        {
            GameManager.Instance.GameOver();
        }
        
        if (m_PickUpLayerMask == (m_PickUpLayerMask | 1 << otherLayer))
        {
            GameManager.Instance.AddPoint();
            Destroy(other.gameObject);
        }
    }

    private void Initialize()
    {
        transform.position = new Vector3(transform.position.x, m_InitialY, transform.position.z);
        m_Direction = Vector3.zero;
    }
}
