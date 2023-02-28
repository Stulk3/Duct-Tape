using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour
{
    //Observer 
    [SerializeField] private PlayableCharacter _target;

    [SerializeField] private TextMeshProUGUI _tapeIndicator;
    [SerializeField] private Image[] _healthIndicators;

    private void OnEnable()
    {
        _target.onHealthChanged += UpdateHealthValue;
        _target.onTapeChanged += UpdateTapeValue;
    }
    private void OnDisable()
    {
        _target.onHealthChanged -= UpdateHealthValue;
        _target.onTapeChanged -= UpdateTapeValue;
    }

    public void UpdateHealthValue(int value)
    {
        if (_target == null) return;
        SyncHealthIndicator(value);
    }
    public void UpdateTapeValue(int value)
    {
        if (_target == null) return;
        SyncTypeIndicator(value);
    }
    public void SyncHealthIndicator(int value)
    {
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
    public void SyncTypeIndicator(int value)
    {
        _tapeIndicator.text = value.ToString();
    }
}
