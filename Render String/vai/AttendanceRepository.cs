using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Practice2.Models;
using Practice2.Models.ViewModels;

namespace Practice2.Repository
{
    public class AttendanceRepository:Repository<Attendance>
    {
        public List<Attendance> GetAllAttendanceById(int payrollId,int employeeId)
        {
            PayrollRepository payRepo = new PayrollRepository();
            Payroll p = payRepo.Get(payrollId);
            int month = getMonthId(p.Month);
            int year = Int32.Parse(p.Year);
            return this.context.Set<Attendance>()
                .Where(x => x.EmployeeId == employeeId
                        && x.PunchTime.Month == month && x.PunchTime.Year == year
                        ).OrderByDescending(x=>x.PunchTime).ToList();
        }

        public List<Attendance> GetAllAttendanceByDate(int year,int month,int i,int employeeId)
        {
            return this.context.Set<Attendance>()
                .Where(x => x.EmployeeId == employeeId
                            && x.PunchTime.Month == month && x.PunchTime.Year == year && x.PunchTime.Day == i
                ).OrderBy(x=>x.PunchTime).ToList();
        }
        public bool IsAttendanceExist(DateTime date, int attendanceId)
        {
            Attendance ww = this.context.Set<Attendance>().Where(x => x.PunchTime == date && x.AttendanceId != attendanceId)
                .FirstOrDefault();

            if (ww == null)
            {
                return false;
            }

            return true;
        }

        public dynamic GenerateAttendanceReport(int payrollId, int employeeId)
        {
            dynamic data = new ExpandoObject();
            List<AttendanceReport> attendances = new List<AttendanceReport>();
            PayrollRepository payRepo = new PayrollRepository();
            Payroll p = payRepo.Get(payrollId);
            int month = getMonthId(p.Month);
            int year = Int32.Parse(p.Year);
            int daysInMonth = 0;
            DateTime currDate = DateTime.Now;
            if (currDate.Year == year && currDate.Month == month)
            {
                daysInMonth = currDate.Day;
            }
            else if ((currDate.Year == year && currDate.Month < month) || currDate.Year < year)
            {
                return null;
            }
            else
            {
                daysInMonth = System.DateTime.DaysInMonth(year, month);
            }
            string day = "";
            string mon = "";
            int dayOfWeek = 0;
            if (month < 10)
            {
                mon = "0" + month;
            }
            else
            {
                mon = month.ToString();
            }
            string date = "";

            TimeSpan totalSumOfAllShift = new TimeSpan(0, 0, 0);
            TimeSpan totalShift = new TimeSpan(0, 0, 0);

            //for loop start.

            for (int i = 1; i <= daysInMonth; i++)
            {
                if (i < 10)
                {
                    day = "0" + i;
                }
                else
                {
                    day = i.ToString();
                }

                //Adding In and Out of an Employee
                
                dayOfWeek = (int)new DateTime(year, month, i).DayOfWeek;
                AttendanceReport newAttendance = new AttendanceReport();
                newAttendance.Date = year + "-" + mon + "-" + day;
                newAttendance.Day = Enum.GetName(typeof(DayOfWeek), dayOfWeek);
                newAttendance.In = new List<string>();
                newAttendance.Out = new List<string>();
                newAttendance.SumOfAllShift = "";
                List<Attendance> dayAttended = this.GetAllAttendanceByDate(year,month,i, employeeId);
                int j = 1,span = 0;
                foreach (var item in dayAttended)
                {
                    if (j % 2 == 1)
                    {
                        if (j == 1)
                        {
                            newAttendance.IsPresent = true;
                            newAttendance.FirstIn = item.PunchTime.TimeOfDay.ToString(@"hh\:mm");
                        }
                        newAttendance.In.Add(item.PunchTime.TimeOfDay.ToString(@"hh\:mm"));
                    }
                    else
                    {
                        newAttendance.Out.Add(item.PunchTime.TimeOfDay.ToString(@"hh\:mm"));
                        TimeSpan Out;
                        TimeSpan.TryParse(newAttendance.Out[span], out Out);
                        TimeSpan In;
                        TimeSpan.TryParse(newAttendance.In[span], out In);
                        TimeSpan sumOfAll = Out - In;
                        if (newAttendance.SumOfAllShift == "")
                        {
                            newAttendance.SumOfAllShift += sumOfAll.ToString(@"hh\:mm");
                        }
                        else
                        {
                            TimeSpan Before;
                            TimeSpan.TryParse(newAttendance.SumOfAllShift, out Before);
                            sumOfAll += Before;
                            newAttendance.SumOfAllShift = sumOfAll.ToString(@"hh\:mm");
                        }
                        
                        span++;
                    }
                    j++;
                }

                if (span > 0)
                {
                    newAttendance.LastOut = newAttendance.Out.LastOrDefault();
                    TimeSpan Out;
                    TimeSpan.TryParse(newAttendance.LastOut, out Out);
                    TimeSpan In;
                    TimeSpan.TryParse(newAttendance.FirstIn, out In);
                    TimeSpan sumOfAll = Out - In;
                    TimeSpan workingHour = new TimeSpan(8, 0, 0);
                    if (sumOfAll < workingHour)
                    {
                        newAttendance.Event.EventName = "";
                        newAttendance.Event.Color = "#ff9999;";
                        newAttendance.Event.EventType = 4;
                    }

                    newAttendance.FirstInMinusLastOut += sumOfAll.ToString(@"hh\:mm");
                }
                else
                {
                    newAttendance.SumOfAllShift = "0";
                    newAttendance.FirstInMinusLastOut = "0";
                    newAttendance.Event.EventName = "[ Absence ]";
                    newAttendance.Event.Color = "Tomato";
                    newAttendance.Event.EventType = -1;
                }

                //Adding Event of and Employee
                newAttendance.Event.EventName = string.Empty;

                //Checking Approved Leave's
                EmpLeaveApplicationRepository lRepo = new EmpLeaveApplicationRepository();
                EmpLeaveApplicationDetail edDetail = lRepo.GetLeaveDetailsByDate(newAttendance.Date, employeeId);

                if (edDetail != null)
                {
                    newAttendance.Event.EventName = "[ " + edDetail.LeaveReason + " ]";
                    newAttendance.Event.EventType = 2;
                    newAttendance.Event.Color = "yellow";
                }

                //Checking if the day is holiday
                HolidaysRepository holyRepo = new HolidaysRepository();
                Holiday holiday = holyRepo.GetHolidaysByDate(year, month,i);

                if (holiday != null)
                {
                    newAttendance.Event.EventName = "[ " + holiday.Name +" ]";
                    newAttendance.Event.EventType = 0;
                    newAttendance.Event.Color = "yellowgreen";
                }
                else
                {
                    //Checking MakeUp day's
                    MakeUpDaysRepository makeRepo = new MakeUpDaysRepository();
                    MakeUpDay makeUp = makeRepo.GetMakeUpDaysByDate(year, month, i);
                    if (makeUp != null)
                    {
                        newAttendance.Event.EventName = "[ " + makeUp.Name + " ]";
                        newAttendance.Event.EventType = 1;
                        newAttendance.Event.Color = "DodgerBlue";
                    }
                }


                
                TimeSpan t;
                TimeSpan.TryParse(newAttendance.SumOfAllShift, out t);
                totalSumOfAllShift += t;

                TimeSpan.TryParse(newAttendance.FirstInMinusLastOut, out t);
                totalShift += t;

                attendances.Add(newAttendance);
            }

            data.report = attendances;
            //data.totalSumOfAllShift = totalSumOfAllShift.ToString(@"dd\.hh\:mm");
            data.totalSumOfAllShift = (int)totalSumOfAllShift.TotalHours + "H" + totalSumOfAllShift.ToString(@"\:mm") + "M";
            data.totalShift = (int)totalShift.TotalHours + "H" + totalShift.ToString(@"\:mm") + "M";

            return data;
        }

