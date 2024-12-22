using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QAPI.Models;

public partial class Post
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
    public bool IsClosed { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    [BindNever]
    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [BindNever]
    [JsonIgnore]
    public virtual User? User { get; set; }
}
