using System;
using System.ComponentModel.DataAnnotations; //Necessary for Data Annotations

namespace ClassLibrary
{
    
    // ================== Patient Model ===================
              
    public class Patient
    {
        [Required]
        public string FullName { get; set; } //required
        [Required]
        public int DPI { get; set; } //required
        [Required]
        public int Age { get; set; } //required
        [Required]
        public int PhoneNumber { get; set; } //required
        [Required]
        public DateTime LastConsultation { get; set; } //required
        public DateTime NextConsultation { get; set; } //optional
        public string TreatmentDescription { get; set; } //optional
    }
}
