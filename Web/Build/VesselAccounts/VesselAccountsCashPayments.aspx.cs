using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.Profile;
using Telerik.Web.UI;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class VesselAccounts_VesselAccountsCashPayments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Reject Selected Requests", "REJECTSELECTEDREQUESTS", ToolBarDirection.Right);
            toolbarmain.AddButton("Approve Selected Requests", "APPROVESELECTEDREQUESTS", ToolBarDirection.Right);

            MenuCashPayment.AccessRights = this.ViewState;
            MenuCashPayment.MenuList = toolbarmain.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashPayments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCashPayment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashPayments.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuPayment.AccessRights = this.ViewState;
            MenuPayment.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");
                // txtFromDate.Text = DateTime.Now.ToShortDateString();
                // txtToDate.Text = DateTime.Now.ToShortDateString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucRequestStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 41, "AAP");
                gvCashPayment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvCashPayment.Rebind();
    }

    protected void ddlvessel_textchangedevent(object sender, System.EventArgs e)
    {
        gvCashPayment.Rebind();
    }
    protected void MenuCashPayment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("APPROVESELECTEDREQUESTS"))
            {
                ArrayList SelectedPvs = new ArrayList();
                string selectedOthers = ",";

                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                            selectedOthers = selectedOthers + index + ",";
                    }
                }

                if (SelectedPvs.Count > 0)
                {
                    PhoenixVesselAccountsCashAdvanceRequest.ApproveSelectedRequests(PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedOthers.Length > 1 ? selectedOthers : null);
                    gvCashPayment.Rebind();

                    string Scriptnew = "";
                    Scriptnew += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Scriptnew += "fnReloadList('code1', null, null);";
                    Scriptnew += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Scriptnew);
                    ucStatus.Text = "Approved";
                }


                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('code1', null, null);";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else if (CommandName.ToUpper().Equals("REJECTSELECTEDREQUESTS"))
            {
                ArrayList SelectedPvs = new ArrayList();
                string selectedOthers = ",";

                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                            selectedOthers = selectedOthers + index + ",";
                    }
                }
                if (SelectedPvs.Count > 0)
                {
                    PhoenixVesselAccountsCashAdvanceRequest.RejectSelectedRequests(PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedOthers.Length > 1 ? selectedOthers : null);
                    gvCashPayment.Rebind();

                    string Scriptnew = "";
                    Scriptnew += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Scriptnew += "fnReloadList('code1', null, null);";
                    Scriptnew += "</script>" + "\n";

                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Scriptnew);

                    ucStatus.Text = "Rejected";
                }


                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('code1', null, null);";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuPayment_TabStripCommand(object sender, EventArgs e)
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
                //  gvCashPayment.CurrentPageIndex = 0;
                gvCashPayment.Rebind();
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

            string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPURPOSE", "FLDSTATUSNAME", "FLDPAIDFROM", "FLDREJECTREMARKS", "FLDPMVNO" };
            string[] alCaptions = { "Vessel", "Date", "Name", "Rank", "Currency", "Amount", "Purpose", "Status", "Paid From", "Reject Remarks", "PMV No." };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixVesselAccountsCashAdvanceRequest.VesselAccountsCashPaymentSearch(
                                                                              // General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : ddlVessel.SelectedValue
                                                                              General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                              , General.GetNullableInteger(ucRequestStatus.SelectedHard)
                                                                              , General.GetNullableInteger(ucZone.SelectedZone)
                                                                              , General.GetNullableInteger(ucTechFleet.SelectedFleet)
                                                                              , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                              , General.GetNullableInteger(ucPool.SelectedPool)

                                                                              , 1
                                                                              , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("AllotmentList", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            Response.AddHeader("Content-Disposition", "attachment; filename=Allotment_List" + ".xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment List</center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
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
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
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

    protected void gvCashPayment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (chk != null && Session["CHECKED_ITEMS"] != null)
            {
                ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                Guid index = new Guid();
                index = new Guid(drv["FLDLINEITEMID"].ToString());
                if (SelectedPvs.Contains(index))
                {
                    chk.Checked = true;
                }
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (drv["FLDISEDITABLE"].ToString() == "0")
                    eb.Visible = false;
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdCash = (LinkButton)e.Item.FindControl("cmdCashRequest");
            if (cmdCash != null)
            {
                cmdCash.Visible = SessionUtil.CanAccess(this.ViewState, cmdCash.CommandName);
                cmdCash.Attributes.Add("onclick", "javascript:openNewWindow('payment', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCashRequestSupplier.aspx?REQUESTID=" + drv["FLDREQUESTID"] + "');return false;");
                if (drv["FLDPAIDFROM"].ToString() == "0")
                    cmdCash.Visible = true;
                else
                    cmdCash.Visible = false;

            }

            //RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            //if (chkSelect != null)
            //{
            //    if (drv["FLDISEDITABLE"].ToString() == "1")
            //        chkSelect.Visible = true;
            //    else
            //        cmdCash.Visible = false;
            //}

        }
    }

    protected void gvCashPayment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCashPayment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCashPayment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
                //Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string VesselId = ((RadLabel)e.Item.FindControl("lblVesselId")).Text.Trim();
                string EarningDeductionId = ((RadLabel)e.Item.FindControl("lblEarningDeductionId")).Text.Trim();
                string LineItemId = ((RadLabel)e.Item.FindControl("lblLineItemId")).Text.Trim();
                //string PaidFrom = ((RadTextBox)e.Item.FindControl("txtPaidFromEdit")).Text;
                string ddlPaidFromEdit = ((RadComboBox)e.Item.FindControl("ddlPaidFromEdit")).SelectedValue;
                string Remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;
                PhoenixVesselAccountsCashAdvanceRequest.PaidFromStatusUpdate(int.Parse(VesselId),
                                                                            new Guid(EarningDeductionId),
                                                                            new Guid(LineItemId),
                                                                            //int.Parse(PaidFrom),
                                                                            int.Parse(ddlPaidFromEdit.ToString()),
                                                                            General.GetNullableString(Remarks.ToString()));

                ucStatus.Text = "Updated";
                //gvCashPayment.Rebind();
                Rebind();
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

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPURPOSE", "FLDSTATUSNAME", "FLDPAIDFROM", "FLDREJECTREMARKS", "FLDPMVNO" };
        string[] alCaptions = { "Vessel", "Date", "Name", "Rank", "Currency", "Amount", "Purpose", "Status", "Paid From", "Reject Remarks", "PMV No." };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselAccountsCashAdvanceRequest.VesselAccountsCashPaymentSearch(
                                                                              // General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : ddlVessel.SelectedValue
                                                                              General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                               , General.GetNullableInteger(ucRequestStatus.SelectedHard)
                                                                               , General.GetNullableInteger(ucZone.SelectedZone)
                                                                              , General.GetNullableInteger(ucTechFleet.SelectedFleet)
                                                                              , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                              , General.GetNullableInteger(ucPool.SelectedPool)
                                                                              , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                              , gvCashPayment.PageSize
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount);
        General.SetPrintOptions("gvCashPayment", "Cash Payment", alCaptions, alColumns, ds);
        gvCashPayment.DataSource = ds;
        gvCashPayment.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;

    }

    protected void Rebind()
    {
        gvCashPayment.SelectedIndexes.Clear();
        gvCashPayment.EditIndexes.Clear();
        gvCashPayment.DataSource = null;
        gvCashPayment.Rebind();
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvCashPayment$ctl00$ctl02$ctl01$chkAllCashPayment")
        {
            ArrayList SelectedPvs = new ArrayList();

            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvCashPayment.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllCashPayment"); // Get the header checkbox
            }
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            foreach (GridItem gvrow in gvCashPayment.Items)
            {
                Guid index = new Guid(((RadLabel)gvrow.FindControl("lblLineItemId")).Text);

                RadCheckBox cbSelected = (RadCheckBox)gvrow.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (cbSelected.Enabled == true)
                    {
                        if (chkAll.Checked == true)
                        {
                            cbSelected.Checked = true;
                            if (!SelectedPvs.Contains(index))
                            {
                                SelectedPvs.Add(index);

                            }
                        }
                        else
                        {
                            cbSelected.Checked = false;
                            if (SelectedPvs.Contains(index))
                            {
                                SelectedPvs.Remove(index);

                            }
                        }
                    }
                }
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;


        }
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedPvs = new ArrayList();

            Guid index = new Guid();


            foreach (GridItem gvrow in gvCashPayment.Items)
            {
                bool result = false;
                index = new Guid(((RadLabel)gvrow.FindControl("lblLineItemId")).Text);

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                if (result)
                {
                    if (!SelectedPvs.Contains(index))
                        SelectedPvs.Add(index);
                }
                else
                {
                    SelectedPvs.Remove(index);

                }

            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}