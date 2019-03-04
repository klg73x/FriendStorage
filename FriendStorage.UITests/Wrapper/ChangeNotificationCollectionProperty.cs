using FriendStorage.Model;
using FriendStorage.UI.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FriendStorage.UITests.Wrapper
{
    [TestClass]
    public class ChangeNotificationCollectionProperty
    {
        private FriendEmail _friendEmail;
        private Friend _friend;

        [TestInitialize]
        public void Initialize()
        {
            _friendEmail = new FriendEmail { Email = "thomas@thomasclaudiushuber.com" };
            _friend = new Friend
            {
                FirstName = "Thomas",
                Address = new Address(),
                Emails = new List<FriendEmail>
                {
                    new FriendEmail { Email="julia@jhu-design.com"},
                    _friendEmail
                }
            };
        }

        [TestMethod]
        public void ShouldInitializeEmailsProperty()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.IsNotNull(wrapper.Emails);
            CheckIfModelEmailsCollectionInInSync(wrapper);
        }

        [TestMethod]
        public void ShouldBeInSyncAfterRemovingEmail()
        {
            var wrapper = new FriendWrapper(_friend);
            var emailToRemove = wrapper.Emails.Single(ew => ew.Model == _friendEmail);
            wrapper.Emails.Remove(emailToRemove);
            CheckIfModelEmailsCollectionInInSync(wrapper);
        }

        [TestMethod]
        public void ShouldBeInSyncAfterAddingEmail()
        {
            _friend.Emails.Remove(_friendEmail);
            var wrapper = new FriendWrapper(_friend);
            wrapper.Emails.Add(new FriendEmailWrapper(_friendEmail));
            CheckIfModelEmailsCollectionInInSync(wrapper);
        }

        private void CheckIfModelEmailsCollectionInInSync(FriendWrapper wrapper)
        {
            Assert.AreEqual(_friend.Emails.Count, wrapper.Emails.Count);
            Assert.IsTrue(_friend.Emails.All(e =>
                            wrapper.Emails.Any(we => we.Model == e)));
        }
    }
}
