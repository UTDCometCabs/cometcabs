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

        IQueryable<Entities.RouteCoordinates> GetRouteCoordinates(long id);
        RouteCoordinates GetRouteCoordinate(float latitude, float longitude);
        void DeleteRouteCoordinate(RouteCoordinates coordinate);
        void DeleteRouteCoordinate(long id);
    }
}
