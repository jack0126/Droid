using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace Jack.Mvvm.Droid
{
    public class ActionExtension : MarkupExtension, ICommand
    {
        public event EventHandler CanExecuteChanged;
        private object targetElement;
        /// <summary>
        /// 动作方法名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ParameterType { get; set; }
        /// <summary>
        /// 动作源
        /// </summary>
        public object Source { get; set; }
        /// <summary>
        /// 动作元素
        /// </summary>
        public FrameworkElement Element { get; set; }
        /// <summary>
        /// 相对类型
        /// </summary>
        public Type AncestorType { get; set; }
        /// <summary>
        /// 相对类型元素
        /// </summary>
        public bool IsAncestorElement { get; set; }
        public ActionExtension()
        {
        }
        public ActionExtension(string name)
        {
            Name = name;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var cxt = SearchSource();
            MethodInfo method;
            if (cxt is Type)
            {
                if (ParameterType != null)
                {
                    method = (cxt as Type).GetMethod(Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { ParameterType }, null);
                }
                else
                {
                    method = (cxt as Type).GetMethod(Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
            }
            else
            {
                if (ParameterType != null)
                {
                    method = cxt?.GetType().GetMethod(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { ParameterType }, null);
                }
                else
                {
                    method = cxt?.GetType().GetMethod(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
            }

            if (method != null)
            {
                var ps = method.GetParameters();
                if (ps.Length == 0)
                {
                    method.Invoke(cxt, null);
                }
                else if (ps.Length == 1 && ps[0] != null && ps[0].ParameterType.IsInstanceOfType(parameter))
                {
                    method.Invoke(cxt, new object[] { parameter });
                }
            }
        }
        /// <summary>
        /// 查找动作源
        /// </summary>
        /// <returns></returns>
        private object SearchSource()
        {
            if (Source != null)
            {
                return Source;
            }
            
            if (Element != null)
            {
                var el = Element;
                while (el !=  null)
                {
                    if (el.DataContext != null)
                    {
                        return el.DataContext;
                    }

                    el = el.Parent as FrameworkElement;
                }
                return null;
            }

            if (targetElement != null &&
                (targetElement is FrameworkElement cur || ActionHelper.MatchTargetElement(targetElement, out cur)))
            {
                if (AncestorType != null)
                {
                    while (true)
                    {
                        if (AncestorType.IsInstanceOfType(cur))
                        {
                            if (IsAncestorElement)
                            {
                                return cur;
                            }
                            break;
                        }
                        else
                        {
                            cur = cur.Parent as FrameworkElement;
                            if (cur == null)
                            {
                                return null;
                            }
                        }
                    }
                }

                while (cur != null)
                {
                    if (cur.DataContext != null)
                    {
                        return cur.DataContext;
                    }

                    cur = cur.Parent as FrameworkElement;
                }
            }
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return null;
            }

            if (serviceProvider?.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
            {
                targetElement = provideValueTarget.TargetObject;
            }

            return this;
        }

    }

    static class ActionHelper
    {
        private static readonly MethodInfo InheritanceContextGetter = typeof(InputBinding).GetProperty("InheritanceContext", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true);

        private static Type IAttachedObjectType;
        private static MethodInfo AssociatedObjectGetter;
        internal static bool MatchTargetElement(object element, out FrameworkElement frameworkElement)
        {
            //InputBinding 匹配
            if (element is InputBinding)
            {
                frameworkElement = InheritanceContextGetter.Invoke(element, null) as FrameworkElement;
                return frameworkElement != null;
            }

            if (IAttachedObjectType == null)
            {
                var doType = element.GetType().GetInterface("System.Windows.Interactivity.IAttachedObject");
                if (doType != null)
                {
                    var prop = doType.GetProperty("AssociatedObject", BindingFlags.Instance | BindingFlags.Public);
                    if (prop != null)
                    {
                        IAttachedObjectType = doType;
                        AssociatedObjectGetter = prop.GetGetMethod();
                    }
                }
            }

            //System.Windows.Interactivity.WPF 匹配
            if (IAttachedObjectType?.IsInstanceOfType(element)??false)
            {
                frameworkElement = AssociatedObjectGetter.Invoke(element, null) as FrameworkElement;
                return frameworkElement != null;
            }
            frameworkElement = null;
            return false;
        }

    }

}
