using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;

namespace Jack.Mvvm.Droid
{
    [ContentProperty("Value")]
    public class AnyExtension : MarkupExtension, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static readonly PropertyChangedEventArgs EventArgs = new PropertyChangedEventArgs("Value");
        
        public bool IsBindingSource { get; set; }
        
        private object any;
        public object Value
        {
            get => any;
            set
            {
                any = value;
                PropertyChanged?.Invoke(this, EventArgs);
            }
        }
        public byte ByteValue
        {
            get => (byte)Value;
            set => Value = value;
        }
        public sbyte SByteValue
        {
            get => (sbyte)Value;
            set => Value = value;
        }
        public short ShortValue
        {
            get => (short)Value;
            set => Value = value;
        }
        public int IntValue
        {
            get => (int)Value;
            set => Value = value;
        }
        public long LongValue
        {
            get => (long)Value;
            set => Value = value;
        }
        public bool BoolValue
        {
            get => (bool)Value;
            set => Value = value;
        }
        public float FloatValue
        {
            get => (float)Value;
            set => Value = value;
        }
        public double DoubleValue
        {
            get => (double)Value;
            set => Value = value;
        }
        public decimal DecimalValue
        {
            get => (decimal)Value;
            set => Value = value;
        }
        public string StringValue
        {
            get => (string)Value;
            set => Value = value;
        }
        public char CharValue
        {
            get => (char)Value;
            set => Value = value;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return IsBindingSource ? this : Value;
        }

        public static T GetValue<T>(FrameworkElement target, [CallerMemberName]string name = null)
        {
            if (target.TryFindResource(name) is AnyExtension any)
            {
                return (T)any.Value;
            }
            return default(T);
        }
        public static void SetValue<T>(FrameworkElement target, T value, [CallerMemberName]string name = null)
        {
            if (target.TryFindResource(name) is AnyExtension any)
            {
                any.Value = value;
            }
        }

        public static AnyExtension Find(FrameworkElement target, string name)
        {
            return target.TryFindResource(name) as AnyExtension;
        }
    }
}
