using System;
using CVARC.Core;
using CVARC.Graphics.DirectX;
using CVARC.Graphics.Winforms;

namespace CVARC.Graphics
{
	public class DrawerFactory : DrawerFactoryBase
	{
		public DrawerFactory(Body root)
		{
			_root = root;
		}

		public DirectXScene GetDirectXScene()
		{
			if(_scene != null)
				return _scene;
			lock(_sync)
			{
				return _scene ?? (_scene = new DirectXScene(_root));
			}
		}

		public override FormDrawer CreateDrawer(VideoModes videoModes, DrawerSettings drawerSettings)
		{
			switch(videoModes)
			{
				case VideoModes.DirectX:
					return new DirectXFormDrawer(GetDirectXScene(), drawerSettings);
				case VideoModes.Winforms:
					return new WinformsDrawer(_root, drawerSettings);
				case VideoModes.No:
					return null;
				default:
					throw new Exception("Video mode not supported");
			}
		}

		private readonly object _sync=new object();
		private readonly Body _root;
		private DirectXScene _scene;
	}
}