using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SensorTagMvvm.Services
{
    public class MvxSwipeRefreshLayout : SwipeRefreshLayout
    {
        private ICommand _refresh;

        protected MvxSwipeRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer)
    : base(javaReference, transfer) { }
        public MvxSwipeRefreshLayout(Context p0)
    : this(p0, null) { }

        public MvxSwipeRefreshLayout(Context p0, IAttributeSet p1)
    : base(p0, p1) { }

        public new bool Refreshing
        {
            get { return base.Refreshing; }
            set { base.Refreshing = value; }
        }

        public new ICommand Refresh
        {
            get { return _refresh; }
            set
            {
                _refresh = value;
                if (_refresh != null)
                    EnsureOnRefreshOverloaded();
            }
        }

        private bool _refreshOverloaded;
        private void EnsureOnRefreshOverloaded()
        {
            if (_refreshOverloaded)
                return;

            _refreshOverloaded = true;
            base.Refresh += (sender, args) => ExecuteCommandOnRefresh(Refresh);
        }

        protected virtual void ExecuteCommandOnRefresh(ICommand command)
        {
            if (command == null)
                return;

            if (!command.CanExecute(null))
                return;

            command.Execute(null);
        }
    }
}