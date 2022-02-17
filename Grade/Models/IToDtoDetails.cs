using AutoMapper;
using Grade.Data;

namespace Grade.Models
{
    
    public interface IToDtoDetails<T>
    {
       
        public abstract T ToModelDetailsDto( GradeContext gradeContext, IMapper mapper);
        
    }
}
