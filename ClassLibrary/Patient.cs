using System;

namespace ClassLibrary
{
    
    // ================== Patient Model ===================
              
    public class Patient
    {
        public string FullName { get; set; } //required
        public int DPI { get; set; } //required
        public int Age { get; set; } //required
        public int PhoneNumber { get; set; } //required
        public DateTime LastConsultation { get; set; } //required
        public DateTime NextConsultation { get; set; } //optional
        public string TreatmentDescription { get; set; } //optional
    }
}
