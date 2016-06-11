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
using System.Threading;

namespace Randomizer
{
    public partial class Form1 : Form
    {
        RandomizerEngine engine;
        Dictionary<string, Color> participants;
        List<Player> players;
        Dictionary<Player, string> playersAndOwners;

        public Form1()
        {
            InitializeComponent();
            engine = new RandomizerEngine();
            PlayerRadiosInit();
            playersAndOwners = new Dictionary<Player, string>();
            CalculateGraph("");
            LoadOldGames();
            InitWithdrawalMeasureCombo();
            PlayOpeningTheme();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playerSelected = GetTriggerPlayer();
            var eventFired = GetEvent();
            var randomizerOutcome = new Dictionary<string, Randomizer.RandomizerEngine.MeasureName>();
            
            if (String.IsNullOrEmpty(this.matchIdLabel.Text))
                return;

            var matchId = GetMatchId();

            if (ValidateInput(playerSelected, eventFired))
            {
                //returned Dictionary<string, string> with <loserName, measure>
                int gameMinute = (this.progressBar.Value/60)+1;
                randomizerOutcome = engine.Randomize(playerSelected, eventFired, matchId, gameMinute);
            }

            foreach (KeyValuePair<string, Randomizer.RandomizerEngine.MeasureName> loserZips in randomizerOutcome)
            {
                if (loserZips.Key.ToLower().Equals(participantTextBox1.Text.ToLower()))
                    this.labelDrinkPlayer1.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox2.Text.ToLower()))
                    this.labelDrinkPlayer2.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox3.Text.ToLower()))
                    this.labelDrinkPlayer3.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox4.Text.ToLower()))
                    this.labelDrinkPlayer4.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox5.Text.ToLower()))
                    this.labelDrinkPlayer5.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox6.Text.ToLower()))
                    this.labelDrinkPlayer6.Text = loserZips.Value.ToString();
                else if (loserZips.Key.ToLower().Equals(participantTextBox7.Text.ToLower()))
                    this.labelDrinkPlayer7.Text = loserZips.Value.ToString();
            }

            CalculateGraph(eventFired);
            UpdateLeaderBoard();
        }

        private void PlayOpeningTheme()
        {
            var soundPlayer = new SoundPlayer("D:\\Arbejde\\randomizer\\Randomizer\\RandomizerSounds\\olympics.wav");
            soundPlayer.Play();
        }

        private int GetMatchId()
        {
            var matchId = 0;
            int.TryParse(matchIdLabel.Text, out matchId);
            return matchId;
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
            for (int i = 1; i < 11; i++)
            {
                this.playerEditComboBox.Items.Add(new ComboItem("Home Player " + i, rdoBtn + i));
            }
            for (int i = 1; i < 11; i++)
            {
                this.playerEditComboBox.Items.Add(new ComboItem("Away Player " + i, rdoBtn + (i + 11)));
            }
        }

        private void InitWithdrawalMeasureCombo()
        {
            withdrawalMeasureCombo.Items.Add(DbFacade.Measures.Small);
            withdrawalMeasureCombo.Items.Add(DbFacade.Measures.Medium);
            withdrawalMeasureCombo.Items.Add(DbFacade.Measures.Large);
            withdrawalMeasureCombo.Items.Add(DbFacade.Measures.Walter);
        }

        private void InitWithdrawalParticipantCombo(Dictionary<string, Color> participants)
        {
            withdrawalParticipantCombo.Items.Clear();
            withdrawalParticipantCombo.Items.AddRange(participants.Keys.ToArray());
        }

        private void LoadOldGames()
        {
            var saveGames = engine.GetSaveGames();
            for (int i = 0; i < saveGames.Count; i++)
            {
                this.loadGameComboBox.Items.Add(saveGames[i]);
            }
        }

        private string GetTriggerPlayer()
        {
            var selected = "";
            try
            {
                selected = teamBox.Controls.OfType<RadioButton>().First(x => x.Checked).Text;
                infoLabel.Text = "";
            }
            catch (Exception)
            {
                infoLabel.Text = "Vælg en spiller";
            }
            

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
            if (button2.Text == "START TIDEN")
            {
                button2.Text = "STOP TIDEN";
                button2.BackColor = Color.Red;
                TimerInit();
            }
            else
            {
                button2.Text = "START TIDEN";
                button2.BackColor = Color.Green;
                TimerStop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
                progressBar.Value++;
            if (progressBar.Value % 60 == 0)
            {
                RegisterNothing(progressBar.Value);
                CalculateGraph("");
            }
        }

        private void richTextButton1_TextChanged(object sender, EventArgs e)
        {
            var playerAffected = (ComboItem)this.playerEditComboBox.SelectedItem;
            if (playerAffected != null)
            {
                var playerAffectedValue = playerAffected.Presentation;
                playerAffectedValue = string.IsNullOrEmpty(playerAffectedValue) ? "" : playerAffectedValue;
                var updateRadio = teamBox.Controls.OfType<RadioButton>().First(x => x.Name.ToLower() == playerAffectedValue.Replace(" ", "").ToLower());
                updateRadio.Text = this.richTextBox1.Text;
                engine.UpdatePlayerName(updateRadio.Text, updateRadio.Name, GetMatchId());
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
            players = new List<Player>();

            if (string.IsNullOrEmpty(this.gameNameLabel.Text))
            {
                SetInfoLabelText("Kampen skal oprettes først", 1);
                return;
            }

            participants = GetParticipantsFromUI();
            var matchId = GetMatchId();

            foreach (var participantTextBox in this.participantsPanel.Controls.OfType<TextBox>())
            {
                if (!string.IsNullOrEmpty(participantTextBox.Text))
                    participants.Add(participantTextBox.Text, participantTextBox.BackColor);
            }
            engine.SaveParticipants(participants, matchId);
            foreach (var playerRadio in this.teamBox.Controls.OfType<RadioButton>())
            {
                players.Add(new Player { Name = playerRadio.Text, radioColor = Color.White, radioButton = playerRadio.Name });
            }

            if (participants.Count > 0)
            {                
                playersAndOwners = engine.DistributeTeams(players, participants.Keys.ToList<string>(), matchId);
            }
            else
            {
                SetInfoLabelText("Husk at angive deltagerne", 1);
                return;
            }
            UpdatePlayerColorsFromOwners(playersAndOwners);
            InitWithdrawalParticipantCombo(participants);
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
            var matchId = engine.SaveNewGame(this.gameNameLabel.Text, DateTime.Now);
            this.teamDistributionButton.Enabled = true;
            this.matchIdLabel.Text = matchId.ToString();
        }

        #endregion

        private void UpdatePlayerColorsFromOwners(Dictionary<Player, string> playersAndOwners)
        {
            foreach (var player in playersAndOwners.Keys)
            {
                var affectedRadio = this.teamBox.Controls.OfType<RadioButton>().First(x => x.Name == player.radioButton);
                affectedRadio.BackColor = participants[playersAndOwners[player]];
                affectedRadio.Text = player.Name;
                var radioColorArgb = affectedRadio.BackColor.ToArgb();
                if (radioColorArgb == Color.DimGray.ToArgb() || radioColorArgb == Color.DarkRed.ToArgb() || radioColorArgb == Color.MediumOrchid.ToArgb())
                {
                    affectedRadio.ForeColor = Color.White;
                }
            }
        }

        private void UpdateLeaderBoard()
        {
            foreach (var series in chart1.Series)
            {
                int lastY = 0;
                try
                {
                    lastY = Convert.ToInt32(series.Points.Last().YValues.Last());
                }
                catch (Exception)
                {
                    lastY = 0;
                }
                
                if (series.Name == "player1")
                    player1Total.Text = participantTextBox1.Text + ": " + lastY;
                else if (series.Name == "player2")
                    player2Total.Text = participantTextBox2.Text + ": " + lastY;
                else if (series.Name == "player3")
                    player3Total.Text = participantTextBox3.Text + ": " + lastY;
                else if (series.Name == "player4")
                    player4Total.Text = participantTextBox4.Text + ": " + lastY;
                else if (series.Name == "player5")
                    player5Total.Text = participantTextBox5.Text + ": " + lastY;
                else if (series.Name == "player6")
                    player6Total.Text = participantTextBox6.Text + ": " + lastY;
                else if (series.Name == "player7")
                    player7Total.Text = participantTextBox7.Text + ": " + lastY;
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

            var matchId = GetMatchId();

            var calculatedPoints = engine.CalculateGraph(matchId);

            for (int i = 0; i < calculatedPoints.Keys.Count; i++) //Iterates the participants
            {
                var minutesAndMeasures = calculatedPoints.ElementAt(i).Value;

                for (int j = 0; j < minutesAndMeasures.Count; j++)
                {
                    var index = chart1.Series[i].Points.AddXY(minutesAndMeasures[j].XPoint, minutesAndMeasures[j].YPoint);
                    chart1.Series[i].Points[index].AxisLabel = minutesAndMeasures[j].EventName;
                }
            }
            chart1.Visible = true;
            chart1.Show();
        }

        private void RegisterNothing(int gameMinute)
        {
            engine.RegisterNothing(GetMatchId(), gameMinute);
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
            this.labelDrinkPlayer1.Text = "";
        }

        private void player2DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer2.Text = "";
        }

        private void player3DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer3.Text = "";
        }

        private void player4DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer4.Text = "";
        }

        private void player5DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer5.Text = "";
        }

        private void player6DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer6.Text = "";
        }

        private void player7DrinkOk_Click(object sender, EventArgs e)
        {
            this.labelDrinkPlayer7.Text = "";
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

            participants = engine.LoadParticipantsFromMatchId(saveGame.MatchId);
            LoadPlayersFromSaveGame(saveGame);
            var playersAndOwners = GetPlayersAndOwners(saveGame);
            UpdatePlayerColorsFromOwners(playersAndOwners);
            matchIdLabel.Text = saveGame.MatchId.ToString();
            CalculateGraph("");
            progressBar.Value = saveGame.ProgressBarValue*60;
            InitWithdrawalParticipantCombo(participants);
        }

        private void LoadPlayersFromSaveGame(SaveGame saveGame)
        {
            var players = engine.LoadPlayersFromSaveGame(saveGame);
            var participantsFlowPanels = participantsPanel.Controls.OfType<FlowLayoutPanel>();
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
                    //case (20):
                    //    refereePlayer.Text = player.Name;
                    //    break;
                }
            }
        }

        private Dictionary<Player, string> GetPlayersAndOwners(SaveGame saveGame)
        {
            return engine.GetPlayersAndOwners(saveGame);
        }

        #region Send to bank
        private void bankButton1_Click(object sender, EventArgs e)
        {
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer1.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer1.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer1.Text = newBank.ToString();
            this.labelDrinkPlayer1.Text = "";
        }

        private void bankButton2_Click(object sender, EventArgs e)
        {
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer2.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer2.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer2.Text = newBank.ToString();
            this.labelDrinkPlayer2.Text = "";
        }

        private void bankButton3_Click(object sender, EventArgs e)
        {
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer3.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer3.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer3.Text = newBank.ToString();
            this.labelDrinkPlayer3.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer4.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer4.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer4.Text = newBank.ToString();
            this.labelDrinkPlayer4.Text = "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Tennedz
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer5.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer5.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer5.Text = newBank.ToString();
            this.labelDrinkPlayer5.Text = "";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Trusser
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer6.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer6.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer6.Text = newBank.ToString();
            this.labelDrinkPlayer6.Text = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //AAllex
            int drinksToTransfer = 0;
            var measureText = labelDrinkPlayer7.Text;
            drinksToTransfer = ParseLabelTextToDrinkSize(measureText);
            int currentBank = 0;
            int.TryParse(bankPlayer7.Text, out currentBank);
            var newBank = drinksToTransfer + currentBank;
            this.bankPlayer7.Text = newBank.ToString();
            this.labelDrinkPlayer7.Text = "";
        }
        #endregion

        private int ParseLabelTextToDrinkSize(string measureText)
        {
            if (string.Compare(measureText, "lille", true) == 0)
            {
                return 1;
            }
            else if (string.Compare(measureText, "memmel", true) == 0)
            {
                return 3;
            }
            else if (string.Compare(measureText, "stor", true) == 0)
            {
                return 6;
            }
            else if (string.Compare(measureText, "walter", true) == 0)
            {
                return 11;
            }
            return 0;
        }

        private void withdrawalButton_Click(object sender, EventArgs e)
        {
            if (withdrawalParticipantCombo.SelectedItem == null || withdrawalMeasureCombo.SelectedItem == null)
            {
                infoLabel.Text = "Vælg en deltager OG en størrelse.";
                return;
            }
            infoLabel.Text = "";
            var selectedParticipant = withdrawalParticipantCombo.SelectedItem;
            var selectedMeasure = (DbFacade.Measures)withdrawalMeasureCombo.SelectedItem;

            if (selectedParticipant.ToString().Equals(participantTextBox1.Text))
            {
                int newBank = GetNewBank(bankPlayer1.Text, (int)selectedMeasure);
                bankPlayer1.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox2.Text))
            {
                int newBank = GetNewBank(bankPlayer2.Text, (int)selectedMeasure);
                bankPlayer2.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox3.Text))
            {
                int newBank = GetNewBank(bankPlayer3.Text, (int)selectedMeasure);
                bankPlayer3.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox4.Text))
            {
                int newBank = GetNewBank(bankPlayer4.Text, (int)selectedMeasure);
                bankPlayer4.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox5.Text))
            {
                int newBank = GetNewBank(bankPlayer5.Text, (int)selectedMeasure);
                bankPlayer5.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox6.Text))
            {
                int newBank = GetNewBank(bankPlayer6.Text, (int)selectedMeasure);
                bankPlayer6.Text = newBank < 0 ? "0" : newBank.ToString();
            }
            else if (selectedParticipant.ToString().Equals(participantTextBox7.Text))
            {
                int newBank = GetNewBank(bankPlayer7.Text, (int)selectedMeasure);
                bankPlayer7.Text = newBank < 0 ? "0" : newBank.ToString();
            }
        }

        private int GetNewBank(string currentBank, int selectedMeasure)
        {
            var currentBankInt = 0;
            var numericBank = int.TryParse(currentBank, out currentBankInt) ? currentBankInt : 0;
            return numericBank - selectedMeasure < 0 ? 0 : numericBank - selectedMeasure;
        }
    }
}
