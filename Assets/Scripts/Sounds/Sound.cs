// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;

// 要求附加此脚本的游戏对象必须有AudioSource组件
[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    // 私有变量，用于存储AudioSource组件
    private AudioSource audioSource;

    // 在对象被创建时调用，用于初始化AudioSource组件
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 公开方法，用于设置声音项的属性
    public void SetSound(SoundItem soundItem)
    {
        // 设置音频的音高，随机变化在指定的最小值和最大值之间
        audioSource.pitch = Random.Range(soundItem.soundPitchRandomVariationMin, soundItem.soundPitchRandomVariationMax);
        // 设置音频的音量
        audioSource.volume = soundItem.soundVolume;
        // 设置音频剪辑
        audioSource.clip = soundItem.soundClip;
    }

    // 当此脚本启用时，如果AudioSource有音频剪辑，则播放音频
    private void OnEnable()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    // 当此脚本禁用时，停止音频播放
    private void OnDisable()
    {
        audioSource.Stop();
    }
}

/*Sound类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

[RequireComponent(typeof(AudioSource))]属性要求附加此脚本的游戏对象必须有AudioSource组件，这是因为该脚本依赖于AudioSource来播放音频。

audioSource是一个私有变量，用于存储游戏对象上的AudioSource组件。

Awake方法在游戏对象被创建时调用，用于获取并存储游戏对象上的AudioSource组件。

SetSound方法是一个公开方法，用于设置音频的属性，包括音高、音量和音频剪辑。音高随机变化在SoundItem提供的最小值和最大值之间，音量和音频剪辑直接设置。

OnEnable方法在脚本被启用时调用，如果AudioSource有音频剪辑，则播放音频。

OnDisable方法在脚本被禁用时调用，停止音频播放。

这个类的主要用途是作为一个音频播放控制器，允许开发者通过设置SoundItem对象来控制音频的播放。通过这种方式，可以在游戏的不同部分中重用音频播放逻辑，同时保持代码的简洁性和可维护性。*/