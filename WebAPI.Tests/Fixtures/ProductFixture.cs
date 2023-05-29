using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Tests.Fixtures;

public class ProductFixture
{
    public static Guid Id_1 = Guid.NewGuid();
    public static Guid Id_2 = Guid.NewGuid();
    public static Guid Id_3 = Guid.NewGuid();
    public static Guid Id_4 = Guid.NewGuid();
    public static Guid Id_5 = Guid.NewGuid();

    public static List<ProductSchema> Schemas = new List<ProductSchema>()
        {
            new ProductSchema { Name = "Product 1", Price = 100, Description = "Description for Product 1", ImageUrl = "link 1", Brand = "brand 1"},
            new ProductSchema { Name = "Product 2", Price = 100, Description = "Description for Product 2", ImageUrl = "link 2", Brand = "brand 2"},
            new ProductSchema { Name = "Product 3", Price = 300, Description = "Description for Product 3", ImageUrl = "link 3", Brand = "brand 3"},
            new ProductSchema { Name = "Product 4", Price = 100, Description = "Description for Product 4", ImageUrl = "link 4", Brand = "brand 4"},
            new ProductSchema { Name = "Product 5", Price = 500, Description = "Description for Product 5", ImageUrl = "link 5", Brand = "brand 5"}
        };

        public static List<ProductEntity> Entities = new List<ProductEntity>()
        {
            new ProductEntity { Id = Id_1, Name = "Product 1", Price = 100, Description = "Description for Product 1", ImageUrl = "link 1", Brand = "brand 1", SalesCategory = "New"},
            new ProductEntity { Id = Id_2, Name = "Product 2", Price = 100, Description = "Description for Product 2", ImageUrl = "link 2", Brand = "brand 2", SalesCategory = "New"},
            new ProductEntity { Id = Id_3, Name = "Product 3", Price = 300, Description = "Description for Product 3", ImageUrl = "link 3", Brand = "brand 3", SalesCategory = "New"},
            new ProductEntity { Id = Id_4, Name = "Product 4", Price = 100, Description = "Description for Product 4", ImageUrl = "link 4", Brand = "brand 4", SalesCategory = "New"},
            new ProductEntity { Id = Id_5, Name = "Product 5", Price = 500, Description = "Description for Product 5", ImageUrl = "link 5", Brand = "brand 5", SalesCategory = "New"}
        };

        public static List<ProductDTO> DTOs = new List<ProductDTO>()
        {
            new ProductDTO { Id = Id_1, Name = "Product 1", Price = 100, Description = "Description for Product 1", ImageUrl = "link 1", Brand = "brand 1", SalesCategory = "New", Rating = 0},
            new ProductDTO { Id = Id_2, Name = "Product 2", Price = 100, Description = "Description for Product 2", ImageUrl = "link 2", Brand = "brand 2", SalesCategory = "New", Rating = 0},
            new ProductDTO { Id = Id_3, Name = "Product 3", Price = 300, Description = "Description for Product 3", ImageUrl = "link 3", Brand = "brand 3", SalesCategory = "New", Rating = 0},
            new ProductDTO { Id = Id_4, Name = "Product 4", Price = 100, Description = "Description for Product 4", ImageUrl = "link 4", Brand = "brand 4", SalesCategory = "New", Rating = 0},
            new ProductDTO { Id = Id_5, Name = "Product 5", Price = 500, Description = "Description for Product 5", ImageUrl = "link 5", Brand = "brand 5", SalesCategory = "New", Rating = 0}
        };

        public static List<string> ErrorMessages = new List<string>()
        {
            "A product with the same name already exists"
        };
}
