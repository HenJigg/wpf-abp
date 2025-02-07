using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Demo.Dtos
{
    public class AbpDemoDto : EntityDto
    {
        public string Name { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }
    }
}
