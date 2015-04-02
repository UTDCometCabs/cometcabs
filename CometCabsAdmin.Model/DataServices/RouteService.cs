using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Model.DataServices
{
    public class RouteService : IRouteService
    {
        private IRepository<Route> _routeRepository;
        private IRepository<RouteCoordinates> _routeCoordinateRepository;

        public RouteService(IRepository<Route> routeRepository
            , IRepository<RouteCoordinates> routeCoordinateRepository)
        {
            _routeRepository = routeRepository;
            _routeCoordinateRepository = routeCoordinateRepository;
        }
        #region IRouteService Members

        public IQueryable<Entities.Route> GetRoutes()
        {
            return _routeRepository.Table;
        }

        public Entities.Route GetRoute(long id)
        {
            return _routeRepository.GetById(id);
        }

        public void InsertRoute(Entities.Route route)
        {
            _routeRepository.Insert(route);
        }

        public void UpdateRoute(Entities.Route route)
        {
            _routeRepository.Update(route);
        }

        public void DeleteRoute(Entities.Route route)
        {
            DeleteCoordinateByRoute(route);
            _routeRepository.Delete(route);
        }


        public void DeleteRouteCoordinate(Entities.Route route)
        {
            DeleteCoordinateByRoute(route);
        }

        #endregion

        private void DeleteCoordinateByRoute(Entities.Route route)
        {
            foreach (RouteCoordinates coordinate in route.RouteCoordinates)
            {
                _routeCoordinateRepository.Delete(coordinate);
            }
        }
    }
}
