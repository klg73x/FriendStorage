using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {

        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                if (!Equals(Model.FirstName, value))
                {
                    Model.FirstName = value;
                    OnPropertyChanged();
                }

            }
        }
        
    }
}
