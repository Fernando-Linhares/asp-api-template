using System.ComponentModel.DataAnnotations.Schema;

namespace Api.App.Models;

public class Rule
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Group? Group { get; set; }
    public Guid? GroupId { get; set; }
    public User CreatedBy { get; set; }
    [ForeignKey("User")]
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}