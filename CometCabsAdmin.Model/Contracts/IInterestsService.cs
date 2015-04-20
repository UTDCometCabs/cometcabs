using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IInterestsService
    {
        IQueryable<Interests> GetInterests();
        Interests GetInterest(long id);
        long InsertInterest(Interests interests);
        void UpdateInterest(Interests interests);
        void DeleteInterest(Interests interests);
    }
}
