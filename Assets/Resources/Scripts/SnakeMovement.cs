using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    public float speed = 0.1f;

    void Start()
    {
        InvokeRepeating(nameof(Move), speed, speed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Move()
    {
        float x = Mathf.Round(transform.position.x) + _direction.x;
        float y = Mathf.Round(transform.position.y) + _direction.y;

        transform.position = new Vector3(x, y, 0.0f);
    }
}