using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionAdminRAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminRAList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminRAList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminRAList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRA.AccessRights = this.ViewState;
        MenuRA.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Incident/NearMiss Deletion", "INCDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task Deletion", "CARDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Date Change", "RADATECHANGE", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Deletion", "RADELETION", ToolBarDirection.Right);

        MenuRAMain.AccessRights = this.ViewState;
        MenuRAMain.MenuList = toolbarmain.Show();
        MenuRAMain.SelectedMenuIndex = 3;

        if (!IsPostBack)
        {
            ucConfirm.Attributes.Add("style", "display:none");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPANYID"] = "";
            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                ucVessel.Company = nvcCompany.Get("QMS");
                ucVessel.bind();
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            BindStatus();
            BindType();
        }
      //  BindData();
    }

    private void BindType()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivity.ListNonRoutineRiskAssessmentType();
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCODE";
        ddlRAType.DataBind();
        ddlRAType.Items.Insert(0, new RadComboBoxItem("--Select--", "ALL"));
    }

    private void BindStatus()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDSTATUSID", typeof(string));
        dt.Columns.Add("FLDSTATUSNAME", typeof(string));

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
        {
            dt.Rows.Add("1", "Draft");
            dt.Rows.Add("2", "Approved");
            dt.Rows.Add("3", "Issued");
            dt.Rows.Add("4", "Pending approval");
            dt.Rows.Add("5", "Approved for use");
            dt.Rows.Add("6", "Not Approved");
        }
        else
        {
            dt.Rows.Add("1", "Draft");
            dt.Rows.Add("4", "Pending Office Approval");
            dt.Rows.Add("5", "Approved for use");
            dt.Rows.Add("6", "Rejected");
        }

        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MenuRAMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RADATECHANGE"))
        {
            Response.Redirect("../Inspection/InspectionAdminRiskAssessment.aspx", true);
        }
        if (CommandName.ToUpper().Equals("CARDELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminCARList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("INCDELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminIncidentList.aspx", true);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVITYCONDITIONS", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Category", "Activity / Conditions", "Status" };

        int vesselid = 0;
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            vesselid = int.Parse(ucVessel.SelectedVessel);
        }
        else
            vesselid = 0;

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            DataSet ds = PhoenixInspectionRiskAssessment.PhoenixInspectionRiskAssessmentSearch(
                    vesselid,
                    gvRiskAssessment.CurrentPageIndex + 1,
                    gvRiskAssessment.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(txtRefNo.Text),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.SetPrintOptions("gvRiskAssessment", "Risk Assessment", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //  gvRiskAssessment.DataSource = ds;
                // gvRiskAssessment.DataBind();

                if (ViewState["RAID"] == null)
                {
                    ViewState["RAID"] = ds.Tables[0].Rows[0]["FLDRAID"].ToString();
                    // gvRiskAssessment.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //ShowNoRecordsFound(dt, gvRiskAssessment);
            }
            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;
        }
        else
        {
            DataSet ds = PhoenixInspectionRiskAssessment.PhoenixInspectionMainFleetRiskAssessmentSearch(
                    General.GetNullableInteger(ucVessel.SelectedVessel),
                    gvRiskAssessment.CurrentPageIndex + 1,
                    gvRiskAssessment.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(txtRefNo.Text),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()),
                    General.GetNullableString(ddlRAType.SelectedValue));

            General.SetPrintOptions("gvRiskAssessment", "Risk Assessment", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                // gvRiskAssessment.DataSource = ds;
                // gvRiskAssessment.DataBind();

                if (ViewState["RAID"] == null)
                {
                    ViewState["RAID"] = ds.Tables[0].Rows[0]["FLDRAID"].ToString();
                    // gvRiskAssessment.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                // ShowNoRecordsFound(dt, gvRiskAssessment);
            }
            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVITYCONDITIONS", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Category", "Activity / Conditions", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            int vesselid = 0;
            if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
            {
                vesselid = int.Parse(ucVessel.SelectedVessel);
            }
            else
                vesselid = 0;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {

                DataSet ds = PhoenixInspectionRiskAssessment.PhoenixInspectionRiskAssessmentSearch(
                        vesselid,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvRiskAssessment.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableString(txtRefNo.Text),
                        General.GetNullableInteger(ddlStatus.SelectedValue),
                        General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

                General.ShowExcel("Risk Assessment", ds.Tables[0], alColumns, alCaptions, null, null);
            }
            else
            {
                DataSet ds = PhoenixInspectionRiskAssessment.PhoenixInspectionMainFleetRiskAssessmentSearch(
                        General.GetNullableInteger(ucVessel.SelectedVessel),
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvRiskAssessment.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableString(txtRefNo.Text),
                        General.GetNullableInteger(ddlStatus.SelectedValue),
                        General.GetNullableInteger(ViewState["COMPANYID"].ToString()),
                        General.GetNullableString(ddlRAType.SelectedValue));

                General.ShowExcel("Risk Assessment", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRA_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtRefNo", txtRefNo.Text.Trim());
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            Filter.CurrentGenericRAFilter = criteria;
            ViewState["PAGENUMBER"] = 1;
            gvRiskAssessment.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtRefNo.Text = "";
            ddlStatus.SelectedIndex = 0;
            ucVessel.SelectedVessel = "";
            ddlRAType.SelectedIndex = 0;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvRiskAssessment.Rebind();
        }
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

    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblRiskAssessmentID = (RadLabel)e.Item.FindControl("lblRiskAssessmentID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblTypeId = (RadLabel)e.Item.FindControl("lblTypeId");

            LinkButton cmdRA = (LinkButton)e.Item.FindControl("cmdRA");
            if (cmdRA != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRA.CommandName)) cmdRA.Visible = false;
                if (lblTypeId != null && lblTypeId.Text.Equals("1"))
                    cmdRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblRiskAssessmentID.Text + "&showmenu=0&showexcel=NO');return true;");
                if (lblTypeId != null && lblTypeId.Text.Equals("2"))
                    cmdRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblRiskAssessmentID.Text + "&showmenu=0&showexcel=NO');return true;");
                if (lblTypeId != null && lblTypeId.Text.Equals("3"))
                    cmdRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblRiskAssessmentID.Text + "&showmenu=0&showexcel=NO');return true;");
                if (lblTypeId != null && lblTypeId.Text.Equals("4"))
                    cmdRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lblRiskAssessmentID.Text + "&showmenu=0&showexcel=NO');return true;");
            }

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            }

            RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblWorkActivity.ClientID;
            }
        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = e.Item.ItemIndex;

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblRiskAssessmentID");
                RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatus");

                if (e.CommandName.ToUpper().Equals("RA"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                else if (e.CommandName.ToUpper().Equals("DELETERA"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["NROW"] = e.Item.ItemIndex;
                    // ucConfirm.Visible = true;
                    RadWindowManager1.RadConfirm("Are you sure you want to delete this RA.?", "Confirm", 320, 150, null, "Audit/Inspection Plan");
                    // ucConfirm.Text = "Are you sure you want to delete this RA.?";
                    // return;
                }
                BindData();
                gvRiskAssessment.Rebind();
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
            int nRow = int.Parse(ViewState["NROW"].ToString());

            RadLabel lbl = (RadLabel)gvRiskAssessment.Items[nRow].FindControl("lblRiskAssessmentID");
            RadLabel lblVesselid = (RadLabel)gvRiskAssessment.Items[nRow].FindControl("lblVesselid");
            RadLabel lblTypeId = (RadLabel)gvRiskAssessment.Items[nRow].FindControl("lblTypeId");

            PhoenixInspectionRiskAssessment.DeleteRATemplate(int.Parse(lblVesselid.Text),
                new Guid(lbl.Text), int.Parse(lblTypeId.Text));

            ucStatus.Text = "Deleted Successfully";
            gvRiskAssessment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessment.Rebind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessment.Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentID = (RadLabel)gvRiskAssessment.Items[rowindex].FindControl("lblRiskAssessmentID");
            if (lblRiskAssessmentID != null)
            {
                ViewState["RAID"] = lblRiskAssessmentID.Text;
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
        //gvRiskAssessment.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessment.Rows.Count; i++)
        //{
        //    if (gvRiskAssessment.DataKeys[i].Value.ToString().Equals(ViewState["RAID"].ToString()))
        //    {
        //        gvRiskAssessment.SelectedIndex = i;
        //    }
        //}
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
