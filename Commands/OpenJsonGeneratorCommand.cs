using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using Task = System.Threading.Tasks.Task;

namespace NewJsonGenerator
{
    [Command(PackageIds.OpenJsonGeneratorCommand)]
    internal sealed class OpenJsonGeneratorCommand : BaseCommand<OpenJsonGeneratorCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)

        {
            try
            {
                EnvDTE80.DTE2 dte = VS.GetServiceAsync<EnvDTE.DTE, EnvDTE80.DTE2>().Result;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                var item = dte.SelectedItems.Item(1);
                openFileDialog.Filter = "Assembly (*.dll)|*.dll|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Path.GetDirectoryName(item.ProjectItem.ContainingProject.FullName);

                if (openFileDialog.ShowDialog() == true)
                {

                    JsonGeneratorWindow.LoadedAssembly = Assembly.LoadFile(openFileDialog.FileName);
                    JsonGeneratorWindow.Item = item;
                    JsonGeneratorWindow.TypeList = JsonGeneratorWindow.LoadedAssembly.GetTypes();
                    var result = Array.FindAll(JsonGeneratorWindow.TypeList, type => type.Name == Path.GetFileNameWithoutExtension(JsonGeneratorWindow.Item.Name));
                    if (result?.Length > 0)
                    {
                        JsonGeneratorWindow.FilteredTypelist = result;
                    }
                    else JsonGeneratorWindow.FilteredTypelist = null;                   

                }
                else
                    return Task.CompletedTask;            


                return JsonGeneratorPane.ShowAsync();


            }
            catch (Exception ex)
            {
                VS.MessageBox.Show(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
