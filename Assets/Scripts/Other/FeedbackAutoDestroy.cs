using UnityEngine;

public class FeedbackAutoDestroy : MonoBehaviour
{
    public static FeedbackAutoDestroy Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, GetComponent<AudioSource>().clip.length + 0.5f);//Se détruit 0.5f plus tard pour laisser le temps au son de se jouer
        }
        else
        {
            Instance = this;
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, GetComponent<AudioSource>().clip.length + 0.5f);//Se détruit 0.5f plus tard pour laisser le temps au son de se jouer
        }
    }
}
