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

        /// <summary>
        /// Static constructor.
        /// </summary>
        static STabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(STabItem), new FrameworkPropertyMetadata(typeof(STabItem)));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public STabItem()
        {            
            DependencyPropertyDescriptor
                .FromProperty(TabStripPlacementProperty, typeof(STabItem))
                .AddValueChanged(this,OnTabStribPlacementChanged);

            UpdateTabStripPlacementVisuals();
        }

        /// <summary>
        /// Called when the Tab Strip placement property has changed.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event arguments.</param>
        void OnTabStribPlacementChanged(object sender, EventArgs e)
        {
            UpdateTabStripPlacementVisuals();
        }

        /// <summary>
        /// Update the Tab Strip placement visuals depending on tab strip placement.
        /// </summary>
         void UpdateTabStripPlacementVisuals()
        {
            switch (TabStripPlacement)
            {
                case Dock.Left:
                    SetSelectedBorderThickness(new Thickness(4, 0, 0, 0));
                    SetOuterBorderThickness(new Thickness(0));
                    SetStoryboardScaleDimensionTargetProperties(ScaleTransform.ScaleYProperty);
                    break;
                case Dock.Top:
                    SetSelectedBorderThickness(new Thickness(0, 4, 0, 0));
                    SetOuterBorderThickness(new Thickness(0, 0, 1, 0));
                    SetStoryboardScaleDimensionTargetProperties(ScaleTransform.ScaleXProperty);
                    break;
                case Dock.Right:
                    SetSelectedBorderThickness(new Thickness(0, 0, 4,0));
                    SetOuterBorderThickness(new Thickness(0));
                    SetStoryboardScaleDimensionTargetProperties(ScaleTransform.ScaleYProperty);
                    break;
                case Dock.Bottom:
                    SetSelectedBorderThickness(new Thickness(0, 0, 0, 4));
                    SetOuterBorderThickness(new Thickness(0, 0, 1, 0));
                    SetStoryboardScaleDimensionTargetProperties(ScaleTransform.ScaleXProperty);
                    break;
            }
        }

        /// <summary>
        /// Called when the template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _outerBorder = Template.FindName("PART_OuterBorder", this) as Border;
            _selectedBorder = Template.FindName("PART_SelectedBorder", this) as Border;
            _selectedStoryboard = Template.FindName("PART_SelectedStoryboard", this) as Storyboard;
            _unselectedStoryboard = Template.FindName("PART_UnselectedStoryboard", this) as Storyboard;

            UpdateTabStripPlacementVisuals();
        }

        /// <summary>
        /// Called when the left mouse button is pressed down.
        /// </summary>
        /// <param name="e">The event args.</param>
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

        /// <summary>
        /// Called when the left mouse button is released.
        /// </summary>
        /// <param name="e">The event args.</param>
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

        /// <summary>
        /// Called when the mouse leaves the control.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsPressed = false;
            UpdateVisualState();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        protected virtual void UpdateVisualState()
        {
            VisualStateManager.GoToState(
                this,
                IsPressed
                    ? "Pressed"
                    : "Unpressed",
                false);
        }

        /// <summary>
        /// Set the selected border thicknes.
        /// </summary>
        /// <param name="thickness">The thickness to make the selected border.</param>
        void SetSelectedBorderThickness(Thickness thickness)
        {
            if (_selectedBorder == null)
            {
                return;
            }

            _selectedBorder.BorderThickness = thickness;
        }

        /// <summary>
        /// Set the outer border thicknes..
        /// </summary>
        /// <param name="thickness">The thickness to set the outer border.</param>
        void SetOuterBorderThickness(Thickness thickness)
        {
            if(_outerBorder == null)
            {
                return;
            }

            _outerBorder.BorderThickness = thickness;
        }

        /// <summary>
        /// Set the storyboard scale dimension target properties. 
        /// Dependency property needs to change from Scale X to Scale Y depending on the tab strip placement.
        /// </summary>
        /// <param name="scaleDimensionDependencyProperty">The scale dimension dependency property.</param>
        void SetStoryboardScaleDimensionTargetProperties(DependencyProperty scaleDimensionDependencyProperty)
        {
            if (_selectedStoryboard != null && _selectedStoryboard.Children.Any())
            {
                Storyboard.SetTargetProperty(_selectedStoryboard.Children[0], new PropertyPath(scaleDimensionDependencyProperty));
            }

            if (_unselectedStoryboard != null && _unselectedStoryboard.Children.Any())
            {
                Storyboard.SetTargetProperty(_unselectedStoryboard.Children[0], new PropertyPath(scaleDimensionDependencyProperty));
            }
        }

        /// <summary>
        /// Flag indicating if the control being is pressed or not.
        /// </summary>
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
