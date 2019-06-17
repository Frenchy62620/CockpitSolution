using System.Windows;
using Caliburn.Micro;

namespace CockpitBuilder.Result
{
    public class CloseAppResult : CancelResult
    {
        public override void Execute(CoroutineExecutionContext context)
        {
            Application.Current.Shutdown();
            base.Execute(context);
        }
    }
}
