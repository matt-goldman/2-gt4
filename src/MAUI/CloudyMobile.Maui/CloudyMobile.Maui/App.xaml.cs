﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using System.Threading.Tasks;
using Application = Microsoft.Maui.Controls.Application;

namespace CloudyMobile.Maui
{
	public partial class App : Application
	{
		public static Constants Constants { get; set; } = new Constants ();

		public App()
		{
			InitializeComponent();
		}

		protected override IWindow CreateWindow(IActivationState activationState)
		{
			this.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>()
				.SetImageDirectory("Assets");

			return new Microsoft.Maui.Controls.Window(new NavigationPage(new MainPage()));
		}
	}
}
