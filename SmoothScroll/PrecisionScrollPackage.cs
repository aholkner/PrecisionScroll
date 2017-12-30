using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace PrecisionScroll
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(PrecisionScrollPackage.PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids80.NoSolution)]
	[ProvideOptionPage(typeof(OptionsPage), "PrecisionScroll", "General", 0, 0, true)]

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	public sealed class PrecisionScrollPackage : Package
	{
        public const string PackageGuidString = "86AEF3D9-AB16-4498-B024-6F5A597A55C8";

		public static OptionsPage OptionsPage;

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			OptionsPage = base.GetDialogPage(typeof(OptionsPage)) as OptionsPage;
		}
	}
}
