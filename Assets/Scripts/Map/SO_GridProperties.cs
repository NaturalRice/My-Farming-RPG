// 使用Unity引擎的命名空间
using System.Collections.Generic;
using UnityEngine;

// 创建一个可以在Unity编辑器中通过菜单创建的ScriptableObject资产
[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
    // 场景名称
    public SceneName sceneName;
    // 网格的宽度
    public int gridWidth;
    // 网格的高度
    public int gridHeight;
    // 网格的原点X坐标
    public int originX;
    // 网格的原点Y坐标
    public int originY;

    // 可以被序列化的GridProperty对象列表
    [SerializeField]
    public List<GridProperty> gridPropertyList;
}

/*命名空间引用：

using System.Collections.Generic;：引用了 System.Collections.Generic 命名空间，这允许使用泛型集合类，如 List<T>。
using UnityEngine;：引用了 UnityEngine 命名空间，这提供了Unity引擎的基本功能。
类定义：

[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]：这是一个属性，用于在Unity编辑器中创建一个新的资产时指定文件名和菜单路径。通过这个属性，用户可以在Unity的“Assets”菜单下找到“Create”子菜单，然后选择“Scriptable Objects” -> “Grid Properties”来创建这个类的实例。
public class SO_GridProperties : ScriptableObject：定义了一个公共类 SO_GridProperties，它继承自 ScriptableObject。ScriptableObject 是 Unity 中用于创建自定义资产的基类。
成员变量：

public SceneName sceneName;：一个公共变量，用于存储与这个网格属性相关联的场景名称。SceneName 可能是一个枚举或类，用于标识不同的场景。
public int gridWidth;：一个公共变量，用于存储网格的宽度。
public int gridHeight;：一个公共变量，用于存储网格的高度。
public int originX;：一个公共变量，用于存储网格原点的X坐标。
public int originY;：一个公共变量，用于存储网格原点的Y坐标。
[SerializeField] public List<GridProperty> gridPropertyList;：一个可以被序列化的公共列表，用于存储 GridProperty 对象。GridProperty 对象包含了网格的布尔属性，如是否可以挖掘、是否可以放置物品等。
序列化属性：

[SerializeField]：这是一个属性，用于指示字段可以被Unity编辑器序列化，即可以在Unity编辑器中直接编辑这个字段的值。
总的来说，SO_GridProperties 类是一个用于定义和管理与特定场景相关联的网格属性的脚本对象。通过Unity的创建资产菜单，可以方便地创建和编辑这个类的实例，从而在游戏中使用和管理网格属性。*/