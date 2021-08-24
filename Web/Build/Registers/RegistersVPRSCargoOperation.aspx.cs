using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVPRSCargoOperation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVPRSCargoOperation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCargoOperations')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCargoOperations.AccessRights = this.ViewState;
            MenuCargoOperations.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCargoOperations.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CargoOperations_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCargoOperations.SelectedIndexes.Clear();
        gvCargoOperations.EditIndexes.Clear();
        gvCargoOperations.DataSource = null;
        gvCargoOperations.Rebind();
    }

    private bool IsValidCargoOperation(string cargooperation, string operationtype, string vesseltype, string valuetype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((cargooperation == null) || (cargooperation == ""))
            ucError.ErrorMessage = "Cargo Operation is required.";

        if (General.GetNullableGuid(operationtype) == null)
            ucError.ErrorMessage = "Cargo Operation Type is required.";

        if (General.GetNullableGuid(vesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        if ((valuetype == null) || (valuetype == ""))
            ucError.ErrorMessage = "Value Type is required.";

        return (!ucError.IsError);
    }

    protected void gvCargoOperations_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string cargooperation = ((RadTextBox)e.Item.FindControl("txtCargoOperationAdd")).Text;
                string operationtype = ((UserControlVPRSCargoOperationType)e.Item.FindControl("ucCargoOperationTypeAdd")).SelectedCargoOperationType;
                string vesseltype = (((UserControlVPRSVesselType)e.Item.FindControl("ucVesselTypeAdd")).SelectedVesselType);
                string valuetype = (((RadDropDownList)e.Item.FindControl("ddlValueTypeAdd")).SelectedValue);
                string sortorder = (((UserControlMaskNumber)e.Item.FindControl("txtAddSortOrder")).Text);

                if (!IsValidCargoOperation(cargooperation, operationtype, vesseltype, valuetype))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVPRSCargoOperation.InsertCargoOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    cargooperation,
                    General.GetNullableGuid(vesseltype),
                    General.GetNullableGuid(operationtype), valuetype, General.GetNullableInteger(sortorder),
                    ((RadTextBox)e.Item.FindControl("txtShortcodeAdd")).Text);

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersVPRSCargoOperation.DeleteCargoOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoOperationsId")).Text));
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargoOperations_ItemDataBound(Object sender, GridItemEventArgs e)
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

                //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                //{
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                //}

                UserControlVPRSCargoOperationType ucCargoOperationTypeEdit = (UserControlVPRSCargoOperationType)e.Item.FindControl("ucCargoOperationTypeEdit");
                if (ucCargoOperationTypeEdit != null)
                    ucCargoOperationTypeEdit.SelectedCargoOperationType = drv["FLDCARGOOPERATIONTYPEID"].ToString();

                UserControlVPRSVesselType ucVesselTypeEdit = (UserControlVPRSVesselType)e.Item.FindControl("ucVesselTypeEdit");
                if (ucVesselTypeEdit != null)
                    ucVesselTypeEdit.SelectedVesselType = drv["FLDVESSELTYPE"].ToString();

                RadDropDownList ddlValueTypeEdit = (RadDropDownList)e.Item.FindControl("ddlValueTypeEdit");
                if (ddlValueTypeEdit != null)
                    ddlValueTypeEdit.SelectedValue = drv["FLDVALUETYPE"].ToString();
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCARGOOPERATION", "FLDSHORTCODE", "FLDCARGOOPERATIONTYPENAME", "FLDVESSELTYPENAME", "FLDVALUETYPENAME", "FLDSORTORDER" };
        string[] alCaptions = { "Cargo Operation", "Short Code", "Cargo Operations Type", "Vessel Type", "Value Type", "Sort Order" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVPRSCargoOperation.CargoOperationSearch(
            "",
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCargoOperations.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCargoOperations", "Cargo Operations", alCaptions, alColumns, ds);

        gvCargoOperations.DataSource = ds;
        gvCargoOperations.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCARGOOPERATION", "FLDSHORTCODE", "FLDCARGOOPERATIONTYPENAME", "FLDVESSELTYPENAME", "FLDVALUETYPENAME", "FLDSORTORDER" };
        string[] alCaptions = { "Cargo Operation", "Short Code", "Cargo Operations Type", "Vessel Type", "Value Type", "Sort Order" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersVPRSCargoOperation.CargoOperationSearch(
            "",
            sortexpression,
            sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"CargoOperations.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cargo Operations</h3></td>");
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

    protected void gvCargoOperations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCargoOperations.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargoOperations_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string cargooperation = ((RadTextBox)e.Item.FindControl("txtCargoOperationEdit")).Text;
            string operationtype = ((UserControlVPRSCargoOperationType)e.Item.FindControl("ucCargoOperationTypeEdit")).SelectedCargoOperationType;
            string vesseltype = ((UserControlVPRSVesselType)e.Item.FindControl("ucVesselTypeEdit")).SelectedVesselType;
            string valuetype = ((RadDropDownList)e.Item.FindControl("ddlValueTypeEdit")).SelectedValue;
            string sortorder = ((UserControlMaskNumber)e.Item.FindControl("txtEditSortOrder")).Text;

            if (!IsValidCargoOperation(cargooperation, operationtype, vesseltype, valuetype))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersVPRSCargoOperation.UpdateCargoOperation(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoOperationsIdEdit")).Text),
                cargooperation, General.GetNullableGuid(vesseltype), General.GetNullableGuid(operationtype), valuetype, General.GetNullableInteger(sortorder)
                , ((RadTextBox)e.Item.FindControl("txtShortcodeEdit")).Text);

            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargoOperations_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
}
