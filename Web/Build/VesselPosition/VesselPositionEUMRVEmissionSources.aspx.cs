using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVEmissionSources : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["vesselid"] != null)
            {
                UcVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                UcVessel.Enabled = false;
                tblsearch.Visible = false;
            }
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmissionSource')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (Request.QueryString["vesselid"] == null)
            {
                toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx", "Search", "<i class=\"fas fa-search\"></i>", "Find");
                toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx", "Filter", "<i class=\"fas fa-eraser\"></i>", "Clear");
            }
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["Lanchfrom"] != null)
                {
                    if (Request.QueryString["Lanchfrom"].ToString() == "0")
                        ViewState["Lanchfrom"] = Request.QueryString["Lanchfrom"].ToString();

                    if (Request.QueryString["Lanchfrom"].ToString() == "1")
                        ViewState["Lanchfrom"] = Request.QueryString["Lanchfrom"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())!=null && PhoenixSecurityContext.CurrentSecurityContext.VesselID>0)
                {
                    UcVessel.bind();
                    UcVessel.DataBind();
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    tblsearch.Visible = false;
                }
                bindOilType();

                gvEmissionSource.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (ViewState["Lanchfrom"].ToString() == "0")
            {
                toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else if (ViewState["Lanchfrom"].ToString() == "0")
            {
                toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
            {
                MenuProcedureDetailList.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    void bindOilType()
    {
        DataSet Ds = PhoenixRegistersOilType.ListOilType(1, 1); // For only fueloil
        ddlECAOilType.DataSource = Ds;
        ddlECAOilType.DataBind();
        ddlECAOilType.Items.Insert(0, "--Select--");
    }
    protected void Location_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtParentComponentNumberfilter.Text = string.Empty;
                txtParentComponentNamefilter.Text = string.Empty;
                ddlECAOilType.SelectedIndex = 0;
                txtPerformence.Text = string.Empty;
                txtYearOfInstallation.Text = string.Empty;
                txtIdentificationNofilter.Text = string.Empty;

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

    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (ViewState["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }

    private bool IsValidProcedure(int? vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (vessel == null)
            ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }

    protected void gvEmissionSource_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMREmissionSource.DeleteEUMRVEmissionSource(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblEmissionid")).Text));
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


                UserControlMaskNumber txtSFOC = (UserControlMaskNumber)e.Item.FindControl("txtSFOC");
                UserControlMaskNumber txtPower = (UserControlMaskNumber)e.Item.FindControl("txtPower");

                if (drv["FLDCOMPONENTNAME"].ToString().ToUpper() == "MAIN ENGINE" && txtSFOC != null)
                {
                    txtSFOC.Enabled = false;
                    //txtSFOC.CssClass = "readonlytextbox";
                }

                if (drv["FLDCOMPONENTNAME"].ToString().ToUpper() == "MAIN ENGINE" && txtPower != null)
                {
                    txtPower.Enabled = false;
                    //txtPower.CssClass = "readonlytextbox";
                }


                if (del != null)
                {
                    if (drv["FLDEMISSIONANDFUELTYPEID"].ToString() == "")
                        del.Visible = false;
                }
                RadCheckBox chkappyn = (RadCheckBox)e.Item.FindControl("chkappyn");
                if (chkappyn != null)
                {
                    if (drv["FLDAPPLICABLEYN"].ToString() == "1")
                        chkappyn.Checked = true;
                }
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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvEmissionSource_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Performence = (((UserControlMaskNumber)e.Item.FindControl("txtPower")).Text);
            string YearOfInstallation = (((UserControlMaskNumber)e.Item.FindControl("txtYrInst")).Text);
            string IdentificationNo = (((UserControlMaskNumber)e.Item.FindControl("txtSFOC")).Text);

            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("OilTypeListEdit");
            string FuelId = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    FuelId += li.Value + ",";
                }
            }

            string EmissionId = ((RadLabel)e.Item.FindControl("lblEmissionid")).Text;
            string ComponentId = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            string ComponentNumber = (((RadLabel)e.Item.FindControl("lblComponentNumber")).Text);
            string ComponentName = (((RadLabel)e.Item.FindControl("lblComponent")).Text);
            string IdNo = (((RadLabel)e.Item.FindControl("lblIDNo")).Text);

            string PowerHeader= (((RadLabel)e.Item.FindControl("lblPower")).Text);
            string PowerUnit = (((RadLabel)e.Item.FindControl("lblPowerUnit")).Text);
            string SFOCHeader = (((RadLabel)e.Item.FindControl("lblSFOC")).Text);
            string SFOCUnit = (((RadLabel)e.Item.FindControl("lblSFOCUnit")).Text);

            int? vesselid;
            if (General.GetNullableInteger(UcVessel.SelectedVessel.ToString()) != null)
                vesselid = General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
            else
                vesselid = null;

            if (!IsValidProcedure(vesselid))
            {
                ucError.Visible = true;
                return;
            }
            RadCheckBox chkappyn = (RadCheckBox)e.Item.FindControl("chkappyn");
            if (EmissionId != "")
            {
                PhoenixVesselPositionEUMREmissionSource.UpdateEUMRVEmissionSource(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(EmissionId),
                       new Guid(ComponentId),
                       General.GetNullableString(FuelId),
                       General.GetNullableString(Performence),
                       General.GetNullableString(YearOfInstallation),
                       General.GetNullableString(IdentificationNo),
                       General.GetNullableString(ComponentNumber),
                       General.GetNullableString(ComponentName),
                       General.GetNullableInteger(IdNo), 
                       General.GetNullableInteger(chkappyn.Checked == true ? "1" : null),
                       General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSerialNo")).Text),
                       General.GetNullableString(PowerHeader),
                       General.GetNullableString(PowerUnit),
                       General.GetNullableString(SFOCHeader),
                       General.GetNullableString(SFOCUnit)
                       );
            }
            else
            {
                PhoenixVesselPositionEUMREmissionSource.InsertEUMRVEmissionSource(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ComponentId),
                    General.GetNullableString(FuelId),
                    General.GetNullableString(Performence),
                    General.GetNullableString(YearOfInstallation),
                    General.GetNullableString(IdentificationNo),
                    General.GetNullableString(ComponentNumber),
                    General.GetNullableString(ComponentName),
                    General.GetNullableInteger(IdNo),
                    General.GetNullableInteger(Request.QueryString["vesselid"] == null ? vesselid.ToString() : Request.QueryString["vesselid"].ToString()),
                    General.GetNullableInteger(chkappyn.Checked == true ? "1" : null),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSerialNo")).Text),
                    General.GetNullableString(PowerHeader),
                    General.GetNullableString(PowerUnit),
                    General.GetNullableString(SFOCHeader),
                    General.GetNullableString(SFOCUnit)
                     );
            }

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void ClearValue(object sender, EventArgs e)
    //{
    //    ((TextBox)gvEmissionSource.FooterRow.FindControl("txtParentComponentNumber")).Text = null;
    //    ((TextBox)gvEmissionSource.FooterRow.FindControl("txtParentComponentName")).Text = null;
    //    ((TextBox)gvEmissionSource.FooterRow.FindControl("txtParentComponentID")).Text = null;
    //}
    protected void ClearFilterValue(object sender, EventArgs e)
    {
        txtParentComponentNumberfilter.Text = null;
        txtParentComponentNamefilter.Text = null;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDIDNO", "FLDPEFORMENCEPOWER", "FLDYEAROFINSTALLATION", "FLDIDENTIFICATIONNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Power", "Year Of Inst", "SFOC", "Fuel Types Used" };

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
        if (General.GetNullableInteger(UcVessel.SelectedVessel.ToString()) != null)
            vesselid = General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
        else
            vesselid = null;

        DataSet ds = PhoenixVesselPositionEUMREmissionSource.EUMRVEmissionSourceSearch(
             General.GetNullableString(txtPerformence.Text),
             null,
             General.GetNullableGuid(ddlECAOilType.SelectedValue.ToString()),
             General.GetNullableString(txtYearOfInstallation.Text),
             General.GetNullableString(txtIdentificationNofilter.Text),
             sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvEmissionSource.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableString(txtParentComponentNumberfilter.Text),
            General.GetNullableString(txtParentComponentNamefilter.Text),
            vesselid);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"EmissionSources.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Emission Sources</h3></td>");
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

    protected void gvEmissionSource_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmissionSource.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDIDNO", "FLDPEFORMENCEPOWER", "FLDYEAROFINSTALLATION", "FLDIDENTIFICATIONNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Power", "Year Of Inst", "SFOC", "Fuel Types Used" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int? vesselid;
        if (General.GetNullableInteger(UcVessel.SelectedVessel.ToString()) != null)
            vesselid = General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
        else
            vesselid = null;

        DataSet ds = PhoenixVesselPositionEUMREmissionSource.EUMRVEmissionSourceSearch(
             General.GetNullableString(txtPerformence.Text),
             null,
             General.GetNullableGuid(ddlECAOilType.SelectedValue.ToString()),
             General.GetNullableString(txtYearOfInstallation.Text),
             General.GetNullableString(txtIdentificationNofilter.Text),
             sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvEmissionSource.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableString(txtParentComponentNumberfilter.Text),
            General.GetNullableString(txtParentComponentNamefilter.Text),
            General.GetNullableInteger(Request.QueryString["vesselid"] == null ? vesselid.ToString() : Request.QueryString["vesselid"].ToString()));

        General.SetPrintOptions("gvEmissionSource", "Emission Sources", alCaptions, alColumns, ds);
        gvEmissionSource.DataSource = ds;
        gvEmissionSource.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (Request.QueryString["view"] != null)
            {
                gvEmissionSource.Columns[9].Visible = false;
                // gvEmissionSource.FooterRow.Visible = false;
            }
        }


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvEmissionSource.SelectedIndexes.Clear();
        gvEmissionSource.EditIndexes.Clear();
        gvEmissionSource.DataSource = null;
        gvEmissionSource.Rebind();
    }
}
