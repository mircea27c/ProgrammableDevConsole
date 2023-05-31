using UnityEngine;

public class ConsoleSpawner : MonoBehaviour
{
    [SerializeField] GameObject console_prefab;

    private static DeveloperConsole console;

    private void Awake()
    {
        if (console == null) {
            console = Instantiate(console_prefab).GetComponent<DeveloperConsole>();
            DontDestroyOnLoad(console.gameObject);
        }
    }
}
