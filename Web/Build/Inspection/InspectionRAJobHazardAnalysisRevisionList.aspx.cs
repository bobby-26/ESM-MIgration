using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class InspectionRAJobHazardAnalysisRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvJHARevisions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuJobHazardAnalysis.AccessRights = this.ViewState;
        MenuJobHazardAnalysis.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["JOBHAZARDID"] = "";

            if (Request.QueryString["referenceid"] != null && Request.QueryString["referenceid"].ToString() != "")
            {
                ViewState["REFNO"] = Request.QueryString["referenceid"].ToString();
                gvJHARevisions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }                      
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHAZARDNUMBER","FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Hazard Number", "Vessel name","Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardRevisionSearch(
                    General.GetNullableString(txtHazardNo.Text),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvJHARevisions.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null);

        General.SetPrintOptions("gvJHARevisions", "Job Hazard Analysis Revisions", alCaptions, alColumns, ds);

        gvJHARevisions.DataSource = ds;
        gvJHARevisions.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
            {
                ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
            }
        }
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDHAZARDNUMBER","FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Hazard Number","Vessel Name","Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardRevisionSearch(
                    General.GetNullableString(txtHazardNo.Text),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null);

            General.ShowExcel("Job Hazard Analysis Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvJHARevisions.SelectedIndexes.Clear();
        gvJHARevisions.EditIndexes.Clear();
        gvJHARevisions.DataSource = null;
        gvJHARevisions.Rebind();
    }
    protected void MenuJobHazardAnalysis_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAJobHazardAnalysis.aspx?status=", false);
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            //NameValueCollection criteria = new NameValueCollection();
            //criteria.Clear();
            //criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
            //criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            //criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            //Filter.CurrentJHAFilter = criteria;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtHazardNo.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            Rebind();
        }
    }
    protected void gvJHARevisions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                Label lbl = (Label)e.Item.FindControl("lblJobHazardid");
                Label lblstatus = (Label)e.Item.FindControl("lblStatusID");
                Response.Redirect("../Inspection/InspectionRAJobHazardAnalysis.aspx?jobhazardid=" + lbl.Text + "&status=" + lblstatus.Text, false);
            }

            if (e.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                Label lbljobhazardid = (Label)e.Item.FindControl("lblJobHazardid");
                PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lbljobhazardid.Text));
                Rebind();
                ucStatus.Text = "Revison Completed ";
            }
            if (e.CommandName.ToUpper().Equals("FILTER"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                Label lbljobhazardid = (Label)e.Item.FindControl("lblJobHazardid");
                Label lblrefno = (Label)e.Item.FindControl("lblReferencid");
                ViewState["REFNO"] = lblrefno.Text;
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CLEARFILTER"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                ViewState["REFNO"] = "";
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("JOBHAZARD"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
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
    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentJHAFilter = criteria;
        Rebind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentJHAFilter = criteria;
        Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblJobHazardid = (Label)gvJHARevisions.Items[rowindex].FindControl("lblJobHazardid");
            if (lblJobHazardid != null)
            {
                ViewState["JOBHAZARDID"] = lblJobHazardid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvJHARevisions.SelectedIndexes.Clear();
        for (int i = 0; i < gvJHARevisions.Items.Count; i++)
        {
            //if (gvJHARevisions.DataKeys[i].Value.ToString().Equals(ViewState["JOBHAZARDID"].ToString()))
            //{
                //gvJHARevisions.SelectedIndex = i;
            //}
        }
    }

    protected void gvJHARevisions_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            Label lbljobhazardid = ((Label)e.Item.FindControl("lblJobHazardid"));
            PhoenixInspectionRiskAssessmentJobHazard.DeleteRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(lbljobhazardid.Text));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJHARevisions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJHARevisions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
