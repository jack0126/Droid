using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Jack.Mvvm.Droid
{
    public class EnumExtension : MarkupExtension
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type == null || !Type.IsEnum)
            {
                return null;
            }
            var values = Enum.GetValues(Type);
            if (string.IsNullOrEmpty(Name))
            {
                return values;
            }

            foreach (var e in values)
            {
                if (Enum.GetName(Type, e) == Name)
                {
                    return e;
                }
            }
            return null;
        }

        public override string ToString()
        {
            if (ProvideValue(null) == null)
            {
                return "Error";
            }
            if (string.IsNullOrEmpty(Name))
            {
                return Type.Name;
            }
            return $"{Name}({Type.Name})";
        }
    }
}
