using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Input;
using static System.Math;
using System.Windows;
using System.Windows.Interop;
using System;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SmoothScroll
{
    internal sealed class SmoothScrollMouseProcessor : MouseProcessorBase
    {
        private const int WM_MOUSEHWHEEL = 0x020E;

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetMessageExtraInfo();

        private bool Enabled => SmoothScrollPackage.OptionsPage?.Enabled ?? true;
        private double HorizontalSensitivity => SmoothScrollPackage.OptionsPage?.HorizontalSensitivity ?? 1.0;
        private double VerticalSensitivity => SmoothScrollPackage.OptionsPage?.VerticalSensitivity ?? 1.0;

        private readonly IWpfTextView wpfTextView;
        private double accumulatedOffset;
        private ScrollDirection accumulatedOffsetDirection;

        internal SmoothScrollMouseProcessor(IWpfTextView _wpfTextView)
        {
            this.wpfTextView = _wpfTextView;
            ((ContentControl)wpfTextView).Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            var source = PresentationSource.FromVisual(wpfTextView.VisualElement);
            ((HwndSource)source)?.AddHook(Hook);
        }

        private static bool IsMouseEvent()
        {
            var extra = (uint)GetMessageExtraInfo();
            bool isTouchOrPen = (extra & 0xFFFFFF00) == 0xFF515700;
            return !isTouchOrPen;
        }

        private static int HIWORD(IntPtr ptr)
        {
            var val32 = ptr.ToInt32();
            return ((val32 >> 16) & 0xFFFF);
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_MOUSEHWHEEL:
                    int delta = (short)HIWORD(wParam);
                    wpfTextView.ViewScroller.ScrollViewportHorizontallyByPixels((int)(delta * HorizontalSensitivity));
                    return (IntPtr)1;
            }
            return IntPtr.Zero;
        }

        public override void PreprocessMouseWheel(MouseWheelEventArgs e)
        {
            if (!IsMouseEvent())
                return;

            if (!Enabled || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            var direction = e.Delta < 0 ? ScrollDirection.Down : ScrollDirection.Up;

            // Reset accumulated offset if scroll direction has changed
            if (accumulatedOffsetDirection != direction)
            {
                accumulatedOffset = 0;
                accumulatedOffsetDirection = direction;
            }

            accumulatedOffset += Abs(e.Delta) / 120.0 * VerticalSensitivity;
            int lines = (int)Floor(accumulatedOffset);
            if (lines > 0)
                wpfTextView.ViewScroller.ScrollViewportVerticallyByLines(direction, lines);
            e.Handled = true;
        }
    }
}
