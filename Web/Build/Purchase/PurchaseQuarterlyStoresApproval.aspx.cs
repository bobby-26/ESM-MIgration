using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;
using System.Text;

public partial class Purchase_PurchaseQuarterlyStoresApproval : PhoenixBasePage
{
	string sessionId = "";
	protected void Page_Load(object sender, EventArgs e)
	{
		
		if(!IsPostBack)
		{
			sessionId = Request.QueryString["sessionId"].ToString();
			sessionValidation();
		}
	}
	private void sessionValidation()
	{
		try
		{
			int isvalid = 0;
			PhoenixPurchaseGroupedStore.ApprovalValidation(new Guid(sessionId), ref isvalid);
			if (isvalid == 1)
			{
				//string result;
				RequisitionCreate();
				Response.Write("Requisitions created in parked status successfully ");
				//Response.Write(result);
			}
			else
			{
				Response.Write("Requisition already created");
			}
		}
		catch(Exception ex)
		{
			Response.Write(ex.Message);
		}
	}
	private string RequisitionCreate()
	{
		DataTable dt;
		dt = PhoenixPurchaseGroupedStore.RequisitionCreate(new Guid(sessionId));
		int i = 0;
		StringBuilder mailbody = new StringBuilder();
		string reqnos = "";
		if (dt.Rows.Count > 0)
		{
			mailbody.Append("<table border='1px'>");
			mailbody.Append("<tr>");
			mailbody.Append("<td>Form No.</td>");

			mailbody.Append("</tr>");
			while (i < dt.Rows.Count)
			{

				mailbody.Append("<tr>");
				mailbody.Append("<td>" + dt.Rows[i]["FLDFORMNO"].ToString() + "</td>");
				mailbody.Append("</tr>");
				reqnos = reqnos + dt.Rows[i]["FLDFORMNO"].ToString() + ",";
				i = i + 1;
				
			}
			mailbody.Append("</table>");
			DataRow dr = dt.Rows[0];
			vesselEmail(reqnos.TrimEnd(','), dr["FLDVESSELEMAIL"].ToString(), dr["FLDSUPTNAME"].ToString(), dr["FLDSUPTEMAIL"].ToString());
		}
		return mailbody.ToString();
	}
	private void vesselEmail(string reqNos,string toemail,string supntname,string supntemail)
	{
		string subject = "Quarterly Stores Requisition";
		StringBuilder mailbody = new StringBuilder();
		mailbody.Append("Good Day Captain,");
		mailbody.Append("<br>Please find list of Quarterly stores. ");
		mailbody.Append("<br>We have created office reqn for deck / engine and galley store from our end with numbers ");
		mailbody.Append("(" + reqNos + ").");
		mailbody.Append("<br>Please update the Quarterly store list for Deck, Engine and Galley.Amend the quantities as required.");
		mailbody.Append("<br>You will be prompted to enter a comment while; Increasing the Qty or adding a new item.Please fill up appropriate comments for Superintendents to approve.");
		mailbody.Append("<br>Stores will preferably be supplied in earliest key port so please also raise reqn for lubs, chemical, paints and other items as required such that next supply for these items will be made ONLY after four month.");
		mailbody.Append("<br>Please send the requirement soonest.");
		mailbody.Append("<br><br>Note : Above requisitions will be reflected in vessel in next office import.");
		mailbody.Append("<br>This is an automated email,Please DO NOT reply to this email.");
		mailbody.Append("<br><br>Thanks and Regards,");
		mailbody.Append("<br>" + supntname);
		mailbody.Append("<br>Email :" + supntemail);

		PhoenixMail.SendMail(toemail.Replace(";", ",").TrimEnd(','),
					null,
					null,
					subject,
					mailbody.ToString(),
					true,
					System.Net.Mail.MailPriority.Normal,
					"",
					null,
					null);

	}
}
