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
public partial class Inspection_InspectionPSCMOUDeficiencyScore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPSCMOUDeficiencyScore.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMenuPSCMOU')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("../Inspection/InspectionPSCMOUFlagScore.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuPSCMOU.AccessRights = this.ViewState;
            MenuPSCMOU.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindPSCMOU();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTYPEDESCRIPTION", "FLDPSCMOU", "FLDSCORE" };
        string[] alCaptions = { "Ship Type", "PSC MOU", "Weightage" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPSCMOUMatrix.PSCMOUDeficencyScoreSearch(
            General.GetNullableInteger(ddlindextype.SelectedValue),
            General.GetNullableGuid(ddlCompany.SelectedValue),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Ship Type.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Severity</h3></td>");
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

    protected void MenuPSCMOU_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvMenuPSCMOU.Rebind();
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
        string[] alColumns = { "FLDTYPEDESCRIPTION", "FLDPSCMOU", "FLDSCORE" };
        string[] alCaptions = { "Ship Type", "PSC MOU", "Weightage" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPSCMOUMatrix.PSCMOUDeficencyScoreSearch(
            General.GetNullableInteger(ddlindextype.SelectedValue),
            General.GetNullableGuid(ddlCompany.SelectedValue),
            sortexpression, sortdirection,
            gvMenuPSCMOU.CurrentPageIndex + 1,
            gvMenuPSCMOU.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvMenuPSCMOU", "Deficiency/Detention Index  ", alCaptions, alColumns, ds);

        gvMenuPSCMOU.DataSource = ds;
        gvMenuPSCMOU.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvMenuPSCMOU_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidFlag(
                        ((RadComboBox)e.Item.FindControl("ddlIndexAdd")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDefindexAdd")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlpscmouregionNameAdd")).SelectedValue,
                        ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("uclimitAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionPSCMOUMatrix.PSCMOUDeficiencyScoreInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlIndexAdd")).SelectedValue),                       
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDefindexAdd")).SelectedValue),
                        "",
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("uclimitAdd")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucAverageAdd")).Text),
                        General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlpscmouregionNameAdd")).SelectedValue),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text)                        
                        
                    );
                    ucStatus.Text = "Information updated";
                    BindData();
                    gvMenuPSCMOU.Rebind();
                }
            }

            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    if (!IsValidFlag(
                        ((RadComboBox)e.Item.FindControl("ddlIndexedit")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDefindexedit")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit")).SelectedValue,
                        ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("uclimitEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionPSCMOUMatrix.PSCMOUDeficiencyScoreUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblIndexEdit")).Text),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlIndexedit")).SelectedValue),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDefindexedit")).SelectedValue),
                        "",
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("uclimitEdit")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucAverageEdit")).Text),
                        General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit")).SelectedValue),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text)
                         );
                    ucStatus.Text = "Information updated";
                    gvMenuPSCMOU.Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixInspectionPSCMOUMatrix.PSCMOUDeficiencyScoreDelete(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblIndexId")).Text));
                    gvMenuPSCMOU.Rebind();
                    ucStatus.Text = "Information Delete";
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvMenuPSCMOU_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadComboBox ddlDefindexedit = (RadComboBox)e.Item.FindControl("ddlDefindexedit");
            RadComboBox ddlpscmouregionNameedit = (RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit");
            RadComboBox ddlIndexedit = (RadComboBox)e.Item.FindControl("ddlIndexedit");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (ddlpscmouregionNameedit != null)
            {
                ddlpscmouregionNameedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ddlpscmouregionNameedit.DataTextField = "FLDCOMPANYNAME";
                ddlpscmouregionNameedit.DataValueField = "FLDCOMPANYID";
                ddlpscmouregionNameedit.DataBind();
                if (drv["FLDPSCMOU"] != null) ddlpscmouregionNameedit.SelectedValue = drv["FLDPSCMOU"].ToString();
            }

            if (ddlDefindexedit != null)
            {
                ddlDefindexedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDefindexedit.DataTextField = "FLDNAME";
                ddlDefindexedit.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDefindexedit.DataBind();
                ddlDefindexedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (drv["FLDDEFINDEX"] != null) ddlDefindexedit.SelectedValue = drv["FLDDEFINDEX"].ToString();
            }
            if(ddlIndexedit != null)
            {
                ddlIndexedit.SelectedValue = drv["FLDINDEX"].ToString();
            }

        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            RadComboBox ddlpscmouregionNameAdd = (RadComboBox)footer.FindControl("ddlpscmouregionNameAdd");
            RadComboBox ddlDefindexAdd = (RadComboBox)footer.FindControl("ddlDefindexAdd");


            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }


            if (ddlpscmouregionNameAdd != null)
            {
                ddlpscmouregionNameAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ddlpscmouregionNameAdd.DataTextField = "FLDCOMPANYNAME";
                ddlpscmouregionNameAdd.DataValueField = "FLDCOMPANYID";
                ddlpscmouregionNameAdd.DataBind();
                ddlpscmouregionNameAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

            if (ddlDefindexAdd != null)
            {
                ddlDefindexAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDefindexAdd.DataTextField = "FLDNAME";
                ddlDefindexAdd.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDefindexAdd.DataBind();
                ddlDefindexAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }

    private bool IsValidFlag(string type, string index, string pscmou, string limit, string score)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //    RadGrid _gridView = gvRASeverity;

        if (General.GetNullableInteger(index) == null)
            ucError.ErrorMessage = "Index is required.";

        if (General.GetNullableInteger(type) == null)
            ucError.ErrorMessage = "Index type is required.";

        //if (shipcode.Trim().Equals(""))
        //    ucError.ErrorMessage = "Ship code is required.";

        if (General.GetNullableGuid(pscmou) == null)
            ucError.ErrorMessage = "PSC MOU is required.";

        if (General.GetNullableInteger(limit) == null)
            ucError.ErrorMessage = "Limit count is required.";

        if (General.GetNullableInteger(score) == null)
            ucError.ErrorMessage = "Score is required.";
        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvMenuPSCMOU_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvMenuPSCMOU_SortCommand(object sender, GridSortCommandEventArgs e)
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


    protected void ddlCompany_TextChanged(object sender, EventArgs e)
    {
        gvMenuPSCMOU.Rebind();
    }

    protected void ddlindextype_TextChanged(object sender, EventArgs e)
    {
        gvMenuPSCMOU.Rebind();
    }
}