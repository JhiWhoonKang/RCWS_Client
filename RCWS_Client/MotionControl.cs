using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCWS_Client
{
    public partial class MotionControl : Form
    {
        private StreamWriter streamWriter;

        public MotionControl(StreamWriter streamWriter)
        {
            InitializeComponent();
            this.streamWriter = streamWriter;
            this.KeyPress += MotionControl_KeyPress;
        }

        private async void MotionControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            char direction;

            switch (e.KeyChar)
            {
                case 'w':
                    direction = 'U';
                    await streamWriter.WriteAsync(direction);
                    break;

                case 'a':
                    direction = 'L';
                    await streamWriter.WriteAsync(direction);
                    break;

                case 's':
                    direction = 'D';
                    await streamWriter.WriteAsync(direction);
                    break;

                case 'd':
                    direction = 'R';
                    await streamWriter.WriteAsync(direction);
                    break;
                default:
                    return;
            }
            await streamWriter.WriteAsync(direction + "\n");
            streamWriter.Flush();
        }
    }
}
