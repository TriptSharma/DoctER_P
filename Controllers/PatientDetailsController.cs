using DoctER_P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoctER_P.Controllers
{
    public class PatientDetailsController : ApiController
    {
        private SqlUtility util;
        public PatientDetailsController()
        {
            util = new SqlUtility();
        }

        //GET api/patientMasters
        [HttpGet]
        public IHttpActionResult GetPatients()
        {
            try
            {
                DataTable result = util.executeSproc("SP_PatientDetail_SelectAll", new List<SqlParameter>() { });
                if (result.Rows.Count > 0)
                {
                    var details = from DataRow row in result.Rows
                                   select new
                                   {
                                       patientDetailId = row["PatientDetailId"],
                                       patientId = row["PatientId"],
                                       doctorId = row["DoctorId"],
                                       doctorName = row["DoctorName"],
                                       diseaseId = row["DiseaseId"],
                                       diseaseName = row["DiseaseName"]
                                   };
                    return Ok(new { result = details });
                }
                else
                {
                    return Ok(new { result = 0 });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //GET api/patientMasters/1
        [HttpGet]
        public IHttpActionResult GetDetail(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientDetailId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Default, id));

                var result = util.executeSproc("SP_PatientDetail_Select", parameters);

                if (result.Rows.Count > 0)
                {
                    var patient = from DataRow row in result.Rows
                                  select new
                                  {
                                      patientDetailId = row["PatientDetailId"],
                                      patientId = row["PatientId"],
                                      doctorId = row["DoctorId"],
                                      doctorName = row["DoctorName"],
                                      diseaseId = row["DiseaseId"],
                                      diseaseName = row["DiseaseName"]
                                  };
                    return Ok(new { result = patient });
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //POST api/patientMasters
        [HttpPost]
        public IHttpActionResult CreatePatient(PatientDetailModel detailParam)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, detailParam.PatientId));
                parameters.Add(new SqlParameter("@DoctorId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, detailParam.DoctorId));

                var result = util.executeSproc("SP_PatientDetail_Insert", parameters);

                return Created("patientDetails/" + detailParam.PatientDetailId, new { results = "created" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //PUT api/patientMasters/
        [HttpPut]
        public IHttpActionResult UpdatePatient(PatientDetailModel detailParam)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientDetailId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, detailParam.PatientDetailId));
                parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, detailParam.PatientId));
                parameters.Add(new SqlParameter("@DoctorId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, detailParam.DoctorId));

                var result = util.executeSproc("SP_PatientDetail_Update", parameters);

                return Ok(new { results = detailParam });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //DELETE api/patientMasters/3
        [HttpDelete]
        public IHttpActionResult DeletePatient(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientDetailId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, id));
                var result = util.executeSproc("SP_PatientDetail_Delete", parameters);
                return Ok(new { result = "Deleted" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
