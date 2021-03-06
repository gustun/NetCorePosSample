﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Pos.Api.ViewModel
{
    public class ProductViewModel : NewProductModel
    {
        public Guid Id { get; set; }
    }

    public class NewProductModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
