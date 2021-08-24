using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class Registers_RegistersContractCBARevisionNationalityMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCBARevisionNationalityMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersContractCBARevisionNationalityMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCBARevisionNationalityMapping.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegistersContractCBARevisionNationalityMapping.AccessRights = this.ViewState;
            MenuRegistersContractCBARevisionNationalityMapping.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");


            toolbar = new PhoenixToolbar();

            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);

            MenuNationalityMapping.MenuList = toolbar.Show();
            //MenuNationalityMapping.ClearSelection();

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRegistersContractCBARevisionNationalityMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        Rebind();
    }

    protected void ddlUnion_Changed(object sender, EventArgs e)
    {
        try
        {
            Rebind();
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

        string[] alColumns = { "FLDCBANAME", "FLDNATIONALITYNAME" };
        string[] alCaptions = { "Union", "Nationality" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersContractCBARevisionMapping.SearchCBANationalityMapping(General.GetNullableInteger(ddlUnion.SelectedAddress)
                                                                                       //int.Parse((ddlUnion.SelectedAddress == null || ddlUnion.SelectedAddress == "" || ddlUnion.SelectedAddress == "Dummy") ? "0" : ddlUnion.SelectedAddress)
                                                                                       //   , sortexpression
                                                                                       //   , 1
                                                                                       , 1
                                                                                       , iRowCount
                                                                                       , ref iRowCount
                                                                                       , ref iTotalPageCount);


        
     
            General.ShowExcel("Nationality Mapping", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }


    protected void MenuRegistersContractCBARevisionNationalityMapping_TabStripCommand(object sender, EventArgs e)
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
                ddlUnion.SelectedAddress = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
               
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('Nationality Mapping','','" + Session["sitepath"] + "/Registers/RegistersContractCBANationalityMappingAdd.aspx?UnionID=" + ddlUnion.SelectedAddress + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRegistersContractCBARevisionNationalityMapping.SelectedIndexes.Clear();
        gvRegistersContractCBARevisionNationalityMapping.EditIndexes.Clear();
        gvRegistersContractCBARevisionNationalityMapping.DataSource = null;
        gvRegistersContractCBARevisionNationalityMapping.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCBANAME", "FLDNATIONALITYNAME" };
            string[] alCaptions = { "Union", "Nationality" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            DataTable dt = PhoenixRegistersContractCBARevisionMapping.SearchCBANationalityMapping(General.GetNullableInteger(ddlUnion.SelectedAddress)
                                                                                    //int.Parse((ddlUnion.SelectedAddress == null || ddlUnion.SelectedAddress == "" || ddlUnion.SelectedAddress == "Dummy") ? "0" : ddlUnion.SelectedAddress)
                                                                                    //  , sortexpression
                                                                                    // , 1
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvRegistersContractCBARevisionNationalityMapping.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvRegistersContractCBARevisionNationalityMapping", "CBA Nationality Mapping", alCaptions, alColumns, ds);
            
            gvRegistersContractCBARevisionNationalityMapping.DataSource = dt;
            gvRegistersContractCBARevisionNationalityMapping.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvRegistersContractCBARevisionNationalityMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("INACTIVE"))
            {

                ViewState["MAPPINGID"] = ((RadLabel)e.Item.FindControl("lblMappingId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to make Inactive?", "DeleteRecord", 320, 150, null, "Inactive");
                return;

          
            }


            else if (e.CommandName == "Page")
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

    protected void gvRegistersContractCBARevisionNationalityMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
          
            RadLabel lblMappingId = (RadLabel)e.Item.FindControl("lblMappingId");
            RadLabel lblUnion = (RadLabel)e.Item.FindControl("lblUnion");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('Nationality Mapping', '','" + Session["sitepath"] + "/Registers/RegistersContractCBANationalityMappingAdd.aspx?MappingID=" + lblMappingId.Text +"'); return false;");
            }


        }
    }

    protected void gvRegistersContractCBARevisionNationalityMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersContractCBARevisionNationalityMapping.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuNationalityMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Registers/RegistersContractCBARevision.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InactiveNationalityMapping(Guid MappingId)
    {
        PhoenixRegistersContractCBARevisionMapping.InactiveNationalityMapping(MappingId);
    }


    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            InactiveNationalityMapping(new Guid(ViewState["MAPPINGID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}