using System;
using System.Windows;
using System.Windows.Interactivity;

namespace KI.UI.Control
{
    public class DropAcceptDescription
    {
        public event Action<DragEventArgs> DragOver;

        public void OnDragOver(DragEventArgs dragEventArgs)
        {
            DragOver?.Invoke(dragEventArgs);
        }

        public event Action<DragEventArgs> DragDrop;

        public void OnDrop(DragEventArgs dragEventArgs)
        {
            DragDrop?.Invoke(dragEventArgs);
        }
    }

    public class DropBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(DropAcceptDescription), typeof(DropBehavior), new PropertyMetadata(null));

        public DropAcceptDescription Description
        {
            get { return (DropAcceptDescription)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;
            this.AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            this.AssociatedObject.PreviewDrop -= AssociatedObject_PreviewDrop;
            base.OnDetaching();
        }

        void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            var desc = Description;
            if (desc == null)
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
                return;
            }

            desc.OnDragOver(e);
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            var desc = Description;
            if (desc == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            desc.OnDrop(e);
            e.Handled = true;
        }
    }
}
