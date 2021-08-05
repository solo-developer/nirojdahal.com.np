using System;
using System.Collections.Generic;
using System.Text;

namespace Personal.Domain.Services.Interface
{
    public interface ISlugGeneratorService
    {
        string Generate(string title);
    }
}
