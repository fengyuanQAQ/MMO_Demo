### 一 基础阶段

##### 1、人物移动

（1）鼠标控制人物移动:从摄像机发出一条射线，判断其与地面是否相交，找出目标点，然后移动旋转即可。

* 求得当前点与目标点的方向后要将y方向置为0，不然会出现旋转鬼畜的现象。
* 开场要将角色水平置于地面，否则会由于重力效果，自动生成一个夹角影响判断。

##### 2、层和tag

（1）层：每一个物体都属于一个层，层与层之间产生碰撞。

（2）tag:通过tag寻找物体。

##### 3、碰撞

（1）条件

* 所在层能够碰撞(通过Physics设置)
* 都有碰撞体(Collider或者CharacterController)
* 其中一个物体需要有刚体

（2）CharacterController

+ 只能检测触发不能检测碰撞，但是会被带有Collider组件的物体阻挡下来，并且通过OnControllerColliderHit()回调。
+ 如果想实现CharacterController推箱子，箱子必须带有Rigidbody,推理需要在OnControllerColliderHit里面发出。

##### 4、射线

（1）两种射线：线段、球形

+ 射线只能找一天线上的物品
+ 球形射线能找周围的所有物品

##### 5、预设

简单的案例:在一个区域内随机生成箱子，玩家拾取箱子之后把箱子销毁。

（1）生成箱子

+ 创建一个生成箱子的区域
+ 生成箱子:Resources.Load->设置Parent->转换坐标->
  + transform.TransformPoint()//将相对坐标转换为世界坐标
  + 这个坐标受到Scale的影响，如果Y方向缩小了100倍，想要相对向上移动0.5m，需要最后+50

（2）角色与箱子的交互

+ 玩家发出射线检测箱子是否存在

+ 如果箱子存在，触发点击后的事件

+ 生成箱子的时候绑定事件

