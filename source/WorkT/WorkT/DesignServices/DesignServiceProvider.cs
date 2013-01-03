using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkT.Services;

namespace WorkT.DesignServices
{
    public class DesignServiceProvider : ServiceProviderBase
    {
        public DesignServiceProvider()
        {
            GroupsDataService = new DesignGroupsDataService();
        }
    }
}
