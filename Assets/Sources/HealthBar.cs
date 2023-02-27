using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    //Observer 
    [SerializeField] private PlayableCharacter _target;
    [SerializeField] private Image[] _healthIndicators;

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
        ActivateIndicator(value);
    }
    public void ActivateIndicator(int value)
    {
        Debug.Log(value);
        if(value == 0)
        {
            foreach(Image indicator in _healthIndicators)
            {
                indicator.gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < _healthIndicators.Length; i++)
        {
            if(i < value)
            {
                _healthIndicators[i].gameObject.SetActive(true);
            }
            else
            {
                _healthIndicators[i].gameObject.SetActive(false);
            }
            
        }
        
    }
}
