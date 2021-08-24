using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Web;

public partial class VesselPositionEUMRVShipSpecificProcedure : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvProcedure.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                //Page.ClientScript.RegisterForEventValidation(gvProcedure.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvProcedure')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx", "Search", "search.png", "Find");
            toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx", "Filter", "clear-filter.png", "Clear");

            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                //  PhoenixToolbar toolbarmain = new PhoenixToolbar();
                ////  toolbarmain.AddButton("Copy", "COPY");
                //  MenuProcedureDetailList.AccessRights = this.ViewState;
                //  MenuProcedureDetailList.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;


                if (Session["EUMRVVESSEL"] != null)
                    UcVessel.SelectedVessel = Session["EUMRVVESSEL"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvProcedure.EditIndex = -1;
                gvProcedure.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                txtprocedurefilter.Text = "";
                txtCode.Text = "";
                gvProcedure.EditIndex = -1;
                gvProcedure.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("COPY"))
        {
            int? Fromvesselid, Tovesselid;
            Fromvesselid = General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
            Tovesselid = General.GetNullableInteger(UcToVessel.SelectedVessel.ToString());
            if (!IsValiedVessel(Fromvesselid, Tovesselid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixVesselPositionEUMRV.CopyEUMRVProcedure(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Fromvesselid, Tovesselid);
            ucStatus.Text = "Procedure coppied successfully.";
            BindData();
            SetPageNavigator();
        }
    }
    private bool IsValiedVessel(int? Fromvesselid, int? Tovesselid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Fromvesselid == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (Tovesselid == null)
            ucError.ErrorMessage = "To vessel is required.";

        return (!ucError.IsError);
    }

    private bool IsValidProcedure(string Procedure, string ProcedureCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((ProcedureCode.Trim() == null) || (ProcedureCode.Trim() == ""))
            ucError.ErrorMessage = "code is required.";

        if ((Procedure.Trim() == null) || (Procedure.Trim() == ""))
            ucError.ErrorMessage = "Decription is required.";



        return (!ucError.IsError);
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        Session["EUMRVVESSEL"] = General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
        BindData();
    }
    protected void gvProcedure_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Procedure = (((TextBox)_gridView.FooterRow.FindControl("txtProcedureAdd")).Text);
                string ProcedureCode = (((TextBox)_gridView.FooterRow.FindControl("txtProcedureCodeAdd")).Text);
                string Guidance = (((TextBox)_gridView.FooterRow.FindControl("txtGuidanceAdd")).Text);

                int? vesselid;
                if (Session["EUMRVVESSEL"] == null)
                    vesselid = -1;
                else
                    vesselid = General.GetNullableInteger(Session["EUMRVVESSEL"].ToString());

                if (!IsValidProcedure(Procedure, ProcedureCode))
                {
                    ucError.Visible = true;
                    return;
                }
                if (vesselid == null || vesselid == -1)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionEUMRV.InsertEUMRVProcedure(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Procedure, ProcedureCode, General.GetNullableString(Guidance), vesselid);

                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMRV.DeleteEUMRVProcedure(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureIdadd")).Text));
            }

            if (e.CommandName.ToUpper() == "NAV")
            {
                Guid? ProcedureID = General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureIdadd")).Text);
                string Table = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureCode")).Text;
                string DetailId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureDetailID")).Text;
                if (Table.ToUpper() == "F.1") { Response.Redirect("../Registers/RegistersEUMRVdefinitionandabbreviation.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim()); }
                if (Table.ToUpper() == "B.5" || Table.ToUpper() == "C.2.2" || Table.ToUpper() == "C.2.5" || Table.ToUpper() == "C.2.8" || Table.ToUpper() == "E.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetail.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetail.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.2.3")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC23.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC23.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.2.10" || Table.ToUpper() == "C.2.11" || Table.ToUpper() == "C.2.12" || Table.ToUpper() == "C.6.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6" || Table.ToUpper() == "C.5.2" || Table.ToUpper() == "C.6.2" || Table.ToUpper() == "C.4.2")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC210.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC210.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.3" || Table.ToUpper() == "C.4.1")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC3.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC3.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.5.1")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC5.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC5.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "D.1")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD1.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD1.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "D.2" || Table.ToUpper() == "D.3" || Table.ToUpper() == "D.4")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD2.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD2.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "E.2")
                {
                    if (General.GetNullableGuid(DetailId) == null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailE2.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                    else
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailE2.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.1")
                    Response.Redirect("../Registers/RegistersEUMRVExemptionArticle.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.7")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoring.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.6")
                    Response.Redirect("../Registers/RegistersEUMRVDeterminationDensity.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.4")
                    Response.Redirect("../Registers/RegistersEUMRVMesurementInstrument.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.1")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoringConsumption.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "B.3")
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
            }
            if (e.CommandName.ToUpper() == "VIEW")
            {
                Guid? ProcedureID = General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureIdadd")).Text);
                string Table = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureCode")).Text;
                string DetailId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureDetailID")).Text;
                if (Table.ToUpper() == "F.1") { Response.Redirect("../Registers/RegistersEUMRVdefinitionandabbreviation.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim()); }
                if (Table.ToUpper() == "B.5" || Table.ToUpper() == "C.2.2" || Table.ToUpper() == "C.2.5" || Table.ToUpper() == "C.2.8" || Table.ToUpper() == "E.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6" )
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailView.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "B.3")
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.1")
                    Response.Redirect("../Registers/RegistersEUMRVExemptionArticle.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.2.7")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoring.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.2.6")
                    Response.Redirect("../Registers/RegistersEUMRVDeterminationDensity.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.2.4")
                    Response.Redirect("../Registers/RegistersEUMRVMesurementInstrument.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.2.1")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoringConsumption.aspx?vesselid=" + UcVessel.SelectedVessel + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&view=1");
                if (Table.ToUpper() == "C.2.3")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewC23.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "C.2.10" || Table.ToUpper() == "C.2.11" || Table.ToUpper() == "C.2.12" || Table.ToUpper() == "C.6.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6" || Table.ToUpper() == "C.5.2" || Table.ToUpper() == "C.6.2" || Table.ToUpper() == "C.4.2")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewC210.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "C.3" || Table.ToUpper() == "C.4.1")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewC3.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "C.5.1")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewC5.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "D.1")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewD1.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "D.2" || Table.ToUpper() == "D.3" || Table.ToUpper() == "D.4")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewD2.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
                if (Table.ToUpper() == "E.2")
                {
                    if (General.GetNullableGuid(DetailId) != null)
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailViewE2.aspx?DetailID=" + General.GetNullableGuid(DetailId) + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvProcedure_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvProcedure_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvProcedure, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvProcedure_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string Proceduer = (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtProcedureEdit")).Text);
            string ProcedureCode = (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtProcedureCodeEdit")).Text);
            string Guidance = (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGuidanceEdit")).Text);

            if (!IsValidProcedure(Proceduer, ProcedureCode))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselPositionEUMRV.UpdateEUMRVProcedure(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                Proceduer,
                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureId")).Text),
                ProcedureCode
                , General.GetNullableString(Guidance)
                );

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();

            if (Request.QueryString["Vesselid"] != null)
            {
                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }



    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDUREGUIDANCE" };
        string[] alCaptions = { "Table", "Procedure(Guidance)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int? vesselid;
        if (Session["EUMRVVESSEL"] == null)
            vesselid = -1;
        else
            vesselid = General.GetNullableInteger(Session["EUMRVVESSEL"].ToString());

        DataSet ds = PhoenixVesselPositionEUMRV.EUMRVProcedureSearch(
            txtprocedurefilter.Text.Trim(), txtCode.Text.Trim(), sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            vesselid);

        General.SetPrintOptions("gvProcedure", "Ship Specific Procedure", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvProcedure.DataSource = ds;
            gvProcedure.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvProcedure);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDUREGUIDANCE" };
        string[] alCaptions = { "Table", "Procedure(Guidance)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? vesselid;
        if (Session["EUMRVVESSEL"] == null)
            vesselid = -1;
        else
            vesselid = General.GetNullableInteger(Session["EUMRVVESSEL"].ToString());

        DataSet ds = PhoenixVesselPositionEUMRV.EUMRVProcedureSearch(
             txtprocedurefilter.Text.Trim(), txtCode.Text.Trim(), sortexpression,
             sortdirection,
             (int)ViewState["PAGENUMBER"],
             General.ShowRecords(null),
             ref iRowCount,
             ref iTotalPageCount, vesselid);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ShipSpecificProcedure.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Ship Specific Procedure</h3></td>");
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
    protected void gvProcedure_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvProcedure.EditIndex = -1;
        gvProcedure.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
