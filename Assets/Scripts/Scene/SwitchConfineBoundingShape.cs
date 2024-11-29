// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;
// 引用Cinemachine命名空间，提供Cinemachine相机系统相关功能的支持
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // 当此脚本启用时，添加AfterSceneLoadEvent事件的监听
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    // 当此脚本禁用时，移除AfterSceneLoadEvent事件的监听
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;
    }

    /// <summary>
    /// 切换Cinemachine使用的碰撞体，以定义屏幕边缘
    /// </summary>
    private void SwitchBoundingShape()
    {
        // 获取标记为Tags.BoundsConfiner的游戏对象上的多边形碰撞体，该碰撞体被Cinemachine用来防止相机超出屏幕边缘
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

        // 获取此游戏对象上的CinemachineConfiner组件
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        // 将CinemachineConfiner的边界形状设置为新的多边形碰撞体
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        // 由于边界限制已经改变，需要调用此方法来清除缓存
        cinemachineConfiner.InvalidatePathCache();
    }
}

/*SwitchConfineBoundingShape类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

OnEnable和OnDisable方法分别在脚本启用和禁用时调用。它们用于添加和移除对EventHandler.AfterSceneLoadEvent事件的监听，这个事件在场景加载后触发。

SwitchBoundingShape方法作为EventHandler.AfterSceneLoadEvent事件的回调函数，用于在场景加载后切换Cinemachine相机系统的边界限制。

PolygonCollider2D是通过查找标记为Tags.BoundsConfiner的游戏对象并获取其上的PolygonCollider2D组件来获得的。这个碰撞体定义了Cinemachine相机的边界。

CinemachineConfiner是从当前游戏对象上获取的CinemachineConfiner组件，它用于控制Cinemachine相机的边界。

cinemachineConfiner.m_BoundingShape2D将CinemachineConfiner的边界形状设置为新获取的PolygonCollider2D。

cinemachineConfiner.InvalidatePathCache方法用于清除Cinemachine的路径缓存，因为边界形状已经改变，需要重新计算路径。

这个类的主要用途是在场景加载后更新Cinemachine相机系统的边界限制，以确保相机不会超出预定的屏幕边缘。这对于保持游戏视角的一致性和防止相机穿过游戏世界的边界非常有用。*/