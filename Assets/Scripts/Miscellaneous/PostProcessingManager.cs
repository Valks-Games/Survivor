using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    public static GameObject PostProcessing;

    public void Awake()
    {
        if (PostProcessing == null)
        {
            PostProcessing = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (PostProcessing != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
