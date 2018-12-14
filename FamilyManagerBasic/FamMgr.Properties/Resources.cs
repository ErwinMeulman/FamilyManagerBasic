using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace FamMgr.Properties
{
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = Resources.resourceMan = new ResourceManager("FamMgr.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static Bitmap Last
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Last", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap small_arrow_down_fast_icon
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("small-arrow-down-fast-icon", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap small_arrow_down_fast_icon1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("small-arrow-down-fast-icon1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal Resources()
		{
		}
	}
}
