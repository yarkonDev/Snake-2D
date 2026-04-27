using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    public float speed = 0.1f;

    [Header("Настройки тела")]
    public GameObject segmentPrefab;
    public Sprite bodySprite;
    public Sprite tailSprite; 

    public List<Transform> _segments = new List<Transform>();

    void Start()
    {
        _segments.Add(this.transform);
        InvokeRepeating(nameof(Move), speed, speed);
    }

    void Update()
    {
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

            if (i == _segments.Count - 1)
            {
                sr.sprite = tailSprite;
            }
            else
            {
                sr.sprite = bodySprite;
            }
        }
    }

    public void Grow()
    {
        GameObject segment = Instantiate(segmentPrefab);
        segment.transform.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
    }
}