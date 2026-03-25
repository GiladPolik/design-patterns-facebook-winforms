using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public sealed class FacebookSession
    {
        private static FacebookSession s_Instance = null;
        private static readonly object sr_Lock = new object();
        private const string k_AppId = "853850367094583";
        private readonly string[] r_Permissions = {
            "email",
            "public_profile",
            "user_likes",
            "user_posts",
            "user_photos",
            "publish_actions"
        };

        public LoginResult LoginResult { get; private set; }
        public User LoggedInUser
        {
            get
            {
                return LoginResult != null ? LoginResult.LoggedInUser : null;
            }
        }

        private FacebookSession() // private empty ctor
        {
        }

        public static FacebookSession Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (sr_Lock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new FacebookSession();
                        }
                    }
                }

                return s_Instance;
            }
        }

        private readonly List<ILoginObserver> m_LoginObservers = new List<ILoginObserver>();

        public void Attach(ILoginObserver i_Observer)
        {
            m_LoginObservers.Add(i_Observer);
        }

        public void Detach(ILoginObserver i_Observer)
        {
            m_LoginObservers.Remove(i_Observer);
        }

        private void notifyLoginSuccess()
        {
            foreach (ILoginObserver observer in m_LoginObservers)
            {
                observer.OnLoginSuccess();
            }
        }

        private void notifyLoginFailed(string i_Error)
        {
            foreach (ILoginObserver observer in m_LoginObservers)
            {
                observer.OnLoginFailed(i_Error);
            }
        }
        public void Login()
        {
            LoginResult = FacebookService.Login(k_AppId, r_Permissions);
            if (LoggedInUser != null)
            {
                notifyLoginSuccess();
            }
            else
            {
                notifyLoginFailed(LoginResult.ErrorMessage);
            }
        }

        public void Logout()
        {
            FacebookService.LogoutWithUI();
            LoginResult = null;
            foreach (ILoginObserver observer in m_LoginObservers)
            {
                observer.OnLogout();
            }
        }

        public void ConnectWithToken(string i_Token)
        {
            LoginResult = FacebookService.Connect(i_Token);
            if (LoggedInUser != null)
            {
                notifyLoginSuccess();
            }
            else
            {
                notifyLoginFailed(LoginResult.ErrorMessage);
            }
        }
    }
}
