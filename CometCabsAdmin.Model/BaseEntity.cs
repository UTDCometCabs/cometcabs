using System;

namespace CometCabsAdmin.Model
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string IPAddress { get; set; }
    }
}
