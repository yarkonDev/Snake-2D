using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    public float speed = 0.1f;

    [Header("Настройки тела")]
    public GameObject segmentPrefab;
    public Sprite bodySprite;
    public Sprite tailSprite;

    [Header("Настройки проигрыша")]
    public GameObject gameOverPanel;
    public Sprite deadHeadSprite;

    public ScoreManager scoreManager;

    private List<Transform> _segments = new List<Transform>();
    private bool _isDead = false;

    void Start()
    {
        Time.timeScale = 1;
        _segments.Add(this.transform);
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
        yield return new WaitForSeconds(0.2f);
        col.enabled = true;
    }

    void Die()
    {
        _isDead = true;
        CancelInvoke(nameof(Move));

        GetComponent<SpriteRenderer>().sprite = deadHeadSprite;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            scoreManager.AddScore(1);
        }
        else if (other.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public List<Transform> GetSegments() { return _segments; }
}