using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [NonSerialized]
    public float HorizontalSpeed = 1.5f;
    
    [SerializeField]
    private float m_GravityRadius = 3.0f;

    [SerializeField] 
    private float m_GravitySpeed = 5.0f;
    
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
        var shift = Vector3.left * (HorizontalSpeed * Time.deltaTime);

        var playerPosition = GameManager.Instance.Player.transform.position;
        var position = gameObject.transform.position;
        if ((playerPosition - position).magnitude < m_GravityRadius)
        {
            shift += (playerPosition - position).normalized * (m_GravitySpeed * Time.deltaTime);
        }

        transform.position += shift;
    }

    private void Clear()
    {
        Destroy(gameObject);
    }
}
