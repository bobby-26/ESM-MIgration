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

public partial class Accounts_CIDOSpecificReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Delivery"] = "";
            ViewState["vessel"] = "";
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT");
            CIDOSpecific.AccessRights = this.ViewState;
            CIDOSpecific.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ucOwner.SelectedAddress = "3794";
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

    protected void CIDOSpecificReport_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void CIDOSpecific_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        try
        {
            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "Exportcido('" + 33 + "','" + 193 + "','" + 572 + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','CIDO');", true);
              string headyear= ucFinancialYear.SelectedText;
                DataSet dsvessel = PhoenixRegistersVessel.EditVessel(General.GetNullableInteger(ddlvessel.SelectedValue));
                if (dsvessel.Tables.Count > 0)
                {
                    if (dsvessel.Tables[0].Rows.Count > 0)
                    {
                        DataRow drvessel = dsvessel.Tables[0].Rows[0];
                        ViewState["Delivery"] = Convert.ToString(drvessel["FLDORGDATEENTERED"]);
                        ViewState["vessel"] =Convert.ToString(ddlvessel.SelectedItem);
                    }
                }
                Export2XLCIDO(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue), headyear, Convert.ToString(ViewState["vessel"]), Convert.ToString(ViewState["Delivery"]));
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static void Export2XLCIDO(int? finyear, int? Ownerid, int? isbudgeted, int? vesselid , string headyear, string vesselname, string Delivery)
    {
        string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\CidoReport.xlsx";
        string strDestinationfile = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + @"CidoReport_Export.xlsx";

        FileInfo Inputfile = new FileInfo(file);
        if (File.Exists(strDestinationfile)) File.Delete(strDestinationfile);
        Inputfile.CopyTo(strDestinationfile);

        FileInfo newFile = new FileInfo(strDestinationfile);


        using (ExcelPackage pck = new ExcelPackage(newFile))
        {
            PopulatecidoReport(pck, finyear, Ownerid, null, isbudgeted, vesselid, headyear,  vesselname,  Delivery);

            HttpContext.Current.Response.Clear();
            pck.SaveAs(HttpContext.Current.Response.OutputStream);

            HttpCookie token = HttpContext.Current.Request.Cookies["fileDownloadToken"];
            if (token != null)
            {
                token.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(token);
            }

            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=CidoReport_Export.xlsx");


            HttpContext.Current.Response.End();
        }
    }

    private static void PopulatecidoReport(ExcelPackage pck, int? finyear, int? Ownerid, int? level, int? isbudgeted, int? vesselid, string headyear,string vesselname,string Delivery)
    {
        ExcelWorksheet ws = pck.Workbook.Worksheets["CidoReport"];
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
            , 1
            , 1
            , vesselid
            , null
            );

        DataSet dsnonbbudget = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationlist(
            finyear
            , Ownerid
            , 1
            , 0
            , vesselid
            , null
            );

        DataTable dt = dsbudget.Tables[0];
        DataTable dt1 = dsnonbbudget.Tables[0];
        int hrow = 1;
        ws.Cells[hrow, 4].Value = "VESSELID NAME :"+ vesselname;
        ws.Cells[hrow, 8].Value = "* DELIVERY DATE :" + Delivery;
        hrow = hrow + 1;
        ws.Cells[hrow, 7].Value = headyear;
        int nrow = 4;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                ws.Cells[nrow, 1].Value = dr["FLDOWNERBUDGETGROUP"].ToString();
                ws.Cells[nrow, 2].Value = dr["FLDBUDGETAMOUNT"];
                ws.Cells[nrow, 5].Formula = "=" + GetExcelColumnName(2) + nrow.ToString() + "/ 12";
                ws.Cells[nrow, 6].Value = Month;
                DataSet dsbudgetbreakup = null;
                if (dr["FLDOWNERBUDGETGROUP"].ToString() == "STORE" || dr["FLDOWNERBUDGETGROUP"].ToString() == "INSURANCE")
                {

                    DataSet dsbudget1 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationlist(
                        finyear
                        , Ownerid
                        , 2
                        , 1
                        , vesselid
                        , General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString())
                        );


                    if (dsbudget1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drr1 in dsbudget1.Tables[0].Rows)
                        {
                            nrow = nrow + 1;
                            ws.Cells[nrow, 1].Value = drr1["FLDOWNERBUDGETGROUP"].ToString();
                            ws.Cells[nrow, 2].Value = drr1["FLDBUDGETAMOUNT"];
                            ws.Cells[nrow, 5].Formula = "=" + GetExcelColumnName(2) + nrow.ToString() + "/ 12";
                            ws.Cells[nrow, 6].Value = Month;

                            DataSet dsbudgetbreakup1 = null;
                            dsbudgetbreakup1 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
               (General.GetNullableGuid(drr1["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                            DataTable dtbudgetbreakup1 = dsbudgetbreakup1.Tables[0];
                            int dtrowcount1 = dtbudgetbreakup1.Rows.Count;
                            for (int i = 0; i < dtrowcount1; i++)
                            {
                                ws.Cells[nrow, 7 + i].Value = dtbudgetbreakup1.Rows[i]["FLDPAIDAMOUNT"];
                                ws.Cells[nrow, 20].Formula = "=Sum(" + GetExcelColumnName(7) + nrow.ToString() + ":" + GetExcelColumnName(18) + nrow.ToString() + ")";
                                ws.Cells[nrow, 19].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString();
                                ws.Cells[nrow, 3].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString() + "*" + 12;
                                ws.Cells[nrow, 4].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/" + GetExcelColumnName(2) + nrow.ToString() + "*" + 100 + "%";
                            }

                        }

                    }
                }

                dsbudgetbreakup = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                DataTable dtbudgetbreakup = dsbudgetbreakup.Tables[0];
                int dtrowcount = dtbudgetbreakup.Rows.Count;
                for (int i = 0; i < dtrowcount; i++)
                    ws.Cells[nrow, 7 + i].Value = dtbudgetbreakup.Rows[i]["FLDPAIDAMOUNT"];
                ws.Cells[nrow, 20].Formula = "=Sum(" + GetExcelColumnName(7) + nrow.ToString() + ":" + GetExcelColumnName(18) + nrow.ToString() + ")";
                ws.Cells[nrow, 19].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString();
                ws.Cells[nrow, 3].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString() + "*" + 12;
                ws.Cells[nrow, 4].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/" + GetExcelColumnName(2) + nrow.ToString() + "*" + 100 + "%";
                nrow = nrow + 1;

            }
        }

        //ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
        ws.Cells[nrow, 1].Value = "SUB-TOTAL";
        for (int i = 2; i <= 20; i++)
            ws.Cells[nrow, i].Formula = "=Sum(" + GetExcelColumnName(i) + (4).ToString() + ":" + GetExcelColumnName(i) + (nrow - 1).ToString() + ")";

        nrow = nrow + 1;
        // ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
        ws.Cells[nrow, 1].Value = "MONTHLY (%)";
        for (int i = 7; i <= 20; i++)
            ws.Cells[nrow, i].Formula = "=" + GetExcelColumnName(i) + (4).ToString() + "/$" + GetExcelColumnName(5) + (nrow - 1).ToString();
        //nonbudget

        nrow = nrow + 1;
        if (dt1.Rows.Count > 0)
        {
            foreach (DataRow dr1 in dt1.Rows)
            {

                ws.Cells[nrow, 1].Value = dr1["FLDOWNERBUDGETGROUP"].ToString();
                ws.Cells[nrow, 2].Value = dr1["FLDBUDGETAMOUNT"];
                ws.Cells[nrow, 5].Formula = "=" + GetExcelColumnName(2) + nrow.ToString() + "/ 12";
                ws.Cells[nrow, 6].Value = Month;
                DataSet dsbudgetbreakup = null;
                //
                if (dr1["FLDOWNERBUDGETGROUP"].ToString() == "UNBUDGETED EXPENSES")
                {

                    DataSet dsbudget11 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationlist(
                        finyear
                        , Ownerid
                        , 2
                        , 0
                        , vesselid
                        , General.GetNullableGuid(dr1["FLDOWNERBUDGETGROUPID"].ToString())
                        );


                    if (dsbudget11.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drr2 in dsbudget11.Tables[0].Rows)
                        {
                            nrow = nrow + 1;
                            ws.Cells[nrow, 2].Value = drr2["FLDOWNERBUDGETGROUP"].ToString();

                            DataSet dsbudgetbreakup2 = null;
                            if (drr2["FLDOWNERBUDGETGROUP"].ToString() == "ACCIDENT" || drr2["FLDOWNERBUDGETGROUP"].ToString() == "COMMERCIAL EXPENSES")
                            {
                                DataSet dsbudget12 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationlist(
                        finyear
                        , Ownerid
                        , 3
                        , 0
                        , vesselid
                        , General.GetNullableGuid(drr2["FLDOWNERBUDGETGROUPID"].ToString())
                        );
                                if (dsbudget12.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drr3 in dsbudget12.Tables[0].Rows)
                                    {
                                        nrow = nrow + 1;
                                        ws.Cells[nrow, 3].Value = drr3["FLDOWNERBUDGETGROUP"].ToString();

                                        DataSet dsbudgetbreakup3 = null;
                                        dsbudgetbreakup3 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                                        (General.GetNullableGuid(drr3["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                                        DataTable dtbudgetbreakup3 = dsbudgetbreakup3.Tables[0];
                                        int dtrowcount3 = dtbudgetbreakup3.Rows.Count;
                                        for (int i = 0; i < dtrowcount3; i++)
                                        {
                                            ws.Cells[nrow, 7 + i].Value = dtbudgetbreakup3.Rows[i]["FLDPAIDAMOUNT"];
                                            ws.Cells[nrow, 20].Formula = "=Sum(" + GetExcelColumnName(7) + nrow.ToString() + ":" + GetExcelColumnName(18) + nrow.ToString() + ")";
                                            ws.Cells[nrow, 19].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString();
                                            // ws.Cells[nrow, 3].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString() + "*" + 12;
                                            // ws.Cells[nrow, 4].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/" + GetExcelColumnName(2) + nrow.ToString() + "*" + 100 + "%";
                                        }
                                    }
                                }

                            }

                            if (drr2["FLDOWNERBUDGETGROUP"].ToString() != "ACCIDENT" || drr2["FLDOWNERBUDGETGROUP"].ToString() != "COMMERCIAL EXPENSES")
                            {
                                dsbudgetbreakup2 = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
           (General.GetNullableGuid(drr2["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                                DataTable dtbudgetbreakup2 = dsbudgetbreakup2.Tables[0];
                                int dtrowcount1 = dtbudgetbreakup2.Rows.Count;
                                for (int i = 0; i < dtrowcount1; i++)
                                {
                                    ws.Cells[nrow, 7 + i].Value = dtbudgetbreakup2.Rows[i]["FLDPAIDAMOUNT"];
                                    ws.Cells[nrow, 20].Formula = "=Sum(" + GetExcelColumnName(7) + nrow.ToString() + ":" + GetExcelColumnName(18) + nrow.ToString() + ")";
                                    ws.Cells[nrow, 19].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString();
                                    // ws.Cells[nrow, 3].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString() + "*" + 12;
                                    // ws.Cells[nrow, 4].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/" + GetExcelColumnName(2) + nrow.ToString() + "*" + 100 + "%";
                                }
                            }

                        }

                    }
                    //ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
                    ws.Cells[nrow, 1].Value = "SUB-TOTAL";
                    for (int i = 2; i <= 20; i++)
                        ws.Cells[nrow, i].Formula = "=Sum(" + GetExcelColumnName(i) + (4).ToString() + ":" + GetExcelColumnName(i) + (nrow - 1).ToString() + ")";

                    nrow = nrow + 1;
                    // ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
                    ws.Cells[nrow, 1].Value = "MONTHLY (%)";
                    for (int i = 7; i <= 20; i++)
                        ws.Cells[nrow, i].Formula = "=" + GetExcelColumnName(i) + (4).ToString() + "/$" + GetExcelColumnName(5) + (nrow - 1).ToString();
                    //nonbudget
                }

                //
                if (dr1["FLDOWNERBUDGETGROUP"].ToString() != "UNBUDGETED EXPENSES")
                {
                    dsbudgetbreakup = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch
                    (General.GetNullableGuid(dr1["FLDOWNERBUDGETGROUPID"].ToString()), finyear, vesselid, Accountid);
                    DataTable dtbudgetbreakup = dsbudgetbreakup.Tables[0];
                    int dtrowcount = dtbudgetbreakup.Rows.Count;
                    for (int i = 0; i < dtrowcount; i++)
                        ws.Cells[nrow, 7 + i].Value = dtbudgetbreakup.Rows[i]["FLDPAIDAMOUNT"];
                    ws.Cells[nrow, 20].Formula = "=Sum(" + GetExcelColumnName(7) + nrow.ToString() + ":" + GetExcelColumnName(18) + nrow.ToString() + ")";
                    ws.Cells[nrow, 19].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString();
                    ws.Cells[nrow, 3].Formula = "=" + GetExcelColumnName(20) + nrow.ToString() + "/" + GetExcelColumnName(6) + nrow.ToString() + "*" + 12;
                    ws.Cells[nrow, 4].Formula = "=" + GetExcelColumnName(3) + nrow.ToString() + "/" + GetExcelColumnName(2) + nrow.ToString() + "*" + 100 + "%";
                    nrow = nrow + 1;
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
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.OwnerVessellist(3794);
        ddlvessel.DataSource = dsveaaelname;
        ddlvessel.DataTextField = "FLDVESSELNAME";
        ddlvessel.DataValueField = "FLDVESSELID";
        ddlvessel.DataBind();
        ddlvessel.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
