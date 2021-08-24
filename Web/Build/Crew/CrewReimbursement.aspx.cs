using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReimbursement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReimbursement.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Crew/CrewREimbursementFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Crew/CrewReimbursementDetail.aspx');return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReimbursement.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuReimbursement.AccessRights = this.ViewState;
            MenuReimbursement.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvRem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReimbursement_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDREFERENCENO", "FLDEMPLOYEENAME", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAPPROVEDAMOUNT", "FLDACCOUNTOF", "FLDACIVEYN" };
                string[] alCaptions = { "Reference No", "Employee Name", "Reimbursement/Recovery", "Purpose", "Currency", "Amount", "Approved Amount", "Account of", "Active" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string menucode = Filter.CurrentMenuCodeSelection;
                string vesselid = string.Empty;

                NameValueCollection nvc = Filter.CrewReimbursementFilterSelection;
                if (menucode != "CRW-OPR-REM" && menucode!= "CRW3-PAY-REM")
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

                DataTable dt = PhoenixCrewReimbursement.SearchCrewReimbursement(General.GetNullableInteger(vesselid)
                                                                            , null, (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                            , (byte?)General.GetNullableInteger(nvc != null ? nvc["ddlApproved"] : string.Empty)
                                                                            , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                            , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlVesselAccountof"] : string.Empty)
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlChargeableVessel"] : string.Empty)
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDateFrom")) : null
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDateTo")) : null
                                                                            , sortexpression, sortdirection
                                                                            , 1, iRowCount
                                                                            , ref iRowCount, ref iTotalPageCount
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlPaymentMode"] : string.Empty));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Reimbursements/Recoveries", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewReimbursementFilterSelection = null;
                ViewState["PAGENUMBER"] = 1;
                gvRem.CurrentPageIndex = 0;
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

        string[] alColumns = { "FLDREFERENCENO", "FLDEMPLOYEENAME", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAPPROVEDAMOUNT", "FLDACCOUNTOF", "FLDACIVEYN" };
        string[] alCaptions = { "Reference No", "Employee Name", "Reimbursement/Recovery", "Purpose", "Currency", "Amount", "Approved Amount", "Account of", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string menucode = Filter.CurrentMenuCodeSelection;

        string vesselid = string.Empty;
        NameValueCollection nvc = Filter.CrewReimbursementFilterSelection;

        if (menucode != "CRW-OPR-REM" && menucode!= "CRW3-PAY-REM")
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
        DataTable dt = PhoenixCrewReimbursement.SearchCrewReimbursement(General.GetNullableInteger(vesselid)
                                                                             , null, (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                             , (byte?)General.GetNullableInteger(nvc != null ? nvc["ddlApproved"] : string.Empty)
                                                                             , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                             , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                                             , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                             , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlVesselAccountof"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlChargeableVessel"] : string.Empty)
                                                                             , nvc != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDateFrom")) : null
                                                                             , nvc != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDateTo")) : null
                                                                             , sortexpression, sortdirection
                                                                             , (int)ViewState["PAGENUMBER"], gvRem.PageSize
                                                                             , ref iRowCount, ref iTotalPageCount
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlPaymentMode"] : string.Empty));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvRem", "Reimbursements/Recoveries", alCaptions, alColumns, ds);
        gvRem.DataSource = dt;
        gvRem.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvRem.SelectedIndexes.Clear();
        gvRem.EditIndexes.Clear();
        gvRem.DataSource = null;
        gvRem.Rebind();
    }
    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblReimbursmentid")).Text.Trim();
                PhoenixCrewReimbursement.ApproveCrewReimbursement(new Guid(id));
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("CLOSE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblReimbursmentid")).Text.Trim();
                PhoenixCrewReimbursement.CloseCrewReimbursement(new Guid(id));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblReimbursmentid")).Text.Trim();
                PhoenixCrewReimbursement.DeleteCrewReimbursement(new Guid(id));
                Rebind();
            }
            else if (e.CommandName == "Page")
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
    protected void gvRem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdattachedimg");
            if (db != null)
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdClose");
            if (cmdClose != null)
            {
                cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
                cmdClose.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to make this entry in-active? '); return false;");
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null && drv["FLDAPPROVEDYN"].ToString() == "1") ed.Visible = false;
            else if (ed != null && drv["FLDAPPROVEDYN"].ToString() == "0")
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                ed.Attributes.Add("onclick", "javascript:openNewWindow('payment', '', '" + Session["sitepath"] + "/Crew/CrewReimbursementDetail.aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "');return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes["onclick"] = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.CREW + "'); return false;";
            }

            LinkButton pa = (LinkButton)e.Item.FindControl("cmdPayment");
            if (pa != null)
            {
                pa.Visible = SessionUtil.CanAccess(this.ViewState, pa.CommandName);
                pa.Attributes.Add("onclick", "parent.openNewWindow('payment', '', '" + Session["sitepath"] + "/Crew/" + (",1,-1,3,-3,".Contains("," + drv["FLDEARNINGDEDUCTION"].ToString() + ",") ? "CrewReimbursementPayment" : "CrewReimbursementDeduction") + ".aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "');return false;");
            }
            LinkButton app = (LinkButton)e.Item.FindControl("cmdApprove");

            if (app != null && (Filter.CurrentMenuCodeSelection != "CRW-OPR-REM"&&Filter.CurrentMenuCodeSelection != "CRW3-PAY-REM")) { app.Visible = false; pa.Visible = false; }
            if (app != null)
                app.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to approve ?'); return false;");
            if (app != null && drv["FLDAPPROVEDYN"].ToString() == "0")
            {
                app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);
                pa.Visible = false;
            }
            else if (app != null && drv["FLDAPPROVEDYN"].ToString() == "1")
            {
                app.Visible = false;
                pa.Visible = SessionUtil.CanAccess(this.ViewState, pa.CommandName);
            }


            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblApprovedAmount");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            //     e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRem, "Select$" + e.Item.RowIndex.ToString(), false);
            LinkButton refno = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (refno != null)
            {
                if (drv["FLDAPPROVEDYN"].ToString() == "1")
                    refno.Attributes.Add("onclick", "openNewWindow('Details', '', '" + Session["sitepath"] + "/Crew/CrewReimbursementDetail.aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "&readonly=true');return false;");
                else
                    refno.Attributes.Add("onclick", "openNewWindow('Details', '', '" + Session["sitepath"] + "/Crew/CrewReimbursementDetail.aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "');return false;");
            }

            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (ttip != null)
            {
                ttip.Screen = "Crew/CrewToolTipReimbursement.aspx?reimid=" + drv["FLDREIMBURSEMENTID"].ToString();
            }
        }
    }
    protected void gvRem_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement(Monthly / OneTime)";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery(Monthly / OneTime)";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
    }
}
