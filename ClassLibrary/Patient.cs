using System;
using System.ComponentModel.DataAnnotations; //Necessary for Data Annotations

namespace ClassLibrary
{
    
    // ================== Patient Model ===================
              
    public class Patient
    {

        [Required]
        public string FullName { get; set; }
        [Required]
        public int DPI { get; set; } 
        [Required]
        public int Age { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public DateTime LastConsultation { get; set; }
        public DateTime NextConsultation { get; set; }
        public string TreatmentDescription { get; set; }
    }
}
