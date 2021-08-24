using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAProcessRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentProcessRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();
        ucConfirm.Visible = false;
        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RISKASSESSMENTPROCESSID"] = "";
            gvRiskAssessmentProcessRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["referenceid"] != null && Request.QueryString["referenceid"].ToString() != "")
            {
                ViewState["REFNO"] = Request.QueryString["referenceid"].ToString();
            }
            ViewState["callfrom"] = "";
            BindCategory();
        }
       // BindData();
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessRevisionSearch(
                                                                                General.GetNullableString(txtHazardNo.Text),
                                                                                General.GetNullableInteger(ddlCategory.SelectedValue),
                                                                                General.GetNullableInteger(ddlStatus.SelectedValue),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                gvRiskAssessmentProcessRevision.CurrentPageIndex + 1,
                                                                                gvRiskAssessmentProcessRevision.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentProcessRevision", "Risk Assessment-Process Revisions", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvRiskAssessmentProcessRevision.DataSource = ds;
            //gvRiskAssessmentProcessRevision.DataBind();

            if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) == null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSID"].ToString();
                //gvRiskAssessmentProcessRevision.SelectedIndex = 0;
            }
            //SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvRiskAssessmentProcessRevision);
        }
        gvRiskAssessmentProcessRevision.DataSource = ds;
        gvRiskAssessmentProcessRevision.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessRevisionSearch(
                                                                                General.GetNullableString(txtHazardNo.Text),
                                                                                General.GetNullableInteger(ddlCategory.SelectedValue),
                                                                                General.GetNullableInteger(ddlStatus.SelectedValue),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvRiskAssessmentProcessRevision.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

            General.ShowExcel("Risk Assessment-Process Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentProcessRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (lnkRefNo != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    lnkRefNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAProcess.aspx?processid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
                }
            }
            RadLabel lblJobActivity = (RadLabel)e.Item.FindControl("lblJobActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucJobActivity");
            if (uct != null)
            {
                lblJobActivity.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblJobActivity.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
    }

    protected void gvRiskAssessmentProcessRevision_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatusID");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                if (gce.CommandName.ToUpper().Equals("EDITROW"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    Response.Redirect("../Inspection/InspectionRAProcess.aspx?processid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                if (gce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    if (lblInstallcode != null && lblInstallcode.Text == "0")
                    {
                        PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(lbl.Text)
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , null
                                                                                        , 2);
                        ucStatus.Text = "Approved Successfully";
                    }
                }
                if (gce.CommandName.ToUpper().Equals("ISSUE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                           , new Guid(lbl.Text)
                                                                                           , null
                                                                                           , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                           , 3);
                    ucStatus.Text = "Issued Successfully";
                }
                if (gce.CommandName.ToUpper().Equals("REVISION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(lbl.Text));
                    ucStatus.Text = "Revison Completed ";
                }
                if (gce.CommandName.ToUpper().Equals("FILTER"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    RadLabel lblrefno = (RadLabel)gce.Item.FindControl("lblReferencid");
                    //if(ViewState["REFNO"]
                    ViewState["REFNO"] = lblrefno.Text;
                }
                if (gce.CommandName.ToUpper().Equals("CLEARFILTER"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["REFNO"] = "";
                }
                if (gce.CommandName.ToUpper().Equals("COPY"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["PROCESSID"] = lbl.Text;
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Are you sure to copy the template?";
                    return;
                }
                if (gce.CommandName.ToUpper().Equals("RAPROCESS"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                BindData();
                gvRiskAssessmentProcessRevision.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
                if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentProcess.CopyRiskAssessmentProcess(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["PROCESSID"].ToString()),
                       PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ucStatus.Text = "RA is copied.";
                    gvRiskAssessmentProcessRevision.Rebind();
                }
           // }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvRiskAssessmentProcessRevision.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentProcessRevision.Rebind();
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentProcessRAFilter = criteria;
        gvRiskAssessmentProcessRevision.Rebind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentProcessRAFilter = criteria;
        gvRiskAssessmentProcessRevision.Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentProcessID = (RadLabel)gvRiskAssessmentProcessRevision.Items[rowindex].FindControl("lblRiskAssessmentProcessID");
            if (lblRiskAssessmentProcessID != null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = lblRiskAssessmentProcessID.Text;
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
        //gvRiskAssessmentProcessRevision.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentProcessRevision.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentProcessRevision.DataKeys[i].Value.ToString().Equals(ViewState["RISKASSESSMENTPROCESSID"].ToString()))
        //    {
        //        gvRiskAssessmentProcessRevision.SelectedIndex = i;
        //    }
        //}
    }

    protected void gvRiskAssessmentProcessRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentProcessRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
