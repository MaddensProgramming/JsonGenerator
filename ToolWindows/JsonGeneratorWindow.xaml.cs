using Community.VisualStudio.Toolkit;
using EnvDTE;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NewJsonGenerator
{
    public partial class JsonGeneratorWindow : UserControl
    {

        public static Assembly LoadedAssembly { get; set; }

        public static Type[] TypeList { get; set; }
        public static Type[] FilteredTypelist { get; set; }
        public static SelectedItem Item { get; set; }



        public JsonGeneratorWindow()
        {
            InitializeComponent();      
            VS.Windows.FindWindowAsync(Guid.Parse("7bdd0c95-ebd9-4e92-a6b8-a0dea12557b6")).Result.OnShow+=Show;
        }

        private void Show(object sender, WindowFrameShowEventArgs e)
        {
            ConfigureInputList();
        }

        private void UpdateButton(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            GenerateFileButton.IsEnabled = TypeGrid.SelectedItem != null;

            if (Item.ProjectItem != null && TypeGrid.SelectedItem != null)
                SavingPath.Text = $"{Path.GetDirectoryName(Item.ProjectItem.Properties.Item("FullPath").Value.ToString())}\\{((Type)TypeGrid.SelectedItem).Name}-schema.json";

        }


        private void GenerateFile(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private async Task SaveFile()
        {
            try
            {

                var schema = NJsonSchema.JsonSchema.FromType((Type)TypeGrid.SelectedItem).ToJson();


                System.IO.File.WriteAllText(SavingPath.Text, schema);
                Item.ProjectItem.ProjectItems.AddFromFile(SavingPath.Text);


                await VS.Windows.GetCurrentWindowAsync().Result.CloseFrameAsync(FrameCloseOption.NoSave);

            }
            catch (Exception ex)
            {
                VS.MessageBox.Show(ex.Message);
            }
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            ConfigureInputList();
        }

        private void ConfigureInputList()
        {
            TypeGrid.ItemsSource = null;
            if (ShowAll.IsChecked.Value)
                TypeGrid.ItemsSource = TypeList;
            else TypeGrid.ItemsSource = FilteredTypelist ?? TypeList;
        }
    }
}