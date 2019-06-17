using Caliburn.Micro;
using CockpitBuilder.Result;
using CockpitBuilder.Views.Main.DockPanel;
using System;
using System.Collections.Generic;

namespace CockpitBuilder.Common.Strategies
{
    public class ProfileDialogStrategy
    {
        private readonly IResultFactory resultFactory;
        private const string fileFilter = "Profile CockpitBuilders (*.ckb)|*.ckb|All files (*.*)|*.*";

        public ProfileDialogStrategy(IResultFactory resultFactory)
        {
            this.resultFactory = resultFactory;
        }

        public IEnumerable<IResult> SaveAs(PanelViewModel document, bool quickSave, Action<string> fileSelected)
        {
            if (quickSave && !string.IsNullOrEmpty(document.FilePath))
            {
                fileSelected(document.FilePath);
            }
            else
            {
                var result = resultFactory.ShowFileDialog("Save profile", fileFilter, FileDialogMode.Save, document.FilePath);
                yield return result;

                if (!string.IsNullOrEmpty(result.File))
                    fileSelected(result.File);
                else
                    yield return resultFactory.Cancel();
            }

        }

        public IEnumerable<IResult> Open(Action<string> fileSelected)
        {
            var result = resultFactory.ShowFileDialog("Open profile", fileFilter, FileDialogMode.Open);
            yield return result;

            if (!string.IsNullOrEmpty(result.File))
                fileSelected(result.File);
        }
    }
}
