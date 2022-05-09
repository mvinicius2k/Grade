using Grade.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes.MockData;

public static class PresentersData
{
    public static List<PresenterDto> GetPresenters()
    {
        return new List<PresenterDto>
        {
            new PresenterDto
            {
                Name = "Garrus Vakarian",
                ImageId = null,

            },
            new PresenterDto
            {
                Name = "Raul Menendez",
                ImageId = null,
                
            },
            new PresenterDto
            {
                Name = "Dogmeat",
                ImageId = null,
            }


        };



    }
}