+ ```c#
  //委托
  public Action<GameObject> onHit;
  public void Hit()
  {
      if(onHit!=null)
      {
          onHit(gameObject);
      }
  }
  
  //绑定事件
  GameObject boxGo=Instantiate(boxGameObj) as GameObject;
  boxGo.GetComponent<BoxCtrl>().onHit=OnHit;
  private void OnHit()
  {
     xxxxxx;
  }
  
  //触发事件
  go.Hit();
  
  ```

  ##### 6、Mono脚本的简介

  （1）周期函数执行顺序

  Awake->OnEnable->Start->Update->

  （2）常用数学函数

  * Mathf.Clamp(value,min,max);//夹紧，限制在min-max
  * Mathf.Slerp(current,target,progress)//当progerss>=1 ->max;progress<=-1->min

  ##### 7、单例模式

  （1）CommonSingleTon

  （2）MonoSingleTon

  ```c#
   private static MonoSingleTon _Instance;
      public static MonoSingleTon Instance
      {
          get
          {
              if (_Instance == null)
              {
                  //创建物体
                  GameObject go = new GameObject("MonoSingleTon");
                  _Instance=go.AddComponent<MonoSingleTon>();
                  DontDestroyOnLoad(go);//设置为无法销毁
              }
              return _Instance;
          }
      }
  ```

  ##### 8、协程

  （1）开始一个协程

  StartCoroutine(IEnumerator routine);

  StartCoroutine(String method);

  （2）终止一个协程

  StopCoroutine(string method);//这个方法只能终止传递参数为method的启动协程方式

  StopAllCotoutines();

  （3）协程的输入输出

  + 返回类型为Coroutine类型，返回只能为null,等待帧数,等待时间
  + 不能指定ref,out参数（暂时没用到）

  ##### 9、委托与事件

  （1）委托:委托的本质是一个类，可以想象成一个函数指针

  + 委托的声明

  ```C#
  public delegate void MyDelegate(int,int);
  ```

  + 委托的调用：

  ```c#
  MyDelegate.Invoke(int,int);
  MyDelegate();
  ```

  （2）事件：委托的封装

  + 一般使用EventHandler 和 EventArgs作为基类

  + 私有委托+Remove,Add方法

    ```c#
    public class Customer
    {
        //完整声明
    	private EventHandler orderEventHandler;
        public event EventHandler OnOrder
        {
            remove
            {
             	this.orderEventHandler-=value;   
            }
            add
            {	
                this.orderEventHandler+=value;
            }
        }
        
        //简略声明
        public event EventHandler OnOrder;
    }
    ```

  + 事件处理

  ```c#
  public class Waiter
  {
      public void Action(object Sender,EventArgs e)
      {
          
      }
  }
  ```

  + 事件订阅

  ```c#
  OnOrder+=Waiter.Action;
  ```

  

  ##### 10、摄像机跟随

  （1）键盘控制摄像机与人物视角之间的移动

  * 三层架构
    * CameraFollowAndRotate: 最外层的物体跟随着主角，控制人物的左右旋转
    * CameraUpAndDown: 次外层物体，控制角色上下镜头的移动
    * CameraZoomContainer:看着角色，需要摆在合适的位置
    * CameraContainer:角色视觉的缩放
  * 需要对摄像机进行初始化，给CameraUpAndDown一个初始的仰角或者俯角
  * 对仰角俯角、缩放距离要进行限制，Mathf.Clamp

  （2）EasyTouch的简单使用

  

  ##### 11、UGUI入门

  （1）Canvas:

  + ScreenSpace:UI在摄像机的最前面
  + ScreenSpace-Camera:需要添加一个UI摄像机，UI摄像机只看UI层，主摄像机不看UI层
  + World Space:3DUI

  + 自适应分辨率: UI Scale Mode :Scale With Screen Size

  + UGUI渲染顺序

    * 摄像机的深度
    * Sorting Layer
    * Order in Layer
    * 在父物体中的顺序

    

  （2）Rect Transform

  当分辨率改变时：

  + 锚点改变的是百分比
  + 但是UI元素对应的锚点的距离是不变的

  （3）Image

  ![image-20201020204316489](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201020204316489.png)

  + Simple：Preserve Aspect 等比例缩放

  + Sliced：图片->Multi->Slice->Boarder(九宫格) =》固定四周，只缩放中间位置

  + Tiled:缩放的部分平铺

    ![image-20201020205109975](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201020205109975.png)

  + Filled：冷却效果

  ![image-20201020205347894](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201020205347894.png)

  

  （4）Raw Image

  + 用于显示Texture纹理类型给图片，常用于显示网络图片
  + 映射摄像机画面，new RenderTexture ->NewCamera->RawImage的Texture属性

  （5）Text

  + 富文本：

    + b:粗体的   <b>Bold</b>b>

    + i:斜体的  <i>Italic</i>

    + size:大小 <size=50>size</size>

    + color:颜色 <color=green>color</color>

  + Shadow

  + Outline(轮廓 描边)

  （6）交互组件

  ![image-20201020224923311](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201020224923311.png)

  + Selectable

    * Interactable:是否可交互
    * FadeDuration: 延迟时间
    * Transition:过渡

  + 事件

    + Button:onClick
    + Toggle:onValueChanged
    + Slider:  onValueChanged
    + ScrollBar:onValueChanged
    + InputFiled:onValueChanged
    + DropDown:OptionData添加选项

    

  ##### 12 、DOTween组件

  + 引入命名空间 DG.Tweening
  + 常用方法
    * DoTween.TO(Getter,Setter,EndValue,Dureation)
    * 扩展方法->扩展到transform组件上面
      + DoXXXX 补间动画：DOLocalMoveX(200,2)
      + SetXXXX 设置参数:.SetEase(Ease.OutQuad) .SetLoops(2)
      + OnXXXX 回调:OnDebugText,OnRewind(多用于关掉界面的动画)
      + transform.PlayForwards,PlayBackwards(反向播放动画)

  ##### 13、MVC架构构建UI界面

  ![image-20201022224659234](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201022224659234.png)

  

  + UIViewBase:找到自身的Button组件,提供各个Mono脚本周期状态的方法，(OnStart,OnAwake等)

    + 使用虚方法，在Onxxxx中调用这些虚方法，这样才能达到多态的效果

    ```C#
     protected virtual void OnAwake(){}
     protected virtual void OnStart(){}
    ```

  + UISceneViewBase:挂载一个中心物体，上面再挂在UIWindowVeiw

  + UIWindowViewBase:

  ##### 14、Animation系统

  + 动画导入，分动画

  ![image-20201022224342715](C:\Users\FantancyWind\AppData\Roaming\Typora\typora-user-images\image-20201022224342715.png)

  

  



