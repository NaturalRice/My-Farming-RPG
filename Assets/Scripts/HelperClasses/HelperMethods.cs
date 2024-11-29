using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合类
using UnityEngine; // 引用Unity引擎的命名空间

public static class HelperMethods // 定义一个公共静态类
{
    /// <summary>
    /// 在positionToCheck位置获取类型为T的组件。如果至少找到一个，则返回true，并将找到的组件返回在componentAtPositionList中
    /// </summary>
    public static bool GetComponentsAtCursorLocation<T>(out List<T> componentsAtPositionList, Vector3 positionToCheck) // 公共静态方法
    {
        bool found = false; // 初始化found标志为false

        List<T> componentList = new List<T>(); // 创建一个列表来存储找到的组件

        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(positionToCheck); // 调用Physics2D.OverlapPointAll获取所有与positionToCheck重叠的Collider2D

        // 循环遍历所有碰撞体以获取类型为T的对象
        T tComponent = default(T); // 初始化tComponent为默认值

        for (int i = 0; i < collider2DArray.Length; i++) // 遍历碰撞体数组
        {
            tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>(); // 尝试获取父对象中的T类型组件
            if (tComponent != null) // 如果找到组件
            {
                found = true; // 设置found标志为true
                componentList.Add(tComponent); // 将找到的组件添加到列表中
            }
            else // 如果没有在父对象中找到
            {
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>(); // 尝试获取子对象中的T类型组件
                if (tComponent != null) // 如果找到组件
                {
                    found = true; // 设置found标志为true
                    componentList.Add(tComponent); // 将找到的组件添加到列表中
                }
            }
        }

        componentsAtPositionList = componentList; // 将组件列表赋值给out参数

        return found; // 返回found标志
    }


    /// <summary>
    /// 在以点为中心，具有大小和角度的盒子区域内获取类型为T的组件。如果至少找到一个，则返回true，并将找到的组件返回在list中
    /// </summary>
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentsAtBoxPosition, Vector2 point, Vector2 size, float angle) // 公共静态方法
    {
        bool found = false; // 初始化found标志为false
        List<T> componentList = new List<T>(); // 创建一个列表来存储找到的组件

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point, size, angle); // 调用Physics2D.OverlapBoxAll获取所有与盒子区域重叠的Collider2D

        // 循环遍历所有碰撞体以获取类型为T的对象
        for (int i = 0; i < collider2DArray.Length; i++) // 遍历碰撞体数组
        {
            T tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>(); // 尝试获取父对象中的T类型组件
            if (tComponent != null) // 如果找到组件
            {
                found = true; // 设置found标志为true
                componentList.Add(tComponent); // 将找到的组件添加到列表中
            }
            else // 如果没有在父对象中找到
            {
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>(); // 尝试获取子对象中的T类型组件
                if (tComponent != null) // 如果找到组件
                {
                    found = true; // 设置found标志为true
                    componentList.Add(tComponent); // 将找到的组件添加到列表中
                }
            }
        }

        listComponentsAtBoxPosition = componentList; // 将组件列表赋值给out参数

        return found; // 返回found标志
    }

    /// <summary>
    /// 返回在以点为中心，具有大小和角度的盒子区域内的类型为T的组件数组。numberOfCollidersToTest参数传递了要测试的碰撞体数量。找到的组件返回在数组中
    /// </summary>
    public static T[] GetComponentsAtBoxLocationNonAlloc<T>(int numberOfCollidersToTest, Vector2 point, Vector2 size, float angle) // 公共静态方法
    {
        Collider2D[] collider2DArray = new Collider2D[numberOfCollidersToTest]; // 创建一个Collider2D数组来存储碰撞体

        Physics2D.OverlapBoxNonAlloc(point, size, angle, collider2DArray); // 调用Physics2D.OverlapBoxNonAlloc获取所有与盒子区域重叠的Collider2D，不分配新数组

        T tComponent = default(T); // 初始化tComponent为默认值

        T[] componentArray = new T[collider2DArray.Length]; // 创建一个T类型数组来存储找到的组件

        for (int i = collider2DArray.Length - 1; i >= 0; i--) // 逆向遍历碰撞体数组
        {
            if (collider2DArray[i] != null) // 如果碰撞体不为空
            {
                tComponent = collider2DArray[i].gameObject.GetComponent<T>(); // 尝试获取T类型组件

                if (tComponent != null) // 如果找到组件
                {
                    componentArray[i] = tComponent; // 将找到的组件添加到数组中
                }
            }
        }

        return componentArray; // 返回组件数组
    }

}

/*HelperMethods 类：这个静态类包含一些辅助方法，用于在Unity游戏中获取特定位置或区域内的组件。

GetComponentsAtCursorLocation 方法：在指定位置positionToCheck获取所有类型为T的组件。如果至少找到一个，则返回true，并将找到的组件返回在componentAtPositionList中。

GetComponentsAtBoxLocation 方法：在以点为中心，具有大小和角度的盒子区域内获取所有类型为T的组件。如果至少找到一个，则返回true，并将找到的组件返回在listComponentsAtBoxPosition中。

GetComponentsAtBoxLocationNonAlloc 方法：在以点为中心，具有大小和角度的盒子区域内获取类型为T的组件数组。这个方法使用Physics2D.OverlapBoxNonAlloc方法，它不分配新的数组，而是填充传入的数组。找到的组件返回在componentArray中。

这些方法通常用于游戏中的射线检测、碰撞检测和组件检索，特别是在需要处理多个组件或优化性能时。通过使用这些静态方法，开发者可以轻松地在特定区域或位置查找和操作组件。*/