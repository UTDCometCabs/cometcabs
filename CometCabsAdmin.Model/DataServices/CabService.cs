using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.DataServices
{
    public class CabService : ICabService
    {
        private IRepository<Cab> _cabRepository;
        private IRepository<CabActivity> _cabActivityRepository;
        private IRepository<CabCoordinate> _cabCoordinateRepository;

        public CabService(IRepository<Cab> cabRepository
            , IRepository<CabActivity> cabActivityRepository
            , IRepository<CabCoordinate> cabCoordinateRepository)
        {
            _cabRepository = cabRepository;
            _cabActivityRepository = cabActivityRepository;
            _cabCoordinateRepository = cabCoordinateRepository;
        }

        #region ICabService Members

        public IQueryable<Entities.Cab> GetCabs()
        {
            return _cabRepository.Table;
        }

        public Entities.Cab GetCab(long id)
        {
            return _cabRepository.GetById(id);
        }

        public void InsertCab(Entities.Cab Cab)
        {
            _cabRepository.Insert(Cab);
        }

        public void UpdateCab(Entities.Cab Cab)
        {
            _cabRepository.Update(Cab);
        }

        public void DeleteCab(Entities.Cab Cab)
        {
            _cabRepository.Delete(Cab);
        }

        public IQueryable<CabActivity> GetCabActivity()
        {
            return _cabActivityRepository.Table;
        }

        public CabActivity GetCabActivity(long id)
        {
            return _cabActivityRepository.GetById(id);
        }

        public long InsertCabActivity(CabActivity activity)
        {
            _cabActivityRepository.Insert(activity);

            return activity.Id;
        }

        public IQueryable<CabCoordinate> GetCabCoordinates()
        {
            return _cabCoordinateRepository.Table;
        }

        public CabCoordinate GetCabCoordinate(long id)
        {
            return _cabCoordinateRepository.GetById(id);
        }

        public void InsertCabCoordinate(CabCoordinate coordinate)
        {
            _cabCoordinateRepository.Insert(coordinate);
        }

        public void UpdateCabCoordinate(CabCoordinate coordinate)
        {
            _cabCoordinateRepository.Insert(coordinate);
        }

        #endregion
    }
}
