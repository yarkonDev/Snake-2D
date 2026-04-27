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
        Bounds bounds = this.gridArea.bounds;
        float x, y;
        bool isOccupied;

        do
        {
            isOccupied = false;
            x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

            foreach (Transform segment in snake._segments)
            {
                if (segment.position.x == x && segment.position.y == y)
                {
                    isOccupied = true;
                    break;
                }
            }
        } while (isOccupied);

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