using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionAuditInterfaceChecklist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["FLDREVIEWNAME"] = "AudiT Inspection";
                ViewState["MAPPINGID"] = "";
                ViewState["CHAPTER"] = string.Empty;
                ViewState["REVIEWSCHEDULEID"] = "";
                ViewState["AuditDate"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["INSPECTIONID"] = "";
                ViewState["SHORTCODE"] = "";
                ViewState["VesselId"] = "";
                ViewState["QUESTIONTYPE"] = "";

                if (Request.QueryString["REVIEWSCHEDULEID"] != null)
                {
                    ViewState["REVIEWSCHEDULEID"] = Request.QueryString["REVIEWSCHEDULEID"].ToString();
                }

                if (Request.QueryString["INSPECTIONID"] != null)
                {
                    ViewState["INSPECTIONID"] = "";
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();
                }

                if (Request.QueryString["SHORTCODE"] != null)
                {
                    ViewState["SHORTCODE"] = Request.QueryString["SHORTCODE"].ToString();
                }

                if (Request.QueryString["VesselId"] != null)
                {
                    ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
                    BindVesselList();
                }

                if (Request.QueryString["QUESTIONTYPE"] != null)
                {
                    ViewState["QUESTIONTYPE"] = Request.QueryString["QUESTIONTYPE"].ToString();

                    BindQuestionType();
                }


                gvInspectionCheckItems.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindLocation();
                BindResponsibility();
                Filter.CurrentInspectionChapter = null;
            }
            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddFontAwesomeButton("../Inspection/InspectionAuditInterfaceChecklist.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            tool.AddFontAwesomeButton("../Inspection/InspectionAuditInterfaceChecklist.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuInterfaceSearch.AccessRights = this.ViewState;
            MenuInterfaceSearch.MenuList = tool.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindVesselList()
    {
        DataTable dt;
        dt = PhoenixInspectionAuditInterfaceDetails.InspectionVesselList(General.GetNullableInteger(ViewState["VesselId"].ToString())
                                        , General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                        , General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
            );
        if (dt.Rows.Count > 0)
        {

            DataRow dr = dt.Rows[0];
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtType.Text = dr["FLDVESSELTYPE"].ToString();
            txtFlag.Text = dr["FLDVESSELFLAG"].ToString();
            txtMaster.Text = dr["MASTERNAME"].ToString();
            txtChiefEngineer.Text = dr["CHIEFENGINEER"].ToString();
            txtAuditor.Text = dr["AUDITORNAME"].ToString();
            txtAuditDate.Text = General.GetDateTimeToString(dr["AUDITDATE"].ToString());
            ViewState["AuditDate"] = General.GetDateTimeToString(dr["AUDITDATE"].ToString());
            txtInspection.Text = ViewState["SHORTCODE"].ToString();

        }
    }


    private void BindQuestionType()
    {
        DataTable ds = new DataTable();
        btnAnsSearch.DataSource = PhoenixInspectionAuditInterfaceDetails.InspectionCheckitemAnswer(General.GetNullableInteger(ViewState["QUESTIONTYPE"].ToString()));
        btnAnsSearch.DataBind();
    }


    private void BindLocation()
    {
        ddlLocation.DataSource = PhoenixInventoryComponent.GetLocationComponents();
        ddlLocation.DataTextField = "FLDCOMPONENTNAME";
        ddlLocation.DataValueField = "FLDGLOBALCOMPONENTID";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, new RadComboBoxItem("--Select--"));
    }

    private void BindResponsibility()
    {
        ddlResponsible.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlResponsible.DataTextField = "FLDGROUPRANK";
        ddlResponsible.DataValueField = "FLDGROUPRANKID";
        ddlResponsible.DataBind();
        ddlResponsible.Items.Insert(0, new RadComboBoxItem("--Select--"));
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
    protected void Rebind()
    {
        gvInspectionCheckItems.SelectedIndexes.Clear();
        gvInspectionCheckItems.EditIndexes.Clear();
        gvInspectionCheckItems.DataSource = null;
        gvInspectionCheckItems.Rebind();

        gvDeficiency.SelectedIndexes.Clear();
        gvDeficiency.EditIndexes.Clear();
        gvDeficiency.DataSource = null;
        gvDeficiency.Rebind();
    }

    protected void MenuInterfaceSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                gvInspectionCheckItems.Rebind();
            }

            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["CHAPTER"] = string.Empty;
                ddlResponsible.ClearSelection();
                ddlLocation.ClearSelection();
                txtItem.Text = "";
                btnPendingComplete.SelectedIndex = -1;
                btnAnsSearch.SelectedIndex = -1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void btnPendingComplete_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvInspectionCheckItems.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnAnsSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvInspectionCheckItems.Rebind();
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
            ViewState["PAGENUMBER"] = 1;
            Filter.CurrentInspectionChapter = null;
            Filter.CurrentInspectionVesselCheckItem = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    #region Grid Deficiency
    protected void BindDeficiency()
    {
        try
        {
            DataSet ds = PhoenixInspectionAuditInterfaceDetails.DeficiencyCategory(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                            , General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                                            , General.GetNullableInteger(ViewState["VesselId"].ToString()));

            gvDeficiency.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDeficiency();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CATEGORY"))
            {
                ViewState["CHAPTER"] = ((RadLabel)e.Item.FindControl("lblChapterId")).Text;

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    #endregion

    #region Bind CheckItem
    private void BindCheckItems()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet dt = PhoenixInspectionAuditInterfaceDetails.SearchCheckItems(

                         General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                       , General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                       , General.GetNullableInteger(ViewState["VesselId"].ToString())
                       , General.GetNullableString(txtItem.Text)
                       , General.GetNullableGuid(ViewState["CHAPTER"].ToString() == null ? null : ViewState["CHAPTER"].ToString())
                       , General.GetNullableInteger(ddlResponsible.SelectedValue)
                       , General.GetNullableGuid(ddlLocation.SelectedValue)
                       , General.GetNullableInteger(btnPendingComplete.SelectedValue)
                       , General.GetNullableInteger(btnAnsSearch.SelectedValue)
                       , gvInspectionCheckItems.CurrentPageIndex + 1
                       , gvInspectionCheckItems.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount
                       );

        gvInspectionCheckItems.DataSource = dt;
        gvInspectionCheckItems.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


    }
    protected void gvInspectionCheckItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionCheckItems.CurrentPageIndex + 1;
            BindCheckItems();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspectionCheckItems_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblPscViqid = (RadLabel)e.Item.FindControl("lblPscViqid");
                if (drv["FLDJOBDUE"].ToString().Equals("1"))
                {
                    if (lblPscViqid != null)
                    {
                        lblPscViqid.ToolTip = "Component Job Overdue";
                        lblPscViqid.Attributes["style"] = "color:Red !important";
                        lblPscViqid.Font.Bold = true;
                    }
                }

                RadRadioButtonList gre = (RadRadioButtonList)e.Item.FindControl("rblAnswer");
                if (drv["FLDQUESTIONTYPE"].ToString() != null)
                {
                    DataTable ds = new DataTable();
                    ds = PhoenixInspectionAuditInterfaceDetails.InspectionCheckitemAnswer(General.GetNullableInteger(drv["FLDQUESTIONTYPE"].ToString()));
                    gre.DataSource = ds;
                    gre.DataBind();
                    gre.SelectedIndex = -1;
                }

                if (gre != null)
                {
                    gre.SelectedValue = drv["FLDANSWERID"].ToString();
                }
                Guid? CheckItemId = General.GetNullableGuid(((RadLabel)item.FindControl("lblCheckItemid")).Text);
                //Guid? VesselCheckItemId = General.GetNullableGuid(((RadLabel)item.FindControl("lblVesselCheckitemId")).Text);
                LinkButton Rtn = item.FindControl("Cmdjob") as LinkButton;
                string Job = (((RadLabel)item.FindControl("lblJobs")).Text);

                if (Rtn != null && Job == "" && Job == string.Empty)
                {
                    Rtn.Visible = false;
                }
                else
                {
                    Rtn.Visible = SessionUtil.CanAccess(this.ViewState, Rtn.CommandName);
                    Rtn.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Job','Inspection/InspectionAuditInterfaceJobMapping.aspx?CHECKITEMID=" + CheckItemId + "&VesselId=" + ViewState["VesselId"].ToString() + "');return false");
                }

                LinkButton btn = item.FindControl("CmdFile") as LinkButton;
                string Form = (((RadLabel)item.FindControl("lblFormCheckItem")).Text);

                if (btn != null && Form == string.Empty && Form == "")
                {
                    btn.Visible = false;
                }
                else
                {
                    btn.Visible = SessionUtil.CanAccess(this.ViewState, btn.CommandName);
                    btn.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Form','Inspection/InspectionAuditInterfaceFormCheckItem.aspx?CHECKITEMID=" + CheckItemId + "','false','500px','300px');return false");

                }

                LinkButton Dwn = item.FindControl("cmdNote") as LinkButton;
                string Notes = (((RadLabel)item.FindControl("lblGuidenceNote")).Text);

                if (Dwn != null && Notes == "" && Notes == string.Empty)
                {
                    Dwn.Visible = false;
                }
                else
                {
                    Dwn.Visible = SessionUtil.CanAccess(this.ViewState, Dwn.CommandName);
                    Dwn.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Guidance Note','Inspection/InspectionAuditInterfaceGuidanceNote.aspx?CHECKITEMID=" + CheckItemId + "','false','400px','90px');return false");
                }


                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                RadLabel lblMappingid = (RadLabel)e.Item.FindControl("lblMappingid");
                if (General.GetNullableGuid(lblMappingid.Text) != null)
                {
                    att.Visible = true;
                    if (att != null)
                    {
                        att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                        HtmlGenericControl html = new HtmlGenericControl();

                        if (drv["FLDISATTACHMENT"].ToString() == "0")
                        {
                            html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                            att.Controls.Add(html);
                        }
                        else
                        {
                            html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                            att.Controls.Add(html);
                        }

                        att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString()
                                    + "&mod=" + PhoenixModule.QUALITY
                                    + "&type=INSPECTIONCHECKITEM"
                                    + "&cmdname=AUDITINSPECTIONUPLOAD"
                                    + "&VESSELID=" + ViewState["VesselId"].ToString()
                                    + "'); return true;");

                    }
                }
                else
                {
                    att.Visible = false;
                }

                LinkButton Rmk = (LinkButton)e.Item.FindControl("cmdRemarks");

                Guid? ChapterId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblChapter")).Text);

                if (Rmk != null && drv["FLDMAPPINGID"].ToString() == "" && drv["FLDMAPPINGID"].ToString() == string.Empty)
                {
                    Rmk.Visible = false;
                }
                else
                {
                    Rmk.Visible = SessionUtil.CanAccess(this.ViewState, Rmk.CommandName);
                    Rmk.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Remarks','Inspection/InspectionAuditInterfaceRemarks.aspx?Reviewscheduleid=" + ViewState["REVIEWSCHEDULEID"].ToString() + "&CHECKITEMID=" + CheckItemId + "&MappingId=" + drv["FLDMAPPINGID"].ToString() + "&CHAPTER=" + ChapterId + " ','false','400px','250px');return false");

                }

                LinkButton Hst = (LinkButton)e.Item.FindControl("cmdHistory");
                if (Hst != null && drv["FLDMAPPINGID"].ToString() == "" && drv["FLDMAPPINGID"].ToString() == string.Empty)
                {
                    Hst.Visible = false;
                }
                else
                {
                    Hst.Visible = SessionUtil.CanAccess(this.ViewState, Hst.CommandName);
                    Hst.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Remarks','Inspection/InspectionAuditInterfaceHistory.aspx?REVIEWSCHEDULEID=" + ViewState["REVIEWSCHEDULEID"].ToString() + "&MAPPINGID=" + drv["FLDMAPPINGID"].ToString() + "&VESSELID=" + ViewState["VesselId"].ToString() + "','false','500px','300px');return false");
                }
            }

        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rblAnswer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            RadRadioButtonList rblAnswer = (RadRadioButtonList)sender;
            GridDataItem item = (GridDataItem)rblAnswer.Parent.Parent;

            RadLabel QuestionType = (RadLabel)item.FindControl("lblQuestionType");
            RadLabel CheckItemId = (RadLabel)item.FindControl("lblCheckItemid");
            RadLabel ChapterId = (RadLabel)item.FindControl("lblChapter");
            RadLabel lblMappingid = (RadLabel)item.FindControl("lblMappingid");
            //RadRadioButtonList Answer = (RadRadioButtonList)item.FindControl("rblAnswer");
            int? No = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(188, "NO"));
            int? Poor = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(189, "POOR"));
            string Remark = ((RadLabel)item.FindControl("lblRemarks")).Text;

            if (General.GetNullableInteger(rblAnswer.SelectedValue) != null && (General.GetNullableInteger(rblAnswer.SelectedValue) != No || General.GetNullableInteger(rblAnswer.SelectedValue) != Poor))
            {
                PhoenixInspectionAuditInterfaceDetails.InspectionInterfaceCheckItemInsert(
                General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
              , General.GetNullableGuid(CheckItemId.Text)
              , General.GetNullableInteger(QuestionType.Text)
              , null
              , General.GetNullableInteger(rblAnswer.SelectedValue)
              , General.GetNullableGuid(ChapterId.Text)
              , General.GetNullableGuid(lblMappingid.Text)
               );

                Rebind();

            }

            if (General.GetNullableInteger(rblAnswer.SelectedValue) == No || General.GetNullableInteger(rblAnswer.SelectedValue) == Poor)
            {
                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', 'Inspection/InspectionAuditInterfaceRemarks.aspx?Reviewscheduleid=" + ViewState["REVIEWSCHEDULEID"].ToString() + "&CHECKITEMID=" + CheckItemId.Text + "&CHAPTER=" + ChapterId.Text + "','false','450px','250px');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    #endregion
    

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Inspection/InspectionAuditInterface.aspx");
    }

    protected void btnBulk_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionAuditInterfaceDetails.InspectionInterfaceCheckItemUpload(General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString()));

            ucStatus.Text = "Uploaded Successfully";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}

