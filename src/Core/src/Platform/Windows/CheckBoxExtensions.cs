using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui
{
	public static class CheckBoxExtensions
	{
		public static void UpdateIsChecked(this CheckBox nativeCheckBox, ICheckBox check)
		{
			nativeCheckBox.IsChecked = check.IsChecked;
		}

		public static void UpdateColor(this CheckBox nativeCheckBox, ICheckBox check) { }
	}
}