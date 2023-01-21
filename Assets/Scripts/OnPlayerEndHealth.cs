using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class OnPlayerEndHealth : MonoBehaviour
{
    private Health _health;

    private const int MAIN_MENU_BUILD_INDEX = 0;

    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnHealthEnd.AddListener(OnHealthEnd);
    }

    private void OnHealthEnd()
    {
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }

    private void OnDestroy()
    {
        _health.OnHealthEnd.RemoveListener(OnHealthEnd);
    }
}
