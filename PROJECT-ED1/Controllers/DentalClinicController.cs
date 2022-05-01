﻿using Microsoft.AspNetCore.Http;
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
                //Create a list that contains all AVL Tree nodes
                List<Patient> GeneralList = new List<Patient>();
                Data.Instance.DPITree.NodeList.Clear();
                Data.Instance.DPITree.InOrder(Data.Instance.DPITree.Root);
                GeneralList = Data.Instance.DPITree.NodeList;

               

                //Create a new filtered list with the nodes that doesn't have a next consultation
                List<Patient> FilteredList = new List<Patient>();
                foreach (var item in GeneralList)
                {
                    if (item.NextConsultation.Year.ToString() == "1")
                    {
                        FilteredList.Add(item);
                    }
                }

                return View(FilteredList);
            }
            catch
            {
              return View();
            }
       }

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

                        if (Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay).Record.PatientList.Count <= 8 && !Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                            Data.Instance.ConsultationDayTree.Search(Data.Instance.ConsultationDayTree.Root, newNodeConsultationDay).Record.PatientList.Add(patient);
                        else if (!Data.Instance.DPITree.Contains(Data.Instance.DPITree.Root, NewNodeDPI) && !Data.Instance.NameTree.Contains(Data.Instance.NameTree.Root, NewNodeName))
                        {
                            ViewBag.AddPatientToConsultationDay = "The consultation day you are trying to access is already full of patients, try again with another date";
                            return View();
                        }
                            
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DentalClinicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
