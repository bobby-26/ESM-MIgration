using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class AccountsEmployeeAllotmentRequestDetails : PhoenixBasePage
{
    string strMAL, strSPA, strORR, strSLT;//, strSOF
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Allotment", "ALLOTMENT", ToolBarDirection.Right);
            toolbar.AddButton("List", "REQUEST", ToolBarDirection.Right);
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbar.Show();
            MenuOrderForm.SelectedMenuIndex = 0;
            MenuOrderForm.Visible = true;
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Generate Payment Voucher", "PAYMENYVOUCHER", ToolBarDirection.Right);
            MenuPV.AccessRights = this.ViewState;
            MenuPV.MenuList = toolbarsub.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["ID"] = null;
                ViewState["EMPLOYEEID"] = null;
                ViewState["VESSELID"] = null;
                ViewState["MONTH"] = null;
                ViewState["YEAR"] = null;
                ViewState["VESSELNAME"] = null;
                ViewState["ALLOTMENTID"] = "";
                ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
                ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
                ViewState["VESSELNAME"] = Request.QueryString["VESSELNAME"].ToString();
                AllotmentValidation();
                txtVesselName.Text = ViewState["VESSELNAME"].ToString();
                txtMonthAndYear.Text = DateTime.Parse("01/" + ViewState["MONTH"].ToString() + "/" + ViewState["YEAR"].ToString()).ToString("MMM") + "-" + ViewState["YEAR"].ToString();
                EditEmployeedetails(int.Parse(ViewState["EMPLOYEEID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequestDetails.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNONOFFID=" + ViewState["SIGNONOFFID"].ToString() + "", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAllotment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('filter','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestSideLetter.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNONOFFID=" + ViewState["SIGNONOFFID"].ToString() + "');return false;", "SideLetter Request", "covering-letter.png", "SIDELETTER");
            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
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

    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();
            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTDATE", "FLDPAYMENTVOUCHERNUMBER", "FLDJVVOUCHERNO", "FLDVOUCHERNO", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Allotment Type", "Amount(USD)", "Request Status", "Payment Date", "Payment Voucher", "Charging Voucher", "Bank Payment Voucher", "Created By", "Created On" };
            ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                          General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                                         General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                                         General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                                         null);

            SetAllotmentTypeHard();
            gvAllotment.DataSource = ds;
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
        //DataSet ds = PhoenixRegistersHard.ListHard(1,239);
        strMAL = PhoenixCommonRegisters.GetHardCode(1, 239, "MAL");
        strSPA = PhoenixCommonRegisters.GetHardCode(1, 239, "SPA");
        //strSOF = PhoenixCommonRegisters.GetHardCode(1, 239, "SOF");
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
            string[] alCaptions1 = { "Request Date", "Request No.", "Allotment Type", "Amount(USD)", "Request Status", "Payment Date", "Payment Voucher", "Charging Voucher", "Bank Payment Voucher", "Created By", "Created On" };
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
            gvAllotment.Rebind();
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
    public void BindPortageBillData()
    {
        try
        {
            Guid id = new Guid();

            ViewState["OFFSIGNER"] = "0";
            gvPB.DataSource = null;


            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeAllotmentRequestPortageBill(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                     int.Parse(ViewState["EMPLOYEEID"].ToString()),
                                                                                                     int.Parse(ViewState["MONTH"].ToString()),
                                                                                                     int.Parse(ViewState["YEAR"].ToString()), General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()), ref id);
            // string[] nonaggcol = { "From", "To", "Days" };

            ViewState["UID"] = id;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                while (gvPB.Columns.Count > 7)
                {
                    gvPB.Columns.RemoveAt(7);
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GridTemplateColumn field = new GridTemplateColumn();
                    field.HeaderText = dt.Rows[i]["FLDGROUPNAME"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    field.ColumnGroupName = dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1" ? "Earnings" : (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" ? "Deductions" : string.Empty);
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    //field.HeaderStyle.Width = Unit.Pixel(field.HeaderText.Length * 5);
                    if (field.HeaderText == "B.F")
                    {
                        field.HeaderStyle.Width = Unit.Pixel(80);
                    }
                    else if (field.HeaderText == "ALLOTMENT")
                    {
                        field.HeaderStyle.Width = Unit.Pixel(85);
                    }
                    else
                    {
                        field.HeaderStyle.Width = Unit.Pixel(field.HeaderText.Length * 7);
                    }
                    gvPB.Columns.Insert(gvPB.Columns.Count, field);
                }

                gvPB.MasterTableView.ColumnGroups.FindGroupByName("Earnings").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvPB.MasterTableView.ColumnGroups.FindGroupByName("Deductions").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;


            }
            gvPB.DataSource = ds;
            gvPB.PageSize = ds.Tables[0].Rows.Count;
            gvPB.VirtualItemCount = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvAllotment.Rebind();
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Accounts/AccountsEmployeeAllotmentRequest.aspx?EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPB_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        RadGrid gv = (RadGrid)sender;
        if (gv.DataSource.GetType().Name != "DataSet") return;
        DataSet ds = (DataSet)gv.DataSource;
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridEditableItem)
        {


            if (drv.Row.Table.Columns.Count > 0)
            {
                string signonoffid = drv["FLDSIGNONOFFID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    if (header.Rows[i]["FLDORDER"].ToString() == "1")
                    {
                        DataRow[] dr = data.Select("FLDSIGNONOFFID = " + signonoffid + " AND FLDGROUPNAME = '" + header.Rows[i]["FLDGROUPNAME"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString());
                        e.Item.Cells[i + 9].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDTOTALEARNINGS"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDTOTALDEDUCTION"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-2")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDBROUGHTFORWARD"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-3")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDFINALBALANCE"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }

                }
            }

        }

    }
    protected void Rebind()
    {
        gvAllotment.SelectedIndexes.Clear();
        gvAllotment.EditIndexes.Clear();
        gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }


    protected void gvPB_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
        BindPortageBillData();
    }

    protected void gvAllotment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UNLOCK"))
            {
                string allotmentid = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text;
                string allotmenttype = ((RadLabel)e.Item.FindControl("lblAllotmentTypeId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselID")).Text;
                string month = ((RadLabel)e.Item.FindControl("lblMonth")).Text;
                string year = ((RadLabel)e.Item.FindControl("lblYear")).Text;
                PhoenixAccountsAllotmentRequest.AllotmentRequestUnlock(new Guid(allotmentid));
                Rebind();
                ucStatus.Visible = true;
                ucStatus.Text = "Allotment Request Unlocked";
            }
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
            //if (e.CommandName.ToUpper() == "SELECT")
            //{
            //    LinkButton lnkRequestNo = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkRequestNo");
            //    Label lblAllotmentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId");

            //    Response.Redirect("AccountsAllotmentRequestDetails.aspx?ALLOTMENTID=" + lblAllotmentId.Text, false);
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                string allotmenttype = drv["FLDALLOTMENTTYPE"].ToString();

                ImageButton ulb = (ImageButton)e.Item.FindControl("cmdUnlock");
                ImageButton reb = (ImageButton)e.Item.FindControl("cmdReimRec");
                ImageButton slb = (ImageButton)e.Item.FindControl("cmdSideLetter");
                ImageButton vrf = (ImageButton)e.Item.FindControl("cmdVerification");
                ImageButton hlb = (ImageButton)e.Item.FindControl("cmdHistory");


                if (allotmenttype == strMAL || allotmenttype == strSPA)
                {
                    if (ulb != null)
                    {
                        string unlockyn = ((RadLabel)e.Item.FindControl("lblUnlockYN")).Text;
                        string cancelyn = ((RadLabel)e.Item.FindControl("lblCancelYN")).Text;


                        if (unlockyn == "1" && cancelyn.Equals("0"))
                        {
                            ulb.Visible = true;
                            ulb.Visible = SessionUtil.CanAccess(this.ViewState, ulb.CommandName);
                            ulb.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to unlock the request?')");
                        }
                        else
                            ulb.Visible = false;
                    }
                }
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
                ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");

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
                            if (edit != null)
                            {
                                if (drv["FLDCANCELYN"].ToString() == "1")
                                {
                                    edit.Visible = true;
                                    edit.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRejectionBankAccountChange.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return false;");
                                }
                            }
                        }
                }
                if (reb != null)
                {
                    reb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRequesPendingReimbRecoveries.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return false;");
                }
                if (vrf != null)
                {
                    vrf.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "'); return true;");
                }
                if (hlb != null)
                {
                    hlb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentHistory.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&SIGNINOFFID=" + ViewState["SIGNONOFFID"].ToString() + "'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotment.CurrentPageIndex + 1;
        BindData();
    }
}