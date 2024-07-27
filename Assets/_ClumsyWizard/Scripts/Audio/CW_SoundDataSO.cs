using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Background,
    SFX,
}

namespace ClumsyWizard.Audio
{
    [CreateAssetMenu(menuName = "Sound")]
    public class CW_SoundDataSO : ScriptableObject
    {
        [SerializeField] private AudioClip[] clip;
        [field: SerializeField] public SoundType Type { get; private set; }

        public AudioClip GetClip()
        {
            return clip[Random.Range(0, clip.Length)];
        }
    }
}