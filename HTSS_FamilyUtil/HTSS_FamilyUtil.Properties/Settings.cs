using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace HTSS_FamilyUtil.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[DefaultSettingValue("Normal")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public FormWindowState CustomWindowState
		{
			get
			{
				return (FormWindowState)((SettingsBase)this)["CustomWindowState"];
			}
			set
			{
				((SettingsBase)this)["CustomWindowState"] = value;
			}
		}
	}
}
