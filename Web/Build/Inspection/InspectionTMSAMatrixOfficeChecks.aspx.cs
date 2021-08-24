using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Inspection_InspectionTMSAMatrixOfficeChecks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {

            ViewState["INSPECTIONID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["DTKEY"] = "";
            ViewState["ARCHIVED"] = "";
            ViewState["INSPECTIONSCHEDULEID"] = "";


            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
            {
                ViewState["categoryid"] = Request.QueryString["categoryid"].ToString();
            }
            if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != string.Empty)
            {
                ViewState["contentid"] = Request.QueryString["contentid"].ToString();
            }

            if (Request.QueryString["dtkey"] != null && Request.QueryString["dtkey"].ToString() != string.Empty)
            {
                ViewState["DTKEY"] = Request.QueryString["dtkey"].ToString();
            }

            if (Request.QueryString["Archived"] != null && Request.QueryString["Archived"].ToString() != string.Empty)
            {
                ViewState["ARCHIVED"] = Request.QueryString["Archived"].ToString();
            }

            if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

            if (Request.QueryString["inspectionscheduleid"] != null && Request.QueryString["inspectionscheduleid"].ToString() != string.Empty)
                ViewState["INSPECTIONSCHEDULEID"] = Request.QueryString["inspectionscheduleid"].ToString();

            BindPlannedInspection();
            BindData();
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (Request.QueryString["Archived"] == null || Request.QueryString["Archived"].ToString() == string.Empty)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["contentid"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionTMSAMatrix.OfficeCheckUpdate(new Guid(ViewState["categoryid"].ToString()), new Guid(ViewState["contentid"].ToString())
                                                                                        , new Guid(ViewState["INSPECTIONID"].ToString())
                                                                                        , General.GetNullableString(txtOfficeRemarks.Text)
                                                                                        , General.GetNullableInteger(rblofficechecks.SelectedValue)
                                                                                        , new Guid(ddlplannedaudit.SelectedValue)
                                                                                        );
                    //rblbtn1.SelectedItem.Selected = false;
                    BindData();
                }
            }

            ucStatus.Text = "Saved successfully.";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList();", true);
            //String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindPlannedInspection()
    {
        DataSet ds = PhoenixInspectionTMSAMatrix.ListTMSAMatrixPlannedAuditList(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                                                                                    General.GetNullableInteger(ViewState["ARCHIVED"].ToString())
                                                                                    );

        ddlplannedaudit.DataSource = ds.Tables[0];
        ddlplannedaudit.DataTextField = "FLDINSPECTION";
        ddlplannedaudit.DataValueField = "FLDREVIEWSCHEDULEID";
        ddlplannedaudit.DataBind();
        //ddlplannedaudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindData()
    {

        if (Request.QueryString["Archived"] != null && Request.QueryString["Archived"].ToString() != string.Empty)
        {
            DataTable ds = PhoenixInspectionTMSAMatrix.EditTMSAArchiveOfficeCheck(new Guid(ViewState["categoryid"].ToString()), 
                                                                                new Guid(ViewState["contentid"].ToString()),
                                                                                new Guid(ViewState["INSPECTIONSCHEDULEID"].ToString())
                                                                                );
            if (ds.Rows.Count > 0)
            {
                DataRow dr = ds.Rows[0];
                txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
                rblofficechecks.SelectedValue = dr["FLDISOFFICEACCEPTED"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                txtOfficeDoneBy.Text = dr["FLDOFFICEDONE"].ToString();
                ddlplannedaudit.SelectedValue = dr["FLDINSPECTIONSCHEDULEID"].ToString();
                ddlplannedaudit.Enabled = false;
            }
        }
        else
        {
            DataTable ds = PhoenixInspectionTMSAMatrix.EditTMSAOfficeCheck(new Guid(ViewState["categoryid"].ToString()),
                                                                            new Guid(ViewState["contentid"].ToString()),
                                                                            new Guid(ddlplannedaudit.SelectedValue)
                                                                            );
            if (ds.Rows.Count > 0)
            {
                DataRow dr = ds.Rows[0];
                txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
                if (dr["FLDISOFFICEACCEPTED"].ToString() == "")
                {
                    rblofficechecks.ClearSelection();
                }
                else
                    rblofficechecks.SelectedValue = dr["FLDISOFFICEACCEPTED"].ToString();

                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                txtOfficeDoneBy.Text = dr["FLDOFFICEDONE"].ToString();
            }
        }

    }

    protected void ddlplannedaudit_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindData();
    }
}