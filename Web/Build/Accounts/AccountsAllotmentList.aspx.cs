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
using System.Web.Profile;
using Telerik.Web.UI;
using System.Collections;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class Accounts_AccountsAllotmentList : PhoenixBasePage
{
    decimal totalamount = decimal.Parse("0.0");
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            // toolbarmain.AddImageLink("javascript:openNewWindow( 'codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsApproveandGeneratePaymentVoucher.aspx',false,600,400); return false;", "Generate PV", "", "APPROVEANDCREATEDPV", ToolBarDirection.Right);
            toolbarmain.AddButton("PV Direct Remittance to Crew", "PVDIRECTREMITTANCETOCREW", ToolBarDirection.Right);
            toolbarmain.AddButton("PV Remitting Agent", "PVREMITTINGAGENT", ToolBarDirection.Right);
            toolbarmain.AddButton("Approve Selected Requests", "APPROVESELECTEDREQUESTS", ToolBarDirection.Right);
            toolbarmain.AddImageLink("javascript:openNewWindow( 'codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsCreateDefaultAllotment.aspx',false,600,400); return false;", "Create Default Allotment", "", "CREATEDEFAULTALLOTMENT", ToolBarDirection.Right);


            MenuAllotmentList.AccessRights = this.ViewState;
            MenuAllotmentList.MenuList = toolbarmain.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAllotment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            // toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentListFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRequestList.aspx?','small');return true;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbar.Show();


            if (!IsPostBack)
            {

                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");
                if (Session["CHECKED_CURRENCY"] != null)
                    Session.Remove("CHECKED_CURRENCY");
                if (Session["CHECKED_CURRENCYCODE"] != null)
                    Session.Remove("CHECKED_CURRENCYCODE");
                if (ViewState["TOTAL_AMOUNT"] != null)
                    ViewState.Remove("TOTAL_AMOUNT");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TOTAL_AMOUNT"] = "0.0";
                gvAllotment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvAllotment.Rebind();

    }


    protected void ddlvessel_textchangedevent(object sender, System.EventArgs e)
    {
        gvAllotment.Rebind();
    }

    protected void MenuAllotmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            ViewState["AMOUNT"] = txtTotalSelectedAmount.Text;
            if (CommandName.ToUpper().Equals("PVREMITTINGAGENT"))
            {
                ArrayList SelectedPvs = new ArrayList();
                ArrayList SelectedCurrency = new ArrayList();
                ArrayList SelectedCurrencyCode = new ArrayList();

                string selectedOthers = ",";
                string selectedCCode = "";
                string selectedCur = "";


                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    SelectedCurrency = (ArrayList)Session["CHECKED_CURRENCY"];
                    SelectedCurrencyCode = (ArrayList)Session["CHECKED_CURRENCYCODE"];

                    ViewState["COUNT"] = SelectedPvs.Count;

                    if (SelectedCurrency.Count > 1)
                    {
                        ucError.ErrorMessage = "Selected Allotment should be of same currency for Generating PV to remitting agent.";
                        ucError.Visible = true;
                        return;
                    }
                    else if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                            selectedOthers = selectedOthers + index + ",";

                        foreach (string index in SelectedCurrency)
                            selectedCur = index;

                        foreach (string index in SelectedCurrencyCode)
                            selectedCCode = index;
                    }
                    Response.Redirect("../Accounts/AccountsApproveandGeneratePaymentVoucher.aspx?AllotmentIdList=" + selectedOthers + "&AMOUNT=" + ViewState["AMOUNT"].ToString() + "&COUNT=" + ViewState["COUNT"].ToString() + "&CCODE=" + selectedCCode + "&CURRENCYID=" + selectedCur, false);
                }


                if (Session["CHECKED_ITEMS"] == null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

                    if (SelectedPvs == null)
                    {

                        ucError.ErrorMessage = "Please select atleast one Allotment";
                        ucError.Visible = true;
                    }
                    else
                    {
                        Response.Redirect("../Accounts/AccountsApproveandGeneratePaymentVoucher.aspx?AllotmentIdList=" + selectedOthers, false);

                    }
                }

                //    Response.Redirect("../Accounts/AccountsApproveandGeneratePaymentVoucher.aspx?AllotmentIdList=" + selectedOthers, false);
                // String scriptpopup = String.Format("javascript:parent.openNewWindow('Approve And Generate PV','','" + Session["sitepath"] + "/Accounts/AccountsApproveandGeneratePaymentVoucher.aspx?AllotmentIdList=" + selectedOthers + "');");
                // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                //
            }
            if (CommandName.ToUpper().Equals("PVDIRECTREMITTANCETOCREW"))
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
                    //   Response.Redirect("../Accounts/AccountsPVDirectRemittancetoCrew.aspx?AllotmentIdList=" + selectedOthers, false);
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('Approve And Generate PV','','" + Session["sitepath"] + "/Accounts/AccountsPVDirectRemittancetoCrew.aspx?AllotmentIdList=" + selectedOthers + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }


                if (Session["CHECKED_ITEMS"] == null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs == null)
                    {

                        ucError.ErrorMessage = "Please select atleast one Allotment";
                        ucError.Visible = true;
                    }
                    else
                    {
                        Response.Redirect("../Accounts/AccountsPVDirectRemittancetoCrew.aspx?AllotmentIdList=" + selectedOthers, false);
                    }
                }
            }
            if (CommandName.ToUpper().Equals("APPROVESELECTEDREQUESTS"))
            {
                ArrayList SelectedPvs = new ArrayList();
                if (Session["CHECKED_ITEMS"] == null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs == null)
                    {

                        ucError.ErrorMessage = "Please select atleast one Allotment";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    RadWindowManager1.RadConfirm("Are you sure you want to Approve selected Request?", "DeleteRecord", 320, 150, null, "Approve");
                    return;
                }


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
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
                PhoenixAccountsAllotment.ApproveSelectedRequests(PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedOthers.Length > 1 ? selectedOthers : null);
                //Rebind();

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
            gvAllotment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                gvAllotment.CurrentPageIndex = 0;
                gvAllotment.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                Filter.CurrentAccountsAllotmentRequestFilter = null;

                ViewState["PAGENUMBER"] = 1;
                gvAllotment.Rebind();
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

            string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBANK", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPVVOUCHERNO" };
            string[] alCaptions = { "Vessel", "File No.", "Name", "Rank", "Bank Account", "Currency", "Amount", "Status", "PMV No." };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentAccountsAllotmentRequestFilter;

            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("ddlVessel", string.Empty);
                    nvc.Add("ddlMonth", string.Empty);
                    nvc.Add("ddlyear", string.Empty);
                    nvc.Add("ucZone", string.Empty);
                    nvc.Add("ucTechFleet", string.Empty);
                    nvc.Add("ucOwner", string.Empty);
                    nvc.Add("ddlCurrencyCode", string.Empty);
                    nvc.Add("ucPool", string.Empty);
                    nvc.Add("ucNationality", string.Empty);
                    nvc.Add("ddlAllotmentType", string.Empty);
                    nvc.Add("ucRequestStatus", ViewState["REQUESTSTATUS"].ToString());
                    nvc.Add("ddlCountry", string.Empty);
                }

            }

            DataSet ds = PhoenixAccountsAllotment.AccountsAllotmentSearch(General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlMonth") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlyear") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucTechFleet") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucOwner") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCurrencyCode") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucPool") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucNationality") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlAllotmentType") : string.Empty)
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ucRequestStatus") : ViewState["REQUESTSTATUS"].ToString())
                                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
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


    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
                //Rebind();
            }

            else if (e.CommandName == "APPROVE")
            {
                //string allotmentid = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text.Trim();
                ViewState["ALLOTMENTID"] = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Approve?", "DeleteRecords", 320, 150, null, "Approve");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDeleteRecord_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsAllotment.AllotmentApproveRequests(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ALLOTMENTID"].ToString()));
            ucStatus.Text = "Approved";
            gvAllotment.Rebind();
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



    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBANK", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPVVOUCHERNO" };
        string[] alCaptions = { "Vessel", "File No.", "Name", "Rank", "Bank Account", "Currency", "Amount", "Status", "PMV No." };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            NameValueCollection nvc = Filter.CurrentAccountsAllotmentRequestFilter;
            ViewState["REQUESTSTATUS"] = PhoenixCommonRegisters.GetHardCode(1, 238, "PDA");
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("ddlVessel", string.Empty);
                    nvc.Add("ddlMonth", string.Empty);
                    nvc.Add("ddlyear", string.Empty);
                    nvc.Add("ucZone", string.Empty);
                    nvc.Add("ucTechFleet", string.Empty);
                    nvc.Add("ucOwner", string.Empty);
                    nvc.Add("ddlCurrencyCode", string.Empty);
                    nvc.Add("ucPool", string.Empty);
                    nvc.Add("ucNationality", string.Empty);
                    nvc.Add("ddlAllotmentType", string.Empty);
                    nvc.Add("ucRequestStatus", ViewState["REQUESTSTATUS"].ToString());
                    nvc.Add("ddlCountry", string.Empty);

                }

            }

            DataSet ds = PhoenixAccountsAllotment.AccountsAllotmentSearch(General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlMonth") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlyear") : string.Empty)
                                                                     , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ucTechFleet") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ucOwner") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCurrencyCode") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ucPool") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ucNationality") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlAllotmentType") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ucRequestStatus") : ViewState["REQUESTSTATUS"].ToString())
                                                                     , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)

                                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvAllotment.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);


            General.SetPrintOptions("gvAllotment", "Allotment", alCaptions, alColumns, ds);
            gvAllotment.DataSource = ds;
            gvAllotment.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

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
        //gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }


    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvAllotment$ctl00$ctl02$ctl01$chkAllAllotment")
        {
            ArrayList SelectedPvs = new ArrayList();
            ArrayList SelectedCurrency = new ArrayList();
            ArrayList SelectedCurrencyCode = new ArrayList();

            string amount = "";
            string currency = "";
            string currencycode = "";

            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvAllotment.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllAllotment"); // Get the header checkbox
            }
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

            if (Session["CHECKED_CURRENCY"] != null)
                SelectedCurrency = (ArrayList)Session["CHECKED_CURRENCY"];

            if (Session["CHECKED_CURRENCYCODE"] != null)
                SelectedCurrencyCode = (ArrayList)Session["CHECKED_CURRENCYCODE"];

            foreach (GridItem gvrow in gvAllotment.Items)
            {
                Guid index = new Guid(((RadLabel)gvrow.FindControl("lblAllotmentId")).Text);
                amount = ((RadLabel)gvrow.FindControl("lblAmount")).Text;
                currency = ((RadLabel)gvrow.FindControl("lblCurrencyId")).Text;
                currencycode = ((RadLabel)gvrow.FindControl("lblCurrency")).Text;
                totalamount = Decimal.Parse(ViewState["TOTAL_AMOUNT"] == null ? "0.0" : ViewState["TOTAL_AMOUNT"].ToString());
                RadCheckBox cbSelected = (RadCheckBox)gvrow.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                        if (!SelectedPvs.Contains(index))
                        {
                            SelectedPvs.Add(index);
                            totalamount = totalamount + decimal.Parse(amount);
                            ViewState["TOTAL_AMOUNT"] = totalamount.ToString();

                            if (!SelectedCurrency.Contains(currency))
                            {
                                SelectedCurrency.Add(currency);
                            }

                            if (!SelectedCurrencyCode.Contains(currencycode))
                            {
                                SelectedCurrencyCode.Add(currencycode);
                            }
                        }

                    }
                    else
                    {
                        cbSelected.Checked = false;
                        if (SelectedPvs.Contains(index))
                        {
                            SelectedPvs.Remove(index);
                            totalamount = totalamount + decimal.Parse(amount);
                            ViewState["TOTAL_AMOUNT"] = totalamount.ToString();

                            //if (SelectedCurrency.Contains(currency))
                            //{
                            //    SelectedCurrency.Remove(currency);
                            //}
                            //if (SelectedCurrencyCode.Contains(currencycode))
                            //{
                            //    SelectedCurrencyCode.Remove(currencycode);
                            //}
                        }
                    }
                }
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;
            if (SelectedCurrency != null && SelectedCurrency.Count > 0)
                Session["CHECKED_CURRENCY"] = SelectedCurrency;
            if (SelectedCurrencyCode != null && SelectedCurrencyCode.Count > 0)
                Session["CHECKED_CURRENCYCODE"] = SelectedCurrencyCode;

            txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));

        }
    }

    protected void gvAllotment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {


            ViewState["STATUS"] = ((RadLabel)e.Item.FindControl("lblRequestStatus")).Text;

            //ViewState["APPROVED"] = PhoenixCommonRegisters.GetHardCode(1, 238, "APD");
            LinkButton cmdverification = (LinkButton)e.Item.FindControl("cmdverification");
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            if (cmdApprove != null)
            {
                cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdverification.CommandName);
                if (ViewState["STATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 238, "PDA").ToString())
                {
                    cmdApprove.Visible = true;
                }
                else
                {
                    cmdApprove.Visible = false;
                }

            }


            if (cmdverification != null)
            {
                cmdverification.Visible = SessionUtil.CanAccess(this.ViewState, cmdverification.CommandName);


                if (ViewState["STATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 238, "APD").ToString() || ViewState["STATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 238, "PRA").ToString())
                {
                    cmdverification.Visible = true;

                    ViewState["ALLOTMENTID"] = ((RadLabel)e.Item.FindControl("lblAllotmentId")).Text;
                    ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                    ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                    ViewState["VESSELNAME"] = ((RadLabel)e.Item.FindControl("lblVesselName")).Text;
                    ViewState["MONTH"] = ((RadLabel)e.Item.FindControl("lblMonth")).Text;
                    ViewState["YEAR"] = ((RadLabel)e.Item.FindControl("lblYear")).Text;

                    cmdverification.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + ViewState["ALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "&NEWPROCESS=1" + "'); return true;");

                }

                else
                {
                    cmdverification.Visible = false;
                }

            }


            //LinkButton vrf = (LinkButton)e.Item.FindControl("cmdVerification");

            //if (vrf != null)
            //{
            //    //vrf.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() + "'); return true;");
            //    vrf.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsEmployeeAllotmentRequestVerification.aspx?allotmentid=" + ViewState["ALLOTMENTID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + " &VESSELID=" + ViewState["VESSELID"].ToString() + "&VESSELNAME=" + ViewState["VESSELNAME"].ToString() + "&MONTH=" + ViewState["MONTH"].ToString() + "&YEAR=" + ViewState["YEAR"].ToString() +"&NEWPROCESS=1" +"'); return true;");
            //}


        }

    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedPvs = new ArrayList();
            ArrayList SelectedCurrency = new ArrayList();
            ArrayList SelectedCurrencyCode = new ArrayList();

            Guid index = new Guid();
            string amount = "";
            string currency = "";
            string currencycode = "";


            foreach (GridItem gvrow in gvAllotment.Items)
            {
                bool result = false;
                index = new Guid(((RadLabel)gvrow.FindControl("lblAllotmentId")).Text);
                amount = ((RadLabel)gvrow.FindControl("lblAmount")).Text;
                currency = ((RadLabel)gvrow.FindControl("lblCurrencyId")).Text;
                currencycode = ((RadLabel)gvrow.FindControl("lblCurrency")).Text;

                totalamount = Decimal.Parse(ViewState["TOTAL_AMOUNT"] == null ? "0.0" : ViewState["TOTAL_AMOUNT"].ToString());
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

                if (Session["CHECKED_CURRENCY"] != null)
                    SelectedCurrency = (ArrayList)Session["CHECKED_CURRENCY"];

                if (Session["CHECKED_CURRENCYCODE"] != null)
                    SelectedCurrencyCode = (ArrayList)Session["CHECKED_CURRENCYCODE"];

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                    if (!SelectedPvs.Contains(index))
                    {
                        totalamount = totalamount + decimal.Parse(amount);
                        ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                    }
                }
                else
                {
                    if (SelectedPvs.Contains(index))
                    {
                        totalamount = totalamount - decimal.Parse(amount);
                        ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                    }
                }

                if (result)
                {
                    if (!SelectedPvs.Contains(index))
                        SelectedPvs.Add(index);
                    if (!SelectedCurrency.Contains(currency))
                    {
                        SelectedCurrency.Add(currency);
                    }
                    if (!SelectedCurrencyCode.Contains(currencycode))
                    {
                        SelectedCurrencyCode.Add(currencycode);
                    }
                }
                else
                {
                    SelectedPvs.Remove(index);
                    //SelectedCurrency.Remove(currency);
                    //SelectedCurrencyCode.Remove(currencycode);

                }
                txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;
            if (SelectedCurrency != null && SelectedCurrency.Count > 0)
                Session["CHECKED_CURRENCY"] = SelectedCurrency;

            if (SelectedCurrencyCode != null && SelectedCurrencyCode.Count > 0)
                Session["CHECKED_CURRENCYCODE"] = SelectedCurrencyCode;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void GetSelectedPvs()
    {
        if (ViewState["TOTAL_AMOUNT"] != null)
        {
            txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));
        }
        else
        {
            txtTotalSelectedAmount.Text = "0.0";
        }

    }
}