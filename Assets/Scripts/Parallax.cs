using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer;

    [SerializeField] 
    private float m_ParallaxSpeed = 1.0f;
    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        var direction = new Vector2(m_ParallaxSpeed * Time.deltaTime, 0.0f);
        m_MeshRenderer.material.mainTextureOffset += direction;
    }
}
