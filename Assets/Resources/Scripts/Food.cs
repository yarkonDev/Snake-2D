using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public SnakeMovement snake;

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        if (snake == null || gridArea == null) return;

        Bounds bounds = this.gridArea.bounds;
        float x, y;
        bool isOccupied;

        int attempts = 0;
        do
        {
            isOccupied = false;
            x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

            foreach (Transform segment in snake.GetSegments())
            {
                if (segment.position.x == x && segment.position.y == y)
                {
                    isOccupied = true;
                    break;
                }
            }
            attempts++;
        } while (isOccupied && attempts < 100);

        this.transform.position = new Vector3(x, y, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}