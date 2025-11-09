using Newtonsoft.Json;
using NonFollowers.Classes;
using NonFollowers.Methods;

namespace NonFollowers
{
    public partial class MainForm : Form
    {
        #region Private Members
        private string _followersFilePath = string.Empty;
        private string _followingFilePath = string.Empty;

        private List<Follower>? _followers = new List<Follower>();
        private Following? _following = null;
        private List<User>? _users = new List<User>();
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeOpenFileDialog();
        }

        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1.Filter =
                "Json (*.JSON)|*.JSON|" +
                "All files (*.*)|*.*";

            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "My Image Browser";
        }
       
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

        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                ContextCleanup();
                LoadData();

                #region Validations
                if(_following == null)
                    throw new Exception("Following data could not be loaded.");

                if(_following.relationships_following == null || !_following.relationships_following.Any())
                    throw new Exception("Following relationships data could not be loaded.");

                if (_followers == null)
                    throw new Exception("Followers data could not be loaded.");

                if (!_followers.Any())
                    throw new Exception("No followers found in the followers data.");

                if(_users == null)
                    throw new Exception("Users list could not be initialized.");
                #endregion

                foreach (var followingUsr in _following.relationships_following)
                {
                    bool foundInFollowers = false;
                    var followingUsername = followingUsr.title;

                    foreach (var followerUsr in _followers)
                    {

                        string? followerUsername = followerUsr.string_list_data?.FirstOrDefault()?.value;

                        if (followingUsername == followerUsername)
                        {
                            foundInFollowers = true;
                            continue;
                        }
                    }

                    if (!foundInFollowers)
                    {
                        _users.Add(new User { Username = followingUsr.title ?? string.Empty, Url = followingUsr?.string_list_data?.FirstOrDefault()?.href ?? string.Empty });
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
            if (e.ColumnIndex == dataGridView1?.Columns["btnNaviagateTo"]?.Index && e.RowIndex >= 0)
            {

                try
                {
                    var value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                    
                    if (value != null) 
                        LogicMethods.OpenInBrowser(value.ToString());
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        #region Form Methods
        private void EnableCompareButtton()
        {
            if (!string.IsNullOrWhiteSpace(_followingFilePath) && !string.IsNullOrWhiteSpace(_followersFilePath))
                btnCompare.Enabled = true;
        }
        private void ContextCleanup()
        {
            _followers = null;
            _following = null;
            _users = null;
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        private void ResetForm()
        {
            txtFollowersFilePath.Text = string.Empty;
            txtFollowingFilePath.Text = string.Empty;
            btnCompare.Enabled = false;
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void LoadData()
        {
            _followers = LogicMethods.LoadJsonToObjectList<Follower>(_followersFilePath);
            _following = LogicMethods.LoadJsonToObject<Following>(_followingFilePath);
            _users = new List<User>();
        }
        #endregion

        #region Control Events
        #endregion
    }
}
