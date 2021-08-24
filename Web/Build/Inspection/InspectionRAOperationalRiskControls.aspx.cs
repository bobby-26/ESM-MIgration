using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionRAOperationalRiskControls : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAOperationalRiskControls.aspx?", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAOperationalHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAOperationalRiskControls.aspx?", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionOperationalRiskControlMapping.aspx?')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADDNEW");

            MenuRAWorkInjuryCategory.AccessRights = this.ViewState;
            MenuRAWorkInjuryCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvRAOperationalHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCategory()
    {
        DataTable ds = new DataTable();

        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();

        if (ds.Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds;
            ddlCategory.DataBind();
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDELEMENTNAME", "FLDASPECT", "FLDOPERATIONALHAZARD", "FLDCONTROLPRECAUTIONS" };
        string[] alCaptions = { "Process", "Aspect", "Hazards / Risks", "Controls / Precautions" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionOperationalRiskControls.OperationalRiskControlsSearch(
            General.GetNullableInteger(ddlCategory.SelectedValue),
            General.GetNullableString(txtOperationalHazard.Text.Trim()),
            gvRAOperationalHazard.CurrentPageIndex + 1,
            gvRAOperationalHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Aspects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Aspects</h3></td>");
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

    protected void RAWorkInjuryCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRAOperationalHazard.Rebind();
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAOperationalHazard.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDELEMENTNAME", "FLDASPECT", "FLDOPERATIONALHAZARD", "FLDCONTROLPRECAUTIONS" };
        string[] alCaptions = { "Process", "Aspect", "Hazards / Risks", "Controls / Precautions" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionOperationalRiskControls.OperationalRiskControlsSearch(
            General.GetNullableInteger(ddlCategory.SelectedValue),
            General.GetNullableString(txtOperationalHazard.Text.Trim()),
             gvRAOperationalHazard.CurrentPageIndex + 1,
            gvRAOperationalHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAOperationalHazard", "Aspects", alCaptions, alColumns, ds);

        gvRAOperationalHazard.DataSource = ds;
        gvRAOperationalHazard.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRAOperationalHazard_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvRAOperationalHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRAOperationalHazard.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteOperationalHazard(new Guid(((RadLabel)editableItem.FindControl("lblOperationalId")).Text));
                    gvRAOperationalHazard.Rebind();
                }

                //else if (e.CommandName.ToUpper().Equals("TYPEMAPPING"))
                //{
                //    string Operationalhazardid = ((RadLabel)editableItem.FindControl("lblOperationalId")).Text;
                //    Response.Redirect("../Inspection/InspectionOperationalRiskControlMapping.aspx?Operationalhazardid=" + Operationalhazardid);
                //}
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRAOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOperationalRiskControlMapping.aspx?Operationalhazardid=" + lblOperationalId.Text + "');return true;");
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
    protected void gvRAOperationalHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    private void DeleteOperationalHazard(Guid OperationalHazardid)
    {
        PhoenixInspectionOperationalRiskControls.DeleteOperationalRiskControls(PhoenixSecurityContext.CurrentSecurityContext.UserCode, OperationalHazardid);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRAOperationalHazard.Rebind();
    }
}
