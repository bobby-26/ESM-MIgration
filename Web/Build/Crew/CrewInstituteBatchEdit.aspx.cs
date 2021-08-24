using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Crew_CrewInstituteBatchEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "LIST",ToolBarDirection.Right);
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarmain.Show();

       

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;         
            txtInstituteId.Attributes.Add("style", "display:none;");           

            SetBatch();
            SetCourse();
            toolbarmain = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbarmain.Show();
        }
       
    }

    private void SetBatch()
    {
        string batchId = Request.QueryString["batchId"];   
        DataTable dt = PhoenixCrewInstituteBatch.CrewInstituteBatchEdit(General.GetNullableGuid(batchId));
        if (dt.Rows.Count > 0)
        {
            MenuTitle.Title += "Batch:  " + dt.Rows[0]["FLDBATCHNO"].ToString();            
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            txtInstituteName.Text= dt.Rows[0]["FLDNAME"].ToString();
        }
        ProviderDetails(Convert.ToInt64(dt.Rows[0]["FLDVENUE"].ToString()));

    }

    private void SetCourse()
    {
        string  batchId=null;        
        if (Request.QueryString["batchId"] != null)
            batchId = Request.QueryString["batchId"];

        DataTable dt = PhoenixCrewInstituteBatch.CrewBatchCourseMappingEdit(null
                                                                            , General.GetNullableGuid(batchId));
        if (dt.Rows.Count > 0)
        {
            txtCourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtCourse.Text = dt.Rows[0]["FLDABBREVIATION"].ToString() + "-" + dt.Rows[0]["FLDCOURSE"].ToString();
            hdnbatchcoursemappingId.Value = dt.Rows[0]["FLDBATCHCOURSEMAPPINGID"].ToString();
        }
    }

    protected void ProviderDetails(Int64 adddressid)
    {
        DataSet ds = PhoenixRegistersBatch.EditAddress(adddressid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //ucBatchVenue.SelectedAddress = ds.Tables[0].Rows[0]["FLDADDRESSCODE"].ToString();
            txtVenue.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
            txtVenueAddress.Text = ds.Tables[0].Rows[0]["FLDADDRESS"].ToString();
            txtVenuePrimaryContact.Text = ds.Tables[0].Rows[0]["FLDCONTACT"].ToString();
            txtVenueCity.Text = ds.Tables[0].Rows[0]["FLDCITYNAME"].ToString();
            txtVenuePhoneno.Text = ds.Tables[0].Rows[0]["FLDPHONENNO"].ToString();
            txtVenueState.Text = ds.Tables[0].Rows[0]["FLDSTATENAME"].ToString();
            txtVenueEmail.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
            txtVenueCountry.Text = ds.Tables[0].Rows[0]["FLDCOUNTRYNAME"].ToString();
            txtVenuePostalCode.Text = ds.Tables[0].Rows[0]["FLDPOSTALCODE"].ToString();
        }
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewInstituteBatchList.aspx", true);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void MenuCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewInstituteBatchList.aspx" , true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkbtnBatchDuration_Click(object sender, EventArgs e)
    {
        string batchId = Request.QueryString["batchId"].ToString();
        string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewInstituteBatchDurationAdd.aspx?batchId=" + batchId + "&courseId=" + txtCourseId.Text + "&batchcoursemapId=" + hdnbatchcoursemappingId.Value + "');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
    }
}