using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PrecisionScroll
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
	public class OptionsPage : DialogPage
	{
		[Category("General")]
		[DisplayName("Enabled")]
		public bool Enabled { get; set; } = true;

        [Category("General")]
        [DisplayName("Horizontal Sensitivity")]
        public double HorizontalSensitivity { get; set; } = 0.5;

        [Category("General")]
        [DisplayName("Vertical Sensitivity")]
        public double VerticalSensitivity { get; set; } = 1.0;
    }
}