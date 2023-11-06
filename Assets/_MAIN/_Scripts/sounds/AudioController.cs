using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace THJ
{
    public class AudioController : SerializedMonoBehaviour
    {
        public Dictionary<string, AudioClip> soundDB;

        public AudioSource bgmSource;
        public AudioSource bgmExtraSource;
        public AudioSource fxSource;

        public static AudioController Instance;

        bool canPlay;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else Instance = this;

            canPlay = false;
        }

        public void PlayFX(string key)
        {
            if (soundDB.ContainsKey(key))
                fxSource.PlayOneShot(soundDB[key]);
        }

        public void PlayFXWithTime(string key, float time)
        {
            if (!canPlay)
            {
                canPlay = true;
                DOVirtual.DelayedCall(time, () => canPlay = false);
                if (soundDB.ContainsKey(key))
                    fxSource.PlayOneShot(soundDB[key]);
            }
        }

        public void PlayBGM(string key)
        {
            if (bgmSource.isPlaying) bgmSource.DOFade(0f, 0.25f).OnComplete(() =>
                {
                    if (soundDB.ContainsKey(key))
                    {
                        bgmSource.clip = soundDB[key];
                        bgmSource.loop = true;
                        bgmSource.Play();
                        bgmSource.DOFade(1f, 0.25f);
                    }
                }
            );
            else if (soundDB.ContainsKey(key))
            {
                bgmSource.clip = soundDB[key];
                bgmSource.loop = true;
                bgmSource.Play();
                bgmSource.DOFade(1f, 0.25f);
            }
        }

        public void StopBGM()
        {
            if (bgmSource.isPlaying) bgmSource.Pause();
            if (bgmExtraSource.isPlaying) bgmExtraSource.Pause();
        }

        public void FadeToStopBGM()
        {
            if (bgmSource.isPlaying) bgmSource.DOFade(0f, 1f);
            if (bgmExtraSource.isPlaying) bgmExtraSource.DOFade(0f, 0.8f);
        }
    }

}
