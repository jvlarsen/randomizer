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
using System.Data.SqlClient;

namespace Randomizer
{
    public partial class Form1 : Form
    {
        RandomizerEngine engine;
        Dictionary<string, Color> participants;
        Dictionary<string, Color> players;
        Dictionary<string, string> playersAndOwners;

        public Form1()
        {
            InitializeComponent();
            engine = new RandomizerEngine();
            PlayerRadiosInit();
            playersAndOwners = new Dictionary<string, string>();
            CalculateGraph("");
            LoadOldGames();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playerSelected = GetTriggerPlayer();
            var eventFired = GetEvent();
            var randomizerOutcome = new Dictionary<string, int>();

            if (ValidateInput(playerSelected, eventFired))
            {
                //returned Dictionary<string, string> with <loserName, measure>
                var gameMinute = this.progressBar.Value;
                randomizerOutcome = engine.Randomize(playerSelected, eventFired, playersAndOwners, this.gameNameLabel.Text, gameMinute);
            }

            foreach (KeyValuePair<string, int> loserZips in randomizerOutcome)
            {
                if (loserZips.Key.ToLower().Equals("faccio"))
                    this.labelFaccio.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("leffo"))
                    this.labelLeffo.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("nosser"))
                    this.labelNosser.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("tarzan"))
                    this.labelTarzan.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("tennedz"))
                    this.labelTennedz.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("trusser"))
                    this.labelTrusser.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals("aallex"))
                    this.labelAallex.Text = loserZips.Value.ToString();

                //var currentParticipantTextBox = this.participantsPanel.Controls.OfType<FlowLayoutPanel>().First(x => x.Text == loser);
                //var index = currentParticipantTextBox.Text.Substring(18);
                //var currentMeasureTextBox = this.participantPanel3.Controls.OfType<TextBox>().First(x => x.Name == "measureTextBox" + index);
                //currentMeasureTextBox.Text = randomizerOutcome[loser];
            }

            CalculateGraph(eventFired);
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
                this.playerEditComboBox.Items.Add(new ComboItem("Home Team " + i, rdoBtn + i));
            }
            for (int i = 1; i < 12; i++)
            {
                this.playerEditComboBox.Items.Add(new ComboItem("Away Team " + i, rdoBtn + (i + 11)));
            }
        }

        private void LoadOldGames()
        {
            var saveGames = engine.GetSaveGames();
            for (int i = 0; i < saveGames.Count; i++)
            {
                this.loadGameComboBox.Items.Add(new SaveGame(saveGames[i]));
            }
        }

        private string GetTriggerPlayer()
        {
            var selected = "";
            selected = teamBox.Controls.OfType<RadioButton>().First(x => x.Checked).Text;

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
                var playerAffectedValue = playerAffected.Presentation;
                playerAffectedValue = string.IsNullOrEmpty(playerAffectedValue) ? "" : playerAffectedValue;
                var updateRadio = teamBox.Controls.OfType<RadioButton>().First(x => x.Name == playerAffectedValue);
                updateRadio.Text = this.richTextBox1.Text;
            }
        }

        private void submitNewPlayerName(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void distributeTeamsButton_Click(object sender, EventArgs e)
        {
            ClearInfoLabel();
            participants = new Dictionary<string, Color>();
            players = new Dictionary<string, Color>();

            if (string.IsNullOrEmpty(this.gameNameLabel.Text))
            {
                SetInfoLabelText("Kampen skal oprettes først", 1);
                return;
            }

            participants = GetParticipantsFromUI();

            foreach (var participantTextBox in this.participantsPanel.Controls.OfType<TextBox>())
            {
                if (!string.IsNullOrEmpty(participantTextBox.Text))
                    participants.Add(participantTextBox.Text, participantTextBox.BackColor);
            }

            foreach (var playerRadio in this.teamBox.Controls.OfType<RadioButton>())
            {
                players.Add(playerRadio.Text, Color.White);
            }

            if (participants.Count > 0)
            {
                playersAndOwners = engine.DistributeTeams(players.Keys.ToList<string>(), participants.Keys.ToList<string>(), this.gameNameLabel.Text);
                engine.SaveParticipants(participants);
            }
            else
            {
                SetInfoLabelText("Husk at angive deltagerne", 1);
                return;
            }
            UpdatePlayerColorsFromOwners(playersAndOwners);
            this.teamDistributionButton.Enabled = false;
        }

        private Dictionary<string, Color> GetParticipantsFromUI()
        {
            var enteredParticipants = new Dictionary<string, Color>();
            foreach (var currPanel in this.participantsPanel.Controls.OfType<FlowLayoutPanel>())
            {
                var currBox = currPanel.Controls.OfType<TextBox>().First();
                if (!string.IsNullOrEmpty(currBox.Text))
                {
                    var name = currBox.Text;
                    var color = currBox.BackColor;
                    enteredParticipants.Add(name, color);
                }
            }
            return enteredParticipants;
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

            this.gameNameLabel.Text = homeTeam + "-" + awayTeam;
            this.homeTeamTextBox.Text = "";
            this.awayTeamTextBox.Text = "";
            engine.SaveNewGame(this.gameNameLabel.Text, DateTime.Now);
            this.teamDistributionButton.Enabled = true;
        }

        #endregion

        private void UpdatePlayerColorsFromOwners(Dictionary<string, string> playersAndOwners)
        {
            foreach (var player in playersAndOwners.Keys)
            {
                this.teamBox.Controls.OfType<RadioButton>().First(x => x.Text == player).BackColor = participants[playersAndOwners[player]];
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'randomDbDataSet.Graph' table. You can move, or remove it, as needed.
            this.graphTableAdapter.Fill(this.randomDbDataSet.Graph);
        }

        private void CalculateGraph(string eventName)
        {
            ClearAllSeries();
            var matchId = this.gameNameLabel.Text;
            var calculatedPoints = engine.CalculateGraph(matchId);

            for (int i = 0; i < calculatedPoints.Keys.Count; i++)
            {
                var minutesAndMeasures = calculatedPoints.ElementAt(i).Value;
                foreach (KeyValuePair<int, int> item in minutesAndMeasures)
                {
                    chart1.Series[i].Points.AddXY(item.Value, item.Key);
                }
                chart1.Series[i].Points.Last().AxisLabel = eventName;
            }

            chart1.Visible = true;
            chart1.Show();
        }

        private void ClearAllSeries()
        {
            foreach (var series in chart1.Series)
            {
                if (series.Points.Count > 0)
                    series.Points.Clear();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            engine.UndoLatestEvent();
            CalculateGraph("");
        }

        #region DrinkOkClick Handlers
        private void player1DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelFaccio.Text = "";
        }

        private void player2DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelLeffo.Text = "";
        }

        private void player3DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelNosser.Text = "";
        }

        private void player4DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelTarzan.Text = "";
        }

        private void player5DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelTennedz.Text = "";
        }

        private void player6DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelTrusser.Text = "";
        }

        private void player7DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelAallex.Text = "";
        }
        #endregion

        private void loadGameButton_Click(object sender, EventArgs e)
        {
            if (loadGameComboBox.SelectedItem == null)
            {
                this.infoLabel.Text = "Vælg en kamp fra listen.";
                return;
            }
            else
            {
                this.infoLabel.Text = "";
            }
            var saveGame = (SaveGame)loadGameComboBox.SelectedItem;

            LoadPlayersFromSaveGame(saveGame);
            LoadAndSetOwnerColorsOnPlayers(saveGame);

        }

        private void LoadPlayersFromSaveGame(SaveGame saveGame)
        {
            var players = engine.LoadPlayersFromSaveGame(saveGame);
            var participantsFlowPanels = participantsPanel.Controls.OfType<FlowLayoutPanel>();
            //var player1Color = participantsFlowPanels.First().Controls.OfType<FlowLayoutPanel>().First(x => x.)
            foreach (var player in players)
            {
                switch (player.PlayerIndex)
                {
                    case (0):
                        homePlayer1.Text = player.Name;
                        homePlayer1.BackColor = player.Owner.BackColor;
                        break;
                    case (1):
                        homePlayer2.Text = player.Name;
                        break;
                    case (2):
                        homePlayer3.Text = player.Name;
                        break;
                    case (3):
                        homePlayer4.Text = player.Name;
                        break;
                    case (4):
                        homePlayer5.Text = player.Name;
                        break;
                    case (5):
                        homePlayer6.Text = player.Name;
                        break;
                    case (6):
                        homePlayer7.Text = player.Name;
                        break;
                    case (7):
                        homePlayer8.Text = player.Name;
                        break;
                    case (8):
                        homePlayer9.Text = player.Name;
                        break;
                    case (9):
                        homePlayer10.Text = player.Name;
                        break;
                    case (10):
                        awayPlayer1.Text = player.Name;
                        break;
                    case (11):
                        awayPlayer2.Text = player.Name;
                        break;
                    case (12):
                        awayPlayer3.Text = player.Name;
                        break;
                    case (13):
                        awayPlayer4.Text = player.Name;
                        break;
                    case (14):
                        awayPlayer5.Text = player.Name;
                        break;
                    case (15):
                        awayPlayer6.Text = player.Name;
                        break;
                    case (16):
                        awayPlayer7.Text = player.Name;
                        break;
                    case (17):
                        awayPlayer8.Text = player.Name;
                        break;
                    case (18):
                        awayPlayer9.Text = player.Name;
                        break;
                    case (19):
                        awayPlayer10.Text = player.Name;
                        break;
                    case (20):
                        refereePlayer.Text = player.Name;
                        break;
                }
            }
        }

        private void GetBackColorFromParticipant(Participant owner)
        {

        }

        private void LoadAndSetOwnerColorsOnPlayers(SaveGame saveGame)
        {

            

        }
    }
}
