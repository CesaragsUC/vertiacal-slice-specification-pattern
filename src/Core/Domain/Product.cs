namespace Core.Domain;
public sealed class Product
{
    public Guid Id{get;private set;}=Guid.NewGuid();
    public string Name{get;private set;}=default!; 
    public string Category{get;private set;}=default!;
    public decimal Price{get;private set;} 
    public bool IsActive{get;private set;}=true; 
    private Product(){} 
    public Product(string name,string category,decimal price){Name=name;Category=category;Price=price;} 
    public void Deactivate()=>IsActive=false; 
    public void Rename(string name)=>Name=name; 
    public void Reprice(decimal price)=>Price=price;
}