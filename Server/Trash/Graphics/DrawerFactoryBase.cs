using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.Core;

namespace CVARC.Graphics
{
	public abstract class DrawerFactoryBase
	{
		public DrawerControl CreateAndRunDrawerInStandaloneForm(VideoModes videoMode, DrawerSettings drawerSettings, Func<Form> formFactory)
		{
			FormDrawer drawer = CreateDrawer(videoMode, drawerSettings);
			DrawerControl drawerControl = null;
			var localsync = new ManualResetEventSlim();
			var t = new Thread(() =>
				{
					var form = formFactory();
					drawerControl = new DrawerControl(drawer)
						{
							Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom
						};
					form.Controls.Add(drawerControl);
					form.Shown += (o, e) => localsync.Set();
					Application.Run(form);
				});
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			localsync.Wait();
			if (drawer!=null)
				drawer.WaitForInitialization();
			return drawerControl;
		}

		public DrawerControl CreateAndRunDrawerInStandaloneForm(VideoModes videoMode, DrawerSettings drawerSettings)
		{
			return CreateAndRunDrawerInStandaloneForm(videoMode, drawerSettings, EmptyFormFactory);
		}

		public DrawerControl CreateAndRunDrawerInStandaloneForm(VideoModes videoMode)
		{
			return CreateAndRunDrawerInStandaloneForm(videoMode, new DrawerSettings());
		}

		public FormDrawer CreateDrawer(VideoModes videoModes)
		{
			return CreateDrawer(videoModes, new DrawerSettings());
		}

		public abstract FormDrawer CreateDrawer(VideoModes videoModes, DrawerSettings drawerSettings);
	
		private Form EmptyFormFactory()
		{
			return new Form
			{
				Width = SceneConfig.VideoWidth,
				Height = SceneConfig.VideoHeight,
			};
		}
	}
}