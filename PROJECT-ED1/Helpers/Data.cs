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
            return DateTime.Compare(day.Date, newDay.Date);
        }; //Compares two ConsultationDay nodes by their date

           

        public static Action<Patient, Patient> EditData = (originalPatient, newPatient) =>
        {
            originalPatient.NextConsultation = newPatient.NextConsultation;
            originalPatient.TreatmentDescription = newPatient.TreatmentDescription;
        };



        public AVL<Patient> DPITree = new AVL<Patient>(DPIcomparer, EditData);
        public AVL<Patient> NameTree = new AVL<Patient>(NameComparer, EditData);
        public AVL<ConsultationDay> ConsultationDayTree = new AVL<ConsultationDay>(ConsultationDayComparer);

        //New lists to show filtered by nextconsultation
        public List<Patient> FilteredList = new List<Patient>();
        public List<Patient> FilteredList2 = new List<Patient>();
    }
}
