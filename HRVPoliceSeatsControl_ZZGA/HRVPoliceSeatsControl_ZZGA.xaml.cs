using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HighResolutionApps.Interfaces;
using HrvVisualControlStyles;
using VisualContorlBase;
using System.Data;

namespace HighResolutionApps.VisualControls.HRVPoliceSeatsControl_ZZGA
{
    /// <summary>
    /// HRVPoliceSeatsControl_ZZGA.xaml 的交互逻辑
    /// </summary>
    public partial class HRVPoliceSeatsControl_ZZGA : UserControl, IVisualControl, IPageEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HRVPoliceSeatsControl_ZZGA()
        {
            InitializeComponent();
            // 必要 样式使用
            StyleManager.GetResourceDictionnary(this, m_WindowStyleName);
            // 区分当前运行环境
            if (CurrentRunderUnitEnvironment.Mode == EnvironmentMode.Designer)
            {
                // 运行环境是设计端
                // 界面渲染  在设计端调用  在其他运行环境无需调用
                LoadContent();
            }

        }

        /// <summary>
        /// 创建自定义数据，填充到dataTable
        /// </summary>
        public void InitInputDataTable()
        {
            try
            {
                SystemHelper.logger.LogDebug("HRVPoliceCasePrecentControl_ZZGA ==============InitInputDataTable.start============= ");
                DataTable dt = new DataTable();

                dt.Columns.Add("seat_no", typeof(int));
                dt.Columns.Add("seat_state", typeof(string));
                // 1-开始接警，2-闲置，3-接警结束
                dt.Rows.Add(1, "闲置");
                dt.Rows.Add(2, "开始接警");
                dt.Rows.Add(3, "接警结束");
                dt.Rows.Add(4, "开始接警");
                dt.Rows.Add(5, "开始接警");
                dt.Rows.Add(6, "开始接警");
                dt.Rows.Add(7, "闲置");
                dt.Rows.Add(8, "闲置");
                dt.Rows.Add(9, "接警结束");
                dt.Rows.Add(10, "接警结束");
                dt.Rows.Add(11, "接警结束");
                dt.Rows.Add(12, "接警结束");
                dt.Rows.Add(13, "接警结束");
                dt.Rows.Add(14, "接警结束");
                dt.Rows.Add(15, "接警结束");
                dt.Rows.Add(16, "闲置");
                dt.Rows.Add(17, "开始接警");
                dt.Rows.Add(18, "接警结束");
                dt.Rows.Add(19, "接警结束");
                dt.Rows.Add(20, "接警结束");
                dt.Rows.Add(21, "接警结束");
                dt.Rows.Add(22, "接警结束");
                dt.Rows.Add(23, "接警结束");
                dt.Rows.Add(24, "接警结束");
                dt.Rows.Add(25, "接警结束");
                dt.Rows.Add(26, "接警结束");
                dt.Rows.Add(27, "接警结束");
                dt.Rows.Add(28, "接警结束");
                dt.Rows.Add(29, "接警结束");
                dt.Rows.Add(30, "接警结束");

                m_InputDataTable = dt;
                SystemHelper.logger.LogDebug("HRVPoliceCasePrecentControl_ZZGA ==============InitInputDataTable.end============= ");

            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
            }

        }