        private EmployeeAttendanceReport GetHeader(int daysInMonth)
        {
            List<DayCellValue> days = new List<DayCellValue>();

            for (int i = 1; i <= daysInMonth; i++)
            {
                days.Add(new DayCellValue()
                {
                    Color = "",
                    CellValue = i.ToString()
                });
            }

            return new EmployeeAttendanceReport()
            {
                Id = "ID",
                Name = "Name",
                MissingHour = "Missing Hour",
                OverTimeHour = "Over Time Hour",
                DayCellValues = days,
                TotalWorkingDay = "Total Working Day",
                TotalPresent = "Total Present",
                TotalAbsence = "Total Absence"
            };
        }

        public List<EmpCalendar> GetCalendarDataById(int employeeId)
        {
            List<EmpCalendar> calendarData = new List<EmpCalendar>();

            //Checking Approved Leave's
            EmpLeaveApplicationRepository lRepo = new EmpLeaveApplicationRepository();
            List<EmpLeaveApplicationDetail> edDetail = lRepo.GetApprovedLeaveApplicationByEmployeeId(employeeId);

            foreach (var item in edDetail)
            {
                calendarData.Add(new EmpCalendar()
                {
                    title = item.LeaveReason,
                    start = item.LeaveFrom.AsDateTime(),
                    end = item.LeaveTill.AsDateTime()
                });
            }

            //Checking if the day is holiday
            HolidaysRepository holyRepo = new HolidaysRepository();
            List<Holiday> holiday = holyRepo.GetAll();

            foreach (var item in holiday)
            {
                calendarData.Add(new EmpCalendar()
                {
                    title = item.Name,
                    start = item.LeaveDate.AsDateTime(),
                    end = item.LeaveDate.AsDateTime()
                });
            }

            MakeUpDaysRepository makeRepo = new MakeUpDaysRepository();
            List<MakeUpDay> makeUpDays = makeRepo.GetAll();
            foreach (var item in makeUpDays)
            {
                calendarData.Add(new EmpCalendar()
                {
                    title = item.Name,
                    start = item.MakeUpDate.AsDateTime(),
                    end = item.MakeUpDate.AsDateTime()
                });
            }

            return calendarData;
        }

