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
        RandomizerEngine engine;

        public Form1()
        {
            dbFacade = DbFacade.GetInstance();
            InitializeComponent();
            engine = new RandomizerEngine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var eventSoundUrl = GetFiredEventSoundUrl();
            var triggerPlayerName = GetPlayerTriggeringEvent();
            var eventFired = GetEventFired();
            dbFacade.GetEvents();
            engine.Randomize(triggerPlayerName, eventFired);
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

        private string GetPlayerTriggeringEvent()
        {
            if (this.radioButton1.Checked)
            {
                return radioButton1.Text;
            }
            else if (this.radioButton2.Checked)
            {
                return radioButton2.Text;
            }
            else if (this.radioButton3.Checked)
            {
                return radioButton3.Text;
            }
            else if (this.radioButton4.Checked)
            {
                return radioButton4.Text;
            }
            else if (this.radioButton5.Checked)
            {
                return radioButton5.Text;
            }
            else if (this.radioButton6.Checked)
            {
                return radioButton6.Text;
            }
            else if (this.radioButton7.Checked)
            {
                return radioButton7.Text;
            }
            else if (this.radioButton8.Checked)
            {
                return radioButton8.Text;
            }
            else if (this.radioButton9.Checked)
            {
                return radioButton9.Text;
            }
            else if (this.radioButton10.Checked)
            {
                return radioButton10.Text;
            }
            else if (this.radioButton11.Checked)
            {
                return radioButton11.Text;
            }
            else if (this.radioButton12.Checked)
            {
                return radioButton12.Text;
            }
            else if (this.radioButton13.Checked)
            {
                return radioButton13.Text;
            }
            else if (this.radioButton14.Checked)
            {
                return radioButton14.Text;
            }
            else if (this.radioButton15.Checked)
            {
                return radioButton15.Text;
            }
            else if (this.radioButton16.Checked)
            {
                return radioButton16.Text;
            }
            else if (this.radioButton17.Checked)
            {
                return radioButton17.Text;
            }
            else if (this.radioButton18.Checked)
            {
                return radioButton18.Text;
            }
            else if (this.radioButton19.Checked)
            {
                return radioButton19.Text;
            }
            else if (this.radioButton20.Checked)
            {
                return radioButton20.Text;
            }
            else if (this.radioButton21.Checked)
            {
                return radioButton21.Text;
            }
            else if (this.radioButton22.Checked)
            {
                return radioButton22.Text;
            }
            else
            {
                return "no player selected";
            }
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
