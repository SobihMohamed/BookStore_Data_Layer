using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IEntity<TKey> 
    {
        TKey Id { set; get; }
    }
}
