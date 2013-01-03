using System.Collections.ObjectModel;
using WorkT.Model;
using System.Collections.Generic;

namespace WorkT.DesignModel
{
    public class DesignGroups : ObservableCollection<Group>
    {

        public DesignGroups()
            : this(50)
        {

        }

        public DesignGroups(int count)
        {
            var groups = GenerateGroups(count);
            foreach (Group group in groups)
            {
                this.Add(group);
            }

        }

        private static IEnumerable<Group> GenerateGroups(int count)
        {
            var groups = new List<Group>();
            for (int i = 0; i < count; i++)
            {
                var newGroup = new Group()
                {
                    GroupId = i,
                    Name = string.Format("Group {0}", i),
                    Description = string.Format("Group {0} is a test generated group", i),
                };
                groups.Add(newGroup);
            }

            return groups;
        }
    }
}
