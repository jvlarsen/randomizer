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
        RandomizerEngine engine;
        List<string> participants;
        List<string> players;
        Dictionary<string, string> playersAndOwners;
        int currentGamId;

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
            {
                engine.Randomize(playerSelected, eventFired);
            }
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
                this.playerEditComboBox.Items.Add(new ComboItem("Away " + i, rdoBtn + (i + 11)));
            }
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
            if (playerAffected != null)
            {
                var playerAffectedValue = playerAffected.Value;
                playerAffectedValue = string.IsNullOrEmpty(playerAffectedValue) ? "" : playerAffectedValue;
                var updateRadio = teamBox.Controls.OfType<RadioButton>().First(x => x.Name == playerAffectedValue);
                updateRadio.Text = this.richTextBox1.Text;
            }
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            ClearInfoLabel();
            participants = new List<string>();
            players = new List<string>();

            if (string.IsNullOrEmpty(this.gameNameLabel.Text))
            {
                SetInfoLabelText("Kampen skal oprettes først", 1);
                return;
            }

            foreach (var participantTextBox in this.participantNamePanel.Controls.OfType<TextBox>())
            {
                if (!string.IsNullOrEmpty(participantTextBox.Text))
                    participants.Add(participantTextBox.Text);
            }

            foreach (var playerRadio in this.teamBox.Controls.OfType<RadioButton>())
            {
                players.Add(playerRadio.Text);
            }

            if (participants.Count > 0)
            {
                playersAndOwners = engine.DistributeTeams(players, participants);
                engine.SaveParticipants(participants);
                engine.SaveDistribution(playersAndOwners, this.gameNameLabel.Text);
            }
            else
            {
                SetInfoLabelText("Husk at angive deltagerne", 1);
                return;
            }
        }

        private void createNewGameButton_Click(object sender, EventArgs e)
        {
            ClearInfoLabel();
            var homeTeam = this.homeTeamTextBox.Text;
            var awayTeam = this.awayTeamTextBox.Text;
            if (string.IsNullOrEmpty(homeTeam)  || (string.IsNullOrEmpty(awayTeam)))
            {
                SetInfoLabelText("Name both teams", 1);
                return;
            }

            this.gameNameLabel.Text = homeTeam + " - " + awayTeam;
            this.homeTeamTextBox.Text = "";
            this.awayTeamTextBox.Text = "";
            //currentGameId = engine.CreateNewGame(this.currentGameNameLabel.Text);
        }


        private void SetInfoLabelText(string message, int type)
        {
            switch(type)
            {
                case(1): //Error
                    this.infoLabel.BackColor = Color.Red;
                    break;
                case(2): //Warning
                    this.infoLabel.BackColor = Color.Yellow;
                    break;
                case(3): //info
                    this.infoLabel.BackColor = Color.Green;
                    break;
                default:
                    this.infoLabel.BackColor = Color.White;
                    break;
            }
            this.infoLabel.Text = message;
        }

        private void ClearInfoLabel()
        {
            SetInfoLabelText("", 0); //Use this to keep manipulation of the backColor in one place.
        }
    }
}
