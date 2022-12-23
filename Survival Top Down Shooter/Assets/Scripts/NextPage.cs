using UnityEngine;


public class NextPage : MonoBehaviour
{

    [Header("Canvas Pages")]
    [SerializeField] private CanvasGroup _currentPage;
    [SerializeField] private CanvasGroup _nextPage;

    private bool _changePage = false;
    private float _timer;
    private float _duration;


    // Set alpha values accordingly at start
    private void Start()
    {
        _currentPage.alpha = 0;
        _nextPage.alpha = 0;
    }


    // Run fade functions depending on whether the button has been clicked
    private void Update()
    {
        if (!_changePage)
        {
            FadeIn();
        }

        if (_changePage)
        {
            FadeOut();
        }
    }


    // Increase alpha of current page to 1
    private void FadeIn()
    {
        _timer += Time.deltaTime;

        // When dead, fade in the first page
        if (!_changePage && _currentPage.alpha != 1)
        {
            _currentPage.alpha = Mathf.Lerp(_currentPage.alpha, 1, _timer * 0.5f);
        }

        // Once button has been clicked, inactivates first page, then run this code
        if (!_currentPage.gameObject.activeInHierarchy)
        {
            // activate page, then increase alpha
            _nextPage.gameObject.SetActive(true);
            _nextPage.alpha = Mathf.Lerp(_nextPage.alpha, 1, _timer);

            if (_nextPage.alpha == 1)
            {
                // Flick boolean false
                _changePage = false;

                // Reset Timer
                _timer = 0f;
            }
        }
    }


    // Decrease alpha of current page to 0
    private void FadeOut()
    {
        _timer += Time.deltaTime;

        // Reduce alpha value over time
        _currentPage.alpha = Mathf.Lerp(_currentPage.alpha, 0, _timer * 0.5f);

        // When 0, deactivate gameobject
        if (_currentPage.alpha == 0)
        {
            // Inactivate current page
            _currentPage.gameObject.SetActive(false);

            // Reset Timer
            _timer = 0f;

            // Now fade in the next page
            FadeIn();
        }
    }


    // boolean variable activated by button
    public void ChangePage()
    {
        _changePage = true;
        _timer = 0f;
    }
}
