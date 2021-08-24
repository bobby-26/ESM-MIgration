using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersNoonReportRangeConfigPossibelCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersNoonReportRangeConfigPossibelCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRangePossibleCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersRangePossibleCause.AccessRights = this.ViewState;
            MenuRegistersRangePossibleCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COLUMNNAME"] = Request.QueryString["COLUMNNAME"].ToString();
                ViewState["DISPLAYTEXT"] = Request.QueryString["DISPLAYTEXT"].ToString();
                ViewState["LAUNCHFROM"] = Request.QueryString["LAUNCHFROM"];
                //ucTitle.Text= Request.QueryString["DISPLAYTEXT"].ToString()+ " - "+ ucTitle.Text;
                lblPossibleCause.Text = Request.QueryString["DISPLAYTEXT"].ToString();
                gvRangePossibleCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersVesselDirection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
    protected void Rebind()
    {
        gvRangePossibleCause.SelectedIndexes.Clear();
        gvRangePossibleCause.EditIndexes.Clear();
        gvRangePossibleCause.DataSource = null;
        gvRangePossibleCause.Rebind();
    }
    protected void gvRangePossibleCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string DirectionShortName, DirectionName;

                DirectionName = (((RadTextBox)e.Item.FindControl("txtCauseNameAdd")).Text);
                DirectionShortName = (((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text);

                PhoenixRegistersNoonReportRangeConfig.InsertNoonRangeConfigFieldPossibleCause(General.GetNullableString(ViewState["COLUMNNAME"].ToString()),
                                                       General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCauseNameAdd")).Text),
                                                         General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text)
                                                      );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersNoonReportRangeConfig.DeleteNoonRangeConfigFieldPossibleCause(int.Parse(((RadLabel)e.Item.FindControl("lblcauseid")).Text));
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRangePossibleCause_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || ViewState["LAUNCHFROM"] != null)
                    edit.Visible = false;
            }

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, del.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || ViewState["LAUNCHFROM"] != null)
                    del.Visible = false;
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, save.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || ViewState["LAUNCHFROM"] != null)
                    save.Visible = false;
            }

            //ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            //if (cancel != null)
            //{
            //    if (!SessionUtil.CanAccess(this.ViewState, cancel.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            //        cancel.Visible = false;
            //}

            if (e.Item is GridEditableItem)
            {
                RadLabel lb = (RadLabel)e.Item.FindControl("lblShowYN");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (lb != null)
                    lb.Text = drv["FLDSHOWYESNO"].ToString().Equals("1") ? "Yes" : "No";

                //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                //{
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                //}
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || ViewState["LAUNCHFROM"] != null)
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPOSSIBLECAUSE", "FLDPOSSIBLECAUSEDESCRIOPTION" };
        string[] alCaptions = { "Cause", "Recommended Action" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersNoonReportRangeConfig.NoonRangeConfigFieldPossibleCauseSearch(sortexpression,
                                                 sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                 gvRangePossibleCause.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableString(ViewState["COLUMNNAME"].ToString())
                                               );

        General.SetPrintOptions("gvRangePossibleCause", "Possible Cause", alCaptions, alColumns, ds);

        gvRangePossibleCause.DataSource = ds;
        gvRangePossibleCause.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDPOSSIBLECAUSE", "FLDPOSSIBLECAUSEDESCRIOPTION" };
        string[] alCaptions = { "Cause", "Recommended Action" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersNoonReportRangeConfig.NoonRangeConfigFieldPossibleCauseSearch(sortexpression,
                                                 sortdirection,
                                                 1,
                                                 iRowCount,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableString(ViewState["COLUMNNAME"].ToString())
                                               );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"PossibleCause.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Possibel Cause</h3></td>");
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

    protected void gvRangePossibleCause_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRangePossibleCause.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRangePossibleCause_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            PhoenixRegistersNoonReportRangeConfig.UpdateNoonRangeConfigFieldPossibleCause(int.Parse(((RadLabel)e.Item.FindControl("lblCauseidEdit")).Text),
                                                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCauseNameEdit")).Text),
                                                     General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text)
                                                  );
            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
