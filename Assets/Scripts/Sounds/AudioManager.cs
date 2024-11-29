// 引用所需的命名空间
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// 继承自SingletonMonobehaviour，确保AudioManager类在游戏场景中是唯一的
public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    // 可序列化的私有变量，用于存储音频预制体
    [SerializeField] private GameObject soundPrefab = null;

    [Header("Audio Sources")]
    // 可序列化的私有变量，用于存储环境音的AudioSource
    [SerializeField] private AudioSource ambientSoundAudioSource = null;

    // 可序列化的私有变量，用于存储游戏音乐的AudioSource
    [SerializeField] private AudioSource gameMusicAudioSource = null;

    [Header("Audio Mixers")]
    // 可序列化的私有变量，用于存储游戏音频混合器
    [SerializeField] private AudioMixer gameAudioMixer = null;

    [Header("Audio Snapshots")]
    // 可序列化的私有变量，用于存储游戏音乐的AudioMixerSnapshot
    [SerializeField] private AudioMixerSnapshot gameMusicSnapshot = null;

    // 可序列化的私有变量，用于存储环境音的AudioMixerSnapshot
    [SerializeField] private AudioMixerSnapshot gameAmbientSnapshot = null;

    [Header("Other")]
    // 可序列化的私有变量，用于存储声音列表和字典
    [SerializeField] private SO_SoundList so_soundList = null;

    [SerializeField] private SO_SceneSoundsList so_sceneSoundsList = null;
    [SerializeField] private float defaultSceneMusicPlayTimeSeconds = 120f;
    [SerializeField] private float sceneMusicStartMinSecs = 20f;
    [SerializeField] private float sceneMusicStartMaxSecs = 40f;
    [SerializeField] private float musicTransitionSecs = 8f;

    // 存储声音项的字典
    private Dictionary<SoundName, SoundItem> soundDictionary;
    // 存储场景声音项的字典
    private Dictionary<SceneName, SceneSoundsItem> sceneSoundsDictionary;

    // 播放场景声音的协程
    private Coroutine playSceneSoundsCoroutine;

    // 重写Awake方法，在游戏对象创建时初始化
    protected override void Awake()
    {
        base.Awake();

        // 初始化声音字典
        soundDictionary = new Dictionary<SoundName, SoundItem>();

        // 使用so_soundList中的声音项加载声音字典
        foreach (SoundItem soundItem in so_soundList.soundDetails)
        {
            soundDictionary.Add(soundItem.soundName, soundItem);
        }

        // 初始化场景声音字典
        sceneSoundsDictionary = new Dictionary<SceneName, SceneSoundsItem>();

        // 加载场景声音字典
        foreach (SceneSoundsItem sceneSoundsItem in so_sceneSoundsList.sceneSoundsDetails)
        {
            sceneSoundsDictionary.Add(sceneSoundsItem.sceneName, sceneSoundsItem);
        }
    }
    // 当此脚本启用时，添加AfterSceneLoadEvent事件的监听
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += PlaySceneSounds;
    }
    // 当此脚本禁用时，移除AfterSceneLoadEvent事件的监听
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= PlaySceneSounds;
    }
    // 播放场景声音的方法
    private void PlaySceneSounds()
    {
        SoundItem musicSoundItem = null;
        SoundItem ambientSoundItem = null;

        float musicPlayTime = defaultSceneMusicPlayTimeSeconds;

        // 尝试获取当前场景的名称
        if (Enum.TryParse<SceneName>(SceneManager.GetActiveScene().name, true, out SceneName currentSceneName))
        {
            // 获取场景的音乐和环境声音项
            if (sceneSoundsDictionary.TryGetValue(currentSceneName, out SceneSoundsItem sceneSoundsItem))
            {
                soundDictionary.TryGetValue(sceneSoundsItem.musicForScene, out musicSoundItem);
                soundDictionary.TryGetValue(sceneSoundsItem.ambientSoundForScene, out ambientSoundItem);
            }
            else
            {
                return;
            }

            // 停止任何已经播放的场景声音
            if (playSceneSoundsCoroutine != null)
            {
                StopCoroutine(playSceneSoundsCoroutine);
            }

            // 播放场景环境声音和音乐
            playSceneSoundsCoroutine = StartCoroutine(PlaySceneSoundsRoutine(musicPlayTime, musicSoundItem, ambientSoundItem));
        }
    }

    // 协程，用于播放场景声音
    private IEnumerator PlaySceneSoundsRoutine(float musicPlaySeconds, SoundItem musicSoundItem, SoundItem ambientSoundItem)
    {
        // 如果音乐声音项和环境声音项都不为空
        if (musicSoundItem != null && ambientSoundItem != null)
        {
            // 先播放环境声音
            PlayAmbientSoundClip(ambientSoundItem, 0f);

            // 等待随机时间后播放音乐
            yield return new WaitForSeconds(UnityEngine.Random.Range(sceneMusicStartMinSecs, sceneMusicStartMaxSecs));

            // 播放音乐
            PlayMusicSoundClip(musicSoundItem, musicTransitionSecs);

            // 等待音乐播放完毕后再播放环境声音
            yield return new WaitForSeconds(musicPlaySeconds);

            // 播放环境声音
            PlayAmbientSoundClip(ambientSoundItem, musicTransitionSecs);
        }
    }

    // 播放音乐声音片段的方法
    private void PlayMusicSoundClip(SoundItem musicSoundItem, float transitionTimeSeconds)
    {
        // 设置音量
        gameAudioMixer.SetFloat("MusicVolume", ConvertSoundVolumeDecimalFractionToDecibels(musicSoundItem.soundVolume));

        // 设置音频剪辑并播放
        gameMusicAudioSource.clip = musicSoundItem.soundClip;
        gameMusicAudioSource.Play();

        // 过渡到音乐快照
        gameMusicSnapshot.TransitionTo(transitionTimeSeconds);
    }

    // 播放环境声音片段的方法
    private void PlayAmbientSoundClip(SoundItem ambientSoundItem, float transitionTimeSeconds)
    {
        // 设置音量
        gameAudioMixer.SetFloat("AmbientVolume", ConvertSoundVolumeDecimalFractionToDecibels(ambientSoundItem.soundVolume));

        // 设置音频剪辑并播放
        ambientSoundAudioSource.clip = ambientSoundItem.soundClip;
        ambientSoundAudioSource.Play();

        // 过渡到环境声音
        gameAmbientSnapshot.TransitionTo(transitionTimeSeconds);
    }

    /// <summary>
    /// 将音量的小数分数转换为-80到+20分贝范围
    /// </summary>
    private float ConvertSoundVolumeDecimalFractionToDecibels(float volumeDecimalFraction)
    {
        // 将音量从小数分数转换为-80到+20分贝范围

        return (volumeDecimalFraction * 100f - 80f);
    }


    // 播放声音的方法
    public void PlaySound(SoundName soundName)
    {
        // 如果声音字典中包含指定的声音项，并且音频预制体不为空
        if (soundDictionary.TryGetValue(soundName, out SoundItem soundItem) && soundPrefab != null)
        {
            // 从对象池中获取音频游戏对象
            GameObject soundGameObject = PoolManager.Instance.ReuseObject(soundPrefab, Vector3.zero, Quaternion.identity);

            // 获取音频组件
            Sound sound = soundGameObject.GetComponent<Sound>();

            // 设置音频并激活游戏对象
            sound.SetSound(soundItem);
            soundGameObject.SetActive(true);
            StartCoroutine(DisableSound(soundGameObject, soundItem.soundClip.length));
        }
    }

    // 协程，用于在声音播放完毕后禁用音频游戏对象
    private IEnumerator DisableSound(GameObject soundGameObject, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        soundGameObject.SetActive(false);
    }
}

