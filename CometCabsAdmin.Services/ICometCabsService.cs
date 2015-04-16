using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CometCabsAdmin.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICometCabsServices
    {
        [OperationContract]
        bool Login(string userName, string password);
    }

    [DataContract]
    public class UserData
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

        [DataMember]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
