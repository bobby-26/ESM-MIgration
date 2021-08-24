using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionAdminCARList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminCARList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminCARList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuCAR.AccessRights = this.ViewState;
        MenuCAR.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Incident/NearMiss Deletion", "INCDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task Deletion", "CARDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Date Change", "RADATECHANGE", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Deletion", "RADELETION", ToolBarDirection.Right);

        MenuCARMain.AccessRights = this.ViewState;
        MenuCARMain.MenuList = toolbarmain.Show();
        MenuCARMain.SelectedMenuIndex = 1;


        if (!IsPostBack)
        {
            gvShipBoardTasks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ucConfirm.Attributes.Add("style", "display:none");
            ucConfirmDelete.Attributes.Add("style", "display:none");


            ViewState["COMPANYID"] = "";
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
        }
        //BindData();
    }

    protected void MenuCARMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RADATECHANGE"))
        {
            Response.Redirect("../Inspection/InspectionAdminRiskAssessment.aspx", true);
        }
        if (CommandName.ToUpper().Equals("RADELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminRAList.aspx", true);
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

        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTasksSearch(
                                                                  null
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , null
                                                                , null
                                                                , null //General.GetNullableInteger(ddlAcceptance.SelectedValue)
                                                                , null
                                                                , null
                                                                , gvShipBoardTasks.CurrentPageIndex + 1
                                                                , gvShipBoardTasks.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null
                                                                , null
                                                                , General.GetNullableInteger(ddltype.SelectedHard)
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableInteger(ddlSourceType.SelectedValue == "0" ? null : ddlSourceType.SelectedValue)
                                                                , General.GetNullableString(txtSourceRefNo.Text)
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , null
                                                                , null
                                                                );

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    //gvShipBoardTasks.DataSource = ds;
        //    //gvShipBoardTasks.DataBind();

        //    if (ViewState["TASKID"] == null)
        //    {
        //        ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONCORRECTIVEACTIONID"].ToString();                
        //      //  gvShipBoardTasks.SelectedIndex = 0;
        //    }
        //    SetRowSelection();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    //ShowNoRecordsFound(dt, gvShipBoardTasks);
        //}

        gvShipBoardTasks.DataSource = ds;
        gvShipBoardTasks.VirtualItemCount = iRowCount;
    }


    protected void MenuCAR_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            gvShipBoardTasks.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ucVessel.SelectedVessel = "";
            ddlSourceType.SelectedIndex = 0;
            ddltype.SelectedHard = "";
            txtSourceRefNo.Text = "";
            ucVessel.SelectedVessel = "";
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvShipBoardTasks.Rebind();
        }
    }

    protected void gvShipBoardTasks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("DELETECAR"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["NROW"] = e.Item.ItemIndex;
                    //ucConfirm.Visible = true;
                    RadWindowManager1.RadConfirm("Are you sure you want to delete this Corrective Task?", "ConfirmDelete", 320, 150, null, "Delete");
                }
                BindData();
                gvShipBoardTasks.Rebind();
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

    }

    protected void gvShipBoardTasks_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton lnkTaskDetails = (LinkButton)e.Item.FindControl("lnkTaskDetails");
            RadLabel lblTaskDetails = (RadLabel)e.Item.FindControl("lblTaskDetails");
            if (lblTaskDetails != null)
            {
                UserControlToolTip ucToolTip = (UserControlToolTip)e.Item.FindControl("ucToolTip");
                ucToolTip.Position = ToolTipPosition.TopCenter;
                ucToolTip.TargetControlId = lblTaskDetails.ClientID;
            }

            LinkButton lnkTaskSource = (LinkButton)e.Item.FindControl("lnkTaskSource");

            RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
            RadLabel lblSourceType = (RadLabel)e.Item.FindControl("lblSourceType");
            RadLabel lblVesslName = (RadLabel)e.Item.FindControl("lblVesselId");

            if (lnkTaskSource != null)
            {
                if (lblSourceType.Text == "1") //Open Reports
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "2") //Direct
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "3") // Audit/Inspection
                {
                    if (lblVesslName.Text.ToUpper().Equals("OFFICE"))
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    else
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                }
                if (lblSourceType.Text == "4") //Vetting
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "5") //Incident
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionIncidentView.aspx?incidentid=" + lblSourceId.Text + "'); return true;");
                }
                if (lblSourceType.Text == "6") //machinery damage
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageGeneral.aspx?VIEWONLY=1&MACHINERYDAMAGEID=" + lblSourceId.Text + "'); return true;");
                }
            }
        }
    }


    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblLongTermActionId = ((RadLabel)gvShipBoardTasks.Items[rowindex].FindControl("lblLongTermActionId"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["NewTask"] != null && Session["NewTask"].ToString() == "Y")
            {
                ViewState["TASKID"] = null;
                Session["NewTask"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            gvShipBoardTasks.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["NROW"] != null && ViewState["NROW"].ToString() != "")
            {
                int nRow = int.Parse(ViewState["NROW"].ToString());

                RadLabel lblLongTermActionId = ((RadLabel)gvShipBoardTasks.Items[nRow].FindControl("lblLongTermActionId"));

                PhoenixInspectionCorrectiveAction.DeleteCorrectiveActionAdmin(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblLongTermActionId.Text));

                ucStatus.Text = "Deleted Successfully";
                gvShipBoardTasks.Rebind();
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
        //gvShipBoardTasks.SelectedIndex = -1;
        //for (int i = 0; i < gvShipBoardTasks.Rows.Count; i++)
        //{
        //    if (gvShipBoardTasks.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
        //    {
        //        gvShipBoardTasks.SelectedIndex = i;
        //    }
        //}
    }

    protected void gvShipBoardTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipBoardTasks.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
