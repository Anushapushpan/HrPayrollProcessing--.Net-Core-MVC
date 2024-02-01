using BusinessLayer.Master;
using BusinessLayer.Transaction;
using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        List<DropdownSelect> dropdownselects = new List<DropdownSelect>();
        CodeMasterManager objCodesmaster = new CodeMasterManager();
        PrEmployeePayrollManager objPayrollManager= new PrEmployeePayrollManager();
        [HttpGet]
        [Route("EmpStatus/{cmtype}")]
        public IActionResult EmpStatus(string cmtype)
        {
            try
            {
                List<DropdownSelect> dropdownselects = new List<DropdownSelect>();             
                DataTable dt = new DataTable();
                dt = objCodesmaster.FetchEmpStatus(cmtype);
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new DropdownSelect()
                    {
                        Code = d["CM_CODE"].ToString(),
                        Type = d["CM_TYPE"].ToString(),
                        Value = d["CM_VALUE"].ToString(),
                        Text = d["CM_DESC"].ToString()
                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("Department")]
        public IActionResult Department()
        {
            try
            {
                List<DeptDdlSelect> dropdownselects = new List<DeptDdlSelect>();
                DepartmentMasterManager objDeptManager = new DepartmentMasterManager();
                DataTable dt = new DataTable();
                dt = objDeptManager.FetchDeptNo();
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new DeptDdlSelect()
                    {
                        Number= d["DEPT_NO"].ToString(),
                        Name = d["DEPT_NAME"].ToString(),
                        
                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ManagerDropdown")]
        public IActionResult ManagerDropdown()
        {
            try
            {
                List<ManagerDdlSelect> dropdownselects = new List<ManagerDdlSelect>();
                PrEmployeeManager objEmpManager = new PrEmployeeManager();
                DataTable dt = new DataTable();
                dt = objEmpManager.FetchManagerNo();
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new ManagerDdlSelect()
                    {
                        empNo = d["EH_EMP_NO"].ToString(),
                        Name = d["EMP_NAME"].ToString(),

                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Designation/{cmtype}")]
        public IActionResult Designation(string cmtype)
        {
            try
            {       
                DataTable dt = new DataTable();
                dt = objCodesmaster.FetchDesignationFromCodesMaster(cmtype);
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new DropdownSelect()
                    {
                        Code = d["CM_CODE"].ToString(),
                        Type = d["CM_TYPE"].ToString(),
                        Value = d["CM_VALUE"].ToString(),
                        Text = d["CM_DESC"].ToString()
                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Grade/{cmtype}")]
        public IActionResult Grade(string cmtype)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objCodesmaster.FetchGradesFromCodesMaster(cmtype);
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new DropdownSelect()
                    {
                        Code = d["CM_CODE"].ToString(),
                        Type = d["CM_TYPE"].ToString(),
                        Value = d["CM_VALUE"].ToString(),
                        Text = d["CM_DESC"].ToString()
                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetMonth")]
   
        public IActionResult GetMonth(PrEmployeeAttendenceEntity objattnd)
        {
            try
            {
                List<DropdownSelect> dropdownselects = new List<DropdownSelect>();
                CodeMasterManager objCodesmaster = new CodeMasterManager();
                DataTable dt = new DataTable();
                dt = objCodesmaster.FetchMonth(objattnd);
                foreach (DataRow d in dt.Rows)
                {
                    dropdownselects.Add(new DropdownSelect()
                    {
                        Code = d["CM_CODE"].ToString(),
                        Type = d["CM_TYPE"].ToString(),
                        Value = d["CM_VALUE"].ToString(),
                        Text = d["CM_DESC"].ToString()

                    });
                }
                return Ok(dropdownselects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("EmpNo/{yyyymm}")]
        public IActionResult EmpNo(string yyyymm)
        {
            try
            {
                List<DdlEmpNo> ddlEmpNos = new List<DdlEmpNo>();
                DataTable dt = new DataTable();
                dt = objPayrollManager.FetchEmpNo(yyyymm);
                foreach (DataRow d in dt.Rows)
                {
                    ddlEmpNos.Add(new DdlEmpNo()
                    {
                        empNo = d["PR_EMP_NO"].ToString(),
                        
                    });
                }
                return Ok(ddlEmpNos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
    }


