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

namespace WorkT
{

    public class OperationResult<T> : OperationResult where T : class
    {
        public OperationResult()
        {

        }

        public OperationResult(T item)
        {
            this._item = item;
        }

        public OperationResult(T item, Exception error)
            : base(error)
        {
            this._item = item;
        }

        public OperationResult(Exception error)
            : base(error)
        {
        }

        private T _item;
        public T Item
        {
            get { return _item; }
        }
    }

    public class OperationResult
    {
        public OperationResult()
        {

        }

        public OperationResult(Exception error)
        {
            this.Error = error;
        }

        private bool _hasError = false;
        public bool HasError
        {
            get { return _hasError; }
        }

        private Exception _error;
        public Exception Error
        {
            get { return _error; }
            private set
            {
                _error = value;
                if (_error != null)
                {
                    _hasError = true;
                }
                else
                {
                    _hasError = false;
                }
            }
        }

    }
}
