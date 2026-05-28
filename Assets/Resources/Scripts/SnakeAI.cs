using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    [Header("Ссылки")]
    public SnakeMovement snake;
    public GameObject aiIcon;
    public GameObject QoLText;

    public bool isUnlocked = false;
    public bool aiActive = false;

    void Start()
    {
        if (!isUnlocked)
        {
            isUnlocked = PlayerPrefs.GetInt("AI", 0) == 1;
        }

        if (aiIcon != null)
        {
            aiIcon.SetActive(isUnlocked);
        }
        if (QoLText) QoLText.SetActive(isUnlocked);

    }

    void Update()
    {
        if (!isUnlocked || snake == null) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleAI();
        }
    }

    public void ToggleAI()
    {
        if (!isUnlocked) return;
        aiActive = !aiActive;
    }

    public void ThinkBeforeMove()
    {
        if (!aiActive || snake == null || snake._isDead) return;

        GameObject apple = GameObject.FindWithTag("Food");
        if (apple == null) return;

        Vector2 applePos = apple.transform.position;
        Vector2 headPos = transform.position;

        float diffX = applePos.x - headPos.x;
        float diffY = applePos.y - headPos.y;

        Vector2 currentDir = snake._direction;
        Vector2 desiredDirection = currentDir;

        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            if (diffX > 0) desiredDirection = Vector2.right;
            else if (diffX < 0) desiredDirection = Vector2.left;
        }
        else
        {
            if (diffY > 0) desiredDirection = Vector2.up;
            else if (diffY < 0) desiredDirection = Vector2.down;
        }

        if (desiredDirection == -currentDir)
        {
            if (currentDir == Vector2.right || currentDir == Vector2.left)
            {
                desiredDirection = diffY >= 0 ? Vector2.up : Vector2.down;
            }
            else
            {
                desiredDirection = diffX >= 0 ? Vector2.right : Vector2.left;
            }
        }
        if (IsMoveDangerous(desiredDirection))
        {
            Vector2[] alternativeDirs = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
            float bestDistance = float.MaxValue;
            Vector2 safestDir = currentDir;
            bool foundSafe = false;

            foreach (Vector2 dir in alternativeDirs)
            {
                if (dir == -currentDir) continue;

                if (!IsMoveDangerous(dir))
                {
                    float distToApple = Vector2.Distance((Vector2)transform.position + dir, applePos);
                    if (distToApple < bestDistance)
                    {
                        bestDistance = distToApple;
                        safestDir = dir;
                        foundSafe = true;
                    }
                }
            }

            desiredDirection = foundSafe ? safestDir : currentDir;
        }
        if (desiredDirection != snake._direction)
        {
            snake._direction = desiredDirection;

            float angle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    bool IsMoveDangerous(Vector2 targetDir)
    {
        Vector2 futurePos = (Vector2)transform.position + targetDir;

        RaycastHit2D hitWall = Physics2D.Raycast((Vector2)transform.position + targetDir * 0.5f, targetDir, 0.4f);
        if (hitWall.collider != null && hitWall.collider.CompareTag("Obstacle"))
        {
            return true;
        }
        if (snake._segments != null)
        {
            for (int i = 2; i < snake._segments.Count; i++)
            {
                if (snake._segments[i] != null)
                {
                    Vector2 segmentPos = snake._segments[i].position;

                    if (Vector2.Distance(futurePos, segmentPos) < 0.2f)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}