﻿using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class OrderSchema
	{
		public int AddressId { get; set; }
		public List<OrderItemEntity> Items { get; set; } = null!;
	}
}