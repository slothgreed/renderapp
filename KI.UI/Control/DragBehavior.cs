using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KI.UI.Control
{
    public class DragBehavior : Behavior<FrameworkElement>
    {
        private Point MousePos;
        private bool isMouseDown;

        public static readonly DependencyProperty DragDataProperty = DependencyProperty.Register("DragData", typeof(object), typeof(DragBehavior), new PropertyMetadata(null));

        public object DragData
        {
            get { return GetValue(DragDataProperty); }
            set { SetValue(DragDataProperty, value); }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            this.AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            this.AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            this.AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            this.AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
            base.OnDetaching();
        }

        void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MousePos = e.GetPosition(this.AssociatedObject);
            isMouseDown = true;
        }

        void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || !isMouseDown)
            {
                return;
            }

            var point = e.GetPosition(this.AssociatedObject);

            if (Distance(point, MousePos) > 5.0f)
            {
                DragDrop.DoDragDrop(this.AssociatedObject, this.DragData, DragDropEffects.All);
                isMouseDown = false;
                e.Handled = true;
            }
        }

        void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        public float Distance(Point point1, Point point2)
        {
            double x = point1.X - point2.X;
            double y = point1.Y - point2.Y;
            return (float)Math.Sqrt(x * x + y * y);
        }
    }
}
