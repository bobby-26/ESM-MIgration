using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsReportAllotmentPending : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState); PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Confirmation Pending", "PENDINGLIST");
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbarmain.Show();
            MenuRequest.SelectedMenuIndex = 0;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsReportAllotmentPending.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAllotment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsReportAllotmentPending.aspx", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsReportAllotmentPending.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;
                ViewState["ALLOTMENTTYPE"] = "";
                ViewState["ALLOTMENTID"] = "";
                ViewState["PVVOUCHERYN"] = "0";
                ClearFilter();
                gvAllotment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsReportAllotmentPending.aspx";
            }
            else if (CommandName.ToUpper().Equals("PENDINGLIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsReportALTPending.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?MONTH = " + ddlMonth.SelectedMonth + " & YEAR = " + ddlYear.SelectedYear);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDCURRENCYCODE", "FLDACCONTOPENEDBY", "FLDACCOUNTTYPECODE", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDEMPLOYEEADDRESS", "FLDEMPLOYEEMAILID", "FLDCOMPANYNAME", "FLDCOMPANYADDRESS", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions = { "Request Date", "Request No.", "Vessel", "File No.", "Employee", "Rank", "Allotment Type", "Component Name", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Voucher No.", "Beneficiary Name", "Beneficiary Account Number", "Bank SWIFT Code", "Bank IFSC code", "Bank Currency", "Bank Account Opened By", "Account type", "D.O.B", "Nationality", "Seafarer Address", "Beneficiary E-mail ID", "Company Name", "Company Address", "Created By", "Created On" };

            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDCURRENCYCODE", "FLDACCONTOPENEDBY", "FLDACCOUNTTYPECODE", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDEMPLOYEEADDRESS", "FLDEMPLOYEEMAILID", "FLDCOMPANYNAME", "FLDCOMPANYADDRESS", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Vessel", "File No.", "Employee", "Rank", "Allotment Type", "Component Name", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Voucher No.", "Beneficiary Name", "Beneficiary Account Number", "Bank SWIFT Code", "Bank IFSC code", "Bank Currency", "Bank Account Opened By", "Account type", "D.O.B", "Nationality", "Seafarer Address", "Beneficiary E-mail ID", "Company Name", "Company Address", "Created By", "Created On" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string Vessellist = ucVessel.SelectedVessel;
            ds = PhoenixAccountsAllotmentRequest.SearchAllotmentReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableString(txtFileNo.Text)
               , General.GetNullableString(txtName.Text)
               , General.GetNullableInteger(ucRank.SelectedRank)
               , General.GetNullableString(Vessellist)
               , General.GetNullableInteger(ddlMonth.SelectedMonth)
               , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
               , General.GetNullableInteger(ddlAllotmentType.SelectedHard)
               , General.GetNullableInteger(ucRequestStatus.SelectedHard)
               , General.GetNullableInteger(chkIsNotPaymentVoucherYN.Checked == true ? "1" : "0")
               , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvAllotment.PageSize, ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (General.GetNullableString(dr["FLDPAYMENTVOUCHERNUMBER"].ToString()) != null)
                    ViewState["PVVOUCHERYN"] = "1";
                else ViewState["PVVOUCHERYN"] = "0";

                DataColumn GroupBy = new DataColumn();
                GroupBy.ColumnName = "FLDGROUPBY";
                ds.Tables[0].Columns.Add(GroupBy);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["FLDEMPLOYEEID"].ToString() != "")
                    {                        
                        ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDFILENO"].ToString() + " / " + ds.Tables[0].Rows[i]["FLDEMPLOYEENAME"].ToString() + " / " + ds.Tables[0].Rows[i]["FLDRANKNAME"].ToString() ;
                    }
                }
            }
            gvAllotment.DataSource = ds;
            gvAllotment.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (ViewState["PVVOUCHERYN"].ToString() == "0")
                General.SetPrintOptions("gvAllotment", "Allotment Report", alCaptions, alColumns, ds);
            else
                General.SetPrintOptions("gvAllotment", "Allotment Report", alCaptions1, alColumns1, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            ImageButton hlb = (ImageButton)e.Item.FindControl("cmdHistory");
            if (hlb != null)
                hlb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentHistory.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + " &VESSELID=" + drv["FLDVESSELID"].ToString() + "&VESSELNAME=" + drv["FLDVESSELNAME"].ToString() + "&MONTH=" + drv["FLDMONTH"].ToString() + "&YEAR=" + drv["FLDYEAR"].ToString() + "&SIGNINOFFID='); return true;");
        }
    }
    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAllotment.SelectedIndexes.Clear();
        gvAllotment.EditIndexes.Clear();
        gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["ALLOTMENTID"] = "";
                ViewState["ALLOTMENTTYPE"] = "";
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDCURRENCYCODE", "FLDACCONTOPENEDBY", "FLDACCOUNTTYPECODE", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDEMPLOYEEADDRESS", "FLDEMPLOYEEMAILID", "FLDCOMPANYNAME", "FLDCOMPANYADDRESS", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions = { "Request Date", "Request No.", "Vessel", "File No.", "Employee", "Rank", "Allotment Type", "Component Name", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Voucher No.", "Beneficiary Name", "Beneficiary Account Number", "Bank SWIFT Code", "Bank IFSC code", "Bank Currency", "Bank Account Opened By", "Account type", "D.O.B", "Nationality", "Seafarer Address", "Beneficiary E-mail ID", "Company Name", "Company Address", "Created By", "Created On" };

            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDCURRENCYCODE", "FLDACCONTOPENEDBY", "FLDACCOUNTTYPECODE", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDEMPLOYEEADDRESS", "FLDEMPLOYEEMAILID", "FLDCOMPANYNAME", "FLDCOMPANYADDRESS", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Vessel", "File No.", "Employee", "Rank", "Allotment Type", "Component Name", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Voucher No.", "Beneficiary Name", "Beneficiary Account Number", "Bank SWIFT Code", "Bank IFSC code", "Bank Currency", "Bank Account Opened By", "Account type", "D.O.B", "Nationality", "Seafarer Address", "Beneficiary E-mail ID", "Company Name", "Company Address", "Created By", "Created On" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string Vessellist = ucVessel.SelectedVessel;
            ds = PhoenixAccountsAllotmentRequest.SearchAllotmentReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableString(txtFileNo.Text)
               , General.GetNullableString(txtName.Text)
               , General.GetNullableInteger(ucRank.SelectedRank)
               , General.GetNullableString(Vessellist)
               , General.GetNullableInteger(ddlMonth.SelectedMonth)
               , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
               , General.GetNullableInteger(ddlAllotmentType.SelectedHard)
               , General.GetNullableInteger(ucRequestStatus.SelectedHard)
               , General.GetNullableInteger(chkIsNotPaymentVoucherYN.Checked == true ? "1" : "0")
               , sortexpression, sortdirection, 1, PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename= AllotmentReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment  Report </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            if (ViewState["PVVOUCHERYN"].ToString() == "0")
            {
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td width='20%'>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write("<td  class='text'>" + dr[alColumns[i]].ToString() + "</td>");
                    }
                    Response.Write("</tr>");
                }
            }
            else
            {
                for (int i = 0; i < alCaptions1.Length; i++)
                {
                    Response.Write("<td width='20%'>");
                    Response.Write("<b>" + alCaptions1[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns1.Length; i++)
                    {
                        Response.Write("<td  class='text'>");
                        Response.Write(dr[alColumns1[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearFilter()
    {
        nvc.Clear();
        ddlMonth.SelectedMonth = DateTime.Now.Month.ToString();
        ddlYear.SelectedYear = DateTime.Now.Year;
        ddlAllotmentType.SelectedHard = "";
        ucRequestStatus.SelectedHard = "";
        txtFileNo.Text = "";
        ucRank.SelectedRank = "";
        txtName.Text = "";
        chkIsNotPaymentVoucherYN.Checked = true;
    }
}
