using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using System.Web.UI;

public partial class VesselPositionDepartureReviewMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send E.Mail", "MAIL",ToolBarDirection.Right);
       // toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right); Review.AccessRights = this.ViewState;
        Review.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["date"]= "";
            bind();
        }
    }
    protected void bind()
    {
        DataSet ds = PhoenixVesselPositionNoonReport.DeaprtureReportReviewList(General.GetNullableInteger(Request.QueryString["VesselId"]), General.GetNullableGuid(Request.QueryString["NoonReportID"]));
        string html = "";
        html = "<html>Dear Captain,<br /><br />Your Departure Report of " + DateTime.Parse( ds.Tables[1].Rows[0]["FLDDATE"].ToString()).ToString("dd MMM yyyy") + " has been well received. We note the following parameters are not within limits.<br /><br />";
        html = html + "<table border='1' cellspacing='0' width='600px'><tr><td style='width: 200px; text-align-last:left; '>Parameter</td><td style='width: 200px; text-align-last:center; '>Reported Value</td><td style='width: 100px; text-align-last:center; '>Min</td><td style='width: 100px; text-align-last:center; '>Max</td></tr>";
        
        ViewState["date"] = ds.Tables[1].Rows[0]["FLDDATE"].ToString();
        if (ds.Tables[0].Rows.Count>0)
        {
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                html = html + "<tr><td style='width:200px;'>"+dr["FLDPARAMETERNAME"].ToString()+ "</td><td style='color:Red; width:200px; text-align-last:center;'>" + dr["FLDREPORTEDVALUE"].ToString() + "</td><td style='width:100px; text-align-last:center;'>" + dr["FLDMIN"].ToString() + "</td><td style='width:100px; text-align-last:center;'>" + dr["FLDMAX"].ToString() + "</td></tr>";
            }
        }
        html = html + "</table>";
        html = html + "<br />Request your kind response on reason for same and actions being taken.<br /><br />Best Regards.<br /><br />(" + ds.Tables[1].Rows[0]["FLDUSER"].ToString()+ ")</html>";
        
        txtMailContent.Content = html;
    }

    protected void Review_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("MAIL"))
            {
                PhoenixVesselPositionNoonReport.NoonReportReviewUpdate(General.GetNullableInteger(Request.QueryString["VesselId"]), General.GetNullableGuid(Request.QueryString["NoonReportID"]));
                //PhoenixVesselPositionNoonReport.NoonReportReviewMailSend(General.GetNullableInteger(Request.QueryString["VesselId"]), General.GetNullableGuid(Request.QueryString["NoonReportID"]), General.GetNullableDateTime(ViewState["date"].ToString()), txtMailContent.Content.ToString());
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Mail', 'Filter');", true);
                DataSet ds = PhoenixVesselPositionNoonReport.Getmaildetail(General.GetNullableInteger(Request.QueryString["VesselId"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string subject = "Departure Report " + DateTime.Parse(ViewState["date"].ToString()).ToString("dd MMM yyyy hh:mm") + "- Review";

                    PhoenixMail.SendMail(dr["FLDTOMAIL"].ToString().Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                    dr["FLDCCMAIL"].ToString().Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                    "",
                    subject,
                    txtMailContent.Content.ToString(), true,
                    System.Net.Mail.MailPriority.Normal,
                    "",
                    null,
                    null);

                }
                ucStatus.Text = "Mail Sent.";
                string script = "closeTelerikWindow('Filter', 'codehelp1')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
            }
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string script = "closeTelerikWindow('Filter', 'review')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
