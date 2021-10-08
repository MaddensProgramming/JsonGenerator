using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NewJsonGenerator
{
    public class JsonGeneratorPane : BaseToolWindow<JsonGeneratorPane>
    {
        public override string GetTitle(int toolWindowId) => "Generate Json Schema";
          

        public override Type PaneType => typeof(Pane);

  
        public override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            EnvDTE80.DTE2 dte = await VS.GetServiceAsync<EnvDTE.DTE, EnvDTE80.DTE2>();
            return new JsonGeneratorWindow();
        }


        [Guid("7bdd0c95-ebd9-4e92-a6b8-a0dea12557b6")]
        internal class Pane : ToolWindowPane
        {
            
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}