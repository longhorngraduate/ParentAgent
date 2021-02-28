using ParentAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ParentAgent.Helpers
{
    [Serializable]
    public sealed class SessionSingleton
    {
        #region Singleton
        private const string SESSION_SINGLETON_NAME = "mySessionObj_10_1_2020";

        private SessionSingleton()
        {
        }

        public static SessionSingleton Current
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_SINGLETON_NAME] == null)
                {
                    HttpContext.Current.Session[SESSION_SINGLETON_NAME] = new SessionSingleton();
                }

                return (SessionSingleton)HttpContext.Current.Session[SESSION_SINGLETON_NAME];
            }
        }
        #endregion

        public int ParentId { get; set; }
        public Parent oParent { get; set; }
    }


    public class MessageVM
    {
        public MessageVM()
        {
        }

        public bool IsSuccessful { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }


    public static class Helpers
    {
        public static bool IsStaff(IPrincipal User)
        {
            if (User.IsInRole("omniscient") || User.IsInRole("staff"))
                return true;

            return false;
        }
    }
}