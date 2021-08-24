using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionUndesirableEventWorstCase : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionUndesirableEventWorstCase.aspx?", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRASubHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("../Inspection/InspectionUndesirableEventWorstCase.aspx?type=", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuRASubHazard.AccessRights = this.ViewState;
            MenuRASubHazard.MenuList = toolbar.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Back", "LIST", ToolBarDirection.Right);
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CONTACTTYPEID"] = "";
                if ((Request.QueryString["contacttypeid"] != null) && (Request.QueryString["contacttypeid"] != ""))
                {
                    ViewState["CONTACTTYPEID"] = Request.QueryString["contacttypeid"].ToString();
                }

                if ((Request.QueryString["contacttype"] != null) && (Request.QueryString["contacttype"] != ""))
                {
                    txteventname.Text = Request.QueryString["contacttype"].ToString();
                }

                gvRASubHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDEVENTNAME", "FLDWORSTCASENAME", "FLDSEVERITY", "FLDSCORE", "FLDRISKOFESCALATION", "FLDEQUIPMENT", "FLDPROCEDURES", "FLDPPELIST" };
        string[] alCaptions = { "Contact Type / Undesirable Event", "Worst Case", "Severity", "Score", "Risk of Escalation", "Equipment", "Procedures, Forms and Checklists", "PPE" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionOperationalRiskControls.UndisrableEventSearch(
            General.GetNullableGuid(ViewState["CONTACTTYPEID"].ToString()),
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=UndesirableEventWorstCase.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Undesirable Event Worst Case</h3></td>");
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
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRASubHazard.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEVENTNAME", "FLDWORSTCASENAME", "FLDSEVERITY", "FLDSCORE", "FLDRISKOFESCALATION", "FLDEQUIPMENT", "FLDPROCEDURES", "FLDPPELIST" };
        string[] alCaptions = { "Contact Type / Undesirable Event", "Worst Case", "Severity", "Score", "Risk of Escalation", "Equipment", "Procedures, Forms and Checklists","PPE" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionOperationalRiskControls.UndisrableEventSearch(
            General.GetNullableGuid(ViewState["CONTACTTYPEID"].ToString()),
            gvRASubHazard.CurrentPageIndex + 1,
            gvRASubHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRASubHazard", "Undesirable Event Worst Case", alCaptions, alColumns, ds);

        gvRASubHazard.DataSource = ds;
        gvRASubHazard.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRASubHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionContactTypeExtn.aspx?");
            }
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
                    DeleteSubHazard(new Guid(((RadLabel)eeditedItem.FindControl("lbluniqueid")).Text));       
                    gvRASubHazard.Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("CONTROLMAPPING"))
                {
                    string uniqueid = ((RadLabel)eeditedItem.FindControl("lbluniqueid")).Text;
                    Response.Redirect("../Inspection/InspectionHazardControlMapping.aspx?uniqueid=" + uniqueid+ "&contacttypeid=" + ViewState["CONTACTTYPEID"].ToString());
                }
            }

            if (e.Item is GridFooterItem)
            {
                var FooterItem = ((GridFooterItem)e.Item);

                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    GridFooterItem item = (GridFooterItem)gvRASubHazard.MasterTableView.GetItems(GridItemType.Footer)[0];

                    if (!IsValidRASubHazard(((RadDropDownList)FooterItem.FindControl("ddlWorstCase")).SelectedValue.ToString()))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRASubHazard(ViewState["CONTACTTYPEID"].ToString(),
                        ((RadDropDownList)FooterItem.FindControl("ddlWorstCase")).SelectedValue);

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

    protected void gvRASubHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }

                LinkButton cmdControlMapping = (LinkButton)e.Item.FindControl("cmdControlMapping");
                if (cmdControlMapping != null)
                {
                    cmdControlMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdControlMapping.CommandName);
                }
            }

            if (e.Item is GridFooterItem)
            {
                LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                {
                    cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
                }

                RadDropDownList ddlWorstCase = (RadDropDownList)e.Item.FindControl("ddlWorstCase");
                ddlWorstCase.DataTextField = "FLDNAME";
                ddlWorstCase.DataValueField = "FLDHAZARDID";
                ddlWorstCase.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(3, null);
                ddlWorstCase.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertRASubHazard(string eventid, string hazardid)
    {

        PhoenixInspectionOperationalRiskControls.InsertUndesirableEventWorstCase(new Guid(eventid), int.Parse(hazardid));
    }
    
    private bool IsValidRASubHazard(string hazardid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["CONTACTTYPEID"].ToString()) ==null)
            ucError.ErrorMessage = "Contact Type / Undesirable Event is required.";

        if (General.GetNullableInteger(hazardid) == null)
            ucError.ErrorMessage = "Worst Case is required.";

        return (!ucError.IsError);
    }

    private void DeleteSubHazard(Guid uniqueid)
    {
        PhoenixInspectionOperationalRiskControls.DeleteUndesirableEventWorstCase(uniqueid);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
