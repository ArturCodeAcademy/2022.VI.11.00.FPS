using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Mathf;

public class Stamina : MonoBehaviour
{
    public StaminaChangedEvent OnStaminaChanged;

    [SerializeField, Min(0)] private float _recoveryScale = 1;
    [SerializeField, Min(0)] private float _max = 10;

    private float _value;
    private bool _usedOnThisFrame = false;

    private void Awake()
    {
        OnStaminaChanged ??= new StaminaChangedEvent();
        _value = _max;
    }

    private void LateUpdate()
    {
        if (!_usedOnThisFrame && _value < _max)
        {     
            _value += Time.deltaTime * _recoveryScale;
            _value = Min(_max, _value);
            OnStaminaChanged?.Invoke(GetArgs());
        }
        if (_usedOnThisFrame)
            _usedOnThisFrame = false;
    }

    public StaminaChangedEventArgs GetArgs()
        => new StaminaChangedEventArgs() { MaxValue = _max, Value = _value };

    public bool UseStamina(float usage = 1)
    {
        _usedOnThisFrame = true;

        if (_value < usage)
            return false;

        _value -= usage;
        _value = Max(0, _value);
        OnStaminaChanged?.Invoke(GetArgs());
        return true;
    }
}
