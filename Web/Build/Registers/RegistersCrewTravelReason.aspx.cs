using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersCrewTravelReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewTravelReason.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTravelreason')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewTravelReason.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvTravelreason.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDREASON", "FLDSUBACCOUNT", "FLDONSIGNERSUBACCOUNT",  "FLDREASONFOR" };
        string[] alCaptions = { "Travel reason", "Off-signers Budget Code", "On-signers Budget Code",  "Reason For" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersTravelReason.TravelReasonSearch(
                txtSearch.Text,
                sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvTravelreason.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=TravelReason.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Reason</h3></td>");
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

    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvTravelreason.Rebind();
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

        string[] alColumns = { "FLDREASON", "FLDSUBACCOUNT", "FLDONSIGNERSUBACCOUNT",  "FLDREASONFOR" };
        string[] alCaptions = { "Travel reason", "Off-signers Budget Code", "On-signers Budget Code", "Reason For" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersTravelReason.TravelReasonSearch(
                    txtSearch.Text,
                    sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvTravelreason.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvTravelreason", "Travel Reason", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTravelreason.DataSource = ds;
            gvTravelreason.VirtualItemCount = iRowCount;
        }
        else
        {
            gvTravelreason.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvTravelreason.Rebind();
    }

    private void InsertTravelReason(string reason, int budgetid, int onBudgetId)
    {
        PhoenixRegistersTravelReason.InsertTravelReason(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reason, budgetid, onBudgetId);
    }

    private void UpdateTravelReason(string Reasonid, string reason, int budgetid, int onBudgetId)
    {
        PhoenixRegistersTravelReason.UpdateTravelReason(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Reasonid), reason, budgetid, onBudgetId);
    }

    private bool IsValidReason(string reason, string budgetid,string onBudgetId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (reason.Trim().Equals(""))
            ucError.ErrorMessage = "Travel reason is required.";

        if (General.GetNullableInteger(budgetid) == null)
            ucError.ErrorMessage = "Off-signers Budget code is required.";

        if (General.GetNullableInteger(onBudgetId) == null)
            ucError.ErrorMessage = "On-signers Budget code is required.";

        return (!ucError.IsError);
    }

    private void DeleteTravelReason(string reasonid)
    {
        PhoenixRegistersTravelReason.DeleteTreavelReason(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(reasonid));
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvTravelreason_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelreason.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTravelreason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidReason(((RadTextBox)e.Item.FindControl("txtreasonadd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtBudgetId")).Text
                    , ((RadTextBox)e.Item.FindControl("txtOnBudgetId")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertTravelReason(
                    ((RadTextBox)e.Item.FindControl("txtreasonadd")).Text,
                    int.Parse(((RadTextBox)e.Item.FindControl("txtBudgetId")).Text),
                    int.Parse(((RadTextBox)e.Item.FindControl("txtOnBudgetId")).Text));

                BindData();
                gvTravelreason.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteTravelReason(((RadLabel)e.Item.FindControl("lblreasonid")).Text);
                BindData();
                gvTravelreason.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidReason(((RadTextBox)e.Item.FindControl("txtreasonedit")).Text,
                 ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
                 ((RadTextBox)e.Item.FindControl("txtOnBudgetIdEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateTravelReason(
                             ((RadLabel)e.Item.FindControl("lblreasonidedit")).Text,
                            ((RadTextBox)e.Item.FindControl("txtreasonedit")).Text,
                              int.Parse(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text),
                              int.Parse(((RadTextBox)e.Item.FindControl("txtOnBudgetIdEdit")).Text));                
                BindData();
                gvTravelreason.Rebind();
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

    protected void gvTravelreason_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            
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
}
