using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class BaseEntity
{
    public DateOnly CreatedDate { get; init; }
    public DateOnly? UpdatedDate { get; set; }
    public DateOnly? DeletedDate { get; set; }



}
