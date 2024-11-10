using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.App.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Collection<Rule> Rules { get; set; }
    public User CreatedBy { get; set; }
    [ForeignKey("User")]
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}