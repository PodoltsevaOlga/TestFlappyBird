using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpsController : MonoBehaviour
{
    [SerializeField] 
    private GameObject PickUpPrefab;
    
    [SerializeField] 
    private float m_HorizontalSpeed = 1.5f;
    
    //needs for distance
    [SerializeField] 
    private CubesController m_CubesController;

    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;

    private float m_DistanceBetween;
    
    private float m_RightBorderX;
    private float m_LastSpawnedX;

    void Start()
    {
        m_DistanceBetween = (m_CubesController.m_DistanceBetween + m_CubesController.m_SpawnRect.width / 2.0f) * 1.5f;
        
        var cam = Camera.main;
        if (cam == null)
            return;
        m_RightBorderX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0.0f, 0.0f)).x + 2.0f;
        m_LastSpawnedX = m_RightBorderX + m_CubesController.m_SpawnRect.width / 2.0f;
    }

    void Update()
    {
        if (Mathf.Abs(m_RightBorderX - m_LastSpawnedX) > m_DistanceBetween)
            SpawnPickUp();
        
        var shift = Vector3.left * (m_HorizontalSpeed * Time.deltaTime);
        m_LastSpawnedX += shift.x;
    }

    private void SpawnPickUp()
    {
        float y = Random.Range(m_MinY, m_MaxY);
        var obj = GameObject.Instantiate(PickUpPrefab, new Vector3(m_RightBorderX, y, 0.0f), Quaternion.identity);
        obj.transform.parent = transform;
        
        var pickUp = obj.GetComponent<PickUp>();
        if (pickUp != null)
            pickUp.HorizontalSpeed = m_HorizontalSpeed;
        
        m_LastSpawnedX = pickUp.transform.position.x;
    }
}