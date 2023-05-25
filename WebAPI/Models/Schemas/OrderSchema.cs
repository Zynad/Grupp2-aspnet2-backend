﻿using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class OrderSchema
	{
		public string Address { get; set; } = null!;
		public List<OrderItemEntity> Items { get; set; } = null!;
	}
}
