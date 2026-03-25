using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.NetworkInformation;
using System.Threading;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form, ILoginObserver
    {
        public FormMain()
        {
            InitializeComponent();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            FacebookSession.Instance.Attach(this);
            comboBoxStrategy.SelectedIndex = 0;
        }

        private User m_LoggedInUser;
        private WeeklyTrafficFacade m_WeeklyTrafficFacade;
        private PostActivityResult m_BestPost;
        private List<Photo> m_AlbumPhotos;
        private IIterator<Photo> m_PhotoIterator;
        private Criteria m_PostCriteria;
        private bool m_IsAnalyzing = false;
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            FacebookSession.Instance.Login();
        }

        private void buttonConnectAsDesig_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookSession.Instance.ConnectWithToken
                    ("EAAUm6cZC4eUEBQTAa3rRgO39UZCIJLeD9OpF5SYAevqSaFI16sfjT6JznpAUbyX5Soyj4Uv2ZBRkesoHO9omNcJ3KSYPZCExgaKrIprACUMIVnhiHzT5a46zbdC2VkvZC04n1ZARj8WmvOCYyuIdmRZBNjtWZCFJrbjFoms5t3sU8G9dO1xDCYH7kkfU67heIUZCFDIuTtL0CzF2JUHBpRpwPdXYilOJW811z3C5fY9TOyBiUwZAqx4ZAV6YS5ZBBtYKdsb7");
                m_LoggedInUser = FacebookSession.Instance.LoggedInUser;
                afterLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(FacebookSession.Instance.LoginResult.ErrorMessage, "Login Failed");
            }
        }

        private void afterLogin()
        {
            User user = m_LoggedInUser;

            buttonLogin.Text = $"Logged in as {user.Name}";
            buttonLogin.BackColor = Color.LightGreen;
            if (!string.IsNullOrEmpty(user.PictureNormalURL))
            {
                pictureBoxProfile.ImageLocation = user.PictureNormalURL;
            }

            pictureBoxProfile.ImageLocation = user.PictureNormalURL;
            buttonLogin.Enabled = false;
            buttonLogout.Enabled = true;
            m_PostCriteria = new Criteria(PostDateFrom.Value, PostDateTo.Value);
            criteriaBindingSource.DataSource = m_PostCriteria;
            comboBoxStrategy.SelectedIndexChanged += (sender, e) => analyzePosts();
            m_PostCriteria.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Criteria.DateFrom) || e.PropertyName == nameof(Criteria.DateTo))
                {
                    analyzePosts();
                }
            };
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookSession.Instance.Logout();
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
        }

        private void buttonGenerateAlbum_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = AlbumDateFrom.Value;
            DateTime dateTo = AlbumDateTo.Value;
            string albumName = AlbumNameTextBox.Text;
            string albumDesc = albumDescriptionTextBox.Text;
            User currentUser = m_LoggedInUser;

            if (string.IsNullOrEmpty(albumName))
            {
                MessageBox.Show("Please enter a name for the album.");
                return;
            }

            buttonGenerateAlbum.Enabled = false;
            buttonGenerateAlbum.Text = "Generating...";
            Thread thread = new Thread(() =>
            {
                try
                {
                    Criteria criteria = new Criteria(dateFrom, dateTo);
                    PhotoFilter photoFilter = new PhotoFilter(criteria, currentUser);
                    List<Photo> photos = photoFilter.m_PhotoList;
                    IAlbumBuilder builder = new SmartAlbumBuilder(currentUser);

                    builder.BuildAlbumName(albumName);
                    builder.BuildPhotoList(photos);
                    builder.BuildDescription(albumDesc);
                    Album newAlbum = builder.GetResult();

                    this.Invoke((Action)(() =>
                    {
                        if (newAlbum != null)
                        {
                            MessageBox.Show($"Album '{newAlbum.Name}' created successfully!");
                        }
                        else
                        {
                            MessageBox.Show($"Album '{albumName}' created successfully! (Simulation)");
                        }

                        m_AlbumPhotos = photos;
                        m_PhotoIterator = new PhotoIterator(photos);
                        updateAlbumUI(m_PhotoIterator.Next());
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }));
                }
                finally
                {
                    this.Invoke((Action)(() =>
                    {
                        buttonGenerateAlbum.Enabled = true;
                        buttonGenerateAlbum.Text = "Generate Smart Album";
                    }));
                }
            });

            thread.Start();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (m_PhotoIterator != null && m_PhotoIterator.HasPrevious())
            {
                updateAlbumUI(m_PhotoIterator.Previous());
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (m_PhotoIterator != null && m_PhotoIterator.HasNext())
            {
                updateAlbumUI(m_PhotoIterator.Next());
            }
        }

        private void updateAlbumUI(Photo i_Photo)
        {
            if (i_Photo != null)
            {
                pictureBoxAlbum.ImageLocation = i_Photo.PictureNormalURL;
            }
            else
            {
                pictureBoxAlbum.Image = Image.FromFile("facebook_block.jpeg");
            }
        }

        private void linkPosts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBoxPosts.Items.Clear();
            if(m_LoggedInUser != null)
            {
                foreach (Post post in m_LoggedInUser.Posts)
                {
                    string postMessage = string.IsNullOrEmpty(post.Message) ? "Couldn't read post" : post.Message;

                    listBoxPosts.Items.Add(postMessage);
                }
            }    
        }

        private void linkAlbums_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBoxAlbums.Items.Clear();
            if (FacebookSession.Instance.LoginResult != null)
            {
                foreach (Album album in m_LoggedInUser.Albums)
                {
                    listBoxAlbums.Items.Add(album.Name);
                }
            }
        }

        private void listBoxAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAlbums.SelectedItems != null)
            {
                Album selectedAlbum = m_LoggedInUser.Albums[listBoxAlbums.SelectedIndex];

                if (selectedAlbum.PictureAlbumURL != null)
                {
                    facebookPictureBoxAlbum.ImageLocation = selectedAlbum.PictureAlbumURL;
                }
                else
                {
                    facebookPictureBoxAlbum.Image = Image.FromFile("facebook_block.jpeg");
                }
            }
        }

        private void listBoxPosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxPostComments.Items.Clear();
            if (listBoxPosts.SelectedIndex >= 0)
            {
                Post selected = m_LoggedInUser.Posts[listBoxPosts.SelectedIndex];

                try
                {
                    foreach (Comment comment in selected.Comments)
                    {
                        listBoxPostComments.Items.Add($"{comment.From.Name}: {comment.Message}");
                    }
                }
                catch
                {
                    listBoxPostComments.Items.Add("Comments are blocked by Facebook API");
                }
            }
        }

        private void buttonSetStatus_Click(object sender, EventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                string statusText = textBoxStatus.Text.Trim();

                if (string.IsNullOrEmpty(statusText))
                {
                    MessageBox.Show("Please enter a status");
                    return;
                }

                try
                {
                    Status postedStatus = m_LoggedInUser.PostStatus(statusText);
                    textBoxStatus.Clear();
                }
                catch
                {
                    MessageBox.Show("Status didn't upload successfully");
                }
            }
        }
        private void linkPages_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBoxPages.Items.Clear();
            try
            {
                foreach (Page page in m_LoggedInUser.LikedPages)
                {
                    listBoxPages.Items.Add(page);
                }

                if (listBoxPages.Items.Count == 0)
                {
                    MessageBox.Show("No liked pages found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching pages: " + ex.Message);
            }
        }

        private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPages.SelectedItem is Page selected)
            {
                pictureBoxPage.ImageLocation = selected.PictureNormalURL;
            }
        }

        private void linkFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBoxFriends.Items.Clear();
            try
            {
                foreach (User friend in m_LoggedInUser.Friends)
                {
                    listBoxFriends.Items.Add(friend);
                }

                if (listBoxFriends.Items.Count == 0)
                {
                    MessageBox.Show("No friends found.");
                }
            }
            catch (Exception ex)
            {
                pictureBoxFriend.Image = Image.FromFile("facebook_block.jpeg");
                MessageBox.Show("Error fetching friends: " + ex.Message);
            }
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFriends.SelectedItem is User friend)
            {
                pictureBoxFriend.ImageLocation = friend.PictureNormalURL;
            }
        }

        private void analyzePosts()
        {
            if (m_IsAnalyzing || FacebookSession.Instance.LoginResult == null)
            { 
                return;
            }

            m_IsAnalyzing = true;
            IRatingStrategy selectedStrategy;
            if (comboBoxStrategy.SelectedIndex == 1) 
            {
                selectedStrategy = new LikeAndCommentStrategy();
            }
            else 
            {
                selectedStrategy = new LikeBasedStrategy();
            }

            WeeklyTrafficFacade trafficFacade = new WeeklyTrafficFacade();

            m_PostCriteria.AnalysisResult = "Calculating traffic...";
            Thread thread = new Thread(() =>
            {
                string result = trafficFacade.Analyze(m_PostCriteria, selectedStrategy);

                this.Invoke((Action)(() =>
                {
                    m_PostCriteria.AnalysisResult = result;
                    m_IsAnalyzing = false;
                }));
            });
            
            thread.Start();
        }

        public void OnLoginSuccess()
        {
            this.Invoke((Action)(() =>
            {
                m_LoggedInUser = FacebookSession.Instance.LoggedInUser;
                afterLogin();
            }));
        }

        public void OnLoginFailed(string i_ErrorMessage)
        {
            this.Invoke((Action)(() =>
            {
                MessageBox.Show(i_ErrorMessage, "Login Failed");
            }));
        }

        public void OnLogout()
        {
            this.Invoke((Action)(() =>
            {
                buttonLogin.Text = "Login";
                buttonLogin.BackColor = buttonLogout.BackColor;
                buttonLogin.Enabled = true;
                buttonLogout.Enabled = false;
                pictureBoxProfile.Image = null;
            }));
        }
    }
}
