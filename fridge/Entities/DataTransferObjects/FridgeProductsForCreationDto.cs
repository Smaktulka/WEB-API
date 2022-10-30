﻿using Entities.Models;
using fridge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class FridgeProductsForCreationDto
    {
        [ForeignKey(nameof(Products))]
        public Guid ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity is required and it can't be less than 0.")]
        public int Quantity { get; set; }
    }
}
