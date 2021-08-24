using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAHazardMachineryDamageMapping : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAHazardMachineryDamageMapping.aspx?" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRASubHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddImageButton("../Inspection/InspectionRAHazardMachineryDamageMapping.aspx?type=" + Request.QueryString["type"], "Find", "search.png", "FIND");

            MenuRASubHazard.AccessRights = this.ViewState;
            MenuRASubHazard.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                string type = "4";
                gvRASubHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["TYPE"] = type;
                ucHazard.Type = type;
            }
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBHAZARDNAME", "FLDSELECTEDFORPROCESSYNNAME", "FLDSELECTEDFORCOSTYNNAME" };
        string[] alCaptions = { "Sub Hazard", "Process Loss", "Cost" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentSubHazard.RiskAssessmentSubHazardSearch(
            General.GetNullableInteger(ViewState["TYPE"].ToString()),
            General.GetNullableInteger(ucHazard.SelectedHazardType),
            General.GetNullableString(""),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRASubHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SubHazard.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Machinery Damage/ Failure and Hazard mapping</h3></td>");
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

    protected void ucHazard_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void chkProcess_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridDataItem gr = (GridDataItem)chk.Parent.Parent;
            RadLabel lblSubHazardId = ((RadLabel)gr.FindControl("lblSubHazardId"));

            PhoenixInspectionRiskAssessmentSubHazard.RASubhazard2MachineryDamageMapping(
                            General.GetNullableGuid(lblSubHazardId.Text),
                            (chk.Checked == true ? 1 : 0),
                            0); // process

            ucStatus.Text = "Hazard has been mapped to Machinery Damage/ Failure successfully.";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkCost_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridDataItem gr = (GridDataItem)chk.Parent.Parent;
            RadLabel lblSubHazardId = ((RadLabel)gr.FindControl("lblSubHazardId"));

            PhoenixInspectionRiskAssessmentSubHazard.RASubhazard2MachineryDamageMapping(
                            General.GetNullableGuid(lblSubHazardId.Text),
                            (chk.Checked == true ? 1 : 0),
                            1); // cost

            ucStatus.Text = "Hazard has been mapped to Machinery Damage/ Failure successfully.";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RASubHazard_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBHAZARDNAME", "FLDSELECTEDFORPROCESSYNNAME", "FLDSELECTEDFORCOSTYNNAME" };
        string[] alCaptions = { "Sub Hazard", "Process Loss", "Cost" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentSubHazard.RiskAssessmentSubHazardSearch(
            General.GetNullableInteger(ViewState["TYPE"].ToString()),
            General.GetNullableInteger(ucHazard.SelectedHazardType),
            General.GetNullableString(""),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRASubHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRASubHazard", "Machinery Damage/ Failure and Hazard mapping", alCaptions, alColumns, ds);

        gvRASubHazard.DataSource = ds;
        gvRASubHazard.VirtualItemCount = iRowCount;
    }
    protected void Rebind()
    {
        gvRASubHazard.SelectedIndexes.Clear();
        gvRASubHazard.EditIndexes.Clear();
        gvRASubHazard.DataSource = null;
        gvRASubHazard.Rebind();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        Rebind();
    }


    protected void gvRASubHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "Page")
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

    private void InsertRASubHazard(string name, string Hazardid, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentSubHazard.InsertRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              Convert.ToInt32(Hazardid), name, Convert.ToInt32(severityid), General.GetNullableString(category));
    }

    private void UpdateRASubHazard(string SubHazardid, string name, string Hazardid, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentSubHazard.UpdateRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(SubHazardid), Convert.ToInt32(Hazardid), name, Convert.ToInt32(severityid), General.GetNullableString(category));
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRASubHazard(string name, string Hazard, string severityid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        // GridView _gridView = gvRASubHazard;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Impact is required.";

        if (General.GetNullableInteger(Hazard) == null)
            ucError.ErrorMessage = "Hazard is required.";

        if (General.GetNullableInteger(severityid) == null)
            ucError.ErrorMessage = "Severity is required.";

        return (!ucError.IsError);
    }

    private void DeleteSubHazard(Guid SubHazardid)
    {
        PhoenixInspectionRiskAssessmentSubHazard.DeleteRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, SubHazardid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRASubHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRASubHazard.CurrentPageIndex + 1;

        BindData();
    }
}
