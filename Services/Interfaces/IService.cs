using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IService<TDto, TFilter>
    {
        TDto Get(string id);
        IEnumerable<TDto> Get(TFilter filter);
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
