using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip missClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayJump()   => sfxSource.PlayOneShot(jumpClip);
    public void PlaySuccess() => sfxSource.PlayOneShot(successClip);
    public void PlayMiss()    => sfxSource.PlayOneShot(missClip);
}