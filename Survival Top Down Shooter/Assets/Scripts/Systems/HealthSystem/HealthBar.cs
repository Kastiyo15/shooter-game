using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Systems.HealthSystem;


public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Health _health;

    [SerializeField]
    private RectTransform _barRect;
    [SerializeField]
    private RectTransform _chipBarRect;

    [SerializeField]
    private RectMask2D _mask;
    [SerializeField]
    private RectMask2D _chipMask;

    [SerializeField]
    private TMP_Text _hpIndicator;

    private float _maxRightMask;        // maximum width of mask
    private float _intitialRightMask;   // Initial right padding

    private IEnumerator coroutine;


    // Start is called before the first frame update
    private void Start()
    {
        //x = left, w = top, y = bottom, z = right
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        // Set the initial text of the hpindicator
        _hpIndicator.SetText($"{_health.Hp}/{_health.MaxHp}");
        // Set value of initial right mask
        _intitialRightMask = _mask.padding.z;
    }


    public void SetValue(HealthChangeArgs args)
    {
        SetValue(args.NewValue);
    }


    public void SetValue(int newValue)
    {
        // Calculate bar width using, health and max width of bar
        var targetWidth = newValue * _maxRightMask / _health.MaxHp;
        // This is the right side of the bar (empty section after damage)
        var newRightMask = _maxRightMask + _intitialRightMask - targetWidth;
        // Access the padding of the mask
        var padding = _mask.padding;
        // Adjust the right side padding of the mask
        padding.z = newRightMask;
        // Reassign the padding to the mask
        _mask.padding = padding;
        _hpIndicator.SetText($"{newValue}/{_health.MaxHp}");

        // Chip away bar
        coroutine = ChipAway(_mask.padding, newRightMask);
        StartCoroutine(coroutine);
    }


    private IEnumerator ChipAway(Vector4 redBarPadding, float newRightMask)
    {
        yield return new WaitForSeconds(0.15f);
        _chipMask.padding = redBarPadding;
    }
}
