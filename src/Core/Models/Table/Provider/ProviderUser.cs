using System;
using Bit.Core.Enums;
using Bit.Core.Enums.Provider;
using Bit.Core.Models.Business.Provider;
using Bit.Core.Utilities;

namespace Bit.Core.Models.Table.Provider
{
    public class ProviderUser : ITableObject<Guid>, IConsortiumAssociation
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid? UserId { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public AssociationStatusType Status { get; set; }
        public ProviderUserType Type { get; set; }
        public string Permissions { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime RevisionDate { get; set; } = DateTime.UtcNow;

        public void SetNewId()
        {
            if (Id == default)
            {
                Id = CoreHelpers.GenerateComb();
            }
        }
    }
}
