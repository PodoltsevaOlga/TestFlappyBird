using System.Collections.Generic;using UnityEngine;

public class EntityPool
{
    private readonly List<GameObject> m_Pool = new();
    private readonly GameObject m_Prefab;

    private int m_AllCount;

    public EntityPool(GameObject prefab)
    {
        m_Prefab = prefab;
    }

    public GameObject GetEntity()
    {
        GameObject gameObject;
        if (m_Pool.Count == 0)
        {
            gameObject = GameObject.Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
            m_AllCount++;
            gameObject.name += m_AllCount.ToString();
        }
        else
        {
            gameObject = m_Pool[^1];
            m_Pool.RemoveAt(m_Pool.Count - 1);
        }

        gameObject.SetActive(false);
        return gameObject;
    }

    public void ReleaseEntity(GameObject entity)
    {
        m_Pool.Add(entity);
    }
}
