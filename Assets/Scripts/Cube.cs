using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    public string Tag;
    [SerializeField] private float m_HorizontalSpeed = 1.5f;
    [SerializeField] private float m_VerticalSpeed = 1.0f;
    
    private float m_MinY;
    private float m_MaxY;
    private float m_Direction = 1;

    private BoxCollider2D m_Collider;

    [SerializeField] private float m_MinDistance;
    [SerializeField] private float m_MaxDistance;

    private float m_CurrentDistance;
    private float m_InitialY;

    private void Start()
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        m_Collider = GetComponent<BoxCollider2D>();

        m_MinY = cam.ScreenToWorldPoint(Vector3.zero).y + (m_Collider.size.y / 2.0f);
        m_MaxY = cam.ScreenToWorldPoint(new Vector3(0.0f, cam.pixelHeight, 0.0f)).y - (m_Collider.size.y / 2.0f);
        
        m_Direction = Random.Range(-1, 1);
        if (m_Direction == 0)
            m_Direction = 1;
        
        float randDistance = Random.Range(m_MinDistance, m_MaxDistance);
        m_CurrentDistance = Mathf.Min(randDistance, m_InitialY - m_MinY);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tag))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (transform.position.y - m_InitialY > m_CurrentDistance)
        {
            m_Direction = -1;
            m_InitialY = transform.position.y;
            float randDistance = Random.Range(m_MinDistance, m_MaxDistance);
            m_CurrentDistance = Mathf.Min(randDistance, m_InitialY - m_MinY);
        }

        if (m_InitialY - transform.position.y > m_CurrentDistance)
        {
            m_Direction = 1;
            m_InitialY = transform.position.y;
            float randDistance = Random.Range(m_MinDistance, m_MaxDistance);
            m_CurrentDistance = Mathf.Min(randDistance, m_MaxY - m_InitialY);
        }

        var shift = Vector3.left * (m_HorizontalSpeed * Time.deltaTime) +
                    Vector3.up * (m_VerticalSpeed * Time.deltaTime * m_Direction);
        transform.position += shift;
    }
}
