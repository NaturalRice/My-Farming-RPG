// 引用System.Collections和System.Collections.Generic命名空间，提供对集合类的支持
using System.Collections;
using System.Collections.Generic;
// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;

// 要求附加此脚本的游戏对象必须有BoxCollider2D组件
[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    // 可序列化的私有变量，用于在Inspector中设置要传送到的场景名称
    [SerializeField] private SceneName sceneNameGoto = SceneName.Scene1_Farm;
    // 可序列化的私有变量，用于在Inspector中设置传送点的位置
    [SerializeField] private Vector3 scenePositionGoto = new Vector3();

    // 当其他Collider2D进入此游戏对象的触发器时，每帧都会调用此方法
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 获取碰撞对象上的Player组件
        Player player = collision.GetComponent<Player>();

        // 如果碰撞对象上有Player组件
        if (player != null)
        {
            // 计算玩家在新场景中的位置
            float xPosition = Mathf.Approximately(scenePositionGoto.x, 0f) ? player.transform.position.x : scenePositionGoto.x;
            float yPosition = Mathf.Approximately(scenePositionGoto.y, 0f) ? player.transform.position.y : scenePositionGoto.y;
            float zPosition = 0f;

            // 传送到新场景
            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoto.ToString(), new Vector3(xPosition, yPosition, zPosition));
        }
    }
}

/*SceneTeleport类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

[RequireComponent(typeof(BoxCollider2D))]属性要求附加此脚本的游戏对象必须有BoxCollider2D组件，这是因为该脚本依赖于触发器碰撞来激活传送功能。

sceneNameGoto是一个可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置要传送到的目标场景名称。SceneName可能是一个枚举，定义了所有可用的场景名称。

scenePositionGoto是一个可序列化的私有变量，用于设置传送点的位置。如果scenePositionGoto的值被设置为(0,0,0)，则传送时会使用玩家当前的位置。

OnTriggerStay2D方法在其他Collider2D进入此游戏对象的触发器时，每帧都会调用。它用于检测是否有玩家（或其他对象）进入触发器，并执行传送操作。

Player组件是从碰撞对象上获取的，如果存在，则会根据scenePositionGoto计算新位置，并调用SceneControllerManager的FadeAndLoadScene方法来执行场景切换和传送。

FadeAndLoadScene方法负责淡出到黑屏，加载新场景，并在新场景中将玩家传送到指定位置。

这个类的主要用途是在玩家进入特定的触发区域时，触发场景的切换和玩家位置的传送。通过这种方式，可以实现游戏中的无缝场景过渡和位置移动。*/