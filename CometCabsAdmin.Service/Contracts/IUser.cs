using CometCabsAdmin.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CometCabsAdmin.Service.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IUser
    {
        [OperationContract]
        Model.Entities.User Login(string userName, string password);

        [OperationContract]
        Dictionary<int, string> SaveUser(UserData data);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "CometCabsAdmin.Service.ContractType".
    [DataContract]
    public class UserData
    {
        private IEncryption _encription;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _emailAddress = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _address = string.Empty;
        private string _ipAddress = string.Empty;

        public UserData(IEncryption encryption)
        {
            _encription = encryption;
        }

        [DataMember]
        public string Name
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        [DataMember]
        public string Password
        {
            get
            {
                return _encription.Decrypt(_password);
            }
            set
            {
                _password = _encription.Encrypt(value);
            }
        }

        [DataMember]
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value;
            }
        }

        [DataMember]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        [DataMember]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        [DataMember]
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        [DataMember]
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }
    }
}
