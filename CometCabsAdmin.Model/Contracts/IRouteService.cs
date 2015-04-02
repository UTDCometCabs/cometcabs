using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IRouteService
    {
        IQueryable<Route> GetRoutes();
        Route GetRoute(long id);
        void InsertRoute(Route route);
        void UpdateRoute(Route route);
        void DeleteRoute(Route route);
        void DeleteRouteCoordinate(Route route);
    }
}
