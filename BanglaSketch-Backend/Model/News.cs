using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime? Created { get; set; }
    }
}