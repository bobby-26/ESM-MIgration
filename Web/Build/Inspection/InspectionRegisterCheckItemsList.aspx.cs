using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRegisterCheckItemsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterCheckItemsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionCheckItems')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterCheckItemsList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterCheckItemsList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRegisterCheckItemsList.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionVesselCheckitemsAdd.aspx')", "Vessel Add", "<i class=\"fa fa-spare-move\"></i>", "VESSEL");

            MenuCheckItem.AccessRights = this.ViewState;
            MenuCheckItem.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPANYID"] = "";

                gvInspectionCheckItems.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                BindParentItem();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindParentItem()
    {
        ddlParentitem.DataSource = PhoenixInspectionRegisterCheckItems.ListCheckItems();
        ddlParentitem.DataTextField = "FLDITEM";
        ddlParentitem.DataValueField = "FLDINSPECTIONCHECKITEMID";
        ddlParentitem.DataBind();
        ddlParentitem.Items.Insert(0, new RadComboBoxItem("--Select--"));
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDITEM", "CHAPTERCODE", "FLDDEFICIENCYCATEGORYNAME", "FLDDESCRIPTION", "FLDGUIDENCENOTE", "FLDCOMPONENTNAME", "FLDLOCATION", "FLDFORMSCHECKLISTNAME", "FLDFILINGSYSTEMNAME", "FLDJOBS" };
        string[] alCaptions = { "Reference Number", "Item", "Audit / Inspection / Chapter Code", "Category", "Description", "Guidence Note", "Conponent Code", "Location", "Procedure Form& Checklist", "Filing System", "Job Mapping" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRegisterCheckItems.SearchCheckItems(General.GetNullableString(txtItem.Text)
            ,General.GetNullableGuid(ddlParentitem.SelectedValue)
            , sortdirection
            , sortexpression
            , gvInspectionCheckItems.CurrentPageIndex + 1
            , gvInspectionCheckItems.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvInspectionCheckItems", "Global Check Items", alCaptions, alColumns, ds);

        gvInspectionCheckItems.DataSource = ds;
        gvInspectionCheckItems.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvInspectionCheckItems.SelectedIndexes.Clear();
        gvInspectionCheckItems.EditIndexes.Clear();
        gvInspectionCheckItems.DataSource = null;
        gvInspectionCheckItems.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDITEM", "CHAPTERCODE", "FLDDEFICIENCYCATEGORYNAME","FLDDESCRIPTION","FLDGUIDENCENOTE","FLDCOMPONENTNAME","FLDLOCATION","FLDFORMSCHECKLISTNAME","FLDFILINGSYSTEMNAME","FLDJOBS" };
        string[] alCaptions = {"Reference Number", "Item", "Audit / Inspection / Chapter Code","Category","Description","Guidence Note","Conponent Code","Location","Procedure Form& Checklist","Filing System","Job Mapping"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRegisterCheckItems.SearchCheckItems(General.GetNullableString(txtItem.Text)
            , General.GetNullableGuid(ddlParentitem.SelectedValue)
             , sortdirection
             , sortexpression
             , 1
             , iRowCount
             , ref iRowCount
             , ref iTotalPageCount
             , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        General.ShowExcel("Global Check Items", ds.Tables[0], alColumns, alCaptions, null, null);

    }
    protected void gvInspectionCheckItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionCheckItems.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvInspectionCheckItems_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {            

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblCheckitemid = (RadLabel)e.Item.FindControl("lblCheckitemid");

                Response.Redirect("../Inspection/InspectionRegisterCheckItemsAdd.aspx?CHECKITEMID="+lblCheckitemid.Text);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblCheckitemid = (RadLabel)e.Item.FindControl("lblCheckitemid");

                PhoenixInspectionRegisterCheckItems.DeleteCheckItem(new Guid(lblCheckitemid.Text));
                ucStatus.Text = "Check Item deleted successfully.";
                ViewState["PAGENUMBER"] = null;
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;                
            }
           // Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvInspectionCheckItems_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCheckItemid = (RadLabel)e.Item.FindControl("lblCheckitemid");
            LinkButton lnkItem = (LinkButton)e.Item.FindControl("lnkItem");
        }
    }

    protected void MenuCheckItem_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../Inspection/InspectionRegisterCheckItemsAdd.aspx");
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtItem.Text = "";
                ddlParentitem.ClearSelection();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        Rebind();
    }

    protected void lnkDocumentAdd_Click(object sender, EventArgs e)
    {

    }

    protected void ddlParentitem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvInspectionCheckItems.Rebind();
    }
}