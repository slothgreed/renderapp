using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace RenderApp.ViewModel
{

    class DockPaneLayoutUpdateStrategy : ILayoutUpdateStrategy
    {
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {

        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {

        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            var dockVM = anchorableToShow.Content as DockWindowViewModel;
            var panes = layout.Descendents().OfType<LayoutAnchorablePane>();
            LayoutAnchorablePane anchorablePane = null;
            switch (dockVM.InitPlace)
            {
                case DockWindowViewModel.Place.LeftUp:
                    anchorablePane = panes.First(p => p.Name == "LeftUpPane");
                    break;
                case DockWindowViewModel.Place.LeftDown:
                    anchorablePane = panes.First(p => p.Name == "LeftDownPane");
                    break;
                case DockWindowViewModel.Place.RightUp:
                    anchorablePane = panes.First(p => p.Name == "RightUpPane");
                    break;
                case DockWindowViewModel.Place.RightDown:
                    anchorablePane = panes.First(p => p.Name == "RightDownPane");
                    break;
                case DockWindowViewModel.Place.Floating:
                    if (layout.FloatingWindows.Count == 0)
                    {
                        layout.FloatingWindows.Add(new LayoutAnchorableFloatingWindow());
                    }
                    layout.LeftSide.Children.First().Children.Add(anchorableToShow);
                    break;
                default:
                    break;
            }

            if (anchorablePane == null)
            {
                return false;
            }
            else
            {
                anchorablePane.Children.Add(anchorableToShow);
            }

            return true;
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            var panes = layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();

            if (panes != null)
            {
                panes.Children.Add(anchorableToShow);
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    class DockPaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RootNodeTemplate { get; set; }
        public DataTemplate ShaderProgramTemplate { get; set; }
        public DataTemplate ViewportTemplate { get; set; }
        public DataTemplate RenderObjectTemplate { get; set; }
        public DataTemplate LightTemplate { get; set; }
        public DataTemplate MaterialTemplate { get; set; }
        public DataTemplate RendererTemplate { get; set; }
        public DataTemplate VoxelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RootNodeViewModel)
            {
                return RootNodeTemplate;
            }
            else if (item is ViewportViewModel)
            {
                return ViewportTemplate;
            }
            else if (item is RenderObjectViewModel)
            {
                return RenderObjectTemplate;
            }
            else if (item is RendererViewModel)
            {
                return RendererTemplate;
            }
            else if (item is LightViewModel)
            {
                return LightTemplate;
            }
            else if(item is MaterialViewModel)
            {
                return MaterialTemplate;
            }
            else if (item is VoxelViewModel)
            {
                return VoxelTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
