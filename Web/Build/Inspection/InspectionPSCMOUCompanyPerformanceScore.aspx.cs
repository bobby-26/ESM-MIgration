using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class Inspection_InspectionPSCMOUCompanyPerformanceScore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPSCMOUCompanyPerformanceScore.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Performance Matrix','Inspection/InspectionPSCMOUCompanyPerformanceConfiguration.aspx?action=add')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDADDRESS");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Class','Inspection/InspectionPSCCopyMOURegisterData.aspx?type=COMPERFMATRIX')", "Copy", "<i class=\"fa fa-copy\"></i>", "COPY");
            MenuPSCMOU.AccessRights = this.ViewState;
            MenuPSCMOU.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            //toolbarmain.AddButton("Recognized by MOU", "RECORG", ToolBarDirection.Right);
            toolbarmain.AddButton("Performance Level", "PERFLEVEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Company Performance", "CLASSPERF", ToolBarDirection.Right);
            //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;

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


    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PERFLEVEL"))
        {
            Response.Redirect("../Inspection/InspectionPSCMOUCompanyPerformanceLevelList.aspx", false);
        }

    }

    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.SelectedValue = "1";        
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDEFICIENCYINDEX", "FLDDETENTIONINDEX", "FLDCOMPANYPERFORMANCE", "FLDPSCMOUWEIGHTAGE" };
        string[] alCaptions = { "Deficiency Index", "Detention Index", "Company Performance", "Weightage" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreSearch(
            General.GetNullableGuid(ddlCompany.SelectedValue),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Company-Performance-Matrix.xls");
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

        ds = PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreSearch(
            General.GetNullableGuid(ddlCompany.SelectedValue),
            sortexpression, sortdirection,
            gvMenuPSCMOU.CurrentPageIndex + 1,
            gvMenuPSCMOU.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvMenuPSCMOU", "Company Performance ", alCaptions, alColumns, ds);

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
                        ((RadComboBox)e.Item.FindControl("ddlpscmouregionNameAdd")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDefindexAdd")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDetindexAdd")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlCompanyperfAdd")).SelectedValue))
                        //((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text)
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDefindexAdd")).SelectedValue),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDetindexAdd")).SelectedValue),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCompanyperfAdd")).SelectedValue),
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
                        ((RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDefindexedit")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlDetindexedit")).SelectedValue,
                        ((RadComboBox)e.Item.FindControl("ddlCompanyperfedit")).SelectedValue))
                        //((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreUpdate(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblpscmouEdit")).Text),
                            General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDefindexedit")).SelectedValue),
                            General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlDetindexedit")).SelectedValue),                            
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCompanyperfedit")).SelectedValue),                        
                        General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit")).SelectedValue),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text)
                         );
                    ucStatus.Text = "Information updated";
                    gvMenuPSCMOU.Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreDelete(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblpscmouId")).Text));
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
            RadComboBox ddlDetindexedit = (RadComboBox)e.Item.FindControl("ddlDetindexedit");
            RadComboBox ddlpscmouregionNameedit = (RadComboBox)e.Item.FindControl("ddlpscmouregionNameedit");
            RadComboBox ddlCompanyperfedit = (RadComboBox)e.Item.FindControl("ddlCompanyperfedit");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (ddlpscmouregionNameedit != null)
            {
                ddlpscmouregionNameedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ddlpscmouregionNameedit.DataTextField = "FLDCOMPANYNAME";
                ddlpscmouregionNameedit.DataValueField = "FLDCOMPANYID";
                ddlpscmouregionNameedit.DataBind();
                if (drv["FLDPSCMOU"] != null) ddlpscmouregionNameedit.SelectedValue = drv["FLDPSCMOU"].ToString();
            }

            if (ddlDetindexedit != null)
            {
                ddlDetindexedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDetindexedit.DataTextField = "FLDNAME";
                ddlDetindexedit.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDetindexedit.DataBind();
                ddlDetindexedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (drv["FLDDETENTIONINDEXID"] != null) ddlDetindexedit.SelectedValue = drv["FLDDETENTIONINDEXID"].ToString();
            }

            if (ddlDefindexedit != null)
            {
                ddlDefindexedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDefindexedit.DataTextField = "FLDNAME";
                ddlDefindexedit.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDefindexedit.DataBind();
                ddlDefindexedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (drv["FLDDEFICIENCYINDEXID"] != null) ddlDefindexedit.SelectedValue = drv["FLDDEFICIENCYINDEXID"].ToString();
            }

            if (ddlCompanyperfedit != null)
            {
                ddlCompanyperfedit.DataSource = PhoenixInspectionPSCMOUMatrix.ListCompanyPerformanceList();
                ddlCompanyperfedit.DataTextField = "FLDNAME";
                ddlCompanyperfedit.DataValueField = "FLDCOMPANYPERFORMANCEID";
                ddlCompanyperfedit.DataBind();
                ddlCompanyperfedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (drv["FLDCOMPANYPERFORMANCEID"] != null) ddlCompanyperfedit.SelectedValue = drv["FLDCOMPANYPERFORMANCEID"].ToString();
            }
            //GridDecorator.MergeRows(gvMenuPSCMOU, e);

        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");


            RadComboBox ddlDefindexAdd = (RadComboBox)e.Item.FindControl("ddlDefindexAdd");
            RadComboBox ddlDetindexAdd = (RadComboBox)e.Item.FindControl("ddlDetindexAdd");
            RadComboBox ddlpscmouregionNameAdd = (RadComboBox)footer.FindControl("ddlpscmouregionNameAdd");
            RadComboBox ddlCompanyperfAdd = (RadComboBox)e.Item.FindControl("ddlCompanyperfAdd");


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
            if (ddlDetindexAdd != null)
            {
                ddlDetindexAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDetindexAdd.DataTextField = "FLDNAME";
                ddlDetindexAdd.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDetindexAdd.DataBind();
                ddlDetindexAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));               
            }

            if (ddlDefindexAdd != null)
            {
                ddlDefindexAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
                ddlDefindexAdd.DataTextField = "FLDNAME";
                ddlDefindexAdd.DataValueField = "FLDDEFANDDETINDEXID";
                ddlDefindexAdd.DataBind();
                ddlDefindexAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

            if (ddlCompanyperfAdd != null)
            {
                ddlCompanyperfAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListCompanyPerformanceList();
                ddlCompanyperfAdd.DataTextField = "FLDNAME";
                ddlCompanyperfAdd.DataValueField = "FLDCOMPANYPERFORMANCEID";
                ddlCompanyperfAdd.DataBind();
                ddlCompanyperfAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }
    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblCompanygperf")).Text;
                string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblCompanygperf")).Text;

                string currentTypeName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblScore")).Text;
                string previousTypeName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblScore")).Text;

                if (currentTypeName == previousTypeName && currentCategoryName != previousCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                        previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                else if (currentTypeName == previousTypeName && currentCategoryName == previousCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
    }


    private bool IsValidFlag(string pscmou, string defindex, string detindex, string perf)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //    RadGrid _gridView = gvRASeverity;

        if (General.GetNullableInteger(defindex) == null)
            ucError.ErrorMessage = "Deficiency Index is required.";

        if (General.GetNullableInteger(detindex) == null)
            ucError.ErrorMessage = "Detention Index is required.";

        if (General.GetNullableInteger(perf) == null)
            ucError.ErrorMessage = "Company Performance is required.";

        //if (shipcode.Trim().Equals(""))
        //    ucError.ErrorMessage = "Ship code is required.";

        if (General.GetNullableGuid(pscmou) == null)
            ucError.ErrorMessage = "PSC MOU is required.";

        //if (General.GetNullableInteger(score) == null)
        //    ucError.ErrorMessage = "Score is required.";
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

    protected void Rebind()
    {
        gvMenuPSCMOU.SelectedIndexes.Clear();
        gvMenuPSCMOU.EditIndexes.Clear();
        gvMenuPSCMOU.DataSource = null;
        gvMenuPSCMOU.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

}