using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour
{
    public Vector2 _direction = Vector2.right;
    public float speed = 0.1f;

    [Header("Настройки тела")]
    public GameObject segmentPrefab;
    public Sprite bodySprite;
    public Sprite tailSprite;

    [Header("Настройки проигрыша")]
    public GameObject gameOverPanel;
    public Sprite deadHeadSprite;

    [Header("Остальное")]
    public ScoreManager scoreManager;

    private bool _hasUsedReviveInThisRound = false;

    public List<Transform> _segments = new List<Transform>();
    public bool _isDead = false;

    void Start()
    {
        Time.timeScale = 1;
        _hasUsedReviveInThisRound = false;
        _segments.Add(this.transform);
        InvokeRepeating(nameof(Move), speed, speed);
    }

    public void UpdateSpeed()
    {
        CancelInvoke(nameof(Move));
        InvokeRepeating(nameof(Move), speed, speed);
    }

    void Update()
    {
        if (_isDead) return;

        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up; transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down; transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left; transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right; transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Move()
    {
        if (_isDead) return;
        SnakeAI ai = GetComponent<SnakeAI>();
        if (ai != null)
        {
            ai.ThinkBeforeMove();
        }

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
            _segments[i].rotation = _segments[i - 1].rotation;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f
        );

        UpdateSegmentsAppearance();
    }

    void UpdateSegmentsAppearance()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            SpriteRenderer sr = _segments[i].GetComponent<SpriteRenderer>();
            sr.sprite = (i == _segments.Count - 1) ? tailSprite : bodySprite;
        }
    }

    public void Grow()
    {
        GameObject segment = Instantiate(segmentPrefab);
        segment.transform.position = _segments[_segments.Count - 1].position;

        StartCoroutine(EnableCollider(segment.GetComponent<Collider2D>()));

        _segments.Add(segment.transform);
    }
    private System.Collections.IEnumerator EnableCollider(Collider2D col)
    {
        col.enabled = false;
        if (1 > _segments.Count)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
        }
        if (col == null || this == null) yield break;
        col.enabled = true;
    }

    void Die()
    {
        bool hasRevivePerk = PlayerPrefs.GetInt("ReviveAbility", 0) == 1;
        if (hasRevivePerk && !_hasUsedReviveInThisRound)
        {
            ExecuteRevive();
            return;
        }
        _isDead = true;
        CancelInvoke(nameof(Move));
        GetComponent<SpriteRenderer>().sprite = deadHeadSprite;

        gameOverPanel.SetActive(true);
        if(scoreManager != null) scoreManager.UpdateVisuals();
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null || other.gameObject == null) return;
        try
        {
            if (other.GetComponent<BoxCollider2D>() == null) return;
        }
        catch (UnityEngine.MissingReferenceException)
        {
            return;
        }
        if (other.CompareTag("Food"))
        {
            Grow();
            if (scoreManager != null) scoreManager.AddScore(1);
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Player"))
        {
            Die();
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    void ExecuteRevive()
    {
        BoxCollider2D headCollider = GetComponent<BoxCollider2D>();
        if (headCollider != null) headCollider.enabled = false;
        _hasUsedReviveInThisRound = true;
        transform.position = Vector3.zero;
        for (int i = 1; i < _segments.Count; i++)
        {
            if (_segments[i] != null)
            {
                _segments[i].gameObject.tag = "Untagged";

                BoxCollider2D col = _segments[i].GetComponent<BoxCollider2D>();
                if (col != null) col.enabled = false;

                Destroy(_segments[i].gameObject);
            }
        }
        _segments.Clear();
        _segments.Add(this.transform);
        if (headCollider != null) headCollider.enabled = true;
    }

    public List<Transform> GetSegments() { return _segments; }
}