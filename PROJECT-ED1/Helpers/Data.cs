using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace PROJECT_ED1.Helpers
{
    public class Data
    {
        //Singleton
        private static Data _instance = null;

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Data();
                }
                return _instance;
            }
        }

        public static Func<Patient, Patient, int> DPIcomparer = (patient, newPatient) =>
        {
            return patient.DPI.CompareTo(newPatient.DPI);
        };  //Compares two patient nodes by their DPI

        public static Func<Patient, Patient, int> NameComparer = (patient, newPatient) =>
        {
            return patient.FullName.ToLower().CompareTo(newPatient.FullName.ToLower());
        };  //Compares two patient nodes by their Name


        public static Func<ConsultationDay, ConsultationDay, int> ConsultationDayComparer = (day, newDay) =>
        {
            //return day.Date.CompareTo(newDay.Date);
            return DateTime.Compare(day.Date, newDay.Date);
        }; //Compares two ConsultationDay nodes by their date

       


        public AVL<Patient> DPITree = new AVL<Patient>(DPIcomparer);
        public AVL<Patient> NameTree = new AVL<Patient>(NameComparer);
        public AVL<ConsultationDay> ConsultationDayTree = new AVL<ConsultationDay>(ConsultationDayComparer);

    }
}
