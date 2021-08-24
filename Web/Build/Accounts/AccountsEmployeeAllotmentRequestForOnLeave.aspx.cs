using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class AccountsEmployeeAllotmentRequestForOnLeave : PhoenixBasePage
{
    string strMAL, strSPA, strSOF, strORR, strSLT;
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    GridTableCell gridTable = (GridTableCell)gvAllotmentOnLeave.Controls[0];
    //    ViewState["EmpNo"] = "";
    //    foreach (GridDataItem gv in gvAllotmentOnLeave.Items)
    //    {
    //        RadLabel lblempid = (RadLabel)gv.FindControl("lblEmployeeId");
    //        RadLabel lblEmpFileNo = (RadLabel)gv.FindControl("lblEmpFileNo");
    //        RadLabel lnkName = (RadLabel)gv.FindControl("lblEmployeeName");
    //        RadLabel lblRank = (RadLabel)gv.FindControl("lblRank");
    //        if (lblEmpFileNo != null)
    //        {
    //            if (ViewState["EmpNo"].ToString().Trim().Equals("") || !ViewState["EmpNo"].ToString().Trim().Equals(lblempid.Text.Trim()))
    //            {
    //                ViewState["EmpNo"] = lblempid.Text.Trim();
    //                int rowIndex = gridTable.Item.ItemIndex;
    //                // Add new group header row  
    //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
    //                TableCell headerCell = new TableCell();
    //                headerCell.ColumnSpan = gvAllotmentOnLeave.Columns.Count;
    //                headerCell.Text += @"<table width='100%' style='font-size: 11px;'><tr>";
    //                headerCell.Text += @"<td style='font-weight: bold;'>" + string.Format("{0}", lblEmpFileNo != null ? lblEmpFileNo.Text.Trim() : "") + " / " + string.Format("{0}", lnkName != null ? lnkName.Text.Trim() : "") + " / " + string.Format("{0}", lblRank != null ? lblRank.Text.Trim() : "") + "</td></tr></table>";
    //                headerCell.CssClass = "GroupHeaderRowStyleGroupHeaderRowStyle";
    //                // Add header Cell to header Row, and header Row to gridTable  
    //                headerRow.Cells.Add(headerCell);
    //                headerRow.HorizontalAlign = HorizontalAlign.Left;
    //                gridTable.Controls.AddAt(rowIndex, headerRow);

    //            }
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("On board", "ONBOARD");
        toolbarmain.AddButton("On Leave", "ONLEAVEORR");
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbarmain.Show();
        MenuOrderForm.SelectedMenuIndex = 1;
        MenuOrderForm.Visible = true;

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Generate Payment Voucher", "PAYMENYVOUCHER", ToolBarDirection.Right);
        MenuPV.AccessRights = this.ViewState;
        MenuPV.MenuList = toolbarsub.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequestForOnLeave.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvAllotmentOnLeave')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageLink("../Accounts/AccountsEmployeeAllotmentRequestForOnLeave.aspx", "Filter", "search.png", "FIND");
        toolbar.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequestForOnLeave.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        MenuAllotment.AccessRights = this.ViewState;
        MenuAllotment.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["MONTH"] = null;
            ViewState["YEAR"] = null;
            ViewState["VESSELID"] = Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : null;
            ddlVessel.SelectedVessel = Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : string.Empty;
            ddlMonth.SelectedMonth = Request.QueryString["MONTH"] != null ? Request.QueryString["MONTH"].ToString() : DateTime.Now.Month.ToString();
            ddlYear.SelectedYear = int.Parse(Request.QueryString["YEAR"] != null ? Request.QueryString["YEAR"].ToString() : DateTime.Now.Year.ToString());
        }
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ONBOARD"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsEmployeeAllotmentRequest.aspx";
            }
            else if (CommandName.ToUpper().Equals("ONLEAVEORR"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsEmployeeAllotmentRequestForOnLeave.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?MONTH = " + ddlMonth.SelectedMonth + " & YEAR = " + ddlYear.SelectedYear + " & VESSELID = " + ((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearFilter()
    {
        //ddlVessel.SelectedVessel = null;
        //ddlMonth.SelectedMonth = "12";
        //ddlYear.SelectedYear = 1;
        Rebind();
    }
    private void AllotmentValidation()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeORRAllotmentNotinOnboard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                             General.GetNullableInteger((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()),
                                                                                             General.GetNullableInteger(ddlMonth.SelectedMonth),
                                                                                             General.GetNullableInteger(ddlYear.SelectedYear.ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows) // Loop over the rows.
            {
                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(row["FLDALLOTMENTID"].ToString()));
            }
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();
            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Date", "Number", "Allotment Type", "Amount (USD)", "Status", "Voucher No.", "Date", "Voucher No.", "Date", "Created By" };

            ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeORRAllotmentNotinOnboard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                              General.GetNullableInteger((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()),
                                                                                              General.GetNullableInteger(ddlMonth.SelectedMonth),
                                                                                              General.GetNullableInteger(ddlYear.SelectedYear.ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                SetAllotmentTypeHard();
            }
            gvAllotmentOnLeave.DataSource = ds;

            General.SetPrintOptions("gvAllotmentOnLeave", "Allotment Request for OnLeave", alCaptions1, alColumns1, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SetAllotmentTypeHard()
    {
        strMAL = PhoenixCommonRegisters.GetHardCode(1, 239, "MAL");
        strSPA = PhoenixCommonRegisters.GetHardCode(1, 239, "SPA");
        strSOF = PhoenixCommonRegisters.GetHardCode(1, 239, "SOF");
        strORR = PhoenixCommonRegisters.GetHardCode(1, 239, "ORR");
        strSLT = PhoenixCommonRegisters.GetHardCode(1, 239, "SLT");
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                AllotmentValidation();
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
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
            DataSet ds = new DataSet();
            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDVOUCHERNO", "FLDCREATEDDATE"};
            string[] alCaptions1 = { "Date", "Number", "Allotment Type", "Amount (USD)", "Status", "Voucher No.", "Date", "Voucher No.", "Date", "Created By"};

            ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeORRAllotmentNotinOnboard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                             General.GetNullableInteger((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()),
                                                                                             General.GetNullableInteger(ddlMonth.SelectedMonth),
                                                                                             General.GetNullableInteger(ddlYear.SelectedYear.ToString()));
            Response.AddHeader("Content-Disposition", "attachment; filename= AllotmentRequest.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns1.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment Request </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
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
                    Response.Write("<td>");
                    Response.Write(dr[alColumns1[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
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
    protected void MenuPV_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("PAYMENYVOUCHER"))
            {
                string allotmentidlist = ",";
                SelectedAllotment(ref allotmentidlist);
                if (allotmentidlist == ",")
                {
                    ucError.ErrorMessage = "please select aleast one allotment";
                    ucError.Visible = true;

                }
                else if (ddlVessel.SelectedVessel == "Dummy")
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixAccountsAllotmentRequest.AllotmentPaymentVoucherGenerate(allotmentidlist
                        , null
                        , General.GetNullableInteger(ddlVessel.SelectedVessel));
                    ucStatus.Visible = true;
                    ucStatus.Text = "Payment Voucher generated";
                }
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SelectedAllotment(ref string allotmentidlist)
    {
        if (gvAllotmentOnLeave.Items.Count > 0)
        {
            foreach (GridDataItem row in gvAllotmentOnLeave.Items)
            {
                RadLabel lblAllotmentId = (RadLabel)row.FindControl("lblAllotmentId");

                RadCheckBox cb = (RadCheckBox)row.FindControl("chkItem");

                if (cb.Checked == true)
                {
                    allotmentidlist += lblAllotmentId.Text + ",";
                }
            }
        }
    }
    private string getCheckboxListValues(RadCheckBoxList cbl)
    {
        string str = "";
        foreach (ListItem li in cbl.Items)
        {
            if (li.Selected == true)
                str = str + ',' + li.Value.ToString();
        }
        return str;
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvAllotmentOnLeave_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CANCELREQUEST"))
            {
                string allotmentid = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text;
                string allotmenttype = ((RadLabel)e.Item.FindControl("lblAllotmentTypeId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselID")).Text;
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;

                PhoenixAccountsAllotmentRequest.AllotmentRequestCancel(new Guid(allotmentid)
                    , int.Parse(vesselid)
                    , int.Parse(employeeid)
                    , int.Parse(allotmenttype));
                Rebind();
                ucStatus.Visible = true;
                ucStatus.Text = "Allotment Request Cancelled";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentOnLeave_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                string allotmenttype = drv["FLDALLOTMENTTYPE"].ToString();
                ImageButton vrf = (ImageButton)e.Item.FindControl("cmdVerification");
                ImageButton cmdtooltip = (ImageButton)e.Item.FindControl("cmdtooltip");
                ImageButton hlb = (ImageButton)e.Item.FindControl("cmdHistory");
                if (cmdtooltip != null)
                {
                    string warningtext = string.Empty;
                    if (drv["FLDPOSITIVEPBCHECK"].ToString() == "0")
                    {
                        warningtext = "Positive PB Balance Check is pending";
                    }
                    if (drv["FLDBANKCHECK"].ToString() == "0")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Banking Details Verification Check is pending";
                    }
                    if (drv["FLDREIMRECCHECK"].ToString() == "0")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Pending Reimbursement/Recoveries";
                    }
                    if (drv["FLDSIDELETTERCHECK"].ToString() == "0")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Entitled for side letter allotment for this month";
                    }
                    if (drv["FLDCREWDEPTAPPROVEDBY"].ToString() == "")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Crew Department approval is missing.";
                    }
                    if (drv["FLDBANKCURRENCYID"].ToString() == "")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Bank Currency is missing.";
                    }
                    if (warningtext != string.Empty)
                    {
                        cmdtooltip.ToolTip = warningtext;
                        cmdtooltip.Visible = true;
                    }
                }
                ImageButton cbtn = (ImageButton)e.Item.FindControl("cmdCancelRequest");
                RadLabel status = (RadLabel)e.Item.FindControl("lblRequestStatus");

                if (allotmenttype == strORR || allotmenttype == strSLT)
                {
                    if (status != null)
                        if (status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "ACC") || status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "CBA"))
                        {
                            if (cbtn != null)
                            {
                                cbtn.Visible = true;
                                cbtn.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the request?')");
                            }
                        }
                }
                if (vrf != null)
                {
                    vrf.Attributes.Add("onclick", "javascript:Openpopup('Accounts','','../Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + " &VESSELID=" + drv["FLDVESSELID"].ToString() + "&VESSELNAME=" + drv["FLDVESSELNAME"].ToString() + "&MONTH=" + drv["FLDMONTH"].ToString() + "&YEAR=" + drv["FLDYEAR"].ToString() + "'); return true;");
                }
                if (hlb != null)
                {
                    hlb.Attributes.Add("onclick", "javascript:Openpopup('Accounts','','../Accounts/AccountsAllotmentHistory.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + " &VESSELID=" + drv["FLDVESSELID"].ToString() + "&VESSELNAME=" + drv["FLDVESSELNAME"].ToString() + "&MONTH=" + drv["FLDMONTH"].ToString() + "&YEAR=" + drv["FLDYEAR"].ToString() + "&SIGNINOFFID='); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentOnLeave_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotmentOnLeave.CurrentPageIndex + 1;
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
        gvAllotmentOnLeave.SelectedIndexes.Clear();
        gvAllotmentOnLeave.EditIndexes.Clear();
        gvAllotmentOnLeave.DataSource = null;
        gvAllotmentOnLeave.Rebind();
    }
}
