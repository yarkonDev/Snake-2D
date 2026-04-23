using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        for (int i = 0; i < 20; i++)
        {
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            Vector3 newPos = new Vector3(x, y, 0f);

            if (!IsPositionOccupied(newPos))
            {
                transform.position = newPos;
                return;
            }
        }

        transform.position = new Vector3(
            Mathf.Round(Random.Range(bounds.min.x, bounds.max.x)),
            Mathf.Round(Random.Range(bounds.min.y, bounds.max.y)),
            0f
        );
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.3f);

        if (hit != null && hit.gameObject != this.gameObject && hit.name != "GridArea")
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}