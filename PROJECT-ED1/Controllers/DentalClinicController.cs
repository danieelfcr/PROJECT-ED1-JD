using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using PROJECT_ED1.Helpers;

namespace PROJECT_ED1.Controllers
{
    public class DentalClinicController : Controller
    {
        // GET: DentalClinicController
        public ActionResult Index(List<Patient> PatientList)
        {
            return View(Data.Instance.DPITree.NodeList); 
        }

        public IActionResult NextConsultationFilter()
        {
           try
           {
                Data.Instance.DPITree.NodeList.Clear();
                Data.Instance.FilteredList.Clear();
                Data.Instance.FilteredList2.Clear();

                //InOrder of AVL DPI Tree to fill node list
                Data.Instance.DPITree.InOrder(Data.Instance.DPITree.Root);


                foreach (var item in Data.Instance.DPITree.NodeList)
                {
                    if (item.NextConsultation == default)
                    {
                        Data.Instance.FilteredList.Add(item);
                    }
                }

                return View(Data.Instance.FilteredList);
            
           }
           catch
           {
             return View();
           }
        }

        public IActionResult CleaningFilter()
        {
            //Call function sending the minimum quantity of months to evaluate, the treatment and the case number
            InsertNodeToFList(6, "", 1);
            return View(Data.Instance.FilteredList2);
        }

        public IActionResult CariesFilter()
        {
            InsertNodeToFList(4, "Caries", 2);
            return View(Data.Instance.FilteredList2);
        }

        public IActionResult OrthodonticFilter()
        {
            InsertNodeToFList(2, "Orthodontics", 3);
            return View(Data.Instance.FilteredList2);
        }

        public IActionResult SpecificFilter()
        {
            InsertNodeToFList(6, "Specific", 4);
            return View(Data.Instance.FilteredList2);
        }

        public static int LastConsultationDif(Patient patient)
        {
            return Math.Abs((DateTime.Today.Month - patient.LastConsultation.Month) + 12 * (DateTime.Today.Year - patient.LastConsultation.Year));
        }   //Returns the difference in months

        public void InsertNodeToFList(int MinMonths, String Treatment, int CaseNumber)
        {
            //Evaluate each item in list
            foreach (var item in Data.Instance.FilteredList)
            {
                //Evalute if the patient needs a dental cleaning by its first condition (no treatment)
                if (item.TreatmentDescription != "")
                {
                    if ((LastConsultationDif(item) >= MinMonths) && (!item.TreatmentDescription.ToUpper().Contains("CARIES") && !item.TreatmentDescription.ToUpper().Contains("ORTHODONTICS") && PatientNumber(item, MinMonths, Treatment) == CaseNumber))
                    {
                        Data.Instance.FilteredList2.Add(item);
                    }
                    //Evaluate for the second and third case
                    else if ((LastConsultationDif(item) >= MinMonths) && (item.TreatmentDescription.ToUpper().Contains(Treatment.ToUpper())))
                    {
                        if (PatientNumber(item, MinMonths, Treatment) == CaseNumber) //If the patient has the case we are looking for, add to list
                        {
                            Data.Instance.FilteredList2.Add(item);
                        }     
                    }
                }
                else
                {
                    //If the patient has no treatment, evaluate if it has the minimum quantity of months
                    if ((LastConsultationDif(item) >= MinMonths))
                    {
                        if (PatientNumber(item, MinMonths, Treatment) == CaseNumber)
                        {
                            Data.Instance.FilteredList2.Add(item);
                        }
                    }
                }
            }
        }   //Inserts a node in the second filtered list by month

        public int PatientNumber(Patient patient, int MinMonths, string Treatment)
        {
            if ((LastConsultationDif(patient) >= MinMonths) && !patient.TreatmentDescription.ToUpper().Contains("CARIES") && !patient.TreatmentDescription.ToUpper().Contains("ORTHODONTICS") && patient.TreatmentDescription != "")
                return 4;
            else if ((LastConsultationDif(patient) >= MinMonths) && (patient.TreatmentDescription.ToUpper().Contains(Treatment.ToUpper())))
            {
                if (patient.TreatmentDescription.ToUpper().Contains("ORTHODONTICS"))
                {
                    return 3;
                }
                else if (patient.TreatmentDescription.ToUpper().Contains("CARIES"))
                {
                    return 2;
                }
            }
            return 1;

        } //Returns the case number of a patient

