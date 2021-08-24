using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Registers_RegistersDMRRangeConfig : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvMetrology.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$lnkDoubleClick");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../Registers/RegistersDMRRangeConfig.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvMetrology')", "Print Grid", "icon_print.png", "PRINT");
            //MenuDMRRangeConfig.AccessRights = this.ViewState;
            //MenuDMRRangeConfig.MenuList = toolbar.Show();
           
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            toolbarReporttap.AddButton("Copy", "COPY", ToolBarDirection.Right);
            toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
           
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    UcVessel.Enabled = false;

                    lblVesselName.Visible = false;
                    UcVessel.Visible = false;
                    lblTargetVessel.Visible = false;
                    ucTargetVessel.Visible = false;
                    MenuNewSaveTabStrip.Visible = false;
                }


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindData();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvMetrology.Items)
                {
                    if (gvMetrology.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }
                foreach (GridDataItem gvr1 in gvFOFlowmeter.Items)
                {
                    if (gvFOFlowmeter.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr1.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr1.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr1.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr1.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr1.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr1.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr1.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }

                foreach (GridDataItem gvr2 in gvOperationCons.Items)
                {
                    if (gvOperationCons.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr2.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr2.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr2.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr2.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr2.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr2.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr2.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }

                foreach (GridDataItem gvr3 in gvOperationSpeed.Items)
                {
                    if (gvOperationSpeed.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr3.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr3.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr3.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr3.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr3.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr3.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr3.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }

                foreach (GridDataItem gvr4 in gvOthers.Items)
                {
                    if (gvOthers.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr4.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr4.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr4.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr4.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr4.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr4.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr4.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }

                foreach (GridDataItem gvr5 in gvBulks.Items)
                {
                    if (gvBulks.Items.Count > 0)
                    {

                        RadLabel configid = (RadLabel)gvr5.FindControl("lblConfigId");
                        RadLabel fieldname = ((RadLabel)gvr5.FindControl("lblcolumnname"));
                        RadLabel displaytext = ((RadLabel)gvr5.FindControl("lblFieldName"));
                        UserControlNumber minvalue = ((UserControlNumber)gvr5.FindControl("txtMinValueEdit"));
                        UserControlNumber maxvalue = ((UserControlNumber)gvr5.FindControl("txtMaxValueEdit"));
                        CheckBox requiredyn = ((CheckBox)gvr5.FindControl("chkRequiredEdit"));
                        UserControlDate EffectiveDate = ((UserControlDate)gvr5.FindControl("txtEffectiveDate"));

                        if (!IsValidConfig(fieldname.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        if (minvalue.Text != "" || maxvalue.Text != "")
                        {

                            PhoenixRegistersDMRRangeConfig.UpdateDMRRangeConfig(
                                General.GetNullableGuid(configid.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableString(fieldname.Text),
                                General.GetNullableString(displaytext.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableString(minvalue.Text),
                                General.GetNullableString(maxvalue.Text),
                                General.GetNullableInteger(requiredyn.Checked == true ? "1" : "0"),
                                General.GetNullableDateTime(EffectiveDate.Text));
                        }
                    }
                }
                ucStatus.Text = "Control Parameter Updated.";
            }
            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (ucTargetVessel.SelectedVessel != "Dummy")
                {
                    PhoenixRegistersDMRRangeConfig.DMRRangeConfigCopy(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ucTargetVessel.SelectedValue);
                    ucStatus.Text = "Control Parameters Copied";
                }
                else
                {
                    ucError.ErrorMessage = "Target Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDISPLAYTEXT", "FLDMINVALUE", "FLDMAXVALUE", "FLDREQUIREDYESNO" };
        string[] alCaptions = { "Field Name","Min Value", "Max Value", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersDMRRangeConfig.DMRRangeConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ControlParameters.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Control Parameters</h3></td>");
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

    //protected void MenuDMRRangeConfig_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    if (dce.CommandName.ToUpper().Equals("FIND"))
    //    {
    //        ViewState["PAGENUMBER"] = 1;
    //        BindData();
            
    //    }
    //    if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //    {
    //        ShowExcel();
    //    }
    //    if (dce.CommandName.ToUpper().Equals("RESET"))
    //    {
    //        ClearFilter();
    //    }
    //}

    private void ClearFilter()
    {
        //txtAgentName.Text = "";
        UcVessel.SelectedVessel = "";
        BindData();
    }

   

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersDMRRangeConfig.DMRRangeConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        gvMetrology.DataSource = ds.Tables[0];
        gvMetrology.DataBind();

        gvFOFlowmeter.DataSource = ds.Tables[1];
        gvFOFlowmeter.DataBind();

        gvOperationCons.DataSource = ds.Tables[2];
        gvOperationCons.DataBind();

        gvOperationSpeed.DataSource = ds.Tables[3];
        gvOperationSpeed.DataBind();

        gvBulks.DataSource = ds.Tables[5];
        gvBulks.DataBind();

        gvOthers.DataSource = ds.Tables[4];
        gvOthers.DataBind();
      

       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    private bool IsValidConfig(string fieldname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((fieldname == null) || (fieldname == ""))
            ucError.ErrorMessage = "Field Name is required.";

        //if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
        //    ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }

    //protected void gvMetrology_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null)
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    //protected void gvFOFlowmeter_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null)
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    //protected void gvOperationCons_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null)
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    //protected void gvOperationSpeed_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null)
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    //protected void gvOthers_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null )
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    //protected void gvBulks_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
    //    {
    //        _gridView.Columns[1].Visible = false;
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdHistory = (ImageButton)e.Row.FindControl("cmdHistory");
    //        if (cmdHistory != null)
    //        {
    //            if (General.GetNullableGuid(drv["FLDCONFIGID"].ToString()) != null)
    //            {
    //                string vesselid;

    //                if (ViewState["VESSELID"].ToString().Equals(""))
    //                    vesselid = UcVessel.SelectedVessel;
    //                else
    //                    vesselid = ViewState["VESSELID"].ToString();

    //                cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
    //                cmdHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
    //                + vesselid + "&Configid=" + drv["FLDCONFIGID"].ToString() + "'); return true;");
    //            }
    //        }
    //    }
    //}

    protected void gvMetrology_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvMetrology_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
       // GridDataItem item = (GridDataItem)e.Item;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvMetrology.Columns[1].Visible = false;
        }
    

        if (e.Item is GridDataItem )
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                    + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvFOFlowmeter_ItemDataBound1(object sender, GridItemEventArgs e)
    {
       
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvMetrology.Columns[1].Visible = false;
        }


        if (e.Item is GridDataItem)
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                   + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvFOFlowmeter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOperationCons_ItemDataBound1(object sender, GridItemEventArgs e)
    {
       
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvOperationCons.Columns[1].Visible = false;
        }
       

        if (e.Item is GridDataItem)
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                     + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvOperationCons_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOperationSpeed_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvOperationSpeed.Columns[1].Visible = false;
        }
    

        if (e.Item is GridDataItem)
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                    + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvOperationSpeed_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvBulks_ItemDataBound1(object sender, GridItemEventArgs e)
    {
      
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvBulks.Columns[1].Visible = false;
        }
       

        if (e.Item is GridDataItem)
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                   + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvBulks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOthers_ItemDataBound1(object sender, GridItemEventArgs e)
    {
      
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvOthers.Columns[1].Visible = false;
        }
      

        if (e.Item is GridDataItem)
        {
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString()) != null)
                {
                    string vesselid;

                    if (ViewState["VESSELID"].ToString().Equals(""))
                        vesselid = UcVessel.SelectedVessel;
                    else
                        vesselid = ViewState["VESSELID"].ToString();

                    cmdHistory.Visible = SessionUtil.CanAccess(this.ViewState, cmdHistory.CommandName);
                    cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersDMRRangeConfigurationHistory.aspx?Vesselid="
                   + vesselid + "&Configid=" + DataBinder.Eval(e.Item.DataItem, "FLDCONFIGID").ToString() + "'); return true;");
                }
            }
        }
    }

    protected void gvOthers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
