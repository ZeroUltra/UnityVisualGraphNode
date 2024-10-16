# UnityVisualGraphNode
### Unity 可视化节点编辑器

![1](https://raw.githubusercontent.com/ZeroUltra/MediaLibrary/main/Imgs/202211131344795.gif)

![image-20241016143032728](https://raw.githubusercontent.com/ZeroUltra/MediaLibrary/main/Imgs/202410161430531.png)

* 高度自由定制
* 运行时定位节点

### 如何使用

1. 新建脚本继承`NodeBase`即可(可直接使用 Project试图中鼠标右键点击->Create->Visual Node C# Script)

```c#
using VisualGraphRuntime;
using NodeGraphView;

//节点名字 排序(超过10中间会有横线) 图标名字(unity中内置图标名字))
[NodeName("node示例", 1, iconName = "d_ContentSizeFitter Icon")]
//输入输出端口类型 (输入输出端口数量)
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodeSample : NodeBase
{
    public float waitDuration;
}
```

其中图标名字均是unity内部icon可参考:[jasursadikov/unity-editor-icons) (github.com)](https://github.com/jasursadikov/unity-editor-icons)

2. 新建一个NodeGrpah,进行节点编辑

![image-20241016134552468](C:\Users\y\AppData\Roaming\Typora\typora-user-images\image-20241016134552468.png)

3. 根据节点内部逻辑,自行编辑代码
4. 具体可查看Demo文件夹示例



### 注意点

可以对node自行绘制,有两种方式

* 使用IMGUI,和普通继承Editor重写相同,缺点是如果太多会导致界面卡顿(原因是需要太多draw)
* 使用UIElements,需要继承VisualGraphNodeView,使用UIElements的UI绘制,这个不会导致卡顿

具体代码可查看Demo文件夹`NodeSampleEditor`示例

### 其他

[Unity-Visual-Graph-Editor](https://github.com/BusStopStudios/Unity-Visual-Graph-Editor)

基于上面这个repo进行了修改

使用的第三方插件:

[dbrizov/NaughtyAttributes: Attribute Extensions for Unity (github.com)](https://github.com/dbrizov/NaughtyAttributes)

### ChangeLog

#### v1.0.0

提交1.0包
