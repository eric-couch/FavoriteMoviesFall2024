using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteMoviesFall2024.Shared;

public class Movie
{
    public int Id { get; set; }
    public string imdbId { get; set; } = String.Empty;
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; } // Foreign Key
}
