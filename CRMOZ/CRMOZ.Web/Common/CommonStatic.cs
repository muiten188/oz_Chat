using CRMOZ.Model.Models;
using CRMOZ.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRMOZ.Web.Common
{
    public static class CommonStatic
    {
        public static List<HubUser> OnlineUsers { set; get; }
        public static List<Interactive> Interactives { set; get; }
        public static List<InteracGroup> InteracGroups { set; get; }

        public static void AddOnlineUser(HubUser hubUser)
        {
            if(OnlineUsers == null)
            {
                OnlineUsers = new List<HubUser>();
            }

            if (OnlineUsers.FirstOrDefault(p => p.ID == hubUser.ID) == null)
            {
                OnlineUsers.Add(hubUser);
            }
        }

        public static void RemoveOnlineUser(string userID)
        {
            if (OnlineUsers != null)
            {
                var onlineUser = OnlineUsers.FirstOrDefault(p => p.ID == userID);
                if (onlineUser != null)
                {
                    OnlineUsers.Remove(onlineUser);
                }
            }
        }

        public static void AddInteractive(string userId, string recieveId)
        {
            if (Interactives == null)
            {
                Interactives = new List<Interactive>();
            }
            if (Interactives.FirstOrDefault(p => p.FromID == userId && p.ReceiveID == recieveId) == null)
            {
                var interactive = new Interactive() { FromID = userId, ReceiveID = recieveId };
                Interactives.Add(interactive);
            } 
        }

        public static void RemoveInteractives(string userId)
        {
            if (Interactives != null)
            {
                var interactive = Interactives.FirstOrDefault(p => p.FromID == userId);
                if (interactive != null)
                {
                    Interactives.Remove(interactive);
                }
            }
        }

        public static void AddInteracGroup(string userId, int groupId)
        {
            if (InteracGroups == null)
            {
                InteracGroups = new List<InteracGroup>();
            }
            if (InteracGroups.FirstOrDefault(p => p.UserID == userId && p.GroupID == groupId) == null)
            {
                var interacGroup = new InteracGroup() { UserID = userId, GroupID = groupId };
                InteracGroups.Add(interacGroup);
            }
        }

        public static void RemoveInteracGroup(string userId)
        {
            if (InteracGroups != null)
            {
                var interacGroup = InteracGroups.FirstOrDefault(p => p.UserID == userId);
                if (interacGroup != null)
                {
                    InteracGroups.Remove(interacGroup);
                }
            }
        }
    }
}