using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Domain.Master;

public class LookupMaster : DomainBase
{
    public int Id { get; set; }
    public string InternalName { get; set; }
    public string DisplayValue { get; set; }
    public bool IsActive { get; set; }

    public List<LookupValue> LookupValues { get; set; }
}
