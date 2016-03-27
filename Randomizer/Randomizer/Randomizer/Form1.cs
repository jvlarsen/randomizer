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
        string playerSelected;
        string eventFired;

        public Form1()
        {
            InitializeComponent();
            engine = new RandomizerEngine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var eventSoundUrl = GetFiredEventSoundUrl();
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

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton25.Checked)
                eventFired = radioButton25.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                playerSelected = radioButton1.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                playerSelected = radioButton2.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                playerSelected = radioButton3.Text;
        }
    }
}
