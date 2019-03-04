using FriendStorage.Model;
using System;

namespace FriendStorage.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {
            InitializeComplexProperties(model);
        }

        private void InitializeComplexProperties(Friend model)
        {
            if (model.Address == null)
            {
                throw new ArgumentException("Address cannot be null");
            }
            Address = new AddressWrapper(model.Address);
        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int FriendGroupId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? Birthday
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool IsDeveloper
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public AddressWrapper Address
        {
            get;
            private set;
        }
    }
}
