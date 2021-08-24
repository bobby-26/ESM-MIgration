using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;

public partial class VesselAccounts_VesselAccountsComponentTaxTypeMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsComponentTaxTypeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentTaxType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsComponentTaxTypeMap.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','VesselAccounts/VesselAccountsComponentTaxTypeMapAdd.aspx')", "Add Component Tax Type Map", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuComponentTaxType.AccessRights = this.ViewState;
            MenuComponentTaxType.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["COMPONENTID"] = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                lblUnion.Visible = true;
                ddlUnion.Visible = true;
                lblCBARevision.Visible = true;
                ddlRevision.Visible = true;
                lblWageComponents.Visible = false;
                ucWageComponents.Visible = false;

                gvComponentTaxType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNAME", "FLDCOMPONENTTYPE", "FLDISACTIVEDESC" };
            string[] alCaptions = { "Component", "Type", "Active Y/N" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            DataSet ds = PhoenixVesselAccountsComponentTaxTypeMap.SearchComponentTaxTypeMap(int.Parse(ddlType.SelectedValue),
                                                                                            General.GetNullableInteger(ddlUnion.SelectedAddress),
                                                                                            General.GetNullableGuid(ddlRevision.SelectedValue),
                                                                                            General.GetNullableInteger(ddlCountry.SelectedCountry),
                                                                                            null,
                                                                                            null,
                                                                                            sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Component Tax Type Map", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void Rebind()
    {
        gvComponentTaxType.SelectedIndexes.Clear();
        gvComponentTaxType.EditIndexes.Clear();
        // gvComponentTaxType.DataSource = null;
        gvComponentTaxType.Rebind();
    }

    protected void MenuComponentTaxTypeMap_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void MenuComponentTaxType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvComponentTaxType.CurrentPageIndex = 0;
                gvComponentTaxType.Rebind();

            }
         
            if (CommandName.ToUpper().Equals("ADD"))
            {
               
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('ComponentTaxTypeMap','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsComponentTaxTypeMapAdd.aspx?'); return true;");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNAME", "FLDCOMPONENTTYPE", "FLDISACTIVEDESC" };
            string[] alCaptions = { "Component", "Type", "Active Y/N" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsComponentTaxTypeMap.SearchComponentTaxTypeMap(int.Parse(ddlType.SelectedValue),
                                                                                            General.GetNullableInteger(ddlUnion.SelectedAddress),
                                                                                            General.GetNullableGuid(ddlRevision.SelectedValue),
                                                                                            General.GetNullableInteger(ddlCountry.SelectedCountry),
                                                                                            null,
                                                                                            null,
                                                                                            sortexpression, sortdirection
                                                                                            , gvComponentTaxType.CurrentPageIndex + 1
                                                                                            , gvComponentTaxType.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvComponentTaxType", "Component Tax Type", alCaptions, alColumns, ds);
            gvComponentTaxType.DataSource = ds;
            gvComponentTaxType.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComponentTaxType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentTaxType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvComponentTaxType.Rebind();
    }

    protected void gvComponentTaxType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
       
        if (e.Item is GridEditableItem)
        {
            RadLabel lblid = (RadLabel)e.Item.FindControl("lblid");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('ComponentTaxTypeMap', '','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsComponentTaxTypeMapAdd.aspx?Id=" + lblid.Text + "'); return false;");
            }

        }
       
    }

    protected void gvComponentTaxType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
          
             if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   

    private void FetchCBARevision()
    {
        DataTable dt = PhoenixRegistersContract.FetchCBARevision(General.GetNullableInteger(ddlUnion.SelectedAddress));
        ddlRevision.DataSource = dt;
        ddlRevision.DataBind();
        if (dt.Rows.Count > 0) ddlRevision.SelectedIndex = 1;
    }

    protected void ucWageComponents_Changed(object sender, EventArgs e)
    {
        ViewState["COMPONENTID"] = string.Empty;
        Rebind();
    }
    protected void ddlUnion_TextChangedEvent(object sender, EventArgs e)
    {
        FetchCBARevision();
    }
    protected void ddlType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            lblUnion.Visible = true;
            ddlUnion.Visible = true;
            //if (ddlUnion.SelectedAddress != "")
            //{
                lblCBARevision.Visible = true;
                ddlRevision.Visible = true;
            //}

            lblWageComponents.Visible = false;
            ucWageComponents.Visible = false;
        }
        if (ddlType.SelectedValue == "2")
        {
            lblUnion.Visible = false;
            ddlUnion.Visible = false;
            lblCBARevision.Visible = false;
            ddlRevision.Visible = false;
            lblWageComponents.Visible = true;
            ucWageComponents.Visible = true;
        }
        if (ddlType.SelectedValue == "3")
        {
            lblUnion.Visible = false;
            ddlUnion.Visible = false;
            lblCBARevision.Visible = false;
            ddlRevision.Visible = false;
            lblWageComponents.Visible = false;
            ucWageComponents.Visible = false;
        }
    }
  
}