﻿using System;
using System.ComponentModel.DataAnnotations;
using Pos.Core.Enum;

namespace Pos.Api.ViewModel
{
    public class CampaignViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public EDiscountType DiscounType { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }
        public int? MaxUsageCount { get; set; }
        public int? UsageCount { get; set; }
    }
}
