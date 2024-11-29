using System.Collections; // 引用System.Collections命名空间，提供非泛型集合接口
using UnityEngine; // 引用Unity引擎的命名空间

public class ItemNudge : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private WaitForSeconds pause; // 私有字段，用于存储等待时间
    private bool isAnimating = false; // 私有字段，标记是否正在动画中

    private void Awake() // 在对象被创建时调用
    {
        pause = new WaitForSeconds(0.04f); // 初始化等待时间为0.04秒
    }

    private void OnTriggerEnter2D(Collider2D collision) // 当其他Collider2D进入触发器时调用
    {
        if (isAnimating == false) // 如果没有正在动画中
        {
            if (gameObject.transform.position.x < collision.gameObject.transform.position.x) // 如果游戏对象的位置x小于碰撞游戏对象的位置x
            {
                StartCoroutine(RotateAntiClock()); // 调用RotateAntiClock协程逆时针旋转
            }
            else
            {
                StartCoroutine(RotateClock()); // 调用RotateClock协程顺时针旋转
            }

            // 如果碰撞的游戏对象是玩家，则播放沙沙声
            if (collision.gameObject.tag == "Player")
            {
                AudioManager.Instance.PlaySound(SoundName.effectRustle); // 播放沙沙声
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 当其他Collider2D离开触发器时调用
    {
        if (isAnimating == false) // 如果没有正在动画中
        {
            if (gameObject.transform.position.x > collision.gameObject.transform.position.x) // 如果游戏对象的位置x大于碰撞游戏对象的位置x
            {
                StartCoroutine(RotateAntiClock()); // 调用RotateAntiClock协程逆时针旋转
            }
            else
            {
                StartCoroutine(RotateClock()); // 调用RotateClock协程顺时针旋转
            }

            // 如果碰撞的游戏对象是玩家，则播放沙沙声
            if (collision.gameObject.tag == "Player")
            {
                AudioManager.Instance.PlaySound(SoundName.effectRustle); // 播放沙沙声
            }
        }
    }

    private IEnumerator RotateAntiClock() // 逆时针旋转的协程
    {
        isAnimating = true; // 设置动画标记为真

        for (int i = 0; i < 4; i++) // 循环4次
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f); // 旋转子对象

            yield return pause; // 等待
        }

        for (int i = 0; i < 5; i++) // 循环5次
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f); // 旋转子对象

            yield return pause; // 等待
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f); // 旋转子对象

        yield return pause; // 等待

        isAnimating = false; // 设置动画标记为假
    }

    private IEnumerator RotateClock() // 顺时针旋转的协程
    {
        isAnimating = true; // 设置动画标记为真

        for (int i = 0; i < 4; i++) // 循环4次
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f); // 旋转子对象

            yield return pause; // 等待
        }

        for (int i = 0; i < 5; i++) // 循环5次
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f); // 旋转子对象

            yield return pause; // 等待
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f); // 旋转子对象

        yield return pause; // 等待

        isAnimating = false; // 设置动画标记为假
    }
}

/*ItemNudge 类：这个类用于处理游戏中物品的旋转动画，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

字段：

pause：一个WaitForSeconds对象，用于在协程中创建延迟。
isAnimating：一个布尔值，用于标记是否正在执行动画。
Awake 方法：在对象被创建时调用，用于初始化pause对象。

OnTriggerEnter2D 和 OnTriggerExit2D 方法：当其他Collider2D进入或离开触发器时调用，根据游戏对象的位置和碰撞游戏对象的位置决定旋转方向，并播放沙沙声。

RotateAntiClock 和 RotateClock 方法：两个协程方法，用于控制游戏对象的子对象逆时针和顺时针旋转。

这个类通常用于游戏中物品的交互动画，例如，当玩家接近或离开某个物品时，物品会进行旋转动画，增强游戏的视觉效果和互动性。*/