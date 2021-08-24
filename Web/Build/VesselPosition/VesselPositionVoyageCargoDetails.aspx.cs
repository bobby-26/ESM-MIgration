using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;
using System.Web.UI;

public partial class VesselPositionVoyageCargoDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["VESSELID"] = "";

            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            else
                ViewState["VESSELID"] = "";


            if (Request.QueryString["voyageid"] != null)
                ViewState["VOYAGEID"] = Request.QueryString["voyageid"].ToString();
            else
                ViewState["VOYAGEID"] = "";

            if (Request.QueryString["portcallid"] != null)
                ViewState["PORTCALLID"] = Request.QueryString["portcallid"].ToString();
            else
                ViewState["PORTCALLID"] = "";
            if (Request.QueryString["PortName"] != null)
                ucTitle.Text = "<b>Port Name : " + Request.QueryString["PortName"].ToString()+"</b>";
        }
    }
    protected void gvVoyagePort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionVoyageLoadDetails.ListCargoDetails(
           General.GetNullableInteger(ViewState["VESSELID"].ToString()),
           General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
           General.GetNullableGuid(ViewState["PORTCALLID"].ToString()));
            gvVoyagePort.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvVoyagePort.SelectedIndexes.Clear();
        gvVoyagePort.EditIndexes.Clear();
        gvVoyagePort.DataSource = null;
        gvVoyagePort.Rebind();
    }

    protected void gvVoyagePort_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvVoyagePort_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidVoyageCargo(
                    ((UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargo")).SelectedCargoType,
                    ((UserControlMaskNumber)e.Item.FindControl("txtQtyLoadedAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtQtyDischargedAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionVoyageLoadDetails.InsertCargoDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableGuid(ViewState["PORTCALLID"].ToString()),
                    General.GetNullableGuid(((UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargo")).SelectedCargoType),
                    null,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtQtyLoadedAdd")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtQtyDischargedAdd")).Text),
                    null,
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtBerthAdd")).Text));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidVoyageCargo(
                                ((UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargoEdit")).SelectedCargoType,
                                ((UserControlMaskNumber)e.Item.FindControl("txtQtyLoadedEdit")).Text,
                                ((UserControlMaskNumber)e.Item.FindControl("txtQtyDischargedEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionVoyageLoadDetails.UpdateCargoDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoyageCargoIdEdit")).Text),
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableGuid(ViewState["PORTCALLID"].ToString()),
                    General.GetNullableGuid(((UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargoEdit")).SelectedCargoType),
                    null,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtQtyLoadedEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtQtyDischargedEdit")).Text),
                    null,
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtBerthEdit")).Text));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionVoyageLoadDetails.DeleteCargoDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoyageCargoId")).Text));

                
            }
            Rebind();
            String script = "javascript:fnReloadList('codehelp1',null,'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoyagePort_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                UserControlSeaport ucPortEdit = (UserControlSeaport)e.Item.FindControl("ucPortEdit");
                if (ucPortEdit != null) ucPortEdit.SelectedSeaport = drv["FLDPORT"].ToString();

                UserControlCargoTypeMappedVesselType ucCargo = (UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargoEdit");
                if (ucCargo != null)
                {
                    ucCargo.VesselId = int.Parse(ViewState["VESSELID"].ToString());
                    ucCargo.CargoTypeShowYesNo = "1";

                    ucCargo.bind();
                }

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPort");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblPortName");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                UserControlCargoTypeMappedVesselType ucCargo = (UserControlCargoTypeMappedVesselType)e.Item.FindControl("ucCargo");
                if (ucCargo != null)
                {
                    ucCargo.VesselId = int.Parse(ViewState["VESSELID"].ToString());
                    ucCargo.CargoTypeShowYesNo = "1";
                    ucCargo.bind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  
    private bool IsValidVoyageCargo(string cargo, string loadedqty, string dischargedqty)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(cargo) == null)
            ucError.ErrorMessage = "Cargo is required.";

        if (General.GetNullableDecimal(loadedqty) == null && General.GetNullableDecimal(dischargedqty) == null)
            ucError.ErrorMessage = "Either 'Qty Loaded' or 'Qty Discharged' is required.";

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null || General.GetNullableInteger(ViewState["VESSELID"].ToString()) == 0)
            ucError.ErrorMessage = "Please select a vessel.";

        return (!ucError.IsError);
    }
}
