using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctER_P.Models
{
    public class PatientDetailModel
    {
        public int PatientDetailId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName{ get; set; }
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; }
    }
}