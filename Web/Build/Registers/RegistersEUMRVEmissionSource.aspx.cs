using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersEUMRVEmissionSource : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVEmissionSource.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmissionSource')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvEmissionSource.SelectedIndexes.Clear();
        gvEmissionSource.EditIndexes.Clear();
        gvEmissionSource.DataSource = null;
        gvEmissionSource.Rebind();
    }
    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
    private bool IsValidProcedure(string ComponentNumber, string ComponentName, string IdNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((ComponentNumber.Trim() == null) || (ComponentNumber.Trim() == ""))
            ucError.ErrorMessage = "Ref No is required.";
        if ((ComponentName.Trim() == null) || (ComponentName.Trim() == ""))
            ucError.ErrorMessage = "Emission source is required.";
        if ((IdNo.Trim() == null) || (IdNo.Trim() == ""))
            ucError.ErrorMessage = "ID No is required.";
        return (!ucError.IsError);
    }

    protected void gvEmissionSource_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string IdNo = (((UserControlMaskNumber)e.Item.FindControl("txtIDNo")).Text);
                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("OilTypeListAdd");
                string FuelId = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        FuelId += li.Value + ",";
                    }
                }
                string ComponentNumber = (((UserControlMaskNumber)e.Item.FindControl("txtParentComponentNumber")).Text);
                string ComponentName = (((RadTextBox)e.Item.FindControl("txtParentComponentName")).Text);
                if (!IsValidProcedure(ComponentNumber, ComponentName, IdNo))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVEmisionSourceInsert( General.GetNullableString(ComponentName),General.GetNullableString(ComponentNumber),
                    FuelId, General.GetNullableInteger(IdNo),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCompNumberAdd")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtPowerCaptionAdd")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtPowerUnitAdd")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSFOCLabelAdd")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSFOCUnitAdd")).Text)
                    );

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                String Emissionid = ((RadLabel)e.Item.FindControl("lblEmissionid")).Text;
                PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVEmisionSourceDelete(new Guid(Emissionid));
                Rebind();
            }
        }

        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmissionSource_ItemDataBound(Object sender, GridItemEventArgs e)
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

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                RadCheckBoxList chl = (RadCheckBoxList)e.Item.FindControl("OilTypeListEdit");
                if (chl != null)
                {
                    DataSet Ds = PhoenixRegistersEUMRVFuelCategories.EUMRVFuelCategories(); // For only fueloil
                    chl.DataSource = Ds;
                    chl.DataBindings.DataTextField = "FLDCODE";
                    chl.DataBindings.DataValueField = "FLDID";
                    chl.DataBind();

                    foreach (ButtonListItem li in chl.Items)
                    {
                        string[] slist = drv["FLDFUELTYPEID"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
                RadCheckBoxList chl = (RadCheckBoxList)e.Item.FindControl("OilTypeListAdd");
                if (chl != null)
                {
                    DataSet Ds = PhoenixRegistersEUMRVFuelCategories.EUMRVFuelCategories(); // For only fueloil
                    chl.DataSource = Ds;
                    chl.DataBindings.DataTextField = "FLDCODE";
                    chl.DataBindings.DataValueField = "FLDID";
                    chl.DataBind();
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
        string[] alColumns = { "FLDREFNUMBER", "FLDEMISSIONSOURCENAME", "FLDIDNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Fuel Types Used" };
        DataSet ds = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVEmisionSourceList();

        General.SetPrintOptions("gvEmissionSource", "Emission Sources Template", alCaptions, alColumns, ds);

        gvEmissionSource.DataSource = ds;
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDREFNUMBER", "FLDEMISSIONSOURCENAME", "FLDIDNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Fuel Types Used" };

        DataSet ds = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVEmisionSourceList();

        Response.AddHeader("Content-Disposition", "attachment; filename=\"EmissionSources.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Emission Sources Template</h3></td>");
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

    protected void gvEmissionSource_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string IdNo = (((UserControlMaskNumber)e.Item.FindControl("txtIDNoedit")).Text);
            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("OilTypeListEdit");
            string FuelId = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    FuelId += li.Value + ",";
                }
            }
            string ComponentNumber = (((UserControlMaskNumber)e.Item.FindControl("txtParentComponentNumberEdit")).Text);
            string ComponentName = (((RadTextBox)e.Item.FindControl("txtParentComponentNameEdit")).Text);
            String Emissionid = ((RadLabel)e.Item.FindControl("lblComponentNumberedit")).Text;
            if (!IsValidProcedure(ComponentNumber, ComponentName, IdNo))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVEmisionSourceUpdate(ComponentName, ComponentNumber, General.GetNullableString(FuelId),
                                                                                  General.GetNullableInteger(IdNo), new Guid(Emissionid),
                                                                                  General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCompNumber")).Text),
                                                                                  General.GetNullableString(((RadTextBox)e.Item.FindControl("txtPowerCaption")).Text),
                                                                                  General.GetNullableString(((RadTextBox)e.Item.FindControl("txtPowerUnit")).Text),
                                                                                  General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSFOCLabel")).Text),
                                                                                  General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSFOCUnit")).Text)
                                                                                  );

            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmissionSource_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        { 
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
