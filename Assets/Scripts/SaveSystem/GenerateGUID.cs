// 使用Unity引擎的命名空间
using UnityEngine;

// 标记该脚本在编辑器和游戏运行时都会执行
[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    // 可序列化私有字段，用于在Inspector中显示和编辑
    [SerializeField]
    private string _gUID = "";

    // GUID属性的公开接口，用于获取和设置私有字段_gUID的值
    public string GUID { get => _gUID; set => _gUID = value; }

    // Awake方法在游戏对象被创建时调用一次
    private void Awake()
    {
        // 只在编辑器中填充GUID
        if (!Application.IsPlaying(gameObject))
        {
            // 确保对象有一个保证唯一的ID
            if (_gUID == "")
            {
                // 分配GUID
                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}

/*GenerateGUID类继承自MonoBehaviour，这意味着它可以附加到Unity游戏对象上，并在游戏运行时执行。

[ExecuteAlways]属性允许该脚本在Unity编辑器和游戏运行时都执行。这通常用于编辑器工具或需要在两种模式下都工作的脚本。

_gUID是一个私有的字符串字段，用于存储GUID。它被标记为[SerializeField]，这意味着它的值可以在Unity的Inspector窗口中显示和编辑，并且可以在编辑器和游戏运行时之间保持同步。

GUID属性提供了一个公开的接口来访问和修改_gUID字段的值。这是一个常见的做法，用于封装字段并提供对字段值的控制。

Awake方法是Unity生命周期的一部分，它在游戏对象被创建时调用一次。在这个例子中，Awake方法用于在编辑器中生成GUID。Application.IsPlaying(gameObject)检查游戏是否正在运行，如果不是（即在编辑器模式下），则检查_gUID是否为空，如果为空，则生成一个新的GUID并赋值给_gUID。

这个类的主要用途是在Unity编辑器中为游戏对象生成一个唯一的标识符，这可以用于识别和引用游戏对象，特别是在需要确保对象唯一性的场景中，如网络同步、资产管理或持久化存储。通过在编辑器中自动生成GUID，可以简化开发过程，并减少手动分配唯一标识符的需要。*/