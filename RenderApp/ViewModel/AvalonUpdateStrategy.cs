using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
namespace RenderApp.ViewModel
{
    class AvalonUpdateStrategy : ILayoutUpdateStrategy
    {
        private bool IsInitialize = false;
        private LayoutPanel RightPanel;
        private LayoutPanel LeftPanel;
        private LayoutAnchorablePane LeftUpPanel;
        private LayoutAnchorablePane LeftDownPanel;
        private LayoutAnchorablePane RightUpPanel;
        private LayoutAnchorablePane RightDownPanel;
        private LayoutDocumentPane ViewportPanel;
        private void Initialize(LayoutRoot layout)
        {
            if (!IsInitialize)
            {
                LeftUpPanel = new LayoutAnchorablePane();
                LeftDownPanel = new LayoutAnchorablePane();
                RightUpPanel = new LayoutAnchorablePane();
                RightDownPanel = new LayoutAnchorablePane();
                ViewportPanel = new LayoutDocumentPane(); 
                layout.RootPanel.Children.Clear();
                layout.RootPanel.Orientation = Orientation.Horizontal;

                LeftPanel = new LayoutPanel();
                LeftPanel.DockWidth = new GridLength(300);
                LeftPanel.Orientation = Orientation.Vertical;

                RightPanel = new LayoutPanel();
                RightPanel.DockWidth = new GridLength(300);
                RightPanel.Orientation = Orientation.Vertical;
                

                layout.RootPanel.Children.Add(LeftPanel);
                layout.RootPanel.Children.Add(ViewportPanel);
                layout.RootPanel.Children.Add(RightPanel);
                LeftPanel.Children.Add(LeftUpPanel);
                LeftPanel.Children.Add(LeftDownPanel);
                RightPanel.Children.Add(RightUpPanel);
                RightPanel.Children.Add(RightDownPanel);
                IsInitialize = true;
            }
        }
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            

            
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument documentShown)
        {


        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            Initialize(layout);

            var NewPane = new LayoutAnchorablePane(anchorableToShow);
            NewPane.DockWidth = new GridLength(300);
            if (anchorableToShow.Content is AvalonWindowViewModel)
            {
                AvalonWindowViewModel newWindow = anchorableToShow.Content as AvalonWindowViewModel;
                switch (newWindow.WindowPosition)
                {
                    case AvalonWindowViewModel.AvalonWindow.LeftUp:
                        LeftPanel.ReplaceChild(LeftUpPanel, NewPane);
                        LeftUpPanel = NewPane;

                        break;
                    case AvalonWindowViewModel.AvalonWindow.LeftDown:
                        LeftPanel.ReplaceChild(LeftDownPanel, NewPane);
                        LeftDownPanel = NewPane;
                        break;
                    case AvalonWindowViewModel.AvalonWindow.RightUp:
                        RightPanel.ReplaceChild(RightUpPanel, NewPane);
                        RightUpPanel = NewPane;
                        break;
                    case AvalonWindowViewModel.AvalonWindow.RightDown:
                        RightPanel.ReplaceChild(RightDownPanel, NewPane);
                        RightDownPanel = NewPane;
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument documentToShow, ILayoutContainer destinationContainer)
        {
            Initialize(layout);

            var NewPane = new LayoutDocumentPane(documentToShow);
            layout.RootPanel.ReplaceChild(ViewportPanel, NewPane);
            ViewportPanel = NewPane;

            return true;
        }
    }
}
