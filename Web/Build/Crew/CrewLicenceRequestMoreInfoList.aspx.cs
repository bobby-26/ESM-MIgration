using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceRequestMoreInfoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Requestid"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["Requestid"].ToString();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {

        if (ViewState["REQUESTID"] != null)
        {
            DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequest(new Guid(ViewState["REQUESTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                lblCreatedBy.Text = dt.Rows[0]["FLDCREATEDBY"].ToString();
                lblCreatedDate.Text = dt.Rows[0]["FLDCREATEDDATE"].ToString();
                lblRequestSentBy.Text = dt.Rows[0]["FLDREQUESTEDBYNAME"].ToString();
                lblRequestSentDate.Text = dt.Rows[0]["FLDREQUESTEDDATE"].ToString();
                lblReceivedBy.Text = dt.Rows[0]["FLDRECEIVEDBYNAME"].ToString();
                lblReceivedDate.Text = dt.Rows[0]["FLDRECEIVEDDATE"].ToString();
                lblAccountStatus.Text = dt.Rows[0]["FLDACCOUNTSTATUS"].ToString();
                lblRepresentative.Text = dt.Rows[0]["FLDAUTHORIZEDREP"].ToString();
                lblDesignation.Text = dt.Rows[0]["FLDDESIGNATION"].ToString();
            }
        }
    }
}