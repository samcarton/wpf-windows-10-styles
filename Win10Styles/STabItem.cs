using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Win10Styles
{
    [TemplateVisualState(Name = "Pressed", GroupName = "PressedStates")]
    [TemplateVisualState(Name = "Unpressed", GroupName = "PressedStates")]
    [TemplatePart(Name = "PART_OuterBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedStoryboard", Type = typeof(Storyboard))]
    [TemplatePart(Name = "PART_UnselectedStoryboard", Type = typeof(Storyboard))]    
    public class STabItem : TabItem
    {
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register(
                "IsPressed",
                typeof(bool),
                typeof(STabItem),
                new PropertyMetadata(false));

        Border _outerBorder;
        Border _selectedBorder;
        Storyboard _selectedStoryboard;
        Storyboard _unselectedStoryboard;

        static STabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(STabItem), new FrameworkPropertyMetadata(typeof(STabItem)));
        }

        public STabItem()
        {            
            DependencyPropertyDescriptor
                .FromProperty(TabStripPlacementProperty, typeof(STabItem))
                .AddValueChanged(this,OnTabStribPlacementChanged);

            UpdateTabStripPlacementVisuals();
        }

        void OnTabStribPlacementChanged(object sender, EventArgs e)
        {
            UpdateTabStripPlacementVisuals();
        }

         void UpdateTabStripPlacementVisuals()
        {
            switch (TabStripPlacement)
            {
                case Dock.Left:
                    SetSelectedBorderThickness(new Thickness(4, 0, 0, 0));
                    SetOuterBorderThickness(new Thickness(0));
                    SetStoryboardTargetProperties(ScaleTransform.ScaleYProperty);
                    break;
                case Dock.Top:
                    SetSelectedBorderThickness(new Thickness(0, 4, 0, 0));
                    SetOuterBorderThickness(new Thickness(0, 0, 1, 0));
                    SetStoryboardTargetProperties(ScaleTransform.ScaleXProperty);
                    break;
                case Dock.Right:
                    SetSelectedBorderThickness(new Thickness(0, 0, 4,0));
                    SetOuterBorderThickness(new Thickness(0));
                    SetStoryboardTargetProperties(ScaleTransform.ScaleYProperty);
                    break;
                case Dock.Bottom:
                    SetSelectedBorderThickness(new Thickness(0, 0, 0, 4));
                    SetOuterBorderThickness(new Thickness(0, 0, 1, 0));
                    SetStoryboardTargetProperties(ScaleTransform.ScaleXProperty);
                    break;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _outerBorder = Template.FindName("PART_OuterBorder", this) as Border;
            _selectedBorder = Template.FindName("PART_SelectedBorder", this) as Border;
            _selectedStoryboard = Template.FindName("PART_SelectedStoryboard", this) as Storyboard;
            _unselectedStoryboard = Template.FindName("PART_UnselectedStoryboard", this) as Storyboard;

            UpdateTabStripPlacementVisuals();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.Source == this)
            {
                IsPressed = true;
                UpdateVisualState();
                return;
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (e.Source == this)
            {
                if (IsPressed)
                {
                    base.OnMouseLeftButtonDown(e);
                    IsPressed = false;
                    UpdateVisualState();
                }
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsPressed = false;
            UpdateVisualState();
            base.OnMouseLeave(e);
        }

        protected virtual void UpdateVisualState()
        {
            VisualStateManager.GoToState(
                this,
                IsPressed
                    ? "Pressed"
                    : "Unpressed",
                false);
        }

        void SetSelectedBorderThickness(Thickness thickness)
        {
            if (_selectedBorder == null)
            {
                return;
            }

            _selectedBorder.BorderThickness = thickness;
        }

        void SetOuterBorderThickness(Thickness thickness)
        {
            if(_outerBorder == null)
            {
                return;
            }

            _outerBorder.BorderThickness = thickness;
        }

        void SetStoryboardTargetProperties(DependencyProperty dependencyProperty)
        {
            if (_selectedStoryboard != null && _selectedStoryboard.Children.Any())
            {
                Storyboard.SetTargetProperty(_selectedStoryboard.Children[0], new PropertyPath(dependencyProperty));
            }

            if (_unselectedStoryboard != null && _unselectedStoryboard.Children.Any())
            {
                Storyboard.SetTargetProperty(_unselectedStoryboard.Children[0], new PropertyPath(dependencyProperty));
            }
        }

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set
            {
                SetValue(IsPressedProperty, value);
            }
        }
    }
}