        public List<EmpCalendar2> GetCalendar2DataById(int employeeId)
        {
            List<EmpCalendar2> calendarData = new List<EmpCalendar2>();

            //Checking Approved Leave's
            EmpLeaveApplicationRepository lRepo = new EmpLeaveApplicationRepository();
            List<EmpLeaveApplicationDetail> edDetail = lRepo.GetApprovedLeaveApplicationByEmployeeId(employeeId);

            foreach (var item in edDetail)
            {
                calendarData.Add(new EmpCalendar2()
                {
                    Subject = "Approved Leaves",
                    Description = item.LeaveReason,
                    Start = item.LeaveFrom.AsDateTime(),
                    End = item.LeaveTill.AsDateTime().AddDays(1),
                    ThemeColor = "lightgreen",
                    IsFullDay = true
                });
            }

            //Checking if the day is holiday
            HolidaysRepository holyRepo = new HolidaysRepository();
            List<Holiday> holiday = holyRepo.GetAll();

            foreach (var item in holiday)
            {
                calendarData.Add(new EmpCalendar2()
                {
                    Subject = "Holyday",
                    Description = item.Name,
                    Start = item.LeaveDate.AsDateTime(),
                    End = item.LeaveDate.AsDateTime(),
                    ThemeColor = "DodgerBlue",
                    IsFullDay = true
                });
            }

            MakeUpDaysRepository makeRepo = new MakeUpDaysRepository();
            List<MakeUpDay> makeUpDays = makeRepo.GetAll();
            foreach (var item in makeUpDays)
            {
                calendarData.Add(new EmpCalendar2()
                {
                    Subject = "Makeup Day",
                    Description = item.Name,
                    Start = item.MakeUpDate.AsDateTime(),
                    End = item.MakeUpDate.AsDateTime(),
                    ThemeColor = "blue",
                    IsFullDay = true
                });
            }

            return calendarData;
        }

