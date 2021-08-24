using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_InstituteBatchAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarHead = new PhoenixToolbar();

        toolbarHead.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarHead.Show();
        if (!IsPostBack)
        {
            MenuHeader.Title = "Batch Add";
            //cmdHiddenSubmit.Attributes.Add("style", "display:none;");          
            txtInstituteId.Attributes.Add("style", "display:none;");
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
        }

    }
    private bool IsValidBatch()
    {
        ucError.HeaderMessage = "Please provide the following required information";

       
        if (string.IsNullOrEmpty(txtInstituteName.Text) )
        {
            ucError.ErrorMessage = "Institute is required";
        }
        if (ucBatchVenue.SelectedAddress == "Dummy")
        {
            ucError.ErrorMessage = "Venue is required";
        }
        if (string.IsNullOrEmpty(txtCourse.Text))
        {
            ucError.ErrorMessage = "Course is required";
        }

        return (!ucError.IsError);
    }

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidBatch())
                {
                    ucError.Visible = true;
                    return;
                }
                SaveInstituteBatch();
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("CrewInstituteBatchList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindVenueDetails(object sender, EventArgs e)
    {
       
        if (ucBatchVenue.SelectedAddress != "Dummy")
        {
            ProviderDetails(Convert.ToInt64(ucBatchVenue.SelectedAddress));
        }        
       
    }

    protected void ProviderDetails(Int64 adddressid)
    {
        DataSet ds = PhoenixRegistersBatch.EditAddress(adddressid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ucBatchVenue.SelectedAddress = ds.Tables[0].Rows[0]["FLDADDRESSCODE"].ToString();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {             
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            if (nvc != null)
                txtInstituteId.Text = nvc[1];           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SaveInstituteBatch()
    {
        try
        {
            string batchcourseMappingId = null, batchId = null, batchNo = null ;
            if (!IsValidBatch())
            {
                ucError.Visible = true;
                return;
            }
            
            PhoenixCrewInstituteBatch.CrewGenerateBatchNo(General.GetNullableInteger(txtCourseId.Text).Value, ref batchNo);
           
            PhoenixCrewInstituteBatch.CrewInstituteBatchInsert(batchNo
                                                              , General.GetNullableInteger(ucBatchVenue.SelectedAddress).Value
                                                              , General.GetNullableInteger(txtInstituteId.Text)
                                                              , ref batchId);

            PhoenixCrewInstituteBatch.CrewBatchCourseMappingInsert(General.GetNullableGuid(batchId).Value
                                                                  , General.GetNullableInteger(txtCourseId.Text)
                                                                  ,ref batchcourseMappingId);

            Response.Redirect("../Crew/CrewInstituteBatchEdit.aspx?batchId=" + batchId +"&courseId="+txtCourseId.Text+ "&batchcoursemappingId=" + batchcourseMappingId, true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnShowCourse_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtInstituteId.Text))
        {
            ucError.ErrorMessage = "Select the institute";
            ucError.Visible = true;
            return;
        }
        string str = "showPickList('spnPickListCourse', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCourseInstitute.aspx?iframignore=false&instituteId=" + txtInstituteId.Text + "', false);";
         ScriptManager.RegisterStartupScript(this, this.GetType(), "codehelp1",str , true);
        //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "codehelp1", str, false);
    }
}