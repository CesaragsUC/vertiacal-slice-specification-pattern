namespace Application.Domain;

public class Category
{
    protected Category() { }
    public Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }
    public Category(Guid id,string name, bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; } = true;

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Rename(string name)
    {
        Name = name;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Update(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }
}
