using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using Telerik.Web.UI;
public partial class Crew_CrewTravelRequestAttachmentViewDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                if (Request.QueryString["TICKETNO"].ToString() != null && Request.QueryString["ATTACHMENT"].ToString() != null)
                {
                    ViewState["TICKETNO"] = Request.QueryString["TICKETNO"].ToString();
                    ViewState["ATTACHMENT"] = Request.QueryString["ATTACHMENT"].ToString();
                }
            }
            BindData();
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    private void BindData()
    {
        try
        {
            string Attachment = (ViewState["ATTACHMENT"] == null) ? null : (ViewState["ATTACHMENT"].ToString());

            string TicketNo = (ViewState["TICKETNO"] == null) ? null : (ViewState["TICKETNO"].ToString());

            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelAttachmentMappingSearch(General.GetNullableGuid(Attachment), TicketNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string filepath = ds.Tables[0].Rows[0]["FLDFILEPATH"].ToString();

                if (ViewState["ATTACHMENT"] != null && ViewState["ATTACHMENT"].ToString() != "")
                {
                    ifMoreInfo.Attributes["src"] = "../Common/download.aspx?dtkey=" + General.GetNullableGuid(ViewState["ATTACHMENT"].ToString());
                }

                //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
              
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
    }

}