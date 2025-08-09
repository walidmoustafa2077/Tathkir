using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tathkīr_WPF.Extensions;

namespace Tathkīr_WPF.Behaviors
{
    public static class AutoOpenBehavior
    {
        public static readonly DependencyProperty EnableAutoOpenProperty =
            DependencyProperty.RegisterAttached(
                "EnableAutoOpen",
                typeof(bool),
                typeof(AutoOpenBehavior),
                new UIPropertyMetadata(false, OnEnableAutoOpenChanged));

        public static bool GetEnableAutoOpen(DependencyObject obj) =>
            (bool)obj.GetValue(EnableAutoOpenProperty);

        public static void SetEnableAutoOpen(DependencyObject obj, bool value) =>
            obj.SetValue(EnableAutoOpenProperty, value);

        private static void OnEnableAutoOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox)
            {
                if ((bool)e.NewValue)
                {
                    comboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
                }
                else
                {
                    comboBox.PreviewKeyDown -= ComboBox_PreviewKeyDown;
                }
            }
            else if (d is ScrollViewer scrollViewer)
            {
                if ((bool)e.NewValue)
                {
                    scrollViewer.PreviewKeyDown += PreviewKeyDown;
                }
                else
                {
                    scrollViewer.PreviewKeyDown -= PreviewKeyDown;
                }
            }
            else if (d is Border border)
            {
                if ((bool)e.NewValue)
                {
                    border.PreviewKeyDown += PreviewKeyDown;
                }
                else
                {
                    border.PreviewKeyDown -= PreviewKeyDown;
                }
            }
        }

        private static void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;

            // ScrollViewer or Border (for both cases)
            var container = sender as DependencyObject;
            if (container == null)
                return;

            var focusedElement = Keyboard.FocusedElement;

            // Case 1: Focus is directly on the ComboBox
            if (focusedElement is ComboBox comboBox)
            {
                ApplyComboBoxEnterSelection(comboBox, container);
                return;
            }

            // Case 2: Focus is on the editable TextBox inside the ComboBox
            if (focusedElement is TextBox textBox)
            {
                var comboBoxFromTextBox = textBox.FindParent<ComboBox>();
                if (comboBoxFromTextBox != null)
                {
                    ApplyComboBoxEnterSelection(comboBoxFromTextBox, container);
                }
            }
        }

        private static void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.IsEditable)
            {
                // Open the dropdown only if it's not already open and user is typing
                if (!comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = true;

                    // Delay the text re-focus to allow dropdown to open fully without stealing focus
                    comboBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (comboBox.Template.FindName("PART_EditableTextBox", comboBox) is TextBox textBox)
                        {
                            textBox.Focus();
                        }
                    }), System.Windows.Threading.DispatcherPriority.Input);
                }
            }
        }

        private static void ApplyComboBoxEnterSelection(ComboBox comboBox, DependencyObject container)
        {
            comboBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                string typedText = comboBox.Text ?? string.Empty;

                string normalizedInput = typedText.NormalizeArabic();

                var match = comboBox.Items
                    .Cast<object>()
                    .FirstOrDefault(i =>
                        (i?.ToString() ?? string.Empty)
                            .NormalizeArabic()
                            .Contains(normalizedInput, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    comboBox.SelectedItem = match;
                }

                comboBox.IsDropDownOpen = false;
                if (container is UIElement uiElement)
                {
                    uiElement.Focus();
                }
            }), System.Windows.Threading.DispatcherPriority.Input);
        }
    }
}
