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
        List<string> participants;
        List<string> players;
        Dictionary<string, string> playersAndOwners;

        public Form1()
        {
            InitializeComponent();
            engine = new RandomizerEngine();
            PlayerRadiosInit();
            playersAndOwners = new Dictionary<string, string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playerSelected = GetTriggerPlayer();
            var eventFired = GetEvent();
            
            if (ValidateInput(playerSelected, eventFired))
                engine.Randomize(playerSelected, eventFired);
            //TODO: Move this to the return value from the engine
            //soundPlayer = new SoundPlayer(@"D:\Arbejde\randomizer\Randomizer\RandomizerSounds\1000dollars.wav");
            //soundPlayer.Play();
        }

        private bool ValidateInput(string player, string eventFired)
        {
            return !(string.IsNullOrEmpty(player) || (string.IsNullOrEmpty(eventFired)));
        }

        private void TimerInit()
        {
            this.timer1.Enabled = true;
            this.timer1.Start();
            this.timer1.Interval = 1000;
        }

        private void TimerStop()
        {
            this.timer1.Stop();
        }

        private void PlayerRadiosInit()
        {
            var rdoBtn = "radioButton";
            for (int i = 1; i < 12; i++)
            {
                this.playerEditComboBox.Items.Add(new ComboItem("Home " + i, rdoBtn + i));
            }
            for (int i = 1; i < 12; i++)
            {
                this.playerEditComboBox.Items.Add(new ComboItem("Away " + i, rdoBtn + (i+11)));
            }
        }

        private string GetFiredEventSoundUrl(string eventName)
        {
            return "hello";
        }

        private string GetTriggerPlayer()
        {
            var selected = "";
            selected = teamBox.Controls.OfType<RadioButton>().First(x => x.Checked).Name;
            
            return selected;
        }

        private string GetEvent()
        {
            var selected = "";
            try
            {
                selected = eventBox.Controls.OfType<RadioButton>().First(x => x.Checked).Text;                
            }
            catch (Exception)
            {
            
            }

            return selected;
        }

        #region EventHandlers

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "START HALVLEG")
            {
                button2.Text = "STOP HALVLEG";
                button2.BackColor = Color.Red;
                TimerInit();
            }
            else
            {
                button2.Text = "START HALVLEG";
                button2.BackColor = Color.Green;
                TimerStop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar.Value != 45)
            {
                progressBar.Value++;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void richTextButton1_TextChanged(object sender, EventArgs e)
        {
            var playerAffected = (ComboItem)this.playerEditComboBox.SelectedItem;
            var playerAffectedValue = playerAffected.Value;
            playerAffectedValue = string.IsNullOrEmpty(playerAffectedValue) ? "" : playerAffectedValue;
            var updateRadio = teamBox.Controls.OfType<RadioButton>().First(x => x.Name == playerAffectedValue);
            updateRadio.Text = this.richTextBox1.Text;
        }

        #endregion    

        private void button3_Click(object sender, EventArgs e)
        {
            participants = new List<string>();
            players = new List<string>();

            foreach (var participantTextBox in this.participantNamePanel.Controls.OfType<TextBox>())
            {
                participants.Add(participantTextBox.Text);
            }

            foreach (var playerRadio in this.teamBox.Controls.OfType<RadioButton>())
            {
                players.Add(playerRadio.Text);
            }

            playersAndOwners = engine.DistributeTeams(participants, players);
        }
    }
}
