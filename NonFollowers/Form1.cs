using Newtonsoft.Json;
using NonFollowers.Classes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NonFollowers
{
    public partial class Form1 : Form
    {
        private string _followersFilePath = string.Empty;
        private string _followingFilePath = string.Empty;

        private List<Follower>? _followers = new List<Follower>();
        private Following? _following = null;
        private List<User>? _users = new List<User>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeOpenFileDialog();
        }


        #region Logic Methods
        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1.Filter =
                "Json (*.JSON)|*.JSON|" +
                "All files (*.*)|*.*";

            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "My Image Browser";
        }

        private List<T>? LoadJsonToObjectList<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");


            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                List<T>? items = JsonConvert.DeserializeObject<List<T>>(json);

                return items;
            }
        }

        private static T? LoadJsonToObject<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                T? item = JsonConvert.DeserializeObject<T>(json);

                return item;
            }
        }
        #endregion

        private void btnFollowers_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                _followersFilePath += openFileDialog1.FileName;
                txtFollowersFilePath.Text = _followersFilePath;
            }

            EnableCompareButtton();
        }

        private void btnFollowing_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                _followingFilePath += openFileDialog1.FileName;
                txtFollowingFilePath.Text = _followersFilePath;
            }

            EnableCompareButtton();
        }

        private void EnableCompareButtton()
        {
            if (!string.IsNullOrWhiteSpace(_followingFilePath) && !string.IsNullOrWhiteSpace(_followersFilePath))
                btnCompare.Enabled = true;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                _followers = null;
                _following = null;
                _users = null;

                _followers = LoadJsonToObjectList<Follower>(_followersFilePath);
                _following = LoadJsonToObject<Following>(_followingFilePath);

                _users = new List<User>();

                //foreach(var followingUsr in _following.relationships_following)
                //{
                //    bool foundInFollowers = false;

                //    var followingUrl = followingUsr.string_list_data.FirstOrDefault().href;
                //    foreach (var followerUsr in _followers)
                //    {
                //        var followerUrl = followerUsr.string_list_data.FirstOrDefault().href;
                //        if (followingUrl ==  followerUrl)
                //            foundInFollowers = true;
                //    }

                //    if(!foundInFollowers)
                //    {
                //        _users.Add(new User { Username = followingUsr.title, Url = followingUsr.string_list_data.FirstOrDefault().href });
                //    }
                //}

                foreach (var followingUsr in _following.relationships_following)
                {
                    bool foundInFollowers = false;

                    var followingUsername = followingUsr.title;

                    foreach (var followerUsr in _followers)
                    {
                        var followerUsername = followerUsr.string_list_data.FirstOrDefault().value;

                        if (followingUsername == followerUsername)
                        {
                            foundInFollowers = true;
                            continue;
                        }
                    }

                    if (!foundInFollowers)
                    {
                        _users.Add(new User { Username = followingUsr.title, Url = followingUsr.string_list_data.FirstOrDefault().href });
                    }
                }
                userBindingSource.DataSource = _users;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnNaviagateTo"].Index && e.RowIndex >= 0)
            {
                // Get the value of the cell in the second column (index 1)
                var value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;

                //MessageBox.Show("Value in column 2: " + value);

                //ProcessStartInfo sInfo = new ProcessStartInfo(value.ToString());
                //Process.Start(sInfo);

                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = value.ToString(),
                    UseShellExecute = true
                };
                Process.Start(psInfo);
            }
        }

        //private void userBindingSource_CurrentChanged(object sender, EventArgs e)
        //{
        //    userBindingSource.DataSource = _users;
        //    dataGridView1.Refresh();
        //}
    }
}
