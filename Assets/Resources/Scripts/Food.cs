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
        if (gridArea == null) return;

        Bounds bounds = gridArea.bounds;

        for (int i = 0; i < 20; i++)
        {
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            Vector3 newPos = new Vector3(x, y, 0f);

            if (!Physics2D.OverlapCircle(newPos, 0.2f, 1 << 0))
            {
                this.transform.position = newPos;
                return;
            }
        }

        this.transform.position = new Vector3(
            Mathf.Round(Random.Range(bounds.min.x, bounds.max.x)),
            Mathf.Round(Random.Range(bounds.min.y, bounds.max.y)),
            0f
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}