        private void InputDataTable2Chart(DataTable dataTable)
        {
            try
            {
                SystemHelper.logger.LogDebug("HRVRegionControl_ZZGA ==============InputDataTableToChart.start============= ");
                // 判空
                if ((dataTable == null) || (dataTable.Rows.Count == 0))
                {
                    SystemHelper.logger.LogDebug("InputDataTableToChart  DataTable 数据为空");
                    return;
                }

                Dictionary<int, int> allSeatsDict = new Dictionary<int, int>
                {
                    {1,0 },{2,0 },{3,0 },{4,0 },{5,0 },{6,0 },{7,0 },{8,0 },{9,0 },{10,0 },
                    {11,0 },{12,0 },{13,0 },{14,0 },{15,0 },{16,0 },{17,0 },{18,0 },{19,0 },{20,0 },
                    {21,0 },{22,0 },{23,0 },{24,0 },{25,0 },{26,0 },{27,0 },{28,0 },{29,0 },{30,0 }
                };

                Dictionary<int, string> statusImgDict = new Dictionary<int, string>
                {
                    { 1, "busy-01.png" },
                    { 2, "leave unused-01.png" },
                    { 3, "the afterwards treatment-01.png" }
                };

                Dictionary<string, int> seatStatusDict = new Dictionary<string, int>
                {
                    { "开始接警", 1 }, { "闲置", 2 }, { "接警结束", 3 }
                };

                dataTable.Columns[0].ColumnName = "seat_no";
                dataTable.Columns[1].ColumnName = "seat_state";

                int seatNo = 1;     // 座席号
                int status = seatStatusDict["闲置"];     // 坐席状态,默认：2-闲置
                int busyCount = 0;
                int unusedCount = 0;
                int untreatmentCount = 0;

                string imgdir = @"D:\HRV_2.0\APPS\Files_Resource\ZZGA\image\Slicing\";

                for (int i = 0; i < 30; i++)
                {
                    seatNo = i + 1;
                    Image img = FindName("seat_state_" + seatNo) as Image;
                    img.Source = new BitmapImage(new Uri(imgdir + statusImgDict[status]));
                    allSeatsDict[seatNo] = status;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    seatNo = Convert.ToInt32(dataTable.Rows[i]["seat_no"]);
                    status = seatStatusDict[dataTable.Rows[i]["seat_state"].ToString().Trim()];
                    // 坐席号范围不在1-30，跳过
                    if ((seatNo < 1) || (seatNo > 30)) continue;

                    // 座席号的状态
                    if ((status < 1) || (status > 3)) status = 2;

                    allSeatsDict[seatNo] = status;

                    Image img = FindName("seat_state_" + seatNo) as Image;
                    img.Source = new BitmapImage(new Uri(imgdir + statusImgDict[status]));
                    
                }

                foreach (var st in allSeatsDict.Values)
                {
                    switch (st)
                    {
                        case 1:
                            busyCount++;
                            break;
                        case 2:
                            unusedCount++;
                            break;
                        case 3:
                            untreatmentCount++;
                            break;
                    }
                }

                busyTxt.Text = busyCount.ToString();
                unusedTxt.Text = unusedCount.ToString();
                untreatmentTxt.Text = untreatmentCount.ToString();

            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
            }

        }


        #region 依赖属性

        /// <summary>
        /// 注册属性，在控件序列化之前使用会发生序列化异常
        /// </summary>
        public DataTable InputDataTable
        {
            get { return (DataTable)GetValue(InputDataTableProperty); }
            set { SetValue(InputDataTableProperty, value); }
        }
        /// <summary>
        /// 私有属性的 DataTable 对象
        /// </summary>
        private DataTable m_InputDataTable = new DataTable();

