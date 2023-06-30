namespace SuperShop.Web.Data.Entities;

public interface IEntity
{
    int Id { get; set; }


    bool WasDeleted { get; set; }


    // string Name { get; set; }
}