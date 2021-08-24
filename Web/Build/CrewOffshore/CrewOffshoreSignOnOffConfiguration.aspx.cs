using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreSignOnOffConfiguration : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvSignOnOffConfiguration.Items)
        {
          
               Page.ClientScript.RegisterForEventValidation(gvSignOnOffConfiguration.UniqueID, "Edit$" + r.RowIndex.ToString());
           
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Sign On/Off", "SIGNONOFFCONFIG");
            toolbarsub.AddButton("App. Letter", "APPLETTER");
            toolbarsub.AddButton("Signon Date", "SDC");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOnOffConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSignOnOffConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSignOnOffConfiguration.AccessRights = this.ViewState;
            MenuSignOnOffConfiguration.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.bind();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
                gvSignOnOffConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("APPLETTER"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreAppointmentLetterCorrection.aspx", true);
        }

        if (CommandName.ToUpper().Equals("SDC"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreSignOnDateCorrection.aspx", true);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDDMROPERATIONALYNNAME", "FLDCREWENABLEDYNAME", "FLDSIGNONOFFACCESSBYNAME", "FLDLAIDUPDATE", "FLDREACTIVEDATE" };
        string[] alCaptions = { "ID", "Vessel", "DMR Operational Y/N", "Crew Operational Y/N", "Crew Sign On/Off Access By", "Laidup Date", "Re-Activate Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreSignOnOffConfiguration.SearchSignOnOffConfiguraiton(General.GetNullableInteger(UcVessel.SelectedVessel),
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewEnableConfiguration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Crew Enable Configuration</h3></td>");
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

    protected void SignOnOffConfiguration_TabStripCommand(object sender, EventArgs e)
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

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindData();
        gvSignOnOffConfiguration.Rebind();
       // SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDDMROPERATIONALYNNAME", "FLDCREWENABLEDYNAME", "FLDSIGNONOFFACCESSBYNAME", "FLDLAIDUPDATE", "FLDREACTIVEDATE" };
        string[] alCaptions = { "ID", "Vessel", "DMR Operational Y/N", "Crew Operational Y/N", "Crew Sign On/Off Access By", "Laidup Date", "Re-Activate Date" };

        DataSet ds = PhoenixCrewOffshoreSignOnOffConfiguration.SearchSignOnOffConfiguraiton (General.GetNullableInteger(UcVessel.SelectedVessel),
            gvSignOnOffConfiguration.CurrentPageIndex+1,
           gvSignOnOffConfiguration.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSignOnOffConfiguration", "Crew Enable", alCaptions, alColumns, ds);

        gvSignOnOffConfiguration.DataSource = ds;
       
        gvSignOnOffConfiguration.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSignOnOffConfiguration_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvSignOnOffConfiguration_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSignOnOffConfiguration, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        //SetKeyDownScroll(sender, e);
    }

   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       
        BindData();
        //SetPageNavigator();
    }

   


    private bool IsValidInput(string vesselid, string enablecrew,string signonoffaccess)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(enablecrew) == null)
            ucError.ErrorMessage = "Enable Crew Y/N is required.";

        if (General.GetNullableInteger(signonoffaccess) == null)
            ucError.ErrorMessage = "Sign On/Off Access is required.";

        return (!ucError.IsError);
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSignOnOffConfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSignOnOffConfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidInput(((Label)e.Item.FindControl("lblVesselId")).Text,
                ((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0",
                ((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            RadLabel lblConfigurationId = (RadLabel)e.Item.FindControl("lblConfigurationId");

            if (lblConfigurationId != null)
            {
                if (!string.IsNullOrEmpty(lblConfigurationId.Text))
                {
                    PhoenixCrewOffshoreSignOnOffConfiguration.SignOnOffConfigurationUpdate(int.Parse(lblConfigurationId.Text),
                        int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableDMRYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtLaidupDateEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtReactiveDateEdit")).Text)
                        );
                }
                else
                {
                    PhoenixCrewOffshoreSignOnOffConfiguration.SignOnOffConfigurationInsert(int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableDMRYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtLaidupDateEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtReactiveDateEdit")).Text)
                        );
                }
            }
            //_gridView.EditIndex = -1;
            BindData();
            gvSignOnOffConfiguration.Rebind();
        }
      
        else if (e.CommandName.ToUpper().Equals("UPDATECHEKLIST"))
        {
            PhoenixCrewOffshoreSignOnOffConfiguration.RefreshChecklist(General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselId")).Text));
        }
    }

    protected void gvSignOnOffConfiguration_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            RadComboBox ddlSignOnOffAccessByEdit = (RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit");

            if (ddlSignOnOffAccessByEdit != null)
            {
                ddlSignOnOffAccessByEdit.SelectedValue = dr["FLDSIGNONOFFACCESSBY"].ToString();
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }

   protected void gvSignOnOffConfiguration_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;

            if (!IsValidInput(((RadLabel)e.Item.FindControl("lblVesselId")).Text,
                      ((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0",
                      ((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            RadLabel lblConfigurationId = (RadLabel)e.Item.FindControl("lblConfigurationId");

            if (lblConfigurationId != null)
            {
                if (!string.IsNullOrEmpty(lblConfigurationId.Text))
                {
                    PhoenixCrewOffshoreSignOnOffConfiguration.SignOnOffConfigurationUpdate(int.Parse(lblConfigurationId.Text),
                        int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableDMRYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtLaidupDateEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtReactiveDateEdit")).Text)
                        );
                }
                else
                {
                    PhoenixCrewOffshoreSignOnOffConfiguration.SignOnOffConfigurationInsert(int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableCrewYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSignOnOffAccessByEdit")).SelectedValue),
                        int.Parse(((CheckBox)e.Item.FindControl("chkEnableDMRYNEdit")).Checked ? "1" : "0"),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtLaidupDateEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtReactiveDateEdit")).Text)
                        );
                }
            }

            BindData();
            gvSignOnOffConfiguration.Rebind();
            // SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
