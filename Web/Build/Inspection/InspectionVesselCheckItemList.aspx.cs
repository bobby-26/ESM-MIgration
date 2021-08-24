using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionVesselCheckItemList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionVesselCheckItemList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselCheckItems')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionVesselCheckItemList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionVesselCheckItemList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionVesselCheckItemEdit.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuCheckItem.AccessRights = this.ViewState;
            MenuCheckItem.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucVessel.Enabled = true;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                gvVesselCheckItems.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindParentItem();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void ddlParentitem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvVesselCheckItems.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDITEM", "CHAPTERCODE", "FLDDEFICIENCYCATEGORYNAME", "FLDDESCRIPTION", "FLDGUIDENCENOTE", "FLDCOMPONENTNAME", "FLDLOCATION", "FLDFORMSCHECKLISTNAME", "FLDFILINGSYSTEMNAME", "FLDJOBS" };
        string[] alCaptions = { "Reference Number", "Vessel", "Item", "Audit / Inspection / Chapter Code", "Category", "Description", "Guidence Note", "Conponent Code", "Location", "Procedure Form& Checklist", "Filing System", "Job Mapping" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionVesselCheckItems.SearchVesselCheckItems(General.GetNullableString(txtItem.Text)
            , General.GetNullableInteger(ucVessel.SelectedVessel) == null? 0 : General.GetNullableInteger(ucVessel.SelectedVessel)
            ,General.GetNullableGuid(ddlParentitem.SelectedValue)
            , sortdirection
            , sortexpression
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvVesselCheckItems.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvVesselCheckItems", "Inspection Vessel Check Items", alCaptions, alColumns, ds);

        gvVesselCheckItems.DataSource = ds;
        gvVesselCheckItems.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvVesselCheckItems.SelectedIndexes.Clear();
        gvVesselCheckItems.EditIndexes.Clear();
        gvVesselCheckItems.DataSource = null;
        gvVesselCheckItems.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDITEM", "CHAPTERCODE", "FLDDEFICIENCYCATEGORYNAME", "FLDDESCRIPTION", "FLDGUIDENCENOTE", "FLDCOMPONENTNAME", "FLDLOCATION", "FLDFORMSCHECKLISTNAME", "FLDFILINGSYSTEMNAME", "FLDJOBS" };
        string[] alCaptions = { "Reference Number", "Vessel", "Item", "Audit / Inspection / Chapter Code", "Category", "Description", "Guidence Note", "Conponent Code", "Location", "Procedure Form& Checklist", "Filing System", "Job Mapping" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixInspectionVesselCheckItems.SearchVesselCheckItems(General.GetNullableString(txtItem.Text)
            , General.GetNullableInteger(ucVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ucVessel.SelectedVessel)
            ,General.GetNullableGuid(ddlParentitem.SelectedValue)
            , sortdirection
            , sortexpression
            , gvVesselCheckItems.CurrentPageIndex + 1
            , gvVesselCheckItems.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.ShowExcel("Inspection Vessel Check Items", ds.Tables[0], alColumns, alCaptions, null, null);

    }


    protected void gvVesselCheckItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselCheckItems.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                Response.Redirect("../Inspection/InspectionVesselCheckItemEdit.aspx");
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtItem.Text = "";
                ucVessel.SelectedVessel = "";
                ddlParentitem.ClearSelection();
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        Rebind();
    }

    protected void gvVesselCheckItems_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel VesselCheckitemid = (RadLabel)e.Item.FindControl("lblvesselcheckitemid");

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

                Response.Redirect("../Inspection/InspectionVesselCheckItemEdit.aspx?VESSELCHECKITEMID=" + VesselCheckitemid.Text);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionVesselCheckItems.DeleteVesselCheckItem(new Guid(VesselCheckitemid.Text));
                ucStatus.Text = "Check Item deleted successfully.";
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
                gvVesselCheckItems.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void gvVesselCheckItems_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblCheckItemid = (RadLabel)e.Item.FindControl("lblCheckitemid");
                LinkButton lnkItem = (LinkButton)e.Item.FindControl("lnkItem");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            gvVesselCheckItems.Rebind();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}