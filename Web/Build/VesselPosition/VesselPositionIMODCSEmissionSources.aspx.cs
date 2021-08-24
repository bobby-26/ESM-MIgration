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

public partial class VesselPositionIMODCSEmissionSources : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK",ToolBarDirection.Right);
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();
            MenuProcedureDetailList.Visible = true;

            if (Request.QueryString["vesselid"] != null)
            {
                UcVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                UcVessel.Enabled = false;
                tblsearch.Visible = false;
            }
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSEmissionSources.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmissionSource')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                populatedata();

                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.bind();
                    UcVessel.DataBind();
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    tblsearch.Visible = false;
                }


                bindOilType();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void populatedata()
    {
        try
        {
            PhoenixVesselPositionIMODCSEmissionSource.IMODCSEmissionSourceDataPopulatuon(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
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
        if (CommandName.ToUpper().Equals("BACK"))
        {
           Response.Redirect("../VesselPosition/VesselPositionIMODCSConfiguration.aspx?");
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
                PhoenixVesselPositionIMODCSEmissionSource.DeleteIMODCSEmissionSource(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblEmissionid")).Text));
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
            string EmissionSourceId = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            string ComponentId = ((RadLabel)e.Item.FindControl("lblpmscomponentId")).Text;
            string ComponentNumber = (((RadLabel)e.Item.FindControl("lblComponentNumber")).Text);
            string ComponentName = (((RadLabel)e.Item.FindControl("lblComponentedit")).Text);
            string IdNo = (((RadLabel)e.Item.FindControl("lblIDNo")).Text);
            string EngineType = (((RadTextBox)e.Item.FindControl("txtTypeedit")).Text);
            string EngineModel = (((RadTextBox)e.Item.FindControl("txtmodeledit")).Text);

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
                PhoenixVesselPositionIMODCSEmissionSource.UpdateIMODCSEmissionSource(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(EmissionId),
                       new Guid(EmissionSourceId),
                       General.GetNullableGuid(ComponentId),
                       General.GetNullableString(FuelId),
                       General.GetNullableString(Performence),
                       General.GetNullableString(YearOfInstallation),
                       General.GetNullableString(IdentificationNo),
                       General.GetNullableString(ComponentNumber),
                       General.GetNullableString(ComponentName),
                       General.GetNullableInteger(IdNo),
                       General.GetNullableInteger(chkappyn.Checked == true ? "1" : null),
                       General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSerialNo")).Text),
                       General.GetNullableString(EngineType),
                       General.GetNullableString(EngineModel)                 
                       );
            }
            else
            {
                PhoenixVesselPositionIMODCSEmissionSource.InsertIMODCSEmissionSource(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(EmissionSourceId),
                    General.GetNullableGuid(ComponentId),
                    General.GetNullableString(FuelId),
                    General.GetNullableString(Performence),
                    General.GetNullableString(YearOfInstallation),
                    General.GetNullableString(IdentificationNo),
                    General.GetNullableString(ComponentNumber),
                    General.GetNullableString(ComponentName),
                    General.GetNullableInteger(IdNo),
                     General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                     General.GetNullableInteger(chkappyn.Checked == true ? "1" : null),
                     General.GetNullableString(((RadTextBox)e.Item.FindControl("txtSerialNo")).Text),
                     General.GetNullableString(EngineType),
                     General.GetNullableString(EngineModel)
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
    protected void ShowExcel()
    {

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDIDNO", "FLDPEFORMENCEPOWER", "FLDYEAROFINSTALLATION", "FLDIDENTIFICATIONNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Power", "Year Of Inst", "SFOC", "Fuel Types Used" };

        DataSet ds = PhoenixVesselPositionIMODCSEmissionSource.IMODCSEmissionSourceSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=\"EmissionSources.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Ship engines and other fuel oil consumers and fuel oil types used</h3></td>");
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
    protected void gvEmissionSource_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDIDNO", "FLDPEFORMENCEPOWER", "FLDYEAROFINSTALLATION", "FLDIDENTIFICATIONNO", "FLDOILTYPENAME" };
        string[] alCaptions = { "Ref No.", "Emission Source", "ID No", "Power", "Year Of Inst", "SFOC", "Fuel Types Used" };

        DataSet ds = PhoenixVesselPositionIMODCSEmissionSource.IMODCSEmissionSourceSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        General.SetPrintOptions("gvEmissionSource", "Ship engines and other fuel oil consumers and fuel oil types used", alCaptions, alColumns, ds);

        gvEmissionSource.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            
            if (Request.QueryString["view"] != null)
            {
                gvEmissionSource.Columns[9].Visible = false;
                // gvEmissionSource.FooterRow.Visible = false;
            }
        }
  
    }
    protected void Rebind()
    {
        gvEmissionSource.SelectedIndexes.Clear();
        gvEmissionSource.EditIndexes.Clear();
        gvEmissionSource.DataSource = null;
        gvEmissionSource.Rebind();
    }
}
