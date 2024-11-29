// 使用Unity编辑器的命名空间
using UnityEditor;
// 使用Unity引擎的命名空间
using UnityEngine;
// 使用Unity引擎的Tilemap命名空间
using UnityEngine.Tilemaps;

// 使脚本在编辑器和运行时都执行
[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
#if UNITY_EDITOR
    // 私有的Tilemap组件
    private Tilemap tilemap;
    // 可以被序列化的ScriptableObject，用于存储网格属性
    [SerializeField] private SO_GridProperties gridProperties = null;
    // 可以被序列化的网格布尔属性
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;

    // 当组件启用时调用
    private void OnEnable()
    {
        // 只在编辑器中填充
        if (!Application.IsPlaying(gameObject))
        {
            tilemap = GetComponent<Tilemap>();

            if (gridProperties != null)
            {
                gridProperties.gridPropertyList.Clear();
            }
        }
    }

    // 当组件禁用时调用
    private void OnDisable()
    {
        // 只在编辑器中填充
        if (!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();

            if (gridProperties != null)
            {
                // 这是必要的，以确保更新后的gridproperties游戏对象在保存游戏时被保存 - 否则它们不会被保存。
                EditorUtility.SetDirty(gridProperties);
            }
        }
    }

    // 更新网格属性
    private void UpdateGridProperties()
    {
        // 压缩Tilemap边界
        tilemap.CompressBounds();

        // 只在编辑器中填充
        if (!Application.IsPlaying(gameObject))
        {
            if (gridProperties != null)
            {
                Vector3Int startCell = tilemap.cellBounds.min;
                Vector3Int endCell = tilemap.cellBounds.max;

                for (int x = startCell.x; x < endCell.x; x++)
                {
                    for (int y = startCell.y; y < endCell.y; y++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                        if (tile != null)
                        {
                            gridProperties.gridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }
                    }
                }
            }
        }
    }

    // 每帧调用
    private void Update()
    {
        // 只在编辑器中填充
        if (!Application.IsPlaying(gameObject))
        {
            Debug.Log("DISABLE PROPERTY TILEMAPS");
        }
    }
#endif
}

/*命名空间引用：

using UnityEditor;：引用了 Unity 编辑器的命名空间，这允许脚本访问 Unity 编辑器特有的功能。
using UnityEngine;：引用了 Unity 引擎的基本功能。
using UnityEngine.Tilemaps;：引用了 Unity 引擎的 Tilemap 功能。
类定义：

[ExecuteAlways]：这是一个属性，它指示即使在编辑器中，脚本也应该被执行。
public class TilemapGridProperties : MonoBehaviour：定义了一个公共类 TilemapGridProperties，它继承自 MonoBehaviour，这意味着它可以被附加到 Unity 场景中的游戏对象上。
条件编译：

#if UNITY_EDITOR：这是一个预处理指令，它指示只有在 Unity 编辑器中编译时才包含以下代码。
成员变量：

private Tilemap tilemap;：一个私有变量，用于存储附加到同一个游戏对象上的 Tilemap 组件。
[SerializeField] private SO_GridProperties gridProperties = null;：一个可以被序列化的私有变量，用于存储 SO_GridProperties 脚本对象，它包含了网格属性列表。
[SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;：一个可以被序列化的私有变量，用于存储网格的布尔属性类型，默认为 diggable（可挖掘）。
OnEnable方法：

在组件启用时调用，如果游戏对象不在播放模式下，获取 Tilemap 组件并清空网格属性列表。
OnDisable方法：

在组件禁用时调用，如果游戏对象不在播放模式下，更新网格属性并标记 gridProperties 为“脏”（需要保存），以确保在保存游戏时，这些属性会被保存。
UpdateGridProperties方法：

压缩 Tilemap 的边界，并在编辑器中填充网格属性列表。遍历 Tilemap 的每个单元格，并为每个非空的 Tile 添加一个新的 GridProperty 对象。
Update方法：

每帧调用，如果游戏对象不在播放模式下，在控制台输出一条消息，提示禁用属性Tilemap。
总的来说，TilemapGridProperties 类用于在 Unity 编辑器中管理和更新与 Tilemap 相关的属性，并将这些属性存储在一个 ScriptableObject 中，以便在游戏中使用。这个类只在编辑器中工作，不会影响游戏的运行时行为。*/