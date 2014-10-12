using System;
using System.Windows.Forms;

namespace CVARC.Graphics
{
	public partial class DrawerControl : UserControl
	{
		private DrawerControl()
		{
			InitializeComponent();
		}

		public DrawerControl(FormDrawer drawer)
			: this()
		{
			Drawer = drawer;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if(Parent == null)
				return;
			if(_oldParent != null)
				_oldParent.Resize -= ResizeThisToParent;
			ResizeThisToParent(this, e);
			Parent.Resize += ResizeThisToParent;
			_oldParent = Parent;
		}

		private void ResizeThisToParent(object sender, EventArgs e)
		{
			if(Parent != null)
				ClientSize = Parent.ClientSize;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(Drawer != null)
				Drawer.ResizeToFit();
		}


		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (Drawer!=null)
				Drawer.StartDrawing(this);
		}


		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing && (components != null))
				components.Dispose();
			if(Drawer != null)
				Drawer.Dispose();
		}


		public Form TopLevelForm { get { return TopLevelControl as Form; } }

		public FormDrawer Drawer { get; private set; }
		private Control _oldParent;
	}
}