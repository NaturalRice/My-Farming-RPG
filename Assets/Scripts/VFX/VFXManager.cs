// 引用所需的命名空间
using System.Collections;
using UnityEngine;

// 继承自SingletonMonobehaviour，确保VFXManager类在游戏场景中是唯一的
public class VFXManager : SingletonMonobehaviour<VFXManager>
{
    // 私有变量，用于存储等待2秒的时间
    private WaitForSeconds twoSeconds;
    // 可序列化的私有变量，用于在Inspector中设置落叶下落的预制体
    [SerializeField] private GameObject deciduousLeavesFallingPrefab = null;
    // 可序列化的私有变量，用于在Inspector中设置松果下落的预制体
    [SerializeField] private GameObject pineConesFallingPrefab = null;
    // 可序列化的私有变量，用于在Inspector中设置砍树树干的预制体
    [SerializeField] private GameObject choppingTreeTrunkPrefab = null;
    // 可序列化的私有变量，用于在Inspector中设置破坏石头的预制体
    [SerializeField] private GameObject breakingStonePrefab = null;
    // 可序列化的私有变量，用于在Inspector中设置收割的预制体
    [SerializeField] private GameObject reapingPrefab = null;

    // 在对象被创建时调用，用于初始化等待2秒的时间
    protected override void Awake()
    {
        base.Awake();

        twoSeconds = new WaitForSeconds(2f);
    }

    // 当此脚本禁用时，移除收获动作效果事件的监听
    private void OnDisable()
    {
        EventHandler.HarvestActionEffectEvent -= displayHarvestActionEffect;
    }

    // 当此脚本启用时，添加收获动作效果事件的监听
    private void OnEnable()
    {
        EventHandler.HarvestActionEffectEvent += displayHarvestActionEffect;
    }

    // 禁用收获动作效果的协程
    private IEnumerator DisableHarvestActionEffect(GameObject effectGameObject, WaitForSeconds secondsToWait)
    {
        yield return secondsToWait;
        effectGameObject.SetActive(false);
    }

    // 显示收获动作效果的方法
    private void displayHarvestActionEffect(Vector3 effectPosition, HarvestActionEffect harvestActionEffect)
    {
        // 根据收获动作效果类型，实例化并激活相应的效果
        switch (harvestActionEffect)
        {
            case HarvestActionEffect.deciduousLeavesFalling:
                GameObject deciduousLeaveFalling = PoolManager.Instance.ReuseObject(deciduousLeavesFallingPrefab, effectPosition, Quaternion.identity);
                deciduousLeaveFalling.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(deciduousLeaveFalling, twoSeconds));
                break;

            case HarvestActionEffect.pineConesFalling:
                GameObject pineConesFalling = PoolManager.Instance.ReuseObject(pineConesFallingPrefab, effectPosition, Quaternion.identity);
                pineConesFalling.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(pineConesFalling, twoSeconds));
                break;

            case HarvestActionEffect.choppingTreeTrunk:
                GameObject choppingTreeTrunk = PoolManager.Instance.ReuseObject(choppingTreeTrunkPrefab, effectPosition, Quaternion.identity);
                choppingTreeTrunk.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(choppingTreeTrunk, twoSeconds));
                break;

            case HarvestActionEffect.breakingStone:
                GameObject breakingStone = PoolManager.Instance.ReuseObject(breakingStonePrefab, effectPosition, Quaternion.identity);
                breakingStone.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(breakingStone, twoSeconds));
                break;

            case HarvestActionEffect.reaping:
                GameObject reaping = PoolManager.Instance.ReuseObject(reapingPrefab, effectPosition, Quaternion.identity);
                reaping.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(reaping, twoSeconds));
                break;

            case HarvestActionEffect.none:
                break;

            default:
                break;
        }
    }
}

/*VFXManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保VFXManager在游戏场景中是唯一的实例。

twoSeconds是一个私有变量，用于存储等待2秒的时间。

deciduousLeavesFallingPrefab、pineConesFallingPrefab、choppingTreeTrunkPrefab、breakingStonePrefab和reapingPrefab是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置不同效果的预制体。

Awake方法在游戏对象创建时调用，用于初始化等待2秒的时间。

OnDisable和OnEnable方法分别在脚本禁用和启用时调用，用于添加和移除对EventHandler.HarvestActionEffectEvent事件的监听。

DisableHarvestActionEffect是一个协程，用于在等待一定时间后禁用效果游戏对象。

displayHarvestActionEffect方法根据传入的效果类型和位置，实例化并激活相应的效果预制体，并启动DisableHarvestActionEffect协程来在2秒后禁用效果。

这个类的主要用途是管理游戏中的视觉效果，特别是与收获动作相关的效果。通过实现这些方法和协程，它可以在游戏中高效地管理和控制视觉效果的显示和隐藏。*/