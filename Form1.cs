using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banchecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<string> steamIds = new List<string>();
        String apiKey = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string json = File.ReadAllText("apikey.json");
                apiKey = JsonConvert.DeserializeObject<string>(json);
            }
            catch (FileNotFoundException)
            {
                textBox1.Visible = true;
                button4.Visible = true;
                button2.Enabled = false;
            }

            try
            {
                // Read the saved Steam IDs from a file
                string json = File.ReadAllText("steam_ids.json");

                // Deserialize the JSON string to a List<string>
                steamIds = JsonConvert.DeserializeObject<List<string>>(json);

                // Display the saved Steam IDs in the SteamIdsTextBox
                SteamIdsTextBox.Text = string.Join(",", steamIds);

                button2.PerformClick();
            }
            catch (FileNotFoundException)
            {
                // If the file is not found, there are no saved Steam IDs
                steamIds = new List<string>();
            }


        }

        public async Task<bool> isBannedAsync(string steamid)
        {
            var playerBans = await GetPlayerBans(steamid);
            if (playerBans.NumberOfGameBans > 0 || playerBans.NumberOfVACBans > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<PlayerBans> GetPlayerBans(string steamid)
        {
            using (var client = new HttpClient())
            {
                var url = $"https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/?key={apiKey}&steamids={steamid}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<PlayerBansResponse>(json);
                    return data.PlayerBans[0];
                }
                else
                {
                    throw new Exception($"Failed to retrieve player bans for steamid {steamid}. Status code: {response.StatusCode}");
                }
            }
        }

        public class PlayerBansResponse
        {
            [JsonProperty("players")]
            public PlayerBans[] PlayerBans { get; set; }
        }

        public class PlayerBans
        {
            [JsonProperty("SteamId")]
            public string SteamId { get; set; }

            [JsonProperty("CommunityBanned")]
            public bool CommunityBanned { get; set; }

            [JsonProperty("VACBanned")]
            public bool VACBanned { get; set; }

            [JsonProperty("NumberOfVACBans")]
            public int NumberOfVACBans { get; set; }

            [JsonProperty("DaysSinceLastBan")]
            public int DaysSinceLastBan { get; set; }

            [JsonProperty("EconomyBan")]
            public string EconomyBan { get; set; }

            [JsonProperty("GameBan")]
            public int GameBan { get; set; }

            [JsonProperty("NumberOfGameBans")]
            public int NumberOfGameBans { get; set; }

            [JsonProperty("BanCount")]
            public int BanCount { get; set; }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Retrieve the SteamIDs entered in the text box and split them into an array
            string steamIdsText = SteamIdsTextBox.Text;
            string[] steamIds = steamIdsText.Split(',');
            var x = 12;
            var y = 143;
            // Loop through each SteamID in the array and retrieve profile information and ban status
            foreach (string steamId in steamIds)
            {
                // Retrieve profile information
                PlayerSummary playerSummary = await GetPlayerSummary(steamId);

                // Retrieve ban status
                PlayerBans playerBans = await GetPlayerBans(steamId);

                // Display profile picture and name in your program
                PictureBox pictureBox = new PictureBox();
                pictureBox.ImageLocation = playerSummary.AvatarMedium;
                Label nameLabel = new Label();
                nameLabel.Text = playerSummary.PersonaName;

                // Display ban status in your program
                Label banStatusLabel = new Label();
                if (await isBannedAsync(steamId))
                {
                    banStatusLabel.Text = "Banned";
                    banStatusLabel.ForeColor = Color.Red;
                }
                else
                {
                    banStatusLabel.Text = "Unbanned";
                    banStatusLabel.ForeColor = Color.Green;
                }

                // set location
                pictureBox.Location = new Point(x, y);
                pictureBox.Height = 80;
                nameLabel.Location = new Point(x + pictureBox.Width + 10, y);
                banStatusLabel.Location = new Point(x + pictureBox.Width + 10 + nameLabel.Width + 10, y);

                // set properties
                nameLabel.ForeColor = Color.White;
                banStatusLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                nameLabel.Font = new Font("Arial", 14, FontStyle.Regular);
                banStatusLabel.AutoSize = true;

                // Add the controls to your form
                this.Controls.Add(pictureBox);
                this.Controls.Add(nameLabel);
                this.Controls.Add(banStatusLabel);
                y += 100;
            }
        }


        public async Task<PlayerSummary> GetPlayerSummary(string steamId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={apiKey}&steamids={steamId}");
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RootObject<PlayerSummary>>(content);
                return result.Response.Players.FirstOrDefault();
            }
        }
        public class RootObject<T>
        {
            public T Response { get; set; }
        }


        public class PlayerSummary
        {
            public List<PlayerSummary> Players { get; set; }
            public string SteamId { get; set; }
            public string PersonaName { get; set; }
            public string ProfileUrl { get; set; }
            public string Avatar { get; set; }
            public string AvatarMedium { get; set; }
            public string AvatarFull { get; set; }
            public string PersonaState { get; set; }
            public int CommunityVisibilityState { get; set; }
            public int ProfileState { get; set; }
            public string LastLogoff { get; set; }
            public int CommentPermission { get; set; }
            public string RealName { get; set; }
            public string PrimaryClanId { get; set; }
            public int TimeCreated { get; set; }
            public int GameId { get; set; }
            public string GameExtraInfo { get; set; }
            public string GameServerIp { get; set; }
            public string CityId { get; set; }
            public string LocCountryCode { get; set; }
            public string LocStateCode { get; set; }
            public string LocCityId { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Split the Steam IDs in the SteamIdsTextBox and add them to the list
            steamIds = SteamIdsTextBox.Text.Split(',').Select(id => id.Trim()).ToList();

            // Serialize the list to a JSON string and save it to a file
            string json = JsonConvert.SerializeObject(steamIds);
            File.WriteAllText("steam_ids.json", json);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the stored user list?", "Caution", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // Delete the stored user list file
                File.Delete("steam_ids.json");

                // Clear the SteamIDsTextBox
                SteamIdsTextBox.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string input = JsonConvert.SerializeObject(textBox1.Text);
            File.WriteAllText("apikey.json", input);
            apiKey = textBox1.Text;
            button2.Enabled = true;
        }
    }
}
