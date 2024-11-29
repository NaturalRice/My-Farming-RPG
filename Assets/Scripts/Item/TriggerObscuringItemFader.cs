using UnityEngine; // 引用Unity引擎的命名空间

public class TriggerObscuringItemFader : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private void OnTriggerEnter2D(Collider2D collision) // 当其他Collider2D进入触发器时调用
    {
        // 获取我们碰撞的游戏对象，然后获取该对象及其子对象上所有的ObscuringItemFader组件 - 然后触发渐出效果
        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>(); // 获取ObscuringItemFader组件数组

        if (obscuringItemFader.Length > 0) // 如果数组长度大于0
        {
            for (int i = 0; i < obscuringItemFader.Length; i++) // 遍历数组
            {
                obscuringItemFader[i].FadeOut(); // 调用FadeOut方法渐出
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 当其他Collider2D离开触发器时调用
    {
        // 获取我们碰撞的游戏对象，然后获取该对象及其子对象上所有的ObscuringItemFader组件 - 然后触发渐入效果
        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>(); // 获取ObscuringItemFader组件数组

        if (obscuringItemFader.Length > 0) // 如果数组长度大于0
        {
            for (int i = 0; i < obscuringItemFader.Length; i++) // 遍历数组
            {
                obscuringItemFader[i].FadeIn(); // 调用FadeIn方法渐入
            }
        }
    }
}

/*TriggerObscuringItemFader 类：这个类用于在玩家进入或离开触发区域时控制物品的渐入和渐出效果，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

OnTriggerEnter2D 方法：当其他Collider2D进入触发器时调用，用于触发渐出效果。

collision参数：包含与触发器碰撞的游戏对象的信息。
GetComponentsInChildren<ObscuringItemFader>()：获取触发器游戏对象及其子对象上所有的ObscuringItemFader组件。
如果找到ObscuringItemFader组件，则遍历它们并调用FadeOut方法来渐出。
OnTriggerExit2D 方法：当其他Collider2D离开触发器时调用，用于触发渐入效果。

collision参数：包含与触发器碰撞的游戏对象的信息。
GetComponentsInChildren<ObscuringItemFader>()：获取触发器游戏对象及其子对象上所有的ObscuringItemFader组件。
如果找到ObscuringItemFader组件，则遍历它们并调用FadeIn方法来渐入。
这个类通常用于游戏中的物品遮挡效果，例如，当玩家进入某个区域时，某些物品逐渐消失，当玩家离开时，这些物品逐渐显现。这种效果可以用于优化游戏性能，减少同时渲染的物品数量，或者用于创造特定的视觉效果和游戏体验。*/