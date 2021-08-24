using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Inspection_InspectionRegisterPSCUSCGList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterPSCUSCGList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPSCUSCG')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterPSCUSCGList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterPSCUSCGList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('PSCUSCG','PSC USCG Code Add','" + Session["sitepath"] + "/Inspection/InspectionRegisterPSCUSCGCodeAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuPSCUSCG.AccessRights = this.ViewState;
            MenuPSCUSCG.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvPSCUSCG.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindChapter();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvPSCUSCG_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblPSCUSCGID = (RadLabel)e.Item.FindControl("lblPSCUSCGID");

                PhoenixInspectionRegisterPSCUSCG.DeletePSCUSCGCode(new Guid(lblPSCUSCGID.Text));
                ucStatus.Text = "PSC USCG Code deleted successfully.";
                ViewState["PAGENUMBER"] = null;
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvPSCUSCG_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblPSCUSCGID = (RadLabel)e.Item.FindControl("lblPSCUSCGID");

            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('PSCUSCG','PSC USCG Code Edit','" + Session["sitepath"] + "/Inspection/InspectionRegisterPSCUSCGCodeAdd.aspx?PSCUSCGID=" + lblPSCUSCGID.Text + "'); return false;");
            }
        }
    }

    protected void gvPSCUSCG_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPSCUSCG.CurrentPageIndex + 1;
        BindData();
    }

    protected void MenuPSCUSCG_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDescription.Text = "";
                ddlChapter.ClearSelection();
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindChapter()
    {

        ddlChapter.DataSource = PhoenixInspectionRegisterPSCUSCG.ListInspectionPSCUSCGChapter();
        ddlChapter.DataTextField = "FLDCHAPTERNAME";
        ddlChapter.DataValueField = "FLDCHAPTERID";
        ddlChapter.DataBind();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSFICODE", "FLDDESCRIPTION", "FLDPSCMOU", "FLDUSCGMOU", "FLDCHAPTERNAME", "FLDACTIVEYN" };
        string[] alCaptions = { "SFI Code", "Description", "PSC MOU", "USCG MOU", "Chapterr Name", "ActiveYN" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRegisterPSCUSCG.PSCUSCGSearch(General.GetNullableString(txtDescription.Text)
             , General.GetNullableGuid(null)
             , sortdirection
             , sortexpression
             , gvPSCUSCG.CurrentPageIndex + 1
             , gvPSCUSCG.PageSize
             , ref iRowCount
             , ref iTotalPageCount);

        General.SetPrintOptions("gvPSCUSCG", "ConsequenceImpact", alCaptions, alColumns, ds);

        gvPSCUSCG.DataSource = ds;
        gvPSCUSCG.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvPSCUSCG.SelectedIndexes.Clear();
        gvPSCUSCG.EditIndexes.Clear();
        gvPSCUSCG.DataSource = null;
        gvPSCUSCG.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSFICODE", "FLDDESCRIPTION", "FLDPSCMOU", "FLDUSCGMOU", "FLDCHAPTERNAME", "FLDACTIVEYN" };
        string[] alCaptions = { "SFI Code", "Description", "PSC MOU", "USCG MOU", "Chapterr Name", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRegisterPSCUSCG.PSCUSCGSearch(General.GetNullableString(txtDescription.Text)
            , General.GetNullableGuid(null)
            , sortdirection
            , sortexpression
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);
        General.ShowExcel("PSC USCG Code", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        Rebind();
    }
}