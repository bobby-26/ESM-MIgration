using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsRHOPARequirement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Calendar", "WORKCALENDER", ToolBarDirection.Left);
            toolbarmain.AddButton("OPA90", "OPA", ToolBarDirection.Left);
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();
            MenuRHGeneral.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHOPARequirement.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOPA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuOPAList.AccessRights = this.ViewState;
            MenuOPAList.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvOPA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CREW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHConfiguration.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("WORKCALENDER"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("DESIGNATION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHDesignation.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("OPA"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHOPARequirement.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Seafarer and then Navigate to other Tabs";
        ucError.Visible = true;
    }

    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDSTARTDATE", "FLDSTARTREPORTINGHOUR", "FLDFINISHDATE", "FLDENDREPORTINGHOUR" };
        string[] alCaptions = { "Sr.No", "Date of Entry", "Start Reporting Hour", "Date of Exit", "End Reporting Hour" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixVesselAccountsRH.RestHourOPASearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                           , (int)ViewState["PAGENUMBER"]
                                           , General.ShowRecords(null)
                                           , ref iRowCount
                                           , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OPA90.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>OPA90</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDSTARTDATE", "FLDSTARTREPORTINGHOUR", "FLDFINISHDATE", "FLDENDREPORTINGHOUR" };
        string[] alCaptions = { "Sr.No", "Date of Entry", "Start Reporting Hour", "Date of Exit", "End Reporting Hour" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselAccountsRH.RestHourOPASearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvOPA.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount);

        General.SetPrintOptions("gvOPA", "OPA90", alCaptions, alColumns, ds);

        gvOPA.DataSource = ds;
        gvOPA.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvOPA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton CalendarStartEdit = (LinkButton)e.Item.FindControl("cmdPickListCalendarstart");
            if (CalendarStartEdit != null)
                CalendarStartEdit.Attributes.Add("onclick", "return showPickList('spnPickListCalendarstart', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListRHShipWork.aspx?framename=ifMoreInfo', true); ");

            LinkButton CalendarEdit = (LinkButton)e.Item.FindControl("cmdPickListCalendarend");
            if (CalendarEdit != null)
                CalendarEdit.Attributes.Add("onclick", "return showPickList('spnPickListCalendarend', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListRHShipWork.aspx?framename=ifMoreInfo', true); ");
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            LinkButton CalendarStartAdd = (LinkButton)e.Item.FindControl("cmdPickListCalendarstartadd");
            if (CalendarStartAdd != null)
                CalendarStartAdd.Attributes.Add("onclick", "return showPickList('spnPickListCalendarstartadd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListRHShipWork.aspx?framename=ifMoreInfo', true); ");

            LinkButton CalendarAdd = (LinkButton)e.Item.FindControl("cmdPickListCalendarendadd");
            if (CalendarAdd != null)
                CalendarAdd.Attributes.Add("onclick", "return showPickList('spnPickListCalendarendadd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListRHShipWork.aspx?framename=ifMoreInfo', true); ");
        }
    }
    protected void gvOPA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidList(((RadTextBox)e.Item.FindControl("txtShipCalendaraddId")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsRH.RestHourOPAInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        int.Parse(((RadTextBox)e.Item.FindControl("txtShipCalendaraddId")).Text),
                        General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtShipCalendarendaddId")).Text),
                        int.Parse(((RadTextBox)e.Item.FindControl("txtReportingHourAdd")).Text),
                        General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtendReportingHourAdd")).Text));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselAccountsRH.RestHourOPADelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblopaid")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                Rebind();
            }
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
    private bool IsValidList(string Startid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(Startid))
            ucError.ErrorMessage = "Date of Entry is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void gvOPA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOPA.CurrentPageIndex + 1;
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
        gvOPA.EditIndexes.Clear();
        gvOPA.SelectedIndexes.Clear();
        gvOPA.DataSource = null;
        gvOPA.Rebind();
    }

    protected void gvOPA_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidList(((RadTextBox)e.Item.FindControl("txtShipCalendarId")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsRH.RestHourOPAUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblopaeditid")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    int.Parse(((RadTextBox)e.Item.FindControl("txtShipCalendarId")).Text),
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtShipCalendarendId")).Text),
                    int.Parse(((RadTextBox)e.Item.FindControl("txtReportingHourEdit")).Text),
                    int.Parse(((RadTextBox)e.Item.FindControl("txtendReportingHourEdit")).Text)
                    );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
