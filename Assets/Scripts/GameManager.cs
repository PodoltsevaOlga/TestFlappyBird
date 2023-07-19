using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    public static GameManager Instance { 
        get => m_Instance;
        private set => m_Instance = value;
    }
    
    public GameObject Player;

    public int Points { get; private set; }

    [SerializeField] 
    private Text m_StartGameText;
    
    [SerializeField] 
    private Text m_PointsText;

    [SerializeField] private Transform Background; 

    private bool m_IsInGame;

    public static Action OnStartGame;
    public static Action OnEndGame;

    void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance == this)
        {
            Destroy(gameObject);
        }
        
        ChangeScreenSize();
        ChangeFontSize();
    }

    private void Start()
    {
        Pause();
    }

    public void StartGame()
    {
        Resume();
        Points = 0;
        m_PointsText.text = Points.ToString();
        OnStartGame();
    }

    public void GameOver()
    {
        Pause();
        Points = 0;
        OnEndGame();
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
        m_StartGameText.enabled = true;
        m_IsInGame = false;
        m_PointsText.enabled = false;
    }

    private void Resume()
    {
        Time.timeScale = 1.0f;
        m_StartGameText.enabled = false;
        m_IsInGame = true;
        m_PointsText.enabled = true;
    }

    public void AddPoint()
    {
        Points++;
        m_PointsText.text = Points.ToString();
    }

    public void Update()
    {
        if (!m_IsInGame && Utility.IsInput())
        {
            StartGame();
        }
    }

    private void ChangeScreenSize()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        float worldHeight = Camera.main.orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        var localScale = Background.localScale;
        Background.localScale = new Vector3(worldWidth, localScale.y, localScale.z);
    }

    private void ChangeFontSize()
    {
        var rect = m_StartGameText.GetComponent<RectTransform>();
        int fontSize = Screen.width / 40;
        m_StartGameText.fontSize = fontSize;
        rect.sizeDelta = new Vector2(fontSize * 15, fontSize * 2);
    }
}
