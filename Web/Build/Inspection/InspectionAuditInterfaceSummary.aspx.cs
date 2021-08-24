using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Reports;
public partial class InspectionAuditInterfaceSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditInterfaceSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");            

            if (!IsPostBack)
            {
                ViewState["Shortcode"] = string.Empty;
                ViewState["ReviewScheduleId"] = string.Empty;
                ViewState["QuestionType"] = string.Empty;
                ViewState["VesselId"] = string.Empty;
                ViewState["MappingIDList"] = string.Empty;
                ViewState["INTERFACEYN"] = 0;
                ViewState["PAGENUMBER"] = 1;
                ucConfirm.Visible = false;
                if (Request.QueryString["ReviewScheduleId"] != null && Request.QueryString["ReviewScheduleId"].ToString() != string.Empty)
                    ViewState["ReviewScheduleId"] = Request.QueryString["ReviewScheduleId"].ToString();

                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() != string.Empty)
                    ViewState["QuestionType"] = Request.QueryString["Type"].ToString();

                if (Request.QueryString["VesselId"] != null && Request.QueryString["VesselId"].ToString() != string.Empty)
                    ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();

                if (Request.QueryString["INTERFACEYN"] != null && Request.QueryString["INTERFACEYN"].ToString() != string.Empty)
                    ViewState["INTERFACEYN"] = Request.QueryString["INTERFACEYN"].ToString();                


                gvSummary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindParentItem();
                Report();

            }
            if (ViewState["INTERFACEYN"].ToString().Equals("0"))
            {
                toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditInterfaceSummary.aspx", "Upload Deficiency", "<i class=\"fas fa-upload\"></i>", "UPLOAD");
            }
            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar tool = new PhoenixToolbar();
            MenuTitle.Title = ViewState["Shortcode"].ToString();
            MenuTitle.MenuList = tool.Show();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private void Report()
    {
        DataTable dt;
        dt = PhoenixInspectionAuditInterfaceDetails.InspectionAuditInterfaceReportDetailsList(
                                         General.GetNullableGuid(ViewState["ReviewScheduleId"].ToString())
                                        , General.GetNullableInteger(ViewState["VesselId"].ToString()));
        if (dt.Rows.Count > 0)
        {

            DataRow dr = dt.Rows[0];
            txtMvMt.Text = dr["FLDVESSELNAME"].ToString();
            txtmadeby.Text = dr["AUDITORNAME"].ToString();
            ViewState["Shortcode"] = dr["FLDINSPECTIONNAME"].ToString();
            txtFromto.Text = dr["PORTNAME"].ToString();
            txtFrom.Text = General.GetDateTimeToString(dr["FLDCOMPLETIONDATE"].ToString());
            txtTo.Text = General.GetDateTimeToString(dr["FLDPLANNEDDATE"].ToString());
        }
    }


    protected void gvSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSummary.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixInspectionAuditInterfaceDetails.InspectionAuditInterfaceSummary(
                    General.GetNullableGuid(ViewState["ReviewScheduleId"].ToString()),
                     General.GetNullableInteger(ViewState["QuestionType"].ToString()),

                  gvSummary.CurrentPageIndex + 1,
                  gvSummary.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ddlParentitem.SelectedValue));

            string[] alColumns = { "FLDROWNUMBER", "FLDCHECKLISTREFNO", "FLDCHECKLISTCODE", "FLDITEM", "FLDQUICKNAME", "FLDDESCRIPTION", "FLDACTIONREQUIRED", "FLDNCNCATEGORYID", "FLDRESPONSIBILITYID", "DUEDATE", "FLDVERIFICATIONLEVELID" };
            string[] alCaptions = { "S.No", "Comp Code", "VIR Code", "Item", "Condition", "Details of Present Condition", "Action Required", "NC/Obs", "Assigned TO", "Due Date", "Verification Level" };

            gvSummary.DataSource = ds;
            gvSummary.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvSummary$ctl00$ctl02$ctl01$chkUploadHeader")
        {
            GridHeaderItem headerItem = gvSummary.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkUploadHeader");
            foreach (GridDataItem row in gvSummary.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("ChkItem");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }
    }

    protected void gvSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel Compiles = (RadLabel)e.Item.FindControl("lblCompiles");
                if (drv["FLDCODE"].ToString().Equals("0"))
                {
                    Compiles.Attributes["style"] = "color:Green !important";
                    Compiles.Font.Bold = true;
                }
                else if (drv["FLDCODE"].ToString().Equals("1"))
                {
                    Compiles.Attributes["style"] = "color:Red !important";
                    Compiles.Font.Bold = true;
                }
                else if (drv["FLDCODE"].ToString().Equals("2"))
                {
                    Compiles.Attributes.Add("style", "font-weight:bold;");
                    Compiles.BackColor = System.Drawing.Color.White;
                }
                

            }

            if (e.Item is GridEditableItem)
            {

                DataRowView dr = (DataRowView)e.Item.DataItem;

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                RadComboBox gre = (RadComboBox)e.Item.FindControl("ddlAssignedTo");
                if (gre != null)
                {
                    DataTable ds = new DataTable();
                    ds = PhoenixInspectionAuditInterfaceDetails.ResponsibilityList();
                    gre.DataSource = ds;
                    gre.DataTextField = "FLDNAME";
                    gre.DataValueField = "FLDID";
                    gre.DataBind();
                    gre.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    gre.SelectedValue = dr["FLDRESPONSIBILITYID"].ToString();
                }


                RadComboBox cmd = (RadComboBox)e.Item.FindControl("cmdVerificationlevel");
                if (cmd != null)
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixInspectionAuditInterfaceDetails.VerificationLevelList();
                    cmd.DataSource = dt;
                    cmd.DataTextField = "FLDNAME";
                    cmd.DataValueField = "FLDID";
                    cmd.DataBind();
                    cmd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    cmd.SelectedValue = dr["FLDVERIFICATIONLEVELID"].ToString();
                }


                UserControlDate Date = (UserControlDate)e.Item.FindControl("ucDueDate");
                if (Date != null)
                {
                    Date.Text = dr["FLDDUEDATE"].ToString();
                }

                RadDropDownList NC = (RadDropDownList)e.Item.FindControl("ddlncn");
                if (NC != null)
                {
                    NC.SelectedValue = dr["FLDNCNCATEGORYID"].ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                ViewState["Actionrequired"] = ((RadTextBox)e.Item.FindControl("txtActionRequired")).Text;
                ViewState["Duedate"] = ((UserControlDate)e.Item.FindControl("ucDueDate")).Text;
                ViewState["NCNCatgory"] = ((RadDropDownList)e.Item.FindControl("ddlncn")).SelectedValue;
                ViewState["Responsibility"] = ((RadComboBox)e.Item.FindControl("ddlAssignedTo")).SelectedValue;
                ViewState["VerificationLevel"] = ((RadComboBox)e.Item.FindControl("cmdVerificationlevel")).SelectedValue;
                ViewState["mappingId"] = ((RadLabel)e.Item.FindControl("lblMappingId")).Text;

                if (ViewState["Actionrequired"].ToString() != string.Empty && ViewState["Actionrequired"] != null)
                {
                    if (!IsValidReport(General.GetNullableDateTime(ViewState["Duedate"].ToString())
                        , General.GetNullableInteger(ViewState["NCNCatgory"].ToString())
                        , General.GetNullableInteger(ViewState["Responsibility"].ToString())
                        , General.GetNullableInteger(ViewState["VerificationLevel"].ToString())))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditInterfaceDetails.InspectionAuditInterfacereportUpdate(ViewState["Actionrequired"].ToString()
                        , General.GetNullableInteger(ViewState["NCNCatgory"].ToString())
                        , General.GetNullableDateTime(ViewState["Duedate"].ToString())
                        , General.GetNullableInteger(ViewState["Responsibility"].ToString())
                        , General.GetNullableInteger(ViewState["VerificationLevel"].ToString())
                        , General.GetNullableGuid(ViewState["mappingId"].ToString()));

                    ucStatus.Text = "Defeciency is updated successfully.";
                    Rebind();
                }
                else
                {
                    RadWindowManager1.RadConfirm("Corrective action cannot be created. Are you sure to continue?", "confirm", 320, 150, null, "confirm");
                }
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
        gvSummary.SelectedIndexes.Clear();
        gvSummary.EditIndexes.Clear();
        gvSummary.DataSource = null;
        gvSummary.Rebind();

    }

    private bool IsValidReport(DateTime? Duedate, int? NCNCatgory, int? Responsibility, int? VerificationLevel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
            if (Duedate == null)
                ucError.ErrorMessage = "Duedate is required.";

            if (NCNCatgory == null)
                ucError.ErrorMessage = "NCN is required.";

            if (Responsibility == null)
                ucError.ErrorMessage = "Responsibility is required.";

            if (VerificationLevel == null)
                ucError.ErrorMessage = "VerificationLevel is required.";

        return (!ucError.IsError);
    }


    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("UPLOAD"))
            {
                PhoenixInspectionAuditInterfaceDetails.InterfaceAuditDeficiencyupload(
                    General.GetNullableGuid(ViewState["ReviewScheduleId"].ToString()), General.GetNullableString(ViewState["MappingIDList"].ToString()));

                ViewState["MappingIDList"] = string.Empty;
                ucStatus.Text = "Deficiency Upload successfully.";
            }


            Rebind();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                     "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', '','');", true);
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

        string[] alColumns = { "FLDROWNUMBER", "FLDCHECKLISTREFNO", "FLDCHECKLISTCODE", "FLDITEM", "FLDQUICKNAME", "FLDDESCRIPTION", "FLDACTIONREQUIRED", "FLDNCNCATEGORYID", "FLDRESPONSIBILITYID", "DUEDATE", "FLDVERIFICATIONLEVELID" };
        string[] alCaptions = { "S.No", "Comp Code", "VIR Code", "Item", "Condition", "Details of Present Condition", "Action Required", "NC/Obs", "Assigned TO", "Due Date", "Verification Level" };


        DataSet ds = PhoenixInspectionAuditInterfaceDetails.InspectionAuditInterfaceSummary(
                General.GetNullableGuid(ViewState["ReviewScheduleId"].ToString()),
                 General.GetNullableInteger(ViewState["QuestionType"].ToString()),

              gvSummary.CurrentPageIndex + 1,
              gvSummary.PageSize,
        ref iRowCount,
        ref iTotalPageCount,
        General.GetNullableGuid(ddlParentitem.SelectedValue));

        gvSummary.DataSource = ds;
        gvSummary.VirtualItemCount = iRowCount;

        string InspectionName = ds.Tables[0].Rows[0]["INSPECTIONNAME"].ToString();

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionAuditInterfaceReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td></td>");
        Response.Write("<td><h3>" + InspectionName + "</h3></td>");
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
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ViewState["MappingIDList"] = string.Empty;
        foreach (GridDataItem gvrow in gvSummary.Items)
        {
            if (((RadCheckBox)(gvrow.FindControl("ChkItem"))).Checked == true)
            {
                ViewState["MappingIDList"] = ViewState["MappingIDList"].ToString() + gvrow.GetDataKeyValue("FLDMAPPINGID").ToString() + ',';
            }
            //else
            //{
            //    ViewState["MappingIDList"] = ViewState["MappingIDList"].ToString();
            //}

        }
    }
    private void BindParentItem()
    {
        ddlParentitem.DataSource = PhoenixInspectionRegisterCheckItems.ListCheckItems();
        ddlParentitem.DataTextField = "FLDITEM";
        ddlParentitem.DataValueField = "FLDINSPECTIONCHECKITEMID";
        ddlParentitem.DataBind();
        ddlParentitem.Items.Insert(0, new RadComboBoxItem("--Select--"));
    }
    protected void ddlParentitem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvSummary.Rebind();
    }


    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["mappingId"] != null && ViewState["mappingId"].ToString() != string.Empty)
            {
                PhoenixInspectionAuditInterfaceDetails.InspectionAuditInterfacereportUpdate(null
                           , General.GetNullableInteger(ViewState["NCNCatgory"] != null ? ViewState["NCNCatgory"].ToString() : null)
                           , General.GetNullableDateTime(ViewState["Duedate"] != null ? ViewState["Duedate"].ToString() : null)
                           , General.GetNullableInteger(ViewState["Responsibility"] != null ? ViewState["Responsibility"].ToString() : null)
                           , General.GetNullableInteger(ViewState["VerificationLevel"] != null ? ViewState["VerificationLevel"].ToString() : null)
                           , General.GetNullableGuid(ViewState["mappingId"].ToString()));

                ucStatus.Text = "Defeciency is updated successfully.";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}



