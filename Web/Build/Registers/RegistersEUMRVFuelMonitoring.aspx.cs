using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVFuelMonitoring : PhoenixBasePage
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
                toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVFuelMonitoring.aspx?"+Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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
                string[] alColumns = { "FLDMONITORINGMETHODNAME", "FLDAPPROACHUSEDNAME", "FLDVALUEPERCENTAGE" };
                string[] alCaptions = { "Fuel Monitoring", "Approach Used", "Value" };
                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringSearch(
                                                                                      sortexpression, sortdirection
                                                                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                     , gvSWS.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     ,General.GetNullableInteger(UcVessel.SelectedVessel));
                General.ShowExcel("Fuel Monitoring", dt, alColumns, alCaptions, null, string.Empty);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                string ddlMonitoringAdd = (((RadComboBox)e.Item.FindControl("ddlMonitoringAdd")).SelectedValue.ToString().Trim());
                string ddlApproachAdd = (((RadComboBox)e.Item.FindControl("ddlApproachAdd")).SelectedValue.ToString().Trim());
                string txtValueAdd = (((UserControlMaskNumber)e.Item.FindControl("txtValueAdd")).Text.ToString().Trim());
                if (!IsValidSeniority(ddlMonitoringAdd, ddlApproachAdd, txtValueAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                int vesselid = 0;
                if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                    vesselid = int.Parse(UcVessel.SelectedVessel);
                PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringInsert(vesselid, int.Parse(ddlMonitoringAdd), int.Parse(ddlApproachAdd), decimal.Parse(txtValueAdd));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lbldtKey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;
                PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringDelete(new Guid(lbldtKey));
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

    protected void gvSWS_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lbldtKeyEdit")).Text;
            string ddlMonitoringEdit = (((RadComboBox)e.Item.FindControl("ddlMonitoringEdit")).SelectedValue.ToString().Trim());
            string ddlApproachEdit = (((RadComboBox)e.Item.FindControl("ddlApproachEdit")).SelectedValue.ToString().Trim());
            string txtvalueEdit = (((UserControlMaskNumber)e.Item.FindControl("txtvalueEdit")).Text.ToString().Trim());
            if (!IsValidSeniority(ddlMonitoringEdit, ddlApproachEdit, txtvalueEdit))
            {

                ucError.Visible = true;
                return;
            }
            int vesselid = 0;
            if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                vesselid = int.Parse(UcVessel.SelectedVessel);
            PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringUpdate(vesselid, int.Parse(ddlMonitoringEdit), int.Parse(ddlApproachEdit), decimal.Parse(txtvalueEdit), new Guid(dtkey));

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

   

            RadComboBox ddlMonitoringEdit = (RadComboBox)e.Item.FindControl("ddlMonitoringEdit");
            RadComboBox ddlApproachEdit = (RadComboBox)e.Item.FindControl("ddlApproachEdit");


            if (ddlMonitoringEdit != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(3);
                ddlMonitoringEdit.DataSource = ds;
                ddlMonitoringEdit.DataTextField = "FLDNAME";
                ddlMonitoringEdit.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlMonitoringEdit.DataBind();
                ddlMonitoringEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlMonitoringEdit.SelectedIndex = 0;
                ddlMonitoringEdit.SelectedValue = drv["FLDMONITORINGMETHOD"].ToString();
            }

            if (ddlApproachEdit != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(4);
                ddlApproachEdit.DataSource = ds;
                ddlApproachEdit.DataTextField = "FLDNAME";
                ddlApproachEdit.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlApproachEdit.DataBind();
                ddlApproachEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlApproachEdit.SelectedIndex = 0;
                ddlApproachEdit.SelectedValue = drv["FLDAPPROACHUSED"].ToString();
            }


        }
        if (e.Item is GridFooterItem)
        {

            RadComboBox ddlMonitoringAdd = (RadComboBox)e.Item.FindControl("ddlMonitoringAdd");
            RadComboBox ddlApproachAdd = (RadComboBox)e.Item.FindControl("ddlApproachAdd");


            if (ddlMonitoringAdd != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(3);
                ddlMonitoringAdd.DataSource = ds;
                ddlMonitoringAdd.DataTextField = "FLDNAME";
                ddlMonitoringAdd.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlMonitoringAdd.DataBind();
                ddlMonitoringAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlMonitoringAdd.SelectedIndex = 0;
            }
            if (ddlApproachAdd != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(4);
                ddlApproachAdd.DataSource = ds;
                ddlApproachAdd.DataTextField = "FLDNAME";
                ddlApproachAdd.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlApproachAdd.DataBind();
                ddlApproachAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlApproachAdd.SelectedIndex = 0;
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

    private bool IsValidSeniority(string Fuelmonitoring, string Apporoched, string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Fuelmonitoring == "Dummy")
            ucError.ErrorMessage = "Fuel Monitoring is required.";
        if (Apporoched == "Dummy")
            ucError.ErrorMessage = "Approach used is required.";
        if (value == "")
            ucError.ErrorMessage = "Value is required.";
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
        string[] alColumns = { "FLDMONITORINGMETHODNAME", "FLDAPPROACHUSEDNAME", "FLDVALUEPERCENTAGE" };
        string[] alCaptions = { "Fuel Monitoring", "Approach Used", "Value" };
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringSearch(
                                                                                      sortexpression, sortdirection
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvSWS.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount
                                                                                    , General.GetNullableInteger(UcVessel.SelectedVessel));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Fuel Monitoring", alCaptions, alColumns, ds);
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
