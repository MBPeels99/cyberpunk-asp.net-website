using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Night_City.Models
{
    public class Figure
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string StageName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PlaceOfDeath { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string Background { get; set; }
        public string KnownFor { get; set; }
        public string Partner { get; set; }
        public string VoicedBy { get; set; }
        public string Image { get; set; }
    }
}