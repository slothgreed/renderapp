using System.Windows.Controls;
using System.Windows;
namespace RenderApp.ViewModel
{
    class DockPaneStyleSelector : StyleSelector
    {
        public Style AnchorableStyle { get; set; }
        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is RootNodeViewModel)
            {
                return AnchorableStyle;
            }

            return AnchorableStyle;
        }
    }

    class DockPaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AssetTemplate { get; set; }

        public DataTemplate ShaderProgramTemplate { get; set; }

        public DataTemplate TexureTemplate { get; set; }

        public DataTemplate ModelTemplate { get; set; }

        public DataTemplate ViewportTemplate { get; set; }

        public DataTemplate GeometryTemplate { get; set; }

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

            if (item is GeometryViewModel)
            {
                return GeometryTemplate;
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
