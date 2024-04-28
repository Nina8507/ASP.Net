namespace Colibo.Models;

public class MergedUsers
{
  public string? Id { get; set; }
  public string? FullName { get; set; }
  public string? Email { get; set; }
  public string? JobTitle { get; set; }
  public string? Mobile { get; set; }
  public string? Address { get; set; }
  public string? City { get; set; }

  public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        MergedUsers other = (MergedUsers)obj;
        return FullName == other.FullName;
    }

    public override int GetHashCode()
    {
        return (FullName?.GetHashCode() ?? 0);
    }
}