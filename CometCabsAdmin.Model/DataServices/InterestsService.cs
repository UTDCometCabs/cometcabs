using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.DataServices
{
    public class InterestsService : IInterestsService
    {
        private IRepository<Interests> _interestsRepository;

        public InterestsService(IRepository<Interests> interestsRepository)
        {
            _interestsRepository = interestsRepository;
        }

        #region IInterests Members

        public IQueryable<Entities.Interests> GetInterests()
        {
            return _interestsRepository.Table;
        }

        public Entities.Interests GetInterest(long id)
        {
            return _interestsRepository.GetById(id);
        }

        public long InsertInterest(Entities.Interests interests)
        {
            try
            {
                _interestsRepository.Insert(interests);

                return interests.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCab(Entities.Interests interests)
        {
            _interestsRepository.Update(interests);
        }

        public void DeleteCab(Entities.Interests interests)
        {
            _interestsRepository.Delete(interests);
        }

        #endregion
    }
}
