using UnityEngine;

public class ExceptionManager : MonoBehaviour
{
    public void Awake()
    {
        Application.logMessageReceived += HandleException;
        DontDestroyOnLoad(gameObject);
    }

    public void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            // ...
        }
    }
}