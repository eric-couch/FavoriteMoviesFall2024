using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteMoviesFall2024.Shared;

public class Rating
{
    [Key]
    public int Id { get; set; }
    public string Source { get; set; }
    public string Value { get; set; }

    public string OMDBMovieId { get; set; }
}
