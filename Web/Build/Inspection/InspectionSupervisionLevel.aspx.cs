using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class InspectionSupervisionLevel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSupervisionLevel.aspx?", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRALevel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSupervisionLevel.aspx?", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRACategory.AccessRights = this.ViewState;
            MenuRACategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRALevel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDRISKLEVEL", "FLDSCOREFORSUPERVISION", "FLDASPECTSSCORE" };
        string[] alCaptions = { "Supervision Level", "Score for Supervision", "Score for use in Aspect" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRiskAssessmentSupervisionLevel.RiskAssessmentLevelOfRiskSearch(
                                                                            General.GetNullableString(null),
                                                                            sortexpression,
                                                                            sortdirection,
                                                                            1,
                                                                            iRowCount,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SupervisionLevel.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Supervision Level</h3></td>");
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
                gvRALevel.Rebind();
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRALevel.CurrentPageIndex + 1;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDRISKLEVEL", "FLDSCOREFORSUPERVISION", "FLDASPECTSSCORE" };
        string[] alCaptions = { "Supervision Level", "Score for Supervision", "Score for use in Aspect" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRiskAssessmentSupervisionLevel.RiskAssessmentLevelOfRiskSearch(
                                                                          General.GetNullableString(txtRiskLevel.Text),
                                                                          sortexpression,
                                                                          sortdirection,
                                                                          gvRALevel.CurrentPageIndex + 1,
                                                                          gvRALevel.PageSize,
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);

        General.SetPrintOptions("gvRALevel", "Supervision Level", alCaptions, alColumns, ds);
        gvRALevel.DataSource = ds;
        gvRALevel.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRALevel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRALevel.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if(!IsValidLevel((((UserControlMaskNumber)editableItem.FindControl("ucSupervisionEdit")).Text)
                   , (((UserControlMaskNumber)editableItem.FindControl("ucAspectsEdit")).Text)))
                     {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRiskAssessmentSupervisionLevel.InsertRiskAssessmentLevelOfRisk(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        ((RadTextBox)editableItem.FindControl("txtRiskLevelEdit")).Text,
                        General.GetNullableInteger(((UserControlMaskNumber)editableItem.FindControl("ucSupervisionEdit")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)editableItem.FindControl("ucAspectsEdit")).Text),
                        General.GetNullableInteger(((Label)editableItem.FindControl("lblRiskLevelIdEdit")).Text));

                    ucStatus.Text = "Level of Risk is updated.";
                    gvRALevel.Rebind();
                }

                //if (e.CommandName.ToUpper().Equals("DELETE"))
                //{
                //    PhoenixRiskAssessmentSupervisionLevel.DeleteRiskAssessmentLevelOfRisk(
                //                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //                                           General.GetNullableGuid(((Label)editableItem.FindControl("lblRiskLevelId")).Text));

                //    ucStatus.Text = "Supervision Level is deleted.";
                //    gvRALevel.Rebind();
                //}
            }

            //if (e.Item is GridFooterItem)
            //{
            //    var FooterItem = ((GridFooterItem)e.Item);

            //    if (e.CommandName.ToUpper().Equals("ADD"))
            //    {
            //        GridFooterItem item = (GridFooterItem)gvRALevel.MasterTableView.GetItems(GridItemType.Footer)[0];

            //        if (!IsValidLevel(((RadTextBox)item.FindControl("txtRiskLevelAdd")).Text
            //        , (((UserControlMaskNumber)item.FindControl("ucSupervisionAdd")).Text)
            //        , (((UserControlMaskNumber)item.FindControl("ucAspectsAdd")).Text)))
            //        {
            //            ucError.Visible = true;
            //            return;
            //        }

            //        PhoenixRiskAssessmentSupervisionLevel.InsertRiskAssessmentLevelOfRisk(
            //            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //            ((RadTextBox)item.FindControl("txtRiskLevelAdd")).Text,
            //            General.GetNullableInteger(((UserControlMaskNumber)item.FindControl("ucSupervisionAdd")).Text),
            //            General.GetNullableInteger(((UserControlMaskNumber)item.FindControl("ucAspectsAdd")).Text),
            //            null);

            //        ucStatus.Text = "Supervision Level is added.";
            //        gvRALevel.Rebind();
            //    }
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRALevel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                //LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                //if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                //if (e.Item is GridEditableItem)
                //{
                //    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                //    if (db != null)
                //    {
                //        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                //    }
                //}                
            }

            //if (e.Item is GridFooterItem)
            //{
            //    LinkButton db1 = (LinkButton)e.Item.FindControl("cmdAdd");
            //    if (db1 != null)
            //    {
            //        if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName))
            //            db1.Visible = false;
            //    }               
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private bool IsValidLevel(string supervision, string Aspects)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(supervision) == null)
            ucError.ErrorMessage = "Score for Supervision is required.";

        if (General.GetNullableInteger(Aspects) == null)
            ucError.ErrorMessage = "Score for use in Aspect is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