/*AudioManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保AudioManager在游戏场景中是唯一的实例。

soundPrefab是一个可序列化的私有变量，用于存储音频预制体。

ambientSoundAudioSource和gameMusicAudioSource是可序列化的私有变量，分别用于存储环境音和游戏音乐的AudioSource。

gameAudioMixer是可序列化的私有变量，用于存储游戏音频混合器。

gameMusicSnapshot和gameAmbientSnapshot是可序列化的私有变量，分别用于存储游戏音乐和环境音的AudioMixerSnapshot。

so_soundList和so_sceneSoundsList是可序列化的私有变量，分别用于存储声音列表和场景声音列表的ScriptableObject。

defaultSceneMusicPlayTimeSeconds、sceneMusicStartMinSecs、sceneMusicStartMaxSecs和musicTransitionSecs是可序列化的私有变量，用于存储场景音乐播放的默认时间、开始时间范围和过渡时间。

soundDictionary和sceneSoundsDictionary是私有字典，分别用于存储声音项和场景声音项。

Awake方法在游戏对象创建时调用，用于初始化声音字典和场景声音字典。

OnEnable和OnDisable方法分别在脚本启用和禁用时调用，用于添加和移除对EventHandler.AfterSceneLoadEvent事件的监听。

PlaySceneSounds方法在场景加载后调用，用于播放场景的声音。

PlaySceneSoundsRoutine协程用于播放场景的声音，包括环境声音和音乐。

PlayMusicSoundClip和PlayAmbientSoundClip方法用于播放音乐声音片段和环境声音片段。

ConvertSoundVolumeDecimalFractionToDecibels方法用于将音量的小数分数转换为分贝值。

PlaySound方法用于播放指定名称的声音。

DisableSound协程用于在声音播放完毕后禁用音频游戏对象。

这个类的主要用途是管理游戏中的音频播放，包括环境声音、音乐和特效声音。通过使用ScriptableObject和音频混合器，可以灵活地控制音频的播放和过渡，实现复杂的音频效果。
*/