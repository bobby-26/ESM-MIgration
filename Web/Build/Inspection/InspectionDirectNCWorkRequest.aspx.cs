using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionDirectNCWorkRequest : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);		
		txtComponentId.Attributes.Add("style", "visibility:hidden");
		txtJobId.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDirectNCWorkRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuInspectionWorkRequest.AccessRights = this.ViewState;
        MenuInspectionWorkRequest.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuWorkOrderRequestion.AccessRights = this.ViewState;

        if (!IsPostBack)
		{
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();

			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["REVIEWDNC"] = null;
			ViewState["scheduleid"] = null;
			ViewState["NEW"] = "false";
            if (Request.QueryString["REVIEWDNC"] != null && Request.QueryString["REVIEWDNC"].ToString() != "")
            {
                ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"].ToString();
                GetVesselId();
            }
            if (Request.QueryString["scheduleid"] != null && Request.QueryString["scheduleid"].ToString() != "")            
				ViewState["scheduleid"] = Request.QueryString["scheduleid"].ToString();
		}
        if (ViewState["REVIEWDNC"] != null)
        {
            //if NC status is closed, no buttons will be visible
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REVIEWDNC"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
                    MenuWorkOrderRequestion.MenuList = toolbar.Show();
            }

            //if direct observation status is closed, no buttons will be visible
            ds = PhoenixInspectionAuditDirectNonConformity.EditDirectObservation(new Guid(ViewState["REVIEWDNC"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
                    MenuWorkOrderRequestion.MenuList = toolbar.Show();
            }
        }
        imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true); ");
    }
	private void BindFields()
	{
		if (ViewState["WORKORDERID"] != null)
		{
			DataSet ds = PhoenixInspectionWorkOrder.EditWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()));
			DataRow dr = ds.Tables[0].Rows[0];
			lblWorkOrderID.Text = dr["FLDWORKORDERID"].ToString();
			txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
			txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
			txtJobDescription.Text = dr["FLDDETAILS"].ToString();
			txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
			txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
			txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
			txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
			if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
				chkUnexpected.Checked = true;
			txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
			txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
			txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
			ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
			ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
			txtJobId.Text = dr["FLDJOBID"].ToString();
			txtJobName.Text = dr["FLDJOBTITLE"].ToString();
			txtJobCode.Text = dr["FLDJOBCODE"].ToString();
			ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
		}
		else
			ViewState["NEW"] = true;
    }

    private void GetVesselId()
    {
        if (ViewState["REVIEWDNC"] != null && ViewState["REVIEWDNC"].ToString() != "")
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REVIEWDNC"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            ds = PhoenixInspectionAuditDirectNonConformity.EditDirectObservation(new Guid(ViewState["REVIEWDNC"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"] + "&framename=ifMoreInfo', true); ");
        }
    }
	private void BindAuditShortCode()
	{
		if (ViewState["scheduleid"] != null)
		{
			DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["scheduleid"].ToString()));
			if (ds.Tables[0].Rows.Count > 0)
			{
				ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDREVIEWSHORTCODE"].ToString();
				DataSet dsmain = PhoenixInspectionRecordAndResponse.WorkRequestMainType(new Guid(ViewState["scheduleid"].ToString()), General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD")));
				if (dsmain.Tables[0].Rows.Count > 0)
				{
					if (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null && dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() != string.Empty)
					{
						ucMainType.SelectedQuick = (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null ? dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() : null);
					}
				}
			}
		}
	}

	private void BindInspectionShortCode()
	{
		if (ViewState["scheduleid"] != null)
		{
			DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["scheduleid"].ToString()));
			if (ds.Tables[0].Rows.Count > 0)
			{
				ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDINSPECTIONSHORTCODE"].ToString();
				DataSet dsmain = PhoenixInspectionRecordAndResponse.WorkRequestMainType(new Guid(ViewState["scheduleid"].ToString()), General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS")));
				if (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null && dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() != string.Empty)
				{
					ucMainType.SelectedQuick = (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null ? dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() : null);
				}
			}
		}
	}

    protected void MenuInspectionWorkRequest_TabStripCommand(object sender, EventArgs e)
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

	protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
			{
				int? maintype = General.GetNullableInteger(ucMainType.SelectedQuick);
			
				if (ViewState["NEW"].ToString().ToUpper() != "TRUE")
				{
					ucError.ErrorMessage = "Sorry,You can not make any more changes here.";
					ucError.Visible = true;
					return;
				}
				if (!IsValidRequisition(txtComponentId.Text, txtTitle.Text, txtPlannedStartDate.Text, txtJobId.Text, txtJobDescription.Text))
				{
					ucError.Visible = true;
					return;
				}

				string workorderid = null;

				byte? isDefect = null;

				if (chkIsDefect.Checked.Equals(true))
					isDefect = byte.Parse("1");

				PhoenixInspectionWorkOrder.InsertWorkOrder(
					int.Parse(ViewState["VESSELID"].ToString()), null,
					General.GetNullableString(txtTitle.Text), General.GetNullableGuid(txtComponentId.Text), null,
					General.GetNullableGuid(txtJobId.Text), null, null, null,
					General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, null,
					General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
					General.GetNullableDateTime(txtPlannedStartDate.Text),
					General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, maintype, null, null,
					chkUnexpected.Checked.Equals(true) ? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
					isDefect, ref workorderid);
				if (txtJobDescription.Text != "")
					PhoenixInspectionWorkOrder.UpdateDetailsWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderid), General.GetNullableString(txtJobDescription.Text));
				ViewState["WORKORDERID"] = workorderid;

				PhoenixInspectionAuditDirectNonConformity.DirectNCWorkOrderInsert
											   (PhoenixSecurityContext.CurrentSecurityContext.UserCode
											  , new Guid(txtComponentId.Text)
											  , new Guid(ViewState["REVIEWDNC"].ToString())
											  , null
											  , null
											  , new Guid(workorderid)
											  , 1
											  , Convert.ToInt32(ViewState["VESSELID"].ToString()));

				ViewState["NEW"] = "false";
				Rebind();
				BindFields();
            }
			else if (CommandName.ToUpper().Equals("NEW"))
			{
				ResetTextBox();
				ViewState["WORKORDERID"] = null;
				ViewState["NEW"] = "true";
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	private void ResetTextBox()
	{
		txtWorkOrderNumber.Text = "";
		txtTitle.Text = "";
		txtComponentCode.Text = "";
		txtComponentName.Text = "";
		txtComponentId.Text = "";
		txtCreatedDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
		chkUnexpected.Checked = true;
		txtDuration.Text = "";
		txtPlannedStartDate.Text = "";
		txtPriority.Text = "1";
		ucDiscipline.SelectedDiscipline = "";
		txtJobDescription.Text = "";
		txtJobId.Text = "";
		txtJobName.Text = "";
		txtJobCode.Text = "";
		ucWTOApproval.SelectedHard = "530";
		ucMainType.SelectedQuick = "";
	}
	private bool IsValidRequisition(string componentid, string title, string plannedstartdate, string jobid, string workdetails)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (componentid.Trim().Equals(""))
			ucError.ErrorMessage = "Component is required";

		if (title.Trim().Equals(""))
			ucError.ErrorMessage = "Title is required";

		if (!General.GetNullableDateTime(plannedstartdate).HasValue)
			ucError.ErrorMessage = "Planned Start Date is required";

		if (string.IsNullOrEmpty(jobid) && string.IsNullOrEmpty(workdetails))
			ucError.ErrorMessage = "Either Job Description or Work Details is required";

		return (!ucError.IsError);
	}
	private void BindData()
	{
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionAuditDirectNonConformity.InspectionDirectNCWorkRequestSearch(General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()),
                             null, Convert.ToInt32(ViewState["VESSELID"].ToString()),
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             General.ShowRecords(iRowCount),
                             ref iRowCount,
                             ref iTotalPageCount);

            General.SetPrintOptions("gvWorkOrder", "NC Work Request", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OPERATIONMODE"] = "EDIT";
                gvWorkOrder.SelectedIndexes.Clear();

                if (ViewState["WORKORDERID"] == null && ViewState["NEW"] != null && ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    BindFields();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ViewState["NEW"] = "true";
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionAuditDirectNonConformity.InspectionDirectNCWorkRequestSearch(General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()),
                              null, Convert.ToInt32(ViewState["VESSELID"].ToString()),
                              sortexpression, sortdirection,
                              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                              General.ShowRecords(iRowCount),
                              ref iRowCount,
                              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Auditdirectncworkrequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>NC Work Request</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvWorkOrder.SelectedIndexes.Clear();
        gvWorkOrder.EditIndexes.Clear();
        gvWorkOrder.DataSource = null;
        gvWorkOrder.Rebind();
    }
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblWorkOrderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId"));
            if (lblWorkOrderId != null)
                ViewState["WORKORDERID"] = lblWorkOrderId.Text;
            else
                ViewState["WORKORDERID"] = null;
            BindFields();
        }
    }
}
