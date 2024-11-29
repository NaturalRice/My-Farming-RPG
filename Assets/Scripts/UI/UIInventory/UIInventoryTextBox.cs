// 引用所需的命名空间
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryTextBox : MonoBehaviour
{
    // 可序列化的私有变量，用于在Inspector中设置顶部的三个TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI textMeshTop1 = null;
    [SerializeField] private TextMeshProUGUI textMeshTop2 = null;
    [SerializeField] private TextMeshProUGUI textMeshTop3 = null;
    // 可序列化的私有变量，用于在Inspector中设置底部的三个TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI textMeshBottom1 = null;
    [SerializeField] private TextMeshProUGUI textMeshBottom2 = null;
    [SerializeField] private TextMeshProUGUI textMeshBottom3 = null;

    // 设置文本框的文本值
    public void SetTextboxText(string textTop1, string textTop2, string textTop3, string textBottom1, string textBottom2, string textBottom3)
    {
        // 设置顶部三个文本组件的文本
        textMeshTop1.text = textTop1;
        textMeshTop2.text = textTop2;
        textMeshTop3.text = textTop3;
        // 设置底部三个文本组件的文本
        textMeshBottom1.text = textBottom1;
        textMeshBottom2.text = textBottom2;
        textMeshBottom3.text = textBottom3;
    }
}

/*UIInventoryTextBox类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

textMeshTop1、textMeshTop2、textMeshTop3、textMeshBottom1、textMeshBottom2和textMeshBottom3是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置六个TextMeshProUGUI组件。这些组件用于显示文本信息。

SetTextboxText是一个公开方法，用于设置文本框的文本值。它接受六个字符串参数，分别对应六个TextMeshProUGUI组件的文本内容。

在SetTextboxText方法中，将传入的字符串参数分别赋值给六个TextMeshProUGUI组件的text属性，从而更新文本框的显示内容。

这个类的主要用途是作为一个UI控件，用于在游戏中显示物品的详细信息或其他文本信息。通过SetTextboxText方法，可以根据需要动态更新文本框的内容，提供灵活的文本显示功能。*/