        // GET: DentalClinicController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DentalClinicController/Create 
        public ActionResult Create()
        {
            return View(new Patient());
        }

        // POST: DentalClinicController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                ViewBag.AddPatientToConsultationDay = null;


                Patient patient = new Patient
                {
                    FullName = collection["FullName"],
                    DPI = collection["DPI"],
                    Age = Convert.ToInt32(collection["Age"]),
                    PhoneNumber = Convert.ToInt64(collection["PhoneNumber"]),
                    LastConsultation = Convert.ToDateTime(collection["LastConsultation"]),
                    NextConsultation = Convert.ToDateTime(collection["NextConsultation"]),
                    TreatmentDescription = collection["TreatmentDescription"]

                };            
                

                Node<Patient> NewNodeDPI = new Node<Patient>(patient);
                Node<Patient> NewNodeName = new Node<Patient>(patient);
                

                if (patient.NextConsultation != default(DateTime))
                {
                    ConsultationDay consultationDay = new ConsultationDay(patient.NextConsultation);
                    consultationDay.PatientList.Add(patient);
                    Node<ConsultationDay> newNodeConsultationDay = new Node<ConsultationDay>(consultationDay);

                    if (!Data.Instance.ConsultationDayTree.Contains(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay) && !Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                    {
                        
                        Data.Instance.ConsultationDayTree.Root = Data.Instance.ConsultationDayTree.Insert(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay);
                    }
                    else
                    {
                        
                        var aux = Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay);

                        if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay).Record.PatientList.Count < 8 && !Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay).Record.PatientList.Add(patient);
                        else if (!Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                        {
                            ViewBag.AddPatientToConsultationDay = "The consultation day you are trying to access is already full of patients, try again with another date";
                            return View();
                        }
                        //if (!Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))

                    }
                }


                if (!Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                { 
                    Data.Instance.DPITree.Root = Data.Instance.DPITree.Insert(Data.Instance.DPITree.Root, NewNodeDPI);   //Call to insert to DPI TREE function
                    Data.Instance.NameTree.Root = Data.Instance.NameTree.Insert(Data.Instance.NameTree.Root, NewNodeName);   //Call to insert to NAME TREE function

                    //Clear list
                    Data.Instance.DPITree.NodeList.Clear();

                    //Fill list
                    Data.Instance.DPITree.InOrder(Data.Instance.DPITree.Root);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.CreatePatient = "The patient that you are trying to create has either a DPI or a name identical to one that is already in the database. Please, try again or search for the patient.";
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: DentalClinicController/Edit/5
        public ActionResult Edit(string id)
        {
            Patient auxPatient = new Patient
            {
                DPI = id 
            };

            //creation of a new node to search in the DPI tree, using DPI as id

            Node<Patient> auxNode = new Node<Patient>(auxPatient); 
            var node = Data.Instance.DPITree.Search(Data.Instance.DPITree.Root, auxNode);

            return View(node.Record);
        }



        // POST: DentalClinicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                //Record of the patient with editions
                Patient EditedPatient = new Patient
                {
                    FullName = collection["FullName"],
                    DPI = collection["DPI"],
                    Age = Convert.ToInt32(collection["Age"]),
                    PhoneNumber = Convert.ToInt64(collection["PhoneNumber"]),
                    LastConsultation = Convert.ToDateTime(collection["LastConsultation"]),
                    NextConsultation = Convert.ToDateTime(collection["NextConsultation"]),
                    TreatmentDescription = collection["TreatmentDescription"]
                };


                //Creation of a new node that stores the edited patient to search in the AVL trees
                Node<Patient> EditedPatientNode = new Node<Patient>(EditedPatient);

                //The original node that stores the patient record before editing
                var OriginalPatientNode = Data.Instance.DPITree.Search(Data.Instance.DPITree.Root, EditedPatientNode);

                //Creation of a new ConsultationDay using the NextConsultation date of the edited patient
                ConsultationDay auxDate = new ConsultationDay(EditedPatient.NextConsultation);

                //The patient list inside the ConsultationDay is used to store the edited patient
                auxDate.PatientList.Add(EditedPatient);
                
                //Creation of a new ConsultationDay node, used to search in the ConsultationDay tree
                Node<ConsultationDay> nodeNewDate = new Node<ConsultationDay>(auxDate);

                //if the original and edited nextConsultation date are the same, the system only edit the data of the record
                if (OriginalPatientNode.Record.NextConsultation == EditedPatientNode.Record.NextConsultation) 
                {
                    Data.Instance.DPITree.EditData(Data.Instance.DPITree.Root, EditedPatientNode);
                    Data.Instance.NameTree.EditData(Data.Instance.NameTree.Root, EditedPatientNode);
                }
                else if (OriginalPatientNode.Record.NextConsultation == default) //if the original NextConsultation date was not assigned, the nextConsultationDay tree does not contain the record
                {
                    //the patient doesn't have a next consultation date, do the information is edited without rescheduling

                    if (Data.Instance.ConsultationDayTree.Contains(Data.Instance.ConsultationDayTree.Root, nodeNewDate))
                    {
                        if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, nodeNewDate).Record.PatientList.Count != 8)
                        {
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, nodeNewDate).Record.PatientList.Add(EditedPatient);
                        }
                        else
                        {
                            ViewBag.AddPatientToConsultationDay = "The consultation day you are trying to access is already full of patients, try again with another date";
                            return RedirectToAction(nameof(Edit));
                        }
                    }
                    else
                    {
                       Data.Instance.ConsultationDayTree.Root = Data.Instance.ConsultationDayTree.Insert(Data.Instance.ConsultationDayTree.Root, nodeNewDate);
                    }

