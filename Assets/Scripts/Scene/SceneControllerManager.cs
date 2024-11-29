// 引用所需的命名空间
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 继承自SingletonMonobehaviour，确保SceneControllerManager类在游戏场景中是唯一的
public class SceneControllerManager : SingletonMonobehaviour<SceneControllerManager>
{
    // 私有变量，用于标记是否正在执行淡入淡出效果
    private bool isFading;
    // 可序列化的私有变量，用于设置淡入淡出效果的持续时间
    [SerializeField] private float fadeDuration = 1f;
    // 可序列化的私有变量，用于引用控制淡入淡出效果的CanvasGroup组件
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    // 可序列化的私有变量，用于引用用于淡入淡出效果的Image组件
    [SerializeField] private Image faderImage = null;
    // 公开变量，用于设置开始场景的名称
    public SceneName startingSceneName;

    // 淡入淡出效果的协程
    private IEnumerator Fade(float finalAlpha)
    {
        // 设置淡入淡出标记为true，防止FadeAndSwitchScenes协程被重复调用
        isFading = true;

        // 确保CanvasGroup阻止射线投射，以便不接受更多的输入
        faderCanvasGroup.blocksRaycasts = true;

        // 计算CanvasGroup的淡入淡出速度
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // 当CanvasGroup的透明度未达到最终值时循环
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // 将透明度向目标值移动
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            // 等待一帧后继续
            yield return null;
        }

        // 设置淡入淡出标记为false，淡入淡出效果结束
        isFading = false;

        // 停止CanvasGroup阻止射线投射，不再忽略输入
        faderCanvasGroup.blocksRaycasts = false;
    }

    // 场景加载和淡入淡出的协程
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        // 调用场景卸载淡出前的事件
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();

        // 开始淡出并等待结束
        yield return StartCoroutine(Fade(1f));

        // 存储当前场景数据
        SaveLoadManager.Instance.StoreCurrentSceneData();

        // 设置玩家位置
        Player.Instance.gameObject.transform.position = spawnPosition;

        // 调用场景卸载前的事件
        EventHandler.CallBeforeSceneUnloadEvent();

        // 卸载当前激活的场景
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // 开始加载指定的场景并等待结束
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // 调用场景加载后的事件
        EventHandler.CallAfterSceneLoadEvent();

        // 恢复新场景数据
        SaveLoadManager.Instance.RestoreCurrentSceneData();

        // 开始淡入并等待结束
        yield return StartCoroutine(Fade(0f));

        // 调用场景加载淡入后的事件
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }

    // 加载场景并设置为激活场景的协程
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // 允许指定的场景在多个帧中加载，并添加到已加载的场景中
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // 找到最近加载的场景
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // 将新加载的场景设置为激活场景
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    // Start协程
    private IEnumerator Start()
    {
        // 设置初始透明度为黑色屏幕
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        // 开始加载第一个场景并等待结束
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        // 如果有订阅者，调用场景加载后的事件
        EventHandler.CallAfterSceneLoadEvent();

        // 恢复当前场景数据
        SaveLoadManager.Instance.RestoreCurrentSceneData();

        // 场景加载完成后开始淡入
        StartCoroutine(Fade(0f));
    }

    // 外部调用的主要接口，用于在玩家想要切换场景时调用
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        // 如果没有正在进行淡入淡出效果，则开始淡入淡出和场景切换
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }
}

/*SceneControllerManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保SceneControllerManager在游戏场景中是唯一的实例。

isFading是一个私有布尔变量，用于标记是否正在执行淡入淡出效果。

fadeDuration是一个可序列化的私有浮点变量，用于设置淡入淡出效果的持续时间。

faderCanvasGroup和faderImage是可序列化的私有变量，分别用于引用控制淡入淡出效果的CanvasGroup组件和Image组件。

startingSceneName是一个公开变量，用于设置开始场景的名称。

Fade协程用于执行淡入淡出效果，它将CanvasGroup的透明度从当前值变化到目标值。

FadeAndSwitchScenes协程用于执行场景切换的整个过程，包括淡出、存储当前场景数据、设置玩家位置、卸载当前场景、加载新场景、恢复新场景数据和淡入。

LoadSceneAndSetActive协程用于加载指定的场景并将其设置为激活场景。

Start协程在游戏开始时执行，它设置初始透明度为黑色屏幕，加载开始场景，并在场景加载完成后开始淡入。

FadeAndLoadScene方法是一个公开方法，用于在玩家想要切换场景时调用，如果当前没有正在进行淡入淡出效果，则开始淡入淡出和场景切换。

这个类的主要用途是提供一个集中的接口来管理游戏场景的加载和卸载，以及场景之间的过渡效果。通过使用协程，可以确保场景切换过程中的各个步骤按顺序执行，同时提供平滑的淡入淡出效果。*/