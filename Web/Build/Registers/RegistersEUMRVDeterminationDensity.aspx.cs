using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVDeterminationDensity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
            {
                MenuProcedureDetailList.Visible = false;
            }

           
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVDeterminationDensity.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSWS')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                UcVessel.bind();
                UcVessel.DataBind();

                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvSWS.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDFUELTYPENAME", "FLDFULEBUNKEREDNAME", "FLDFULETANKNAME" };
                string[] alCaptions = { "Fuel Type", "Fuel Bunkered", "Fuel Tank" };
                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensity(
                                                                                    sortexpression, sortdirection
                                                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                   , gvSWS.PageSize
                                                                                   , ref iRowCount
                                                                                   , ref iTotalPageCount
                                                                                   , General.GetNullableInteger(UcVessel.SelectedVessel));

                General.ShowExcel("Determination of Density", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
    protected void gvSWS_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string strFuelTypeAdd = (((RadTextBox)e.Item.FindControl("txtFuelTypeAdd")).Text.Trim());
                string strFuelBunkeredAdd = (((RadComboBox)e.Item.FindControl("ddlFuelBunkeredAdd")).SelectedValue.ToString().Trim());
                string strFuelTanksAdd = (((RadComboBox)e.Item.FindControl("ddlFuelTanksAdd")).SelectedValue.ToString().Trim());
                if (!IsValidSeniority(strFuelTypeAdd, strFuelBunkeredAdd, strFuelTanksAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                int vesselid = 0;
                if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                    vesselid = int.Parse(UcVessel.SelectedVessel);
                PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensityInsert(vesselid, strFuelTypeAdd, int.Parse(strFuelBunkeredAdd), int.Parse(strFuelTanksAdd));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;

                PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensityDelete(new Guid(dtkey));
                Rebind();
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

    protected void gvSWS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSWS_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            string dtkey = ((RadLabel)e.Item.FindControl("lbldtKeyEdit")).Text;
            string strFuelTypeEdit = (((RadTextBox)e.Item.FindControl("txtFuelTypeEdit")).Text.Trim());
            string strFuelBunkeredEdit = (((RadComboBox)e.Item.FindControl("ddlFuelBunkeredEdit")).SelectedValue.ToString().Trim());
            string strFuelTanksEdit = (((RadComboBox)e.Item.FindControl("ddlFuelTanksEdit")).SelectedValue.ToString().Trim());
            if (!IsValidSeniority(strFuelTypeEdit, strFuelBunkeredEdit, strFuelTanksEdit))
            {
                ucError.Visible = true;
                return;
            }
            int vesselid = 0;
            if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                vesselid = int.Parse(UcVessel.SelectedVessel);
            PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensityUpdate(vesselid,strFuelTypeEdit, int.Parse(strFuelBunkeredEdit), int.Parse(strFuelTanksEdit), new Guid(dtkey));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSWS_RowDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


            RadComboBox ddlFuelBunkeredEdit = (RadComboBox)e.Item.FindControl("ddlFuelBunkeredEdit");
            RadComboBox ddlFuelTanksEdit = (RadComboBox)e.Item.FindControl("ddlFuelTanksEdit");

            if (ddlFuelBunkeredEdit != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(1);
                ddlFuelBunkeredEdit.DataSource = ds;
                ddlFuelBunkeredEdit.DataTextField = "FLDNAME";
                ddlFuelBunkeredEdit.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlFuelBunkeredEdit.DataBind();
                ddlFuelBunkeredEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlFuelBunkeredEdit.SelectedIndex = 0; ddlFuelBunkeredEdit.SelectedValue = drv["FLDFUELBUNKERED"].ToString();
            }

            if (ddlFuelTanksEdit != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(2);
                ddlFuelTanksEdit.DataSource = ds;
                ddlFuelTanksEdit.DataTextField = "FLDNAME";
                ddlFuelTanksEdit.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlFuelTanksEdit.DataBind();
                ddlFuelTanksEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlFuelTanksEdit.SelectedIndex = 0; ddlFuelTanksEdit.SelectedValue = drv["FLDFULETANK"].ToString();
                ddlFuelTanksEdit.EnableTextSelection = true;
                
            }
        }
        if (e.Item is GridFooterItem)
        {

            RadComboBox ddlFuelBunkeredAdd = (RadComboBox)e.Item.FindControl("ddlFuelBunkeredAdd");
            RadComboBox ddlFuelTanksAdd = (RadComboBox)e.Item.FindControl("ddlFuelTanksAdd");
            
            if (ddlFuelBunkeredAdd != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(1);
                ddlFuelBunkeredAdd.DataSource = ds;
                ddlFuelBunkeredAdd.DataTextField = "FLDNAME";
                ddlFuelBunkeredAdd.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlFuelBunkeredAdd.DataBind();
                ddlFuelBunkeredAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlFuelBunkeredAdd.SelectedIndex = 0;
            }
            if (ddlFuelTanksAdd != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(2);
                ddlFuelTanksAdd.DataSource = ds;
                ddlFuelTanksAdd.DataTextField = "FLDNAME";
                ddlFuelTanksAdd.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlFuelTanksAdd.DataBind();
                ddlFuelTanksAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlFuelTanksAdd.SelectedIndex = 0;
            }
        }
    }
    protected void ddlSeniory_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        ViewState["CURRENTINDEX"] = 1;

        Rebind();
    }

    private bool IsValidSeniority(string FuelType, string fuelBukered, string fueltank)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (FuelType == "")
            ucError.ErrorMessage = "Fuel type/tank is required.";
        if (fuelBukered == "Dummy")
            ucError.ErrorMessage = "Method to determine actual density values of fuel bunkered is required.";
        if (fueltank == "Dummy")
            ucError.ErrorMessage = "Method to determine actual density values of fuel in tank is required.";
        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSWS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSWS.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFUELTYPENAME", "FLDFULEBUNKEREDNAME", "FLDFULETANKNAME" };
        string[] alCaptions = { "Fuel Type", "Fuel Bunkered", "Fuel Tank" };
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensity(
                                                                                     sortexpression, sortdirection
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvSWS.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount
                                                                                    , General.GetNullableInteger(UcVessel.SelectedVessel));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Determination of Density", alCaptions, alColumns, ds);

        gvSWS.DataSource = dt;
        gvSWS.VirtualItemCount = iRowCount;

        if (dt.Rows.Count > 0)
        {
            if (Request.QueryString["view"] != null)
            {
                gvSWS.Columns[4].Visible = false;

            }
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvSWS.SelectedIndexes.Clear();
        gvSWS.EditIndexes.Clear();
        gvSWS.DataSource = null;
        gvSWS.Rebind();
    }
}
