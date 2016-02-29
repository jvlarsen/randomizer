using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randomizer
{
    public partial class Form1 : Form
    {
        SoundPlayer soundPlayer;

        public Form1()
        {
            DbFacade dbFacade = new DbFacade();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var eventSoundUrl = GetFiredEventSoundUrl();
            GetEventPlayer();
            soundPlayer = new SoundPlayer(@"D:\Arbejde\randomizer\Randomizer\RandomizerSounds\1000dollars.wav");
            soundPlayer.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "START HALVLEG")
            {
                button2.Text = "STOP HALVLEG";
                button2.BackColor = Color.Red;
            }
            else
            {
                button2.Text = "START HALVLEG";
                button2.BackColor = Color.Green;
            }
        }

        private string GetEventPlayer()
        {
            if (this.radioButton1.Checked)
            {
                return radioButton1.Text;
            }

            return "hello";
        }

        private string GetEventFired()
        {
            if (radioButton23.Checked)
            {
                return radioButton23.Text;
            }
            else if (radioButton24.Checked)
            {
                return radioButton24.Text;
            }
            else if (radioButton25.Checked)
            {
                return radioButton25.Text;
            }
            else if (radioButton26.Checked)
            {
                return radioButton26.Text;
            }
            else if (radioButton27.Checked)
            {
                return radioButton27.Text;
            }
            else if (radioButton28.Checked)
            {
                return radioButton28.Text;
            }
            else if (radioButton29.Checked)
            {
                return radioButton29.Text;
            }
            else if (radioButton30.Checked)
            {
                return radioButton30.Text;
            }
            else if (radioButton31.Checked)
            {
                return radioButton31.Text;
            }
            else if (radioButton32.Checked)
            {
                return radioButton32.Text;
            }
            else if (radioButton33.Checked)
            {
                return radioButton33.Text;
            }
            else if (radioButton34.Checked)
            {
                return radioButton34.Text;
            }
            else if (radioButton35.Checked)
            {
                return radioButton35.Text;
            }
            else
            {
                return "";
            }
        }

        private string GetFiredEventSoundUrl(string eventName)
        {
            return "hello";
        }
    }
}
