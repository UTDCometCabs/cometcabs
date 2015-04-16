using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CometCabsAdmin.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICometCabsServices
    {
        [OperationContract]
        bool Login(string userName, string password);

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetRouteData();
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

    [DataContract]
    public class RouteData
    {
        private string _routeName;
        private string _routeColor;
        private List<Coordinate> _coordinates;

        [DataMember]
        public string RouteName
        {
            get
            {
                return _routeName;
            }
            set
            {
                _routeName = value;
            }
        }

        [DataMember]
        public string RouteColor
        {
            get
            {
                return _routeColor;
            }
            set
            {
                _routeColor = value;
            }
        }

        [DataMember]
        public List<Coordinate> Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
            }
        }

    }

    public struct Coordinate
    {
        public string k { get; set; }
        public string D { get; set; }
    }
}
