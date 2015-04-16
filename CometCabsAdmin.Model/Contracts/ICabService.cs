using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Contracts
{
    public interface ICabService
    {
        IQueryable<Cab> GetCabs();
        Cab GetCab(long id);
        void InsertCab(Cab cab);
        void UpdateCab(Cab cab);
        void DeleteCab(Cab cab);

        IQueryable<CabActivity> GetCabActivity();
        CabActivity GetCabActivity(long id);
        long InsertCabActivity(CabActivity activity);

        IQueryable<CabCoordinate> GetCabCoordinates();
        CabCoordinate GetCabCoordinate(long id);
        void InsertCabCoordinate(CabCoordinate coordinate);

        //IQueryable<Entities.RouteCoordinates> GetRouteCoordinates(long id);
        //RouteCoordinates GetRouteCoordinate(float latitude, float longitude);
        //void DeleteRouteCoordinate(RouteCoordinates coordinate);
        //void DeleteRouteCoordinate(long id);
    }
}

