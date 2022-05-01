using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class ConsultationDay
    {
        public DateTime Date { get; set; }
        public List<Patient> PatientList { get; set; }

        public ConsultationDay (DateTime date)
        {
            PatientList = new List<Patient>();
            Date = date;
        }
        
    }
}
