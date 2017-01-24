using System.Windows;
using System.Windows.Controls;

namespace Win10Styles
{
    public class STabControl : TabControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new STabItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is STabItem;
        }
    }
}
