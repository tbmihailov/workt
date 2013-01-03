using GalaSoft.MvvmLight;
using WorkT.Model;

namespace WorkT.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        public GroupViewModel()
        {
        }

        private Group _group;
        public Group Group
        {
            get
            {
                if (_group == null)
                {
                    _group = new Group();
                }
                return _group;
            }
            private set { _group = value; }
        }

        public GroupViewModel(Group group)
        {
            this.Group = group;
        }

        private const string GroupIdPropertyName = "GroupId";
        public int GroupId
        {
            get
            {
                return _group.GroupId;
            }

            set
            {
                if (_group.GroupId == value)
                {
                    return;
                }
                _group.GroupId = value;
                RaisePropertyChanged(GroupIdPropertyName);
            }
        }

        private const string NamePropertyName = "Name";
        public string Name
        {
            get
            {
                return _group.Name;
            }

            set
            {
                if (_group.Name == value)
                {
                    return;
                }
                _group.Name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        private const string DescriptionPropertyName = "Description";
        public string Description
        {
            get
            {
                return _group.Description;
            }

            set
            {
                if (_group.Description == value)
                {
                    return;
                }
                _group.Description = value;
                RaisePropertyChanged(DescriptionPropertyName);
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}