                    Data.Instance.DPITree.EditData(Data.Instance.DPITree.Root, EditedPatientNode);
                    Data.Instance.NameTree.EditData(Data.Instance.NameTree.Root, EditedPatientNode);
                
                }
                else
                {
                    //Creation of a new consultation day using the original NextConsultation date to search in the tree

                    ConsultationDay OriginalDate = new ConsultationDay(OriginalPatientNode.Record.NextConsultation);
                    Node<ConsultationDay> NodeOriginalDate = new Node<ConsultationDay>(OriginalDate);

                    //if new date exists in the consultationDay tree, if there is space in the patient list, the edited record is stored, the original record is removed from the list of the original date
                    if (Data.Instance.ConsultationDayTree.Contains(Data.Instance.ConsultationDayTree.Root, nodeNewDate))
                    {
                        if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, nodeNewDate).Record.PatientList.Count != 8)
                        {
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, nodeNewDate).Record.PatientList.Add(nodeNewDate.Record.PatientList[0]);
                            if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Count == 1)
                                Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Clear();
                            else
                                Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Remove(OriginalPatientNode.Record);
                        }
                        else
                        {
                            ViewBag.AddPatientToConsultationDay = "The consultation day you are trying to access is already full of patients, try again with another date";
                            return RedirectToAction(nameof(Edit));
                        }
                    }
                    else
                    {
                        //if the new date is not inside the ConsultationDayTree, the insert method of the tree is called
                        Data.Instance.ConsultationDayTree.Root = Data.Instance.ConsultationDayTree.Insert(Data.Instance.ConsultationDayTree.Root, nodeNewDate);
                        if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Count == 1)
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Clear();
                        else
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, NodeOriginalDate).Record.PatientList.Remove(OriginalPatientNode.Record);
                    }


                    Data.Instance.DPITree.EditData(Data.Instance.DPITree.Root, EditedPatientNode);
                    Data.Instance.NameTree.EditData(Data.Instance.NameTree.Root, EditedPatientNode);


                }


              


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DentalClinicController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DentalClinicController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Filter(string FilterSelection, string Search)
        {
            //Create a list
            List<Patient> filteredList = new List<Patient>();

            if (Search != null)   //Verify that the user wrote something
            {
               
                //Create a new patient with the needed attributes
                Patient patient = new Patient
                {
                    DPI = Search,
                    FullName = Search
                };
                Node<Patient> Node = new Node<Patient>(patient);

                //Create a new node that will collect the patient found
                Node<Patient> foundPatient;

                //Verify what type of selection the user is asking for
                if (FilterSelection == "DPI")
                    foundPatient = Data.Instance.DPITree.Search(Data.Instance.DPITree.Root, Node);

                else
                    foundPatient = Data.Instance.NameTree.Search(Data.Instance.NameTree.Root, Node);

                //If the patient wasn't found, 'foundPatient' will be equals to 'Node'
                if (foundPatient == Node)
                {
                    filteredList.Clear();
                    return View(filteredList);  //Return empty list
                }
                else
                {
                    filteredList.Clear();
                    filteredList.Add(foundPatient.Record);  //Add found patient to list
                    return View(filteredList);  
                }
                
            }
            filteredList.Clear();
            return View(filteredList);
        }
    }
}
