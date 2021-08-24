using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionRAEconomicImpact : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvRASubHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["TYPE"] = Request.QueryString["type"];
                ucHazard.Type = Request.QueryString["type"];
                ViewState["TYPENAME"] = "Economic Hazard / Impact";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAEconomicImpact.aspx?" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRASubHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAEconomicImpact.aspx?type=" + Request.QueryString["type"], "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAEconomicImpact.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAImpactDocumentPPEMapping.aspx?type=" + ViewState["TYPE"].ToString() + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADDNEW");

        MenuRASubHazard.AccessRights = this.ViewState;
        MenuRASubHazard.MenuList = toolbar.Show();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDHAZARDNAME", "FLDSUBHAZARDNAME", "FLDIMPACTNAME", "FLDUNDESIRABLEEVENT", "FLDWORSTCASE"};
        string[] alCaptions = { "Category", "Hazard", "Impact", "Contact Type / Undesirable Event", "Worst Case" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentSubHazardExtn.RiskAssessmentEconomicHazardSearch(
            Convert.ToInt32(ViewState["TYPE"].ToString()),
            General.GetNullableInteger(ucHazard.SelectedHazardType),
            General.GetNullableString(txtName.Text),
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ViewState["TYPENAME"].ToString() + ".xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + ViewState["TYPENAME"].ToString() + "</h3></td>");
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

    protected void gvRASubHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRASubHazard.CurrentPageIndex + 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDHAZARDNAME", "FLDSUBHAZARDNAME", "FLDIMPACTNAME", "FLDUNDESIRABLEEVENT", "FLDWORSTCASE" };
            string[] alCaptions = { "Category", "Hazard", "Impact", "Contact Type / Undesirable Event", "Worst Case" };
            DataSet ds = new DataSet();

            ds = PhoenixInspectionRiskAssessmentSubHazardExtn.RiskAssessmentEconomicHazardSearch(
                Convert.ToInt32(ViewState["TYPE"].ToString()),
                General.GetNullableInteger(ucHazard.SelectedHazardType),
                General.GetNullableString(txtName.Text),
                gvRASubHazard.CurrentPageIndex + 1,
                gvRASubHazard.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


            General.SetPrintOptions("gvRASubHazard", ViewState["TYPENAME"].ToString(), alCaptions, alColumns, ds);

            gvRASubHazard.DataSource = ds;
            gvRASubHazard.VirtualItemCount = iRowCount;
           
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSubHazard_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvRASubHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRASubHazard.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteSubHazard(new Guid(((RadLabel)eeditedItem.FindControl("lblSubHazardId")).Text));
                    gvRASubHazard.Rebind();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RASubHazard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRASubHazard.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtName.Text = "";                
                gvRASubHazard.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvRASubHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdTypeMapping");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                LinkButton cmdTypeMapping = (LinkButton)e.Item.FindControl("cmdTypeMapping");
                RadLabel lblSubHazardId = (RadLabel)e.Item.FindControl("lblSubHazardId");
                if (cmdTypeMapping != null)
                {
                    cmdTypeMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdTypeMapping.CommandName);
                    cmdTypeMapping.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAImpactDocumentPPEMapping.aspx?subhazardid=" + lblSubHazardId.Text + "');return true;");
                }

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteSubHazard(Guid SubHazardid)
    {
        PhoenixInspectionRiskAssessmentSubHazardExtn.DeleteRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, SubHazardid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRASubHazard.Rebind();
    }

}