using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVMesurementInstrument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
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
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVMesurementInstrument.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSWS')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");


            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.bind();
                    ddlVessel.DataBind();
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
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
                string[] alColumns = { "FLDMEASUREMENTEQUIPMENT", "FLDEMISSIONSOURCENAME", "FLDTECHNICALDESC" };
                string[] alCaptions = { "Measurement Equipment", "Emission Source", "Technical Description" };
                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentSearch(
                                                                                      sortexpression, sortdirection
                                                                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                     , gvSWS.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , General.GetNullableInteger(ddlVessel.SelectedVessel));
                General.ShowExcel("Measurement Instrument", dt, alColumns, alCaptions, null, string.Empty);
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
                string txtEmissionAdd = (((RadTextBox)e.Item.FindControl("txtEmissionAdd")).Text.Trim());
                string Mesurementequp = (((RadTextBox)e.Item.FindControl("txtMesurementAdd")).Text.ToString().Trim());
                string TechDesc = (((RadTextBox)e.Item.FindControl("txtTechDescAdd")).Text.ToString().Trim());

                if (!IsValidSeniority(Mesurementequp, txtEmissionAdd, TechDesc))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentInsert(General.GetNullableInteger(ddlVessel.SelectedVessel) != null ? int.Parse(ddlVessel.SelectedVessel) : 0
                                                                                            , Mesurementequp, txtEmissionAdd, TechDesc);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lbldtKey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;
                PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentDelete(new Guid(lbldtKey));
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
            string Mesurementequp = (((RadTextBox)e.Item.FindControl("txtMesurementEdit")).Text.ToString().Trim());
            string TechDesc = (((RadTextBox)e.Item.FindControl("txtTechDescEdit")).Text.ToString().Trim());
            string txtEmissionEdit = (((RadTextBox)e.Item.FindControl("txtEmissionEdit")).Text.Trim());
            if (!IsValidSeniority(Mesurementequp, txtEmissionEdit, TechDesc))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentUpdate(General.GetNullableInteger(ddlVessel.SelectedVessel) != null ? int.Parse(ddlVessel.SelectedVessel) : 0
                                                                                        , Mesurementequp, txtEmissionEdit, TechDesc, new Guid(dtkey));
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

    private bool IsValidSeniority(String Mesurementequp, String Emission, String TechDesc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Mesurementequp == "")
            ucError.ErrorMessage = "Measurement Equipment is required.";
        if (Emission == "Dummy")
            ucError.ErrorMessage = "Emission Source is required.";
        if (TechDesc == "")
            ucError.ErrorMessage = "Technical Description is required.";
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
        string[] alColumns = { "FLDMEASUREMENTEQUIPMENT", "FLDEMISSIONSOURCENAME", "FLDTECHNICALDESC" };
        string[] alCaptions = { "Measurement Equipment", "Emission Source", "Technical Description" };
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentSearch(
                                                                                     sortexpression, sortdirection
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvSWS.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount
                                                                                    , General.GetNullableInteger(ddlVessel.SelectedVessel));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Measurement Instrument", alCaptions, alColumns, ds);
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
