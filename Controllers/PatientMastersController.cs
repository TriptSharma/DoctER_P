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
    public class PatientMastersController : ApiController
    {
        private SqlUtility util;
        public PatientMastersController()
        {
            util = new SqlUtility();
        }

        //GET api/patientMasters
        [HttpGet]
        public IHttpActionResult GetPatients()
        {
            try
            {
                DataTable result = util.executeSproc("SP_PatientMaster_SelectAll", new List<SqlParameter>() { });
                if (result.Rows.Count > 0)
                {
                    var patients = from DataRow row in result.Rows
                                   select new
                                   {
                                       patientId = row["PatientId"],
                                       patientName = row["PatientName"],
                                       patientAddress = row["PatientAddress"]
                                   };
                    return Ok(new { result = patients });
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
        public IHttpActionResult GetPatient(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Default, id));

                var result = util.executeSproc("SP_PatientMaster_Select", parameters);

                if (result.Rows.Count > 0)
                {
                    var patient = from DataRow row in result.Rows
                                  select new
                                  {
                                      patientId = row["PatientId"],
                                      patientName = row["PatientName"],
                                      patientAddress = row["PatientAddress"]
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
        public IHttpActionResult CreatePatient(PatientMasterModel masterParam)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientName", SqlDbType.VarChar, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, masterParam.PatientName));
                parameters.Add(new SqlParameter("@PatientAddress", SqlDbType.VarChar, -1, ParameterDirection.Input, true, 2, 2, "", DataRowVersion.Current, masterParam.PatientAddress));

                var result = util.executeSproc("SP_PatientMaster_Insert", parameters);

                return Created("patientMasters/"+ masterParam.PatientId, new { results = "created" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //PUT api/patientMasters/
        [HttpPut]
        public IHttpActionResult UpdatePatient(PatientMasterModel masterParam)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Default, masterParam.PatientId));
                parameters.Add(new SqlParameter("@PatientName", SqlDbType.VarChar, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, masterParam.PatientName));
                parameters.Add(new SqlParameter("@PatientAddress", SqlDbType.VarChar, -1, ParameterDirection.Input, true, 2, 2, "", DataRowVersion.Current, masterParam.PatientAddress));

                var result = util.executeSproc("SP_PatientMaster_Update", parameters);

                return Ok(new { results = masterParam });
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
                parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int, -1, ParameterDirection.Input, false, 2, 2, "", DataRowVersion.Current, id));
                var result = util.executeSproc("SP_PatientMaster_Delete", parameters);
                return Ok(new { result = "Deleted" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
