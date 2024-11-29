using System.Collections; // 引用System.Collections命名空间，提供非泛型集合接口
using UnityEngine; // 引用Unity引擎的命名空间

[RequireComponent(typeof(SpriteRenderer))] // 属性用于要求附加此脚本的游戏对象必须有SpriteRenderer组件
public class ObscuringItemFader : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private SpriteRenderer spriteRenderer; // 私有字段，存储SpriteRenderer组件

    private void Awake() // 在对象被创建时调用
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); // 获取游戏对象的SpriteRenderer组件
    }

    public void FadeOut() // 公共方法，用于渐出
    {
        StartCoroutine(FadeOutRoutine()); // 调用FadeOutRoutine协程渐出
    }

    public void FadeIn() // 公共方法，用于渐入
    {
        StartCoroutine(FadeInRoutine()); // 调用FadeInRoutine协程渐入
    }

    private IEnumerator FadeInRoutine() // 私有协程方法，用于渐入
    {
        float currentAlpha = spriteRenderer.color.a; // 获取当前的透明度
        float distance = 1f - currentAlpha; // 计算到完全透明的距离

        while (1f - currentAlpha > 0.01f) // 当距离大于0.01f时
        {
            currentAlpha = currentAlpha + distance / Settings.fadeInSeconds * Time.deltaTime; // 根据设置的秒数和时间间隔增加透明度
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha); // 设置新的透明度
            yield return null; // 等待下一帧
        }
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 设置完全透明的颜色
    }

    private IEnumerator FadeOutRoutine() // 私有协程方法，用于渐出
    {
        float currentAlpha = spriteRenderer.color.a; // 获取当前的透明度
        float distance = currentAlpha - Settings.targetAlpha; // 计算到目标透明度的距离

        while (currentAlpha - Settings.targetAlpha > 0.01f) // 当距离大于0.01f时
        {
            currentAlpha = currentAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime; // 根据设置的秒数和时间间隔减少透明度
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha); // 设置新的透明度
            yield return null; // 等待下一帧
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, Settings.targetAlpha); // 设置目标透明度的颜色
    }
}

/*ObscuringItemFader 类：这个类用于控制SpriteRenderer的透明度渐入和渐出效果，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

字段：

spriteRenderer：一个私有字段，用于存储游戏对象的SpriteRenderer组件。
Awake 方法：在对象被创建时调用，用于获取并存储游戏对象的SpriteRenderer组件。

FadeOut 和 FadeIn 方法：两个公共方法，用于启动渐出和渐入的协程。

FadeInRoutine 和 FadeOutRoutine 方法：两个私有协程方法，用于实现渐入和渐出效果。它们通过逐步改变SpriteRenderer的颜色的a（透明度）值来实现渐入和渐出效果。

Settings 类：一个未在代码中显示的类，它应该包含fadeInSeconds、fadeOutSeconds和targetAlpha等设置，用于控制渐入和渐出的速度和目标透明度。

这个类通常用于游戏中需要渐入和渐出效果的场景，例如，当玩家接近某个物品时，物品逐渐显现；当玩家离开时，物品逐渐消失。通过这种方式，游戏可以提供更丰富的视觉反馈和更好的用户体验。

*/