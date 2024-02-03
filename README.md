Изменения в коде в соотвествиями с требоваиями к ТЗ:

1. **Добавление новых классов в `StoreMarket.Contracts.Requests`:**

   ```csharp
   // Group операции
   public class GroupDeleteRequest
   {
       public int Id { get; set; }
   }

   // Продуктовые операции
   public class ProductDeleteRequest
   {
       public int Id { get; set; }
   }
   ```

2. **Добавление новой модели `Group` в `StoreMarket.Models`:**

   ```csharp
   public class Group
   {
       public int Id { get; set; }
       public int Count { get; set; }
       public virtual ICollection<Product> Products { get; set; } = new List<Product>();
   }
   ```

3. **Обновление `StoreContext` в `StoreMarket.Contexts`:**

   Добавление нового `DbSet<Group>` и обновление метода `OnModelCreating`:

   ```csharp
   public class StoreContext : DbContext
   {
       // ... остальной код ...

       public virtual DbSet<Group> Groups { get; set; }

       // ... остальной код ...

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           // ... остальной код ...

           modelBuilder.Entity<Group>(entity =>
           {
               entity.HasKey(g => g.Id).HasName("group_pkey");
               entity.ToTable("Groups");
               entity.Property(g => g.Count).IsRequired();
           });

           // ... остальной код ...
       }
   }
   ```

4. **Обновление `ProductsController` в `StoreMarket.Controllers`:**

   Добавление новых методов для удаления группы, удаления продукта и изменения цены продукта:

   ```csharp
   [HttpDelete]
   [Route("groups/{id}")]
   public ActionResult<GroupResponse> DeleteGroup(int id)
   {
       var group = storeContext.Groups.FirstOrDefault(g => g.Id == id);

       if (group == null)
       {
           return NotFound();
       }

       storeContext.Groups.Remove(group);
       storeContext.SaveChanges();

       return Ok(new GroupResponse(group));
   }

   [HttpDelete]
   [Route("products/{id}")]
   public ActionResult<ProductResponse> DeleteProduct(int id)
   {
       var product = storeContext.Products.FirstOrDefault(p => p.Id == id);

       if (product == null)
       {
           return NotFound();
       }

       storeContext.Products.Remove(product);
       storeContext.SaveChanges();

       return Ok(new ProductResponse(product));
   }

   [HttpPut]
   [Route("products/{id}/price")]
   public ActionResult<ProductResponse> SetProductPrice(int id, decimal price)
   {
       var product = storeContext.Products.FirstOrDefault(p => p.Id == id);

       if (product == null)
       {
           return NotFound();
       }

       product.Price = price;
       storeContext.SaveChanges();

       return Ok(new ProductResponse(product));
   }
   ```

