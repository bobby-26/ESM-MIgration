using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceRejectionGeneratePMV : PhoenixBasePage
{
    string strMAL, strSPA, strSOF, strORR, strSLT;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
       
        toolbar.AddButton("Allotment", "ALLOTMENT",ToolBarDirection.Right);
        toolbar.AddButton("List", "REQUEST", ToolBarDirection.Right);
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 0;
        MenuOrderForm.Visible = true;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Generate Payment Voucher", "PAYMENYVOUCHER",ToolBarDirection.Right);
        MenuPV.AccessRights = this.ViewState;
        MenuPV.MenuList = toolbarsub.Show();

        if (!IsPostBack)
        {
            ViewState["ID"] = null;
            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["MONTH"] = null;
            ViewState["YEAR"] = null;
            ViewState["VESSELNAME"] = null;
            ViewState["REMITTANCEID"] = null;
            ViewState["SIGNONOFFID"] = null;
            ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
            ViewState["VESSELNAME"] = Request.QueryString["VESSELNAME"].ToString();
            AllotmentDetails();
            AllotmentValidation();

            txtVesselName.Text = ViewState["VESSELNAME"].ToString();
            txtMonthAndYear.Text = DateTime.Parse("01/" + ViewState["MONTH"].ToString() + "/" + ViewState["YEAR"].ToString()).ToString("MMM") + "-" + ViewState["YEAR"].ToString();
            EditEmployeedetails(int.Parse(ViewState["EMPLOYEEID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));

        }
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRemittanceRejectionGeneratePMV.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNONOFFID=" + ViewState["SIGNONOFFID"].ToString() + "", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvAllotment')", "Print Grid", "icon_print.png", "PRINT");
      
        //  toolbargrid.AddImageLink("javascript:openNewWindow('filter'," + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestSideLetter.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNONOFFID=" + ViewState["SIGNONOFFID"].ToString() + "');return false;", "SideLetter Request", "covering-letter.png", "SIDELETTER");
          toolbargrid.AddImageLink("javascript:Openpopup('filter','','AccountsEmployeeAllotmentRequestSideLetter.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNONOFFID=" + ViewState["SIGNONOFFID"].ToString() + "');return false;", "SideLetter Request", "covering-letter.png", "SIDELETTER");
       

        MenuAllotment.AccessRights = this.ViewState;
        MenuAllotment.MenuList = toolbargrid.Show();
        BindData();

    }
    private void AllotmentValidation()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                             General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                             General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                             General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                             General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                             null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows) // Loop over the rows.
            {
                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(row["FLDALLOTMENTID"].ToString()));
            }
        }
    }

    private void AllotmentDetails()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsAllotmentRemittanceRejection.AllotmenDetailSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                              ViewState["REMITTANCEID"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["SIGNONOFFID"] = dr["FLDSIGNONOFFID"].ToString();
            ViewState["MONTH"] = dr["FLDMONTH"].ToString();
            ViewState["YEAR"] = dr["FLDYEAR"].ToString();
        }
    }
    private void EditEmployeedetails(int EmployeeId, int VesselId)
    {
        try
        {
            DataTable dt = PhoenixAccountsEmployeeAllotmentRequest.EditAllotmentEmployeeDetails(EmployeeId, VesselId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANK"].ToString();
            }
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
            DataSet ds = new DataSet();
            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTDATE", "FLDPAYMENTVOUCHERNUMBER", "FLDJVVOUCHERNO", "FLDVOUCHERNO", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Allotment Type", "Amount(USD)", "Request Staus", "Payment Date", "Payment Voucher", "Charging Voucher", "Bank Payment Voucher", "Created By", "Created On" };
            ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                          General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                                         General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                                         General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                                         null);
          
                SetAllotmentTypeHard();
                gvAllotment.DataSource = ds;
                gvAllotment.DataBind();
                General.SetPrintOptions("gvAllotment", "Allotment Request", alCaptions1, alColumns1, ds);
         
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTDATE", "FLDPAYMENTVOUCHERNUMBER", "FLDJVVOUCHERNO", "FLDVOUCHERNO", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Allotment Type", "Amount(USD)", "Request Staus", "Payment Date", "Payment Voucher", "Charging Voucher", "Bank Payment Voucher", "Created By", "Created On" };
            ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                 General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                                 null);

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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PAYMENYVOUCHER"))
            {
                string allotmentidlist = ",";
                SelectedAllotment(ref allotmentidlist);
                if (allotmentidlist == ",")
                {
                    ucError.ErrorMessage = "please select aleast one allotment";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixAccountsAllotmentRequest.AllotmentPaymentVoucherGenerate(allotmentidlist
                        , null
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
                    ucStatus.Visible = true;
                    ucStatus.Text = "Payment Voucher generated";
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SelectedAllotment(ref string allotmentidlist)
    {
        if (gvAllotment.MasterTableView.Items.Count > 0)
        {
            foreach (GridDataItem row in gvAllotment.MasterTableView.Items)
                
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
    private string getCheckboxListValues(CheckBoxList cbl)
    {
        string str = "";
        foreach (ListItem li in cbl.Items)
        {
            if (li.Selected == true)
                str = str + ',' + li.Value.ToString();
        }
        return str;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentRemittanceRejection.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
    try
        {
            
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            //if (e.CommandName.ToUpper().Equals("UNLOCK"))
            //{
            //    string allotmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId")).Text;
            //    string allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentTypeId")).Text;
            //    string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID")).Text;
            //    string month = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMonth")).Text;
            //    string year = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYear")).Text;
            //    PhoenixAccountsAllotmentRequest.AllotmentRequestUnlock(new Guid(allotmentid));
            //    BindData();
            //    ucStatus.Visible = true;
            //    ucStatus.Text = "Allotment Request Unlocked";
            //}
            if (e.CommandName.ToUpper().Equals("CANCELREQUEST"))
            {
                string allotmentid = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text;
                string allotmenttype = ((RadLabel)e.Item.FindControl("lblAllotmentTypeId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselID")).Text;
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;

                PhoenixAccountsAllotmentRemittanceRejection.AllotmentRejectionRequestCancel(new Guid(allotmentid)
                    , int.Parse(allotmenttype));
                BindData();
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

    protected void gvAllotment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                string allotmenttype = drv["FLDALLOTMENTTYPE"].ToString();

                ImageButton ulb = (ImageButton)e.Item.FindControl("cmdUnlock");
                ImageButton reb = (ImageButton)e.Item.FindControl("cmdReimRec");
                ImageButton slb = (ImageButton)e.Item.FindControl("cmdSideLetter");
                ImageButton vrf = (ImageButton)e.Item.FindControl("cmdVerification");
                ImageButton hlb = (ImageButton)e.Item.FindControl("cmdHistory");
                ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");

                ImageButton cmdtooltip = (ImageButton)e.Item.FindControl("cmdtooltip");
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
                    if (drv["FLDBANKCURRENCYID"].ToString() == "" || drv["FLDACCOUNTNAME"].ToString() == "" || drv["FLDACCOUNTTYPE"].ToString() == "" || drv["FLDADDRESS1"].ToString() == "" || drv["FLDBANKSWIFTCODE"].ToString() == "" || drv["FLDBANKIFSCCODE"].ToString() == "" || drv["FLDBANKNAME"].ToString() == "" || drv["FLDACCOUNTNUMBER"].ToString() == "")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Bank Details is missing.";
                    }
                    if (drv["FLDACTIVEYN"].ToString() == "0")
                    {
                        warningtext += (warningtext == "" ? "" : " , ") + "Bank Account is InActive";
                    }
                    if (warningtext != string.Empty)
                    {
                        cmdtooltip.ToolTip = warningtext;
                        cmdtooltip.Visible = true;
                    }
                }
                ImageButton cbtn = (ImageButton)e.Item.FindControl("cmdCancelRequest");
                RadLabel status = (RadLabel)e.Item.FindControl("lblRequestStatus");

                if (status != null)
                {
                    if (status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "ACC") || status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "CBA"))
                    {
                        if (cbtn != null)
                        {
                            cbtn.Visible = true;
                            cbtn.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the request?')");
                        }
                        if (edit != null)
                        {
                            if (drv["FLDCANCELYN"].ToString() == "1")
                            {
                                edit.Visible = true;
                                edit.Attributes.Add("onclick", "openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRejectionBankAccountChange.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return false;");
                               
                            }
                        }

                    }
                }
                if (reb != null)
                {
                    reb.Attributes.Add("onclick", "openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRequesPendingReimbRecoveries.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return false;");
                }
                if (vrf != null)
                {
                    vrf.Attributes.Add("onclick", "openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "'); return true;");
                }
                if (hlb != null)
                {
                    hlb.Attributes.Add("onclick", "openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentHistory.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNINOFFID=" + ViewState["SIGNONOFFID"].ToString() + "'); return true;");
                }
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotment.CurrentPageIndex + 1;
        BindData();
    }

}