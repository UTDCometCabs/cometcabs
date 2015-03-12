using CometCabsAdmin.Model.Contracts;

namespace CometCabsAdmin.Model.Common
{
    public class Connections : IConnections
    {
        private string _cometCabsConnectionString;

        public Connections(string cometCabsConnectionString)
        {
            _cometCabsConnectionString = cometCabsConnectionString;
        }

        #region IConnections Members

        public string CometCabsConnectionString
        {
            get
            {
                return _cometCabsConnectionString;
            }
        }

        #endregion
    }
}
