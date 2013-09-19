using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace AsyncPrimes
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		private async void ButtonSubmit_Click(object sender, RoutedEventArgs e)
		{
			int to = 0;
			int from = 0;
			int.TryParse(TextBoxRangeFrom.Text, out from);
			int.TryParse(TextBoxRangeTo.Text, out to);
			bool getPrimesOnly = TogglePrimesOnly.IsOn;
			List<int> primes = await PrimesCalculator.GetPrimesInRangeAsync(from, to);
			List<string> primesWithPartners = await PrimesCalculator.FindPrimesPartnersAsync(primes, getPrimesOnly);
			string result = string.Join(", ", primesWithPartners);
			CurrentTextBlock(sender).Text = result;
		}

		private TextBlock CurrentTextBlock(object sender)
		{
			string buttonName = (sender as Button).Name;

			TextBlock output = new TextBlock();
			switch (buttonName)
			{
				case "ButtonSubmit1":
					output = this.TextBlock1;
					break;
				case "ButtonSubmit2":
					output = this.TextBlock2;
					break;
				case "ButtonSubmit3":
					output = this.TextBlock3;
					break;
				case "ButtonSubmit4":
					output = this.TextBlock4;
					break;
				default:
					break;
			}
			return output;
		}
	}
}