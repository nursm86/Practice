using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Practice2.Models;
using Practice2.Models.ViewModels;
using Practice2.Repository;

namespace Practice2.Controllers
{
    public class AttendanceController : Controller
    {
        private AttendanceRepository attendanceRepo = new AttendanceRepository();

        public ActionResult MonthlyAttendanceReportOfAllEmployee()
        {
            return View();
        }

        //*******************************WEB API*******************************
        [HttpGet]
        public JsonResult GetAllAttendanceById(int payrollId,int employeeId)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.GetAllAttendanceById(payrollId, employeeId)),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAttendanceById(int id)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.Get(id)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAttendance(int payrollId,Attendance elad)
        {
            dynamic data = new ExpandoObject();
            string error = string.Empty;

            if (SaveAttendanceLogic(payrollId, elad, out error))
            {
                data.error = "";
                data.Attendance = elad;
                return Json(JsonConvert.SerializeObject(data));
            }
            else
            {
                data.error = error;
                return Json(JsonConvert.SerializeObject(data));
            }
        }

        private bool SaveAttendanceLogic(int payrollId,Attendance newObj, out string error)
        {
            error = string.Empty;
            if (newObj == null)
            {
                error = "Attendance to save can't be null!";
                return false;
            }

            if (!IsValidToSaveAttendance(payrollId, newObj, out error))
                return false;

            if (newObj.AttendanceId != 0)
            {
                attendanceRepo.Update(newObj);
            }
            else
            {
                newObj.PunchTime = DateTime.Now;
                attendanceRepo.Insert(newObj);
            }

            return true;
        }

        private bool IsValidToSaveAttendance(int payrollId, Attendance newObj, out string error)
        {
            error = "";
            return true;
        }

        [HttpGet]
        public JsonResult GetDeleteAttendanceById(int id)
        {
            attendanceRepo.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerateAttendanceReport(int payrollId, int employeeId)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.GenerateAttendanceReport(payrollId, employeeId)),
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateMonthlyAttendanceReportOfAllEmployee(QueryData data)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.GenerateMonthlyAttendanceReportOfAllEmployee(data)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmpCalendarDataById(int employeeId)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.GetCalendarDataById(employeeId)),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmpCalendar2DataById(int employeeId)
        {
            return Json(JsonConvert.SerializeObject(attendanceRepo.GetCalendar2DataById(employeeId)),
                JsonRequestBehavior.AllowGet);
        }
    }
}