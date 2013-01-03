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
    public static class StringExtensions
    {
        public static string LowerFirstLetter(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }

            string loweredFirstLetter = str[0].ToString().ToLower() + str.Substring(1);
            return loweredFirstLetter;
        }
    }
}
