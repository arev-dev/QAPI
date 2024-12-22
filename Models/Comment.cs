using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QAPI.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [BindNever]
    [JsonIgnore]
    public virtual Post? Post { get; set; }

    [BindNever]
    [JsonIgnore]
    public virtual User? User { get; set; }
}
