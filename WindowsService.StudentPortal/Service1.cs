using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService.StudentPortal
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer= null;
        public Service1()
        {
            InitializeComponent();
        }


        #region OnStart
        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            //1000 = One Second
            this.timer.Interval = 10000;//10 second
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            this.timer.Enabled = true;
        }

        private void timer_tick(object sender, ElapsedEventArgs e)
        {
            
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
        }
        #endregion

    }
}
