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
using WorkT.Assets;

namespace WorkT.Helpers
{
    public sealed class ResourceWrapper
    {
        private static ApplicationStrings _applicationStrings;
        /// <summary>
        /// Application localization strings
        /// </summary>
        public ApplicationStrings ApplicationStrings
        {
            get
            {
                if (_applicationStrings == null)
                {
                    _applicationStrings = new ApplicationStrings();
                }
                return _applicationStrings;
            }
        }
    }
}
