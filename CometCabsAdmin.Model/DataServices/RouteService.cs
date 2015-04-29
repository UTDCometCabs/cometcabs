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
        private IRepository<RouteDirections> _routeDirectionsRepository;

        public RouteService(IRepository<Route> routeRepository
            , IRepository<RouteCoordinates> routeCoordinateRepository
            , IRepository<RouteDirections> routeDirectionsRepository)
        {
            _routeRepository = routeRepository;
            _routeCoordinateRepository = routeCoordinateRepository;
            _routeDirectionsRepository = routeDirectionsRepository;
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
            _routeRepository.Delete(route);
        }

        public IQueryable<Entities.RouteCoordinates> GetRouteCoordinates(long id)
        {
            return _routeCoordinateRepository.Table
                .Where(s => s.RouteId.Equals(id));
        }

        public Entities.RouteCoordinates GetRouteCoordinate(float latitude, float longitude)
        {
            Entities.RouteCoordinates result = _routeCoordinateRepository.Table
                .Where(s => (Math.Abs(Math.Round(s.Latitude, 3) - (Math.Round(latitude, 3))) == 0)
                    && ((Math.Round(s.Longitude, 3) - Math.Round(longitude, 3)) == 0))
                .FirstOrDefault();

            return result;
        }

        public void DeleteRouteCoordinate(Entities.RouteCoordinates coordinate)
        {
            _routeCoordinateRepository.Delete(coordinate);
        }

        public void DeleteRouteCoordinate(long id)
        {
            IQueryable<RouteCoordinates> coordinates = _routeCoordinateRepository.Table
                .Where(s => s.RouteId.Equals(id));

            foreach (RouteCoordinates coordinate in coordinates)
            {
                _routeCoordinateRepository.Delete(coordinate);
            }
        }

        public IQueryable<RouteDirections> GetRouteDirections(long id)
        {
            return _routeDirectionsRepository.Table
                .Where(s => s.RouteId.Equals(id));
        }

        public void DeleteRouteDirections(long id)
        {
            IQueryable<RouteDirections> directions = _routeDirectionsRepository.Table
                .Where(s => s.RouteId.Equals(id));

            foreach (RouteDirections direction in directions)
            {
                _routeDirectionsRepository.Delete(direction);
            }
        }

        public void DeleteRouteDirections(RouteDirections routeDirection)
        {
            _routeDirectionsRepository.Delete(routeDirection);
        }

        #endregion
    }
}
