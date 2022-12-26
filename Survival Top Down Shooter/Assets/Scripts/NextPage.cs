using UnityEngine;
using System;
using UnityEngine.UI;


public class NextPage : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private CanvasGroup _currentPage;
    [SerializeField] private CanvasGroup _nextPage;
    [SerializeField] private Animator _animator;


    private bool _changePage;


    // Set alpha values accordingly at start
    private void Start()
    {
        // Set alpha values to zero
        _currentPage.alpha = 0;
        _nextPage.alpha = 0;

        // Set boolean false
        _changePage = false;
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
        // Once button has been clicked, inactivates first page, then run this code
        if (!_currentPage.gameObject.activeInHierarchy)
        {
            // activate page, then increase alpha
            _nextPage.gameObject.SetActive(true);

            if (_nextPage.alpha == 1)
            {
                // Flick boolean false
                _changePage = false;
            }
        }
    }


    // Decrease alpha of current page to 0
    private void FadeOut()
    {
        // When 0, deactivate gameobject
        if (_currentPage.alpha == 0)
        {
            // Inactivate current page
            _currentPage.gameObject.SetActive(false);

            // Now fade in the next page
            FadeIn();
        }
    }


    // boolean variable activated by button
    public void ChangePage()
    {
        _animator.SetBool("IsContinue", true);
        _changePage = true;
    }
}
