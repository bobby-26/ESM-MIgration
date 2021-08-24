using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRALevelOfRiskExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRALevelOfRiskExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRALevel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRALevelOfRiskExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRACategory.AccessRights = this.ViewState;
            MenuRACategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
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
        string sortexpression;
        int? sortdirection = null;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDRISKLEVEL", "FLDRANGE", "FLDMINRANGE", "FLDMAXRANGE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Risk Level", "Range", "Min. Range", "Max. Range", "Description" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentTypeExtn.RiskAssessmentLevelOfRiskSearch(
                                                                            General.GetNullableString(null),
                                                                            sortexpression,
                                                                            sortdirection,
                                                                            1,
                                                                            iRowCount,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LevelOfRisk.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Level of Risk</h3></td>");
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

    protected void RACategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDRISKLEVEL", "FLDRANGE", "FLDMINRANGE", "FLDMAXRANGE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Risk Level", "Range", "Min. Range", "Max. Range", "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentTypeExtn.RiskAssessmentLevelOfRiskSearch(
                                                                          General.GetNullableString(txtRiskLevel.Text),
                                                                          sortexpression,
                                                                          sortdirection,
                                                                          gvRALevel.CurrentPageIndex + 1,
                                                                          gvRALevel.PageSize,
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);

        General.SetPrintOptions("gvRALevel", "Level of Risk", alCaptions, alColumns, ds);

        gvRALevel.DataSource = ds;
        gvRALevel.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRALevel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidLevel(((RadTextBox)e.Item.FindControl("txtRiskLevelAdd")).Text
                        , (((UserControlMaskNumber)e.Item.FindControl("ucMinRangeAdd")).Text)
                        , (((UserControlMaskNumber)e.Item.FindControl("ucMaxRangeAdd")).Text)))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionRiskAssessmentTypeExtn.InsertRiskAssessmentLevelOfRisk(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        ((RadTextBox)e.Item.FindControl("txtRiskLevelAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtRangeAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text,
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucMinRangeAdd")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucMaxRangeAdd")).Text),
                        null);

                    ucStatus.Text = "Level of Risk is added.";
                    BindData();
                    gvRALevel.Rebind();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidLevel(((RadTextBox)e.Item.FindControl("txtRiskLevelEdit")).Text
                  , (((UserControlMaskNumber)e.Item.FindControl("ucMinRangeEdit")).Text)
                  , (((UserControlMaskNumber)e.Item.FindControl("ucMaxRangeEdit")).Text)))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionRiskAssessmentTypeExtn.InsertRiskAssessmentLevelOfRisk(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        ((RadTextBox)e.Item.FindControl("txtRiskLevelEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtRangeEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text,
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucMinRangeEdit")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucMaxRangeEdit")).Text),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblRiskLevelIdEdit")).Text));

                    ucStatus.Text = "Level of Risk is updated.";
                    gvRALevel.Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixInspectionRiskAssessmentTypeExtn.DeleteRiskAssessmentLevelOfRisk(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblRiskLevelId")).Text));

                    ucStatus.Text = "Level of Risk is deleted.";
                    gvRALevel.Rebind();
                }
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRALevel_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucDescription");
            if (uct != null)
            {
                lblDescription.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblDescription.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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

    private void InsertRACategory(string code, string name)
    {

        PhoenixInspectionRiskAssessmentCategory.InsertRiskAssessmentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              name, code);
    }

    private bool IsValidLevel(string risklevel, string minrange, string maxrange)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(risklevel) == null)
            ucError.ErrorMessage = "Risk Level is required.";

        if (General.GetNullableInteger(minrange) == null && General.GetNullableInteger(maxrange) == null)
            ucError.ErrorMessage = "Minimum or Maximum value of range is required.";

        return (!ucError.IsError);
    }

    private void DeleteDesignation(int categoryid)
    {
        PhoenixInspectionRiskAssessmentCategory.DeleteRiskAssessmentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, categoryid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRALevel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRALevel_SortCommand(object sender, GridSortCommandEventArgs e)
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
}