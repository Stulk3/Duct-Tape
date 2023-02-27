using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    //Observer 
    [SerializeField] private PlayableCharacter _target;
    [SerializeField] private TextMeshProUGUI _render;

    private void OnEnable()
    {
        _target.onHealthChanged += UpdateValue;
    }
    private void OnDisable()
    {
        _target.onHealthChanged -= UpdateValue;
    }

    public void UpdateValue(int value)
    {
        if (_target == null) return;
        Debug.Log(value);
        _render.text = value.ToString();
    }
}
