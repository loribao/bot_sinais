using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.Entities
{
    /// <summary>
    /// Interface para entidades que possuem versionamento
    /// </summary>
    public interface IVersionedEntity
    {
        int Version { get; set; }
    }
}
