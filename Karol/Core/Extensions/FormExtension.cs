using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Karol.Core.Extensions
{
    internal static class FormExtension
    {
        public static void InvokeFormMethod(this Form form, Action action)
        {
            if (form == null)
                return;

            if (!form.IsHandleCreated)
                return;

            if (form.IsDisposed)
                return;

            try
            {
                form.Invoke(action);
            }
            catch (Exception) { }
        }
    }
}
