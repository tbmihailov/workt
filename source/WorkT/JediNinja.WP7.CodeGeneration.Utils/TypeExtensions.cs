using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JediNinja.WP7.CodeGeneration.Utils
{
    public static class TypeExtensions
    {
        public static string GetTypeFriendlyName(this Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (!type.IsGenericType)
            {
                return type.Name;
            }

            int arityIndex = type.Name.IndexOf('`');
            string genericTypeName = type.Name.Substring(0, arityIndex);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(genericTypeName);
            sb.Append("<");
            bool isFirstArgument = true;
            foreach (var argument in type.GetGenericArguments())
            {
                if (!isFirstArgument)
                {
                    sb.Append(", ");
                }
                else
                {
                    isFirstArgument = false;
                }

                sb.Append(GetTypeFriendlyName(argument));
            }
            sb.Append(">");

            string typeName = sb.ToString();

            return typeName;
        }
    }
}
