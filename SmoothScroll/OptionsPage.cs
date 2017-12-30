using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace SmoothScroll
{
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class OptionsPage : DialogPage
	{
		[Category("General")]
		[Description("Enable precision scrolling")]
		[DisplayName("Enabled")]
		public bool ExtEnable { get; set; } = true;
    
	}
}