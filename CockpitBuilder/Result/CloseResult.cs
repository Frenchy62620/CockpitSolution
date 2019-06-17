using System.Windows;
using Caliburn.Micro;

namespace CockpitBuilder.Result
{
    public class CloseResult : Result
    {
        public override void Execute(CoroutineExecutionContext context)
        {
            var window = Window.GetWindow(context.View as DependencyObject);
            window.Close();
            
            base.Execute(context);
        }
    }
}
