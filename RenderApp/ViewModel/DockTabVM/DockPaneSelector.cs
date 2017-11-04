using System.Windows;
using System.Windows.Controls;

namespace RenderApp.ViewModel
{
    class DockPaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AssetTemplate { get; set; }

        public DataTemplate ShaderProgramTemplate { get; set; }

        public DataTemplate ModelTemplate { get; set; }

        public DataTemplate ViewportTemplate { get; set; }

        public DataTemplate RenderObjectTemplate { get; set; }

        public DataTemplate RenderTemplate { get; set; }

        public DataTemplate VoxelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RootNodeViewModel)
            {
                return AssetTemplate;
            }

            if (item is ViewportViewModel)
            {
                return ViewportTemplate;
            }

            if (item is RenderObjectViewModel)
            {
                return RenderObjectTemplate;
            }

            if (item is RendererViewModel)
            {
                return RenderTemplate;
            }

            if (item is VoxelViewModel)
            {
                return VoxelTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
