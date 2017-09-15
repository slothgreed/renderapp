using System.Windows;
using System.Windows.Controls;

namespace RenderApp.ViewModel
{
    class DockPaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AssetTemplate { get; set; }

        public DataTemplate ShaderProgramTemplate { get; set; }

        public DataTemplate TexureTemplate { get; set; }

        public DataTemplate ModelTemplate { get; set; }

        public DataTemplate ViewportTemplate { get; set; }

        public DataTemplate RenderObjectTemplate { get; set; }

        public DataTemplate ShaderTemplate { get; set; }

        public DataTemplate RenderTemplate { get; set; }

        public DataTemplate VoxelTemplate { get; set; }

        public DataTemplate SelectObjectTemplate { get; set; }

        public DataTemplate DijkstraTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RootNodeViewModel)
            {
                return AssetTemplate;
            }

            if (item is ShaderProgramViewModel)
            {
                return ShaderProgramTemplate;
            }

            if (item is TextureViewModel)
            {
                return TexureTemplate;
            }

            if (item is ViewportViewModel)
            {
                return ViewportTemplate;
            }

            if (item is ShaderViewModel)
            {
                return ShaderTemplate;
            }

            if (item is RenderObjectViewModel)
            {
                return RenderObjectTemplate;
            }

            if (item is RenderSystemViewModel)
            {
                return RenderTemplate;
            }

            if (item is VoxelViewModel)
            {
                return VoxelTemplate;
            }

            if (item is DijkstraViewModel)
            {
                return DijkstraTemplate;
            }

            if (item is SelectViewModel)
            {
                return SelectObjectTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
