using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    public string LeftBorderTag;
    public string TopBottomBordersTag;
    [SerializeField] private float m_HorizontalSpeed = 1.5f;
    [SerializeField] private float m_VerticalSpeed = 1.0f;

    private int m_Direction = 1;

    [SerializeField] private float m_MinDistance;
    [SerializeField] private float m_MaxDistance;

    private float m_CurrentDistance;
    private float m_InitialY;

    private void Start()
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        m_Direction = Random.Range(-1, 1);
        if (m_Direction == 0)
            m_Direction = 1;
        
        m_CurrentDistance = Random.Range(m_MinDistance, m_MaxDistance);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(LeftBorderTag))
        {
            gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag(TopBottomBordersTag))
        {
            ChangeDirection(-m_Direction);
        }
    }

    private void Update()
    {
        if (transform.position.y - m_InitialY > m_CurrentDistance)
        {
            ChangeDirection(-1);
        }

        if (m_InitialY - transform.position.y > m_CurrentDistance)
        {
            ChangeDirection(1);
        }

        var shift = Vector3.left * (m_HorizontalSpeed * Time.deltaTime) +
                    Vector3.up * (m_VerticalSpeed * Time.deltaTime * m_Direction);
        transform.position += shift;
    }

    private void ChangeDirection(int direction)
    {
        m_Direction = direction;
        m_InitialY = transform.position.y;
        m_CurrentDistance = Random.Range(m_MinDistance, m_MaxDistance);
    }
}
