using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Entities.Models 
{
    // Notice that this class does not inherit from anything.
    // An Entity Framework model is just a class.
    public class CatPic
    {
        // The [Key] decorator defaults to an autoincrementing integer in this scenario.
        [Key]
        public int Id { get; set; }

        // The data type is a string because we intend to only store the file location rather
        // than the actual file itself. We have marked it requred in case we want to 
        // validate the field itself.
        [Required(ErrorMessage = "Picture path is invalid")]
        public string Pic { get; set; }

        // DateTime will resolve differently based on the database you are using, 
        // but that is not an issue as Entity Framework is independent of your particular
        // SQL flavor. That is set by your configuration in Startup.cs.
        public DateTime UploadedDate { get; set; }
    }
}