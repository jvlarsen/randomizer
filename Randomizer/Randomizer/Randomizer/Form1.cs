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
using Randomizer.Entities;

namespace Randomizer
{
    public partial class Form1 : Form
    {
        SoundPlayer soundPlayer;
        RandomizerEngine engine;
        List<Event> events;
        string playerSelected = "";
        string eventFired = "";

        public Form1()
        {
            InitializeComponent();
            engine = new RandomizerEngine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playerSelected = GetTriggerPlayer();
            var eventFired = GetEvent();
            engine.Randomize(playerSelected, eventFired);
            //TODO: Move this to the return value from the engine
            //soundPlayer = new SoundPlayer(@"D:\Arbejde\randomizer\Randomizer\RandomizerSounds\1000dollars.wav");
            //soundPlayer.Play();



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

        private string GetFiredEventSoundUrl(string eventName)
        {
            return "hello";
        }

        private string GetTriggerPlayer()
        {
            var selected = "";
            selected = groupBox3.Controls.OfType<RadioButton>().First(x => x.Checked).Text;
            
            return selected;
        }

        private string GetEvent()
        {
            var selected = "";
            selected = groupBox4.Controls.OfType<RadioButton>().First(x => x.Checked).Text;

            return selected;
        }
    }
}