        public List<EmployeeAttendanceReport> GenerateMonthlyAttendanceReportOfAllEmployee(QueryData data)
        {
            List<EmployeeAttendanceReport> employeeAttendance = new List<EmployeeAttendanceReport>();

            EmployeeRepository empRepo = new EmployeeRepository();
            List<Employee> employees = empRepo.GetAll();

            PayrollRepository payRepo = new PayrollRepository();
            Payroll p = payRepo.Get(data.PayrollId);
            int month = getMonthId(p.Month);
            int year = Int32.Parse(p.Year);
            int daysInMonth = 0;
            DateTime currDate = DateTime.Now;

            if (currDate.Year == year && currDate.Month == month)
            {
                daysInMonth = currDate.Day;
            }
            else if ((currDate.Year == year && currDate.Month < month) || currDate.Year < year)
            {
                return null;
            }
            else
            {
                daysInMonth = System.DateTime.DaysInMonth(year, month);
            }

            employeeAttendance.Add(GetHeader(daysInMonth));

            foreach (var item in employees)
            {
                EmployeeAttendanceReport newReport = new EmployeeAttendanceReport();
                newReport.Id = item.EmployeeId.ToString();
                newReport.Name = item.FirstName + " " + item.MiddleName + " " + item.LastName;

                dynamic attendance = GenerateAttendanceReport(data.PayrollId, item.EmployeeId);
                List<AttendanceReport> monthlyAttendance = attendance.report;
                newReport.MissingHour = "";
                newReport.OverTimeHour = "0H:0M";
                int a = 0;
                int totalAbsence = 0, totalPresent = 0,totalLeave=0;
                foreach (var report in monthlyAttendance)
                {
                    DayCellValue cell = new DayCellValue();
                    if (report.IsPresent)
                    {
                        totalPresent++;
                        TimeSpan sum;
                        if (data.Shift == 0)
                        {
                            TimeSpan.TryParse(report.SumOfAllShift, out sum);
                        }
                        else
                        {
                            TimeSpan.TryParse(report.FirstInMinusLastOut, out sum);
                        }
                        TimeSpan workingHour = new TimeSpan(data.WorkingHour, 0,0);
                        //TimeSpan.TryParse(newAttendance.In[span], out In);
                        TimeSpan sumOfAll = sum - workingHour;
                        TimeSpan delay = new TimeSpan(0, data.DelayMargin, 0);
                        TimeSpan over = new TimeSpan(0, data.OverTimeMargin, 0);
                        TimeSpan b = new TimeSpan(0,0,0);
                        if (sumOfAll >= b)
                        {
                            if (sumOfAll > over)
                            {
                                if (newReport.OverTimeHour == "0H:0M")
                                {
                                    newReport.OverTimeHour = sumOfAll.Duration().ToString(@"dd\.hh\:mm");
                                }
                                else
                                {
                                    TimeSpan baseTime;
                                    TimeSpan.TryParse(newReport.OverTimeHour, out baseTime);
                                    baseTime += sumOfAll.Duration();
                                    newReport.OverTimeHour = baseTime.ToString(@"dd\.hh\:mm");
                                }
                                cell.Color = "green";
                                cell.CellValue = sumOfAll.ToString(@"hh\:mm");
                            }
                            else
                            {
                                cell.Color = "green";
                                cell.CellValue = "P";
                            }
                        }
                        else
                        {
                            if (sumOfAll.Duration() > delay)
                            {
                                if (newReport.MissingHour == "")
                                {
                                    newReport.MissingHour = sumOfAll.Duration().ToString(@"dd\.hh\:mm");
                                }
                                else
                                {
                                    TimeSpan baseTime;
                                    TimeSpan.TryParse(newReport.MissingHour, out baseTime);
                                    baseTime += sumOfAll.Duration();
                                    newReport.MissingHour = baseTime.ToString(@"dd\.hh\:mm");
                                }
                                cell.Color = "Tomato";
                                cell.CellValue = "-" + sumOfAll.ToString(@"hh\:mm");
                            }
                            else
                            {
                                cell.Color = "green";
                                cell.CellValue = "P";
                            }
                        }
                    }
                    else
                    {
                        if (report.Event.EventName == "")
                        {
                            TimeSpan workingHour = new TimeSpan(data.WorkingHour,0 , 0);
                            if (newReport.MissingHour == "")
                            {
                                newReport.MissingHour = workingHour.Duration().ToString(@"dd\.hh\:mm");
                            }
                            else
                            {
                                TimeSpan Before;
                                TimeSpan.TryParse(newReport.MissingHour, out Before);
                                Before += workingHour.Duration();
                                newReport.MissingHour = Before.Duration().ToString(@"dd\.hh\:mm");
                            }

                            totalAbsence++;
                            cell.Color = "Tomato";
                            cell.CellValue = "A";
                        }
                        else
                        {
                            if (report.Event.EventType == 0)
                            {
                                cell.Color = "DodgerBlue";
                                cell.CellValue = "H";
                            }
                            else if (report.Event.EventType == 2)
                            {
                                totalLeave++;
                                cell.Color = "yellowgreen";
                                cell.CellValue = "AA";
                            }
                            else if (report.Event.EventType == 1)
                            {
                                totalAbsence++;
                                cell.Color = "red";
                                cell.CellValue = "A(M)";
                            }
                        }
                    }

                    newReport.DayCellValues.Add(cell);
                    a++;
                    if (daysInMonth == a)
                    {
                        break;
                    }
                }

                TimeSpan convertSpan;
                TimeSpan.TryParse(newReport.OverTimeHour, out convertSpan);
                newReport.OverTimeHour = (int)convertSpan.TotalHours + "H" + convertSpan.ToString(@"\:mm") + "M";
                TimeSpan.TryParse(newReport.MissingHour, out convertSpan);
                newReport.MissingHour = (int)convertSpan.TotalHours + "H" + convertSpan.ToString(@"\:mm") + "M";

                newReport.TotalAbsence = totalAbsence.ToString();
                newReport.TotalPresent = totalPresent.ToString();
                newReport.TotalWorkingDay = (totalAbsence + totalPresent + totalLeave).ToString();

                employeeAttendance.Add(newReport);
            }

            return employeeAttendance;
        }

        private int getMonthId(string month)
        {
            if (month == "January")
            {
                return 1;
            }
            else if (month == "February")
            {
                return 2;
            }
            else if (month == "March")
            {
                return 3;
            }
            else if (month == "April")
            {
                return 4;
            }
            else if (month == "May")
            {
                return 5;
            }
            else if (month == "June")
            {
                return 6;
            }
            else if (month == "July")
            {
                return 7;
            }
            else if (month == "August")
            {
                return 8;
            }
            else if (month == "September")
            {
                return 9;
            }
            else if (month == "October")
            {
                return 10;
            }
            else if (month == "November")
            {
                return 11;
            }
            else if (month == "December")
            {
                return 12;
            }
            else
            {
                return -1;
            }
        }
    }
}