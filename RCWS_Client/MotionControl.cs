using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCWS_Client
{
    public partial class MotionControl : Form
    {
        public MotionControl()
        {
            InitializeComponent();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {

        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            
        }

        private void keyboard_RCWS_Up(object sender, KeyPressEventArgs e)
        {
            MoveUp();
        }

        private void keyboard_RCWS_Down(object sender, KeyPressEventArgs e)
        {
            MoveDown();
        }

        private void keyboard_RCWS_Right(object sender, KeyPressEventArgs e)
        {
            MoveRight();
        }

        private void keyboard_RCWS_Left(object sender, KeyPressEventArgs e)
        {
            MoveLeft();
        }

        private void MoveUp()
        {
            
        }

        private void MoveRight()
        {
            
        }

        private void MoveLeft()
        {
            
        }

        private void MoveDown()
        {
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    MoveLeft();
                    return true;
                case Keys.Right:
                    MoveRight();
                    return true;
                case Keys.Up:
                    MoveUp();
                    return true;
                case Keys.Down:
                    MoveDown();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
