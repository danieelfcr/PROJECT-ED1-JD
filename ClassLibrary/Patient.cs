using System;

namespace ClassLibrary
{
    
    // ================== Patient Model ===================
              
    public class Patient
    {
        public string FullName { get; set; } 
        public int DPI { get; set; } 
        public int Age { get; set; } 
        public int PhoneNumber { get; set; } 
        public DateTime LastConsultation { get; set; } 
        public DateTime NextConsultation { get; set; } 
        public string TreatmentDescription { get; set; }
    }
}
