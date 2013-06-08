using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomChangeMonitorDemo
{
	public class ClipboardChangeMonitor : ChangeMonitor
	{
		#region Var
		private string _clipboardText;
		private Timer _timer;
		private string _uniqueID; 
		#endregion


		#region Private Property
		private string m_ClipboardText
		{
			get { return _clipboardText ?? string.Empty; }
			set 
			{
				if (_clipboardText == value)
					return;

				_clipboardText = value;
				OnChanged(value);
			}
		}

		private System.Windows.Forms.Timer m_Timer 
		{
			get
			{
				return _timer ?? (_timer = new System.Windows.Forms.Timer());
			}
		} 
		#endregion


		#region Public Property
		public override string UniqueId
		{
            get { return _uniqueID ?? (_uniqueID = Guid.NewGuid().ToString()); }
		} 
		#endregion


		#region Constructor
		public ClipboardChangeMonitor()
		{
			m_Timer.Interval = 1000;
			m_Timer.Tick += m_Timer_Tick;
			m_Timer.Start();

			_clipboardText = Clipboard.GetText();

			InitializationComplete();
		}
		#endregion


		#region Protected Method
		protected override void Dispose(bool disposing)
		{
		} 
		#endregion


		#region Event Process
		void m_Timer_Tick(object sender, EventArgs e)
		{
			m_ClipboardText = Clipboard.GetText();
		}  
		#endregion
	}
}
