using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Data;
using System.IO;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_DiamondSpecificReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            UACCSpecific.AccessRights = this.ViewState;
            UACCSpecific.Title = "UACC Specific Report";
            UACCSpecific.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ucOwner.SelectedAddress = "3817";
                ucOwner_Onchange(null, null);
            }
            ucOwner.Enabled = false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UACCSpecific_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportUACC('" + 33 + "','" + 193 + "','" + 572 + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','UACC');", true);

                Export2XLUACC(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue));

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static void Export2XLUACC(int? finyear, int? Ownerid, int? isbudgeted, int? vesselid)
    {
        string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\DiamondReport.xlsx";
        string strDestinationfile = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + @"DiamondReport_Export.xlsx";

        FileInfo Inputfile = new FileInfo(file);
        if (File.Exists(strDestinationfile)) File.Delete(strDestinationfile);
        Inputfile.CopyTo(strDestinationfile);

        FileInfo newFile = new FileInfo(strDestinationfile);


        using (ExcelPackage pck = new ExcelPackage(newFile))
        {
            PopulateUACCReport(pck, finyear, Ownerid, null, isbudgeted, vesselid);

            HttpContext.Current.Response.Clear();
            pck.SaveAs(HttpContext.Current.Response.OutputStream);

            HttpCookie token = HttpContext.Current.Request.Cookies["fileDownloadToken"];
            if (token != null)
            {
                token.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(token);
            }

            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=UACCReport_Export.xlsx");


            HttpContext.Current.Response.End();
        }
    }

    private static void PopulateUACCReport(ExcelPackage pck, int? finyear, int? Ownerid, int? level, int? isbudgeted, int? vesselid)
    {
        ExcelWorksheet ws = pck.Workbook.Worksheets["ALPINE MIA"];
        int? Accountid = null;
        int? Month = General.GetNullableInteger(DateTime.UtcNow.Month.ToString("00"));
        DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(vesselid)), 1);
        if (dsaccount.Tables[0].Rows.Count > 0)
        {
            Accountid = Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]);
        }
        DataSet dsbudget = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationlist(
          finyear
          , Ownerid
          , 3
          , 1
          , vesselid
          , null
          );
        DataTable dt = dsbudget.Tables[0];
        int nrow = 12;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "OFFICERS/CREW WAGES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }
                }
            }
        }

        DataTable dttravelothercrewexpenses = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dttravelothercrewexpenses.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "TRAVEL/OTHER CREW EXPENSES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtvictualling = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtvictualling.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "VICTUALLING")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtdeckrepairs = dsbudget.Tables[0];
        nrow = nrow + 3;
        if (dtdeckrepairs.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "DECK REPAIRS")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtdeckmaintenancespares = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtdeckmaintenancespares.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "DECK MAINTENANCE & SPARES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtdeckstores = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtdeckstores.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "DECK STORES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtenginerepairs = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtenginerepairs.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "ENGINE REPAIRS")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtenginemaintspares = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtenginemaintspares.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "ENGINE MAINT & SPARES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtsparestranportation = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtsparestranportation.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "SPARES TRANPORTATION")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtenginestores = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtenginestores.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "ENGINE STORES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtsafetyequipment = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtsafetyequipment.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "SAFETYE QUIPMENT")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }


        DataTable dtvettingfees = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtvettingfees.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "VETTING FEES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtsurveyfeesclasscert = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtsurveyfeesclasscert.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "SURVEY FEES/CLASS CERT")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtfreshwater = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtfreshwater.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "FRESH WATER")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        DataTable dtcommunication = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtcommunication.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "COMMUNICATION")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        DataTable dtmiscadminsupervision = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtmiscadminsupervision.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "MISC, ADMIN, SUPERVISION")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }


        DataTable dtmanagementfee = dsbudget.Tables[0];
        nrow = nrow + 1;
        if (dtmanagementfee.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "MANAGEMENT FEE")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }

        nrow = nrow + 7;
        if (dtmanagementfee.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "CHARTERER EXPENSES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }
        nrow = nrow + 7;
        if (dtmanagementfee.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "CHARTERER EXPENSES")
                {
                    ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                    ws.Cells[nrow, 3].Value = dr["FLDBUDGETAMOUNT"];
                    ws.Cells[nrow, 2].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/ 12";
                    DataSet dsbudgetbreakupofficerscrewwages = null;
                    dsbudgetbreakupofficerscrewwages = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakupofficerscrewwages = dsbudgetbreakupofficerscrewwages.Tables[0];
                    int dtrowcount1 = dtbudgetbreakupofficerscrewwages.Rows.Count;
                    for (int i = 0; i < dtrowcount1; i++)
                    {
                        ws.Cells[nrow, 4 + i].Value = dtbudgetbreakupofficerscrewwages.Rows[i]["FLDBUDGETAMOUNT"];

                    }

                }
            }
        }


    }

    private static string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }
    protected void ucOwner_Onchange(object sender, EventArgs e)
    {
        DataSet dsveaaelname = null;
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.OwnerVessellist(3817);
        ddlvessel.DataSource = dsveaaelname;
        ddlvessel.DataTextField = "FLDVESSELNAME";
        ddlvessel.DataValueField = "FLDVESSELID";
        ddlvessel.DataBind();
        ddlvessel.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }
}
