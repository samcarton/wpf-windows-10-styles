using System.Windows;
using System.Windows.Controls;

namespace Win10Styles
{
    public class STabControl : TabControl
    {
        /// <summary>
        /// Get the container for the item.
        /// </summary>
        /// <returns>The container for item override.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new STabItem();
        }

        /// <summary>
        /// Check if an item is its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is its own container, otherwise false.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is STabItem;
        }
    }
}
