using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class VesselAccountsRHOverTimeReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHOverTimeReason.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOTReason')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHOverTimeReason.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRHOTReason.AccessRights = this.ViewState;
            MenuRHOTReason.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvOTReason.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlYear.DataBind();
        ddlYear.SelectedItem.Text = DateTime.Today.Year.ToString();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDTOTALOTHOURS", "FLDOVERTIMEREASON" };
        string[] alCaptions = { "Employee Name", "Rank", "Total OT Hours", "OT Reason" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselAccountsRHOverTimeReason.OverTimeReasonSearch(
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                General.GetNullableInteger(ddlMonth.SelectedValue),
                General.GetNullableInteger(ddlYear.SelectedItem.Text),
                sortexpression,
                sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=OTReason.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>OverTime Reason</h3></td>");
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

    protected void MenuRHOTReason_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDTOTALOTHOURS", "FLDOVERTIMEREASON" };
        string[] alCaptions = { "Employee Name", "Rank", "Total OT Hours", "OT Reason" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselAccountsRHOverTimeReason.OverTimeReasonSearch(
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    General.GetNullableInteger(ddlMonth.SelectedValue),
                    General.GetNullableInteger(ddlYear.SelectedItem.Text),
                    sortexpression,
                    sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvOTReason.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvOTReason", "Overtime Reason", alCaptions, alColumns, ds);

        gvOTReason.DataSource = ds;
        gvOTReason.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvOTReason_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvOTReason_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        
    }

    protected void gvOTReason_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlOTReasonEdit = (RadComboBox)e.Item.FindControl("ddlOTReasonEdit");
            if (ddlOTReasonEdit != null)
            {
                ddlOTReasonEdit.DataSource = PhoenixRegistersOverTimeReason.ListOverTimeReason();
                ddlOTReasonEdit.DataTextField = "FLDOVERTIMEREASON";
                ddlOTReasonEdit.DataValueField = "FLDOVERTIMEREASONID";                
                ddlOTReasonEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlOTReasonEdit.DataBind();
                ddlOTReasonEdit.SelectedValue = dr["FLDOVERTIMEREASONID"].ToString();
            }
        }        
    }
    private bool IsValidReason(int rankgroupid, decimal othours, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int? maxothours = null;
        PhoenixVesselAccountsRHOverTimeReason.GetMaxOTLimit(rankgroupid, ref maxothours);

        if (othours > maxothours)
        {
            if (General.GetNullableInteger(reason) == null)
                ucError.ErrorMessage = "OT Reason is required.";
        }
        return (!ucError.IsError);
    }

    protected void gvOTReason_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidReason(int.Parse(((RadLabel)e.Item.FindControl("lblRankGroupid")).Text),
                                decimal.Parse(((RadLabel)e.Item.FindControl("lblOTHours")).Text),
                                ((RadComboBox)e.Item.FindControl("ddlOTReasonEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            string lblRestHourOTreasonid = ((RadLabel)e.Item.FindControl("lblRestHourOTreasonid")).Text;

            if (lblRestHourOTreasonid != null && lblRestHourOTreasonid != "")
            {
                PhoenixVesselAccountsRHOverTimeReason.OverTimeReasonUpdate(new Guid(((RadLabel)e.Item.FindControl("lblRestHourOTreasonid")).Text)
                                               , decimal.Parse(((RadLabel)e.Item.FindControl("lblOTHours")).Text)
                                               , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlOTReasonEdit")).SelectedValue)
                                              );
            }
            else
            {
                PhoenixVesselAccountsRHOverTimeReason.OverTimeReasonInsert(new Guid(((RadLabel)e.Item.FindControl("lblRestHourStartid")).Text)
                                              , new Guid(((RadLabel)e.Item.FindControl("lblRestHourEmployeeid")).Text)
                                              , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                              , int.Parse(ddlMonth.SelectedValue)
                                              , int.Parse(ddlYear.SelectedItem.Text)
                                              , decimal.Parse(((RadLabel)e.Item.FindControl("lblOTHours")).Text)
                                              , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlOTReasonEdit")).SelectedValue)
                                              );
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOTReason_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void Rebind()
    {
        gvOTReason.EditIndexes.Clear();
        gvOTReason.SelectedIndexes.Clear();
        gvOTReason.DataSource = null;
        gvOTReason.Rebind();
    }

    protected void gvOTReason_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOTReason.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ddlYear.Items.Sort();
    }
}
