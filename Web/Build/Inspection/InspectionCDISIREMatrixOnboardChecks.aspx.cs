using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Inspection_InspectionCDISIREMatrixOnboardChecks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {

            ViewState["INSPECTIONID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["DTKEY"] = "";


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

            if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

            BindData();
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string sss = rblbtn1.SelectedValue;

            if (ViewState["contentid"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionCDISIREMatrix.OnboardCheckUpdate(new Guid(ViewState["categoryid"].ToString()), new Guid(ViewState["contentid"].ToString())
                                                                                        , new Guid(ViewState["INSPECTIONID"].ToString())
                                                                                        , General.GetNullableInteger(rblbtn1.SelectedValue)                                                                                                                                                                                
                                                                                        , General.GetNullableString(txtOnboardRemarks.Text)
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , General.GetNullableString(txtOfficeRemarks.Text)                                                                                        
                                                                                        , General.GetNullableInteger(rblofficechecks.SelectedValue));
                    //rblbtn1.SelectedItem.Selected = false;
                    BindData();
                }
            }

            ucStatus.Text = "Saved successfully.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEvidence()
    {
            imgEvidence.Attributes["onclick"] = "openNewWindow('NATD','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                   + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE" + "&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "'); return false;";

    }
    private void BindData()
    {        
        DataTable ds = PhoenixInspectionCDISIREMatrix.EditCDISIREOnboardCheck(new Guid(ViewState["categoryid"].ToString()), new Guid(ViewState["contentid"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Rows.Count > 0)
        {
            DataRow dr = ds.Rows[0];           

            txtOnboardRemarks.Text = dr["FLDREMARKS"].ToString();
            rblbtn1.SelectedValue = dr["FLDISONBOARDCHECK"].ToString();
            txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
            if(dr["FLDISOFFICEACCEPTED"].ToString() == "" || dr["FLDISOFFICEACCEPTED"] == null)
            { 
                rblofficechecks.ClearSelection();
            }
            else
            { 
                rblofficechecks.SelectedValue = dr["FLDISOFFICEACCEPTED"].ToString();
            }
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            txtOnboardDoneBy.Text = dr["FLDONBOARDDONE"].ToString();
            txtOfficeDoneBy.Text = dr["FLDOFFICEDONE"].ToString();
            if (dr["FLDEVIDENCEREQD"].ToString() == "1")
            {
                trevidence.Visible = true;
                rblofficechecks.SelectedValue = "2";
            }
            else
                trevidence.Visible = false;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                txtOfficeRemarks.Enabled = false;
                txtOfficeDoneBy.Enabled = false;
                rblofficechecks.Enabled = false;
            }
        }
        SetEvidence();
    }
}