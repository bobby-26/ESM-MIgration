using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionEvent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionEvent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionEvent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionEvent.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionEvent.AccessRights = this.ViewState;
            MenuInspectionEvent.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvInspectionEvent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEVENTSHORTCODE", "FLDEVENTNAME" };
        string[] alCaptions = { "Code", "Event Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionEvent.InspectionEventSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtEventShortCode.Text)
                                                                    , General.GetNullableString(txtEventName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionEvent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspection Event</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void InspectionEvent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvInspectionEvent.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                gvInspectionEvent.Rebind();
            }

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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEVENTSHORTCODE", "FLDEVENTNAME" };
        string[] alCaptions = { "Code", "Event Name" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionEvent.InspectionEventSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , General.GetNullableString(txtEventShortCode.Text)
                                                                     , General.GetNullableString(txtEventName.Text)
                                                                     , sortexpression, sortdirection
                                                                     , gvInspectionEvent.CurrentPageIndex + 1
                                                                     , gvInspectionEvent.PageSize
                                                                     , ref iRowCount
                                                                     , ref iTotalPageCount
                                                                     );


        General.SetPrintOptions("gvInspectionEvent", "Inspection Event", alCaptions, alColumns, ds);

        gvInspectionEvent.DataSource = ds;
        gvInspectionEvent.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvInspectionEvent.SelectedIndexes.Clear();
        gvInspectionEvent.EditIndexes.Clear();
        gvInspectionEvent.DataSource = null;
        gvInspectionEvent.Rebind();
    }

    protected void gvInspectionEvent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string shortcode = ((RadTextBox)e.Item.FindControl("txtEventShortCodeAdd")).Text;
                string eventname = ((RadTextBox)e.Item.FindControl("txtEventNameAdd")).Text;
                if (!IsValidData(shortcode, eventname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionEvent.InsertInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , shortcode
                    , eventname
                    );

                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid inspectioneventid = new Guid(item.GetDataKeyValue("FLDINSPECTIONEVENTID").ToString());

                // Guid inspectioneventid = new Guid(e.Item.ToString());
                PhoenixInspectionEvent.DeleteInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                inspectioneventid);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid inspectioneventid = new Guid(item.GetDataKeyValue("FLDINSPECTIONEVENTID").ToString());

                // Guid inspectioneventid = new Guid(e.Item.ToString());
                //string shortcode = ((TextBox)e.Item.FindControl("txtEventShortCodeEdit")).Text;
                string shortcode = ((Label)e.Item.FindControl("lblEventShortCode")).Text;
                string eventname = ((RadTextBox)e.Item.FindControl("txtEventNameEdit")).Text;
                if (!IsValidData(shortcode, eventname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionEvent.UpdateInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , inspectioneventid
                                            , shortcode
                                            , eventname);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string EventShortCode, string InspectionEvent)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (EventShortCode.Equals(""))
            ucError.ErrorMessage = "Event Short Code is required";

        if (InspectionEvent.Equals(""))
            ucError.ErrorMessage = "Inspection Event is required.";

        return (!ucError.IsError);

    }

    protected void gvInspectionEvent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvInspectionEvent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionEvent.CurrentPageIndex + 1;

        BindData();
    }
}
