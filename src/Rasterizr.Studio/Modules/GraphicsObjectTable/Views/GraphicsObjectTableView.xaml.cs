using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.Views
{
    public partial class GraphicsObjectTableView : UserControl
    {
        public GraphicsObjectTableView()
        {
            InitializeComponent();
        }

        private void OnDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                var row = (DataGridRow) grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
                var vm = (GraphicsObjectViewModel) row.DataContext;
                IoC.Get<IShell>().OpenDocument(new GraphicsObjectDocumentViewModel(vm));
            }
        }
    }
}
