using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public SnakeMovement snake;
    public ScoreManager scoreManager;

    [Header("Настройки спрайтов")]
    public Sprite redAppleSprite;
    public Sprite goldAppleSprite;

    [Header("Звук Золотого Яблока")]
    public AudioSource audioSource;
    public AudioClip goldenDingSound;

    private SpriteRenderer _spriteRenderer;
    private bool _isGolden = false;

    private int _applesEatenInRound = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        _applesEatenInRound++;

        bool isDoubleAppleBought = PlayerPrefs.GetInt("DoubleApple", 0) == 1;
        int chanceRange = isDoubleAppleBought ? 6 : 11;

        if (_applesEatenInRound == 1)
        {
            _isGolden = false;
        }
        else if (_applesEatenInRound == 13)
        {
            _isGolden = true;
            _applesEatenInRound = 6;
        }
        else
        {
            _isGolden = Random.Range(1, chanceRange) == 1;
        }

        _spriteRenderer.sprite = _isGolden ? goldAppleSprite : redAppleSprite;

        Bounds bounds = gridArea.bounds;
        float x, y;
        bool isOccupied;

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
        } while (isOccupied);

        transform.position = new Vector3(x, y, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isGolden)
            {
                scoreManager.AddGoldApple();
                audioSource.PlayOneShot(goldenDingSound);
            }
            RandomizePosition();
        }
    }
}