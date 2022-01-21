using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;

namespace Jack.Mvvm.Droid
{
    public abstract class Activity : MvvmSource
    {
        internal static ActivityViewGroup ActivityViewGroup;

        private UserControl _view;
        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 活动状态
        /// </summary>
        public ActivityState Status { get; private set; }
        /// <summary>
        /// 活动参数
        /// </summary>
        public Dictionary<string, object> Options { get; private set; }
        /// <summary>
        /// 父活动
        /// </summary>
        public Activity Context { get; private set; }
        /// <summary>
        /// 绑定的视图
        /// </summary>
        protected UserControl View => _view;
        /// <summary>
        /// 启动活动页面
        /// </summary>
        /// <typeparam name="A">新的活动页面类</typeparam>
        /// <param name="context">活动页面的上下文对象</param>
        /// <param name="name">活动页面的名字</param>
        /// <param name="options">活动页面的启动参数</param>
        public static void StartActivity<A>(Activity context, Dictionary<string, object> options = null, string name = null)
            where A : Activity, new()
        {
            if (ActivityViewGroup != null)
            {
                if (context != null &&
                    (context.Status == ActivityState.Creating || context.Status == ActivityState.Resume))
                {
                    context.DoPause();
                }

                Activity activity = new A
                {
                    Context = context,
                    Name = name,
                    Options = options
                };
                activity.DoCreate();
            }
        }
        public void StartActivity<A>(Dictionary<string, object> options = null, string name = null)
            where A : Activity, new()
        {
            StartActivity<A>(this, options, name);
        }
        internal static void Launcher(Type launchActivity)
        {
            if (ActivityViewGroup != null && launchActivity.IsSubclassOf(typeof(Activity)))
            {
                var a = (Activity)Activator.CreateInstance(launchActivity);
                a.DoCreate();
            }
        }
        /// <summary>
        /// 创建活动页面并返回页面的视图对象
        /// </summary>
        /// <returns>活动视图对象</returns>
        public abstract UserControl OnCreateView();
        /// <summary>
        /// 活动页面视图对象已创建时调用
        /// </summary>
        public virtual void OnViewCreated()
        {
        }
        /// <summary>
        /// 活动页面从不可见进入到可见状态时调用
        /// </summary>
        public virtual void OnResume()
        {
        }
        /// <summary>
        /// 活动页面从可见进入到不可见状态时调用
        /// </summary>
        public virtual void OnPause()
        {
        }
        /// <summary>
        /// 活动页面关闭时调用
        /// </summary>
        public virtual void OnDestroy()
        {
        }
        /// <summary>
        /// 从子活动页面接收返回值
        /// </summary>
        /// <param name="source">子页面名称</param>
        /// <param name="args">结果值</param>
        public virtual void OnResult(string source, params object[] args)
        {
        }
        private void DoCreate()
        {
            Status = ActivityState.Creating;
            _view = OnCreateView();
            _view.DataContext = this;
            ActivityViewGroup.RootView.Children.Add(_view);
            OnViewCreated();
            if (Status != ActivityState.Pause && Status != ActivityState.Destroyed)
            {
                DoResume();
            }
        }
        private void DoResume()
        {
            Status = ActivityState.Resume;
            OnResume();
        }
        private void DoPause()
        {
            Status = ActivityState.Pause;
            OnPause();
        }
        private void DoDestroy()
        {
            Status = ActivityState.Destroyed;
            OnDestroy();
        }
        /// <summary>
        /// 查找当前页面指定名字的控件
        /// </summary>
        /// <typeparam name="View">控件的类型</typeparam>
        /// <param name="name">控件的名字</param>
        /// <returns></returns>
        protected View FindViewByName<View>(string name)
        {
            return (View)_view.FindName(name);
        }
        /// <summary>
        /// 关闭当前活动页面并返回到启动当前页面的上下文页面
        /// </summary>
        /// <param name="args"></param>
        public void Finish(params object[] args)
        {
            if (Status != ActivityState.Destroyed)
            {
                if (Status == ActivityState.Creating || Status == ActivityState.Resume)
                {
                    DoPause();
                }
                DoDestroy();
                ActivityViewGroup.RootView.Children.Remove(_view);
                if (Context?.Status == ActivityState.Pause)
                {
                    Context.DoResume();
                    if (args != null && args.Length != 0)
                    {
                        Context.OnResult(Name, args);
                    }
                }
            }
        }
        /// <summary>
        /// 将回调代码放入到主页程调用
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="priority"></param>
        protected void RunOnUIThread(System.Action callback, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (ActivityViewGroup != null)
            {
                ActivityViewGroup.Dispatcher.Invoke(callback, priority);
            }
        }
        /// <summary>
        /// 显示工作中并使屏幕不可操作
        /// </summary>
        /// <param name="working"></param>
        /// <param name="message"></param>
        protected void ShowWorking(bool working, string message = "处理中...")
        {
            ActivityViewGroup.ShowWorking(working, message);
        }
    }
    public enum ActivityState
    {
        /// <summary>
        /// 新的活动页面
        /// </summary>
        Creating,
        /// <summary>
        /// 活动页面可见（正在前台）
        /// </summary>
        Resume,
        /// <summary>
        /// 活动页面不可见（进入后台）
        /// </summary>
        Pause,
        /// <summary>
        /// 活动页面已关闭
        /// </summary>
        Destroyed
    }
}
