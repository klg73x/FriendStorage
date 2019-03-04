using FriendStorage.Model;

namespace FriendStorage.UI.Wrapper
{
    public class FriendEmailWrapper : ModelWrapper<FriendEmail>
    {
        public FriendEmailWrapper(FriendEmail model) : base(model)
        {

        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