        /// <summary>
        /// datatable
        /// </summary>
        public static readonly DependencyProperty InputDataTableProperty = DependencyProperty.Register(
            "InputDataTable", typeof(DataTable), typeof(HRVPoliceSeatsControl_ZZGA), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnInputDataTableChange)));


        /// <summary>
        /// 改变输入的datatable
        /// </summary> 
        private static void OnInputDataTableChange(DependencyObject o, DependencyPropertyChangedEventArgs ea)
        {
            try
            {
                //绑定数据
                HRVPoliceSeatsControl_ZZGA gridControl = o as HRVPoliceSeatsControl_ZZGA;
                gridControl.ReRenderData();

            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
            }

        }
        /// <summary>
        /// 重绘数据图形界面
        /// </summary>
        void ReRenderData()
        {
            try
            {
                //调用绘制画面方法
                if (InputDataTable != null)
                {
                    SystemHelper.logger.LogDebug("调试信息=====> ReRenderData <=====");

                    InputDataTable2Chart(InputDataTable);
                }
                else
                {
                    SystemHelper.logger.LogDebug("调试信息=====> " + "ReRenderData 处理中。。。。。。");
                }

            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
            }
        }

        #endregion 


        #region 标题修改 序列化控制
        /// <summary>
        /// 主标题
        /// </summary>
        private Label m_lb_ControlInstanceMainTitle;
        /// <summary>
        /// 副标题
        /// </summary>
        private Label m_lb_ControlInstanceSubTitle;
        /// <summary>
        /// 修改组件框中的标题
        /// </summary>
        public override void OnApplyTemplate()
        {
            try
            {
                base.OnApplyTemplate();
                m_lb_ControlInstanceMainTitle = GetTemplateChild("lb_ControlInstanceMainTitle") as Label;
                if (m_lb_ControlInstanceMainTitle != null)
                {
                    m_lb_ControlInstanceMainTitle.Content = m_ControlInstanceMainTitle;
                }
                m_lb_ControlInstanceSubTitle = GetTemplateChild("lb_ControlInstanceSubTitle") as Label;
                if (m_lb_ControlInstanceSubTitle != null)
                {
                    m_lb_ControlInstanceSubTitle.Content = m_ControlInstanceSubTitle;
                }
            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
              System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
            }

        }


        /// <summary>
        /// 重要 是否序列化内容  
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSerializeContent()
        {
            return false;
        }
        #endregion


        #region 接口IVisualControl成员
        /// <summary>
        /// 组件背景窗口名称
        /// </summary>
        private string m_WindowStyleName;
        /// <summary>
        /// 组件背景窗口名称
        /// </summary>
        public string WindowStyleName
        {
            get { return m_WindowStyleName; }
            set { m_WindowStyleName = value; StyleManager.SetStyle(this, m_WindowStyleName); }
        }

        /// <summary>
        /// 加载组件中的显示内容 加载成功 isContentLoaded = true;
        /// </summary>
        /// <returns>加载完成并无错误返回true,有错误返回false</returns>
        public bool LoadContent()
        {
            try
            {
                if (CurrentRunderUnitEnvironment.Mode == EnvironmentMode.Designer)
                {
                    InitInputDataTable();
                    // 绘制自定义数据
                    InputDataTable2Chart(m_InputDataTable);
                }

                // 渲染成功后
                isContentLoaded = true;
                return true;

            }
            catch (Exception ex)
            {
                SystemHelper.logger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                              System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ex.ToString());
                isContentLoaded = false;
                return false;
            }
        }


        /// <summary>
        /// 标识此组件的内容是否已加载，即LoadContent()方法已成功调用
        /// </summary>
        private bool m_isContentLoaded = false;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool isContentLoaded
        {
            get { return m_isContentLoaded; }
            private set { m_isContentLoaded = value; }
        }


        /// <summary>
        /// 加载顺序标识，数字越大越后加载，建议默认值为0，
        /// *此属性应注册为可在设计端修改的属性
        /// </summary>
        private int m_renderOrder = 0;
        /// <summary>
        /// 加载顺序
        /// </summary>
        public int RenderOrder
        {
            get { return m_renderOrder; }
            set { m_renderOrder = value; }
        }


        /// <summary>
        /// 本可视化组件类型的唯一标识
        /// 需要修改 guid
        /// </summary>
        public string ControlGUID
        {
            get { return "4615D155-3FF6-4B10-8098-B607C0F1AF8A"; }
        }


        /// <summary>
        /// 本可视化组件类型名称
        /// 需要修改 组件名称
        /// </summary>
        public string ControlName
        {
            get { return "1.接警席位状态"; }
        }


        /// <summary>
        /// 实例名称即窗口默认主标题
        /// *此属性应注册为可在设计端修改的属性
        /// 此属性默认值建议与ControlName相同
        /// 在设计过程中允许设计人员给此实例命名，便于人工查找区分
        /// 在可视化组件选取列表中将展示此名称
        /// 如：组件名称为：表格组件，实例名称为：2015年度北京市人口统计表
        /// 提示：组件名称可由组件开发者动态生成，如上个例子中，可将dataTable/dataView的名称作为此项为空时的默认名称
        /// 需要修改 默认主标题名称
        /// </summary>
        private string m_ControlInstanceMainTitle = "默认主标题名称";
        /// <summary>
        /// 窗口默认主标题
        /// </summary>
        public string ControlInstanceMainTitle
        {
            get { return m_ControlInstanceMainTitle; }
            set
            {
                m_ControlInstanceMainTitle = value;
                if (m_lb_ControlInstanceMainTitle != null)
                {
                    m_lb_ControlInstanceMainTitle.Content = m_ControlInstanceMainTitle;
                }
            }
        }
        /// <summary>
        /// 实例名称即窗口默认副标题
        /// *此属性应注册为可在设计端修改的属性
        /// 在设计过程中允许设计人员给此实例命名，便于人工查找区分
        /// 在可视化组件选取列表中将展示此名称
        /// 如：组件名称为：表格组件，实例名称为：2015年度北京市人口统计表
        /// 提示：组件名称可由组件开发者动态生成，如上个例子中，可将dataTable/dataView的名称作为此项为空时的默认名称
        /// 需要修改 默认副标题名称
        /// </summary>
        private string m_ControlInstanceSubTitle = "默认副标题名称";
        /// <summary>
        /// 窗口默认副标题
        /// </summary>
        public string ControlInstanceSubTitle
        {
            get { return m_ControlInstanceSubTitle; }
            set
            {
                m_ControlInstanceSubTitle = value;
                if (m_lb_ControlInstanceSubTitle != null)
                {
                    m_lb_ControlInstanceSubTitle.Content = m_ControlInstanceSubTitle;
                }
            }
        }

        /// <summary>
        /// 组件实例描述信息
        /// 用于设计人员描述组件所呈现的相关业务信息
        /// 通过此信息，交互控制人员可分辨组件内显示的业务内容
        /// 需要修改 组件示例
        /// </summary>
        private string m_ControlInstanceDes = "组件示例";
        /// <summary>
        /// 组件描述信息
        /// </summary>
        public string ControlInstanceDes
        {
            get { return m_ControlInstanceDes; }
            set { m_ControlInstanceDes = value; }
        }



        /// <summary>
        /// 可视化组件的描述信息
        /// 包括呈现说明，应用描述
        /// 需要修改  组件描述
        /// </summary>
        public string ControlDescription
        {
            get { return "组件描述"; }
        }


        /// <summary>
        /// 可视化组件配置应用说明
        /// 此属性在页面设计人员引用此组件时提示时使用，告诉设计人员如何配置数据源之用
        /// 必须配置哪几项，属性间相互关系说明等
        /// 需要修改  配置说明
        /// </summary>
        public string ControlUseDescription
        {
            get { return "配置说明"; }
        }

        /// <summary>
        /// 可视化组件版本信息
        /// 需要修改  组件版本
        /// </summary>
        public Version Version { get { return new Version("1.0.0.1"); } }


        /// <summary>
        /// 依赖的可视化开发、运行环境版本号
        /// 开发时使用的开发、运行环境版本号        
        /// </summary>
        public Version HRVRuntimeVersion
        {
            get { return new Version("1.1.0.0"); }
        }


        /// <summary>
        /// 可视化组件所有权公司
        /// </summary>
        public string CompanyName
        {
            get { return "北京威创视讯信息系统有限公司"; }
        }



        /// <summary>
        /// 依赖动态库列表属性（高分平台dll,.net framework系统dll不用描述）
        /// 要求格式：版本号=动态库名称
        /// 样例    ：1.0.0.1=AForge.Imaging.Formats.dll
        /// </summary>
        public string[] DependentDllList
        {
            get
            {
                return new string[] { "" };
            }
        }


        /// <summary>
        /// 依赖文件资源列表属性
        /// 要求资源放置位置为启动程序(可视化组件dll)目录为起点的
        /// ../Files_Resource/xxxx.dll/文件夹内，xxxx为本可视化组件所在的dll名称
        /// 资源文件夹内可创建子文件夹
        /// </summary>
        public string[] DependentResourceFileList
        {
            get { return null; }
        }

        /// <summary>
        /// 依赖字体名称列表
        /// </summary>
        public string[] DependentFontList
        {
            get
            {
                string[] str = new string[1];
                str[0] = "宋体";
                return str;
            }
        }


        /// <summary>
        /// 获取复合数据源样例数据
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public System.Data.DataTable getDataTableSample(string propertyName)
        {
            return null;
        }


        /// <summary>
        /// 通用控制触发窗口调用方法
        /// 通过此方法，可给最终用户提供对本组件的细节控制交互界面 预留
        /// 注意：如果需要与大屏幕上的渲染形成互动，则配置窗口改变的属性应都为同步属性
        /// </summary>
        public void ShowConfigurationWindow()
        {

        }

        /// <summary>
        /// 设置网络授权的ip地址
        /// </summary>
        /// <param name="ip">网络授权的IP地址</param>
        bool IVisualControl.setNetLicenseIP(string ip)
        {
            return false;
        }

        /// <summary>
        /// 独立线程、进程加载及运行标识属性
        ///  *此属性应注册为可在设计端修改的属性
        /// 一般组件请设置InMainProcess为默认值
        /// StandaloneThread：独立线程运行。StandaloneProcess：独立进程运行
        /// </summary>
        EnumRenderMethodType m_RenderMethod = EnumRenderMethodType.InMainProcess;
        /// <summary>
        /// 渲染模式
        /// </summary>
        public EnumRenderMethodType RenderMethod
        {
            get
            {
                return m_RenderMethod;
            }
            set
            {
                m_RenderMethod = value;
            }
        }

        /// <summary>
        /// 内存要求
        /// </summary>
        int m_MemoryRequest = 0;
        /// <summary>
        /// 内存需求
        /// </summary>
        public int MemoryRequest
        {
            get
            {
                return m_MemoryRequest;
            }
            set
            {
                m_MemoryRequest = value;
            }
        }

        /// <summary>
        /// 标识此可视化组件的实例化限制方式
        /// 大型可视化组件，如：3D，GIS等作为所有页面的公用底图使用的组件，可考虑多页面间共用一个实例。设置为：OnlyOneInstance
        /// 普通组件可创建多个实例。设置为：MultiInstance
        /// </summary>
        public EnumInstanceType InstanceType
        {
            get { return EnumInstanceType.MultiInstance; }
        }

        /// <summary>
        /// 标识此可视化组件可支持的windows操作系统版本情况
        /// 系统默认按照x86模式进行渲染，如果组件仅支持x64操作系统，则必须在设计时标识使用独立进程渲染
        /// 仅当此可视化组件被设置为独立进程渲染，同时此项标识为x64的情况下
        /// 渲染引擎可考虑采用x64模式渲染，以增加内存的支持和x64特性的保障。
        /// </summary>
        public EnumOperateSystemSupportType OperateSystemSupportType
        {
            get { return EnumOperateSystemSupportType.X86andX64; }
        }

        /// <summary>
        /// 可视化组件释放资源接口
        /// </summary>
        public void DisposeVisualControl()
        {

        }

        ///此组件在显示时是否可以任意移动、改变大小、角度改变
        ///避免有些组件被错误的移动到其他地方，比如标题，底图，背景图片等
        /// *此属性应注册为可在设计端修改的属性
        /// 默认为可移动,true
        private bool m_IsEditableInOperater = true;
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool IsEditableInOperater
        {
            get { return m_IsEditableInOperater; }
            set { m_IsEditableInOperater = value; }
        }

        /// <summary>
        /// 是否可以跨屏，跨渲染节点显示
        ///  *此属性应注册为可在设计端修改的属性
        /// 默认为false
        /// 此属性为处理那些高分化改造不充分或无法改造的组件而设计
        /// </summary>
        private bool m_IsMustInOnlyOneMachine = false;
        /// <summary>
        /// 是否可以跨屏
        /// </summary>
        public bool IsMustInOnlyOneMachine
        {
            get { return m_IsMustInOnlyOneMachine; }
            private set { m_IsMustInOnlyOneMachine = value; }
        }

        #endregion


        #region IPageEvent接口
        /// <summary>
        /// 整个页面隐藏之后的通知方法
        /// </summary>
        public void AfterPageHide()
        {

        }
        /// <summary>
        /// 整个页面初始化之后的通知方法，区分于组件loaded，
        /// 此方法为所有组件均loaded完成之后触发
        /// </summary>
        public void AfterPageInit()
        {

        }
        /// <summary>
        /// 整个页面显示到大屏之后的通知
        /// </summary>
        public void AfterPageShow()
        {

        }

        /// <summary>
        /// 整个页面隐藏显示之前的通知方法
        /// </summary>
        public void BeforePageHide()
        {

        }

        /// <summary>
        /// 整个页面显示之前的通知方法
        /// </summary>
        public void BeforePageShow()
        {

        }

        /// <summary>
        /// 整个页面释放之前的通知方法，要求组件停止timer，处理逻辑，保存状态等
        /// </summary>
        public void BeforePageDispose()
        {

        }
        #endregion
    }
}
