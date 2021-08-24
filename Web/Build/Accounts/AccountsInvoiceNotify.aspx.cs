using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;

public partial class Accounts_AccountsInvoiceNotify : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddButton("Back", "BACK");
        toolbar.AddButton("Send Mail", "SENDMAIL");

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.MenuList = toolbar.Show();
        MenuInvoice1.SetTrigger(pnlInvoice);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        if (!IsPostBack)
        {
            if (Request.QueryString["INVOICENUMBER"] != null)
            {
                ViewState["INVOICENUMBER"] = Request.QueryString["INVOICENUMBER"].ToString();
            }
            if (Request.QueryString["DTKEY"] != null)
            {
                ViewState["DTKEY"] = Request.QueryString["DTKEY"].ToString();
            }
            if (Request.QueryString["INVOICECODE"] != null && Request.QueryString["INVOICECODE"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["INVOICECODE"];
                DataTable dt = PhoenixRegistersVessel.ListVessel(null, "", 1).Tables[0];
                chkVesselList.DataSource = dt;                
                chkVesselList.DataBind();
            }
            InvoiceEdit();
        }
        
    }
    private void SortSelectedVesselList()
    {
        
        DataTable dtSelected = new DataTable();
        dtSelected.Columns.Add("ItemID", Type.GetType("System.String"));
        dtSelected.Columns.Add("ItemName", Type.GetType("System.String"));
        DataTable dtNonSelected = new DataTable();
        dtNonSelected.Columns.Add("ItemID", Type.GetType("System.String"));
        dtNonSelected.Columns.Add("ItemName", Type.GetType("System.String"));
        if (dtSelected != null && dtNonSelected != null)
        {
            foreach (ListItem item in chkVesselList.Items)
            {
                if (item.Selected)
                {
                    DataRow row = dtSelected.NewRow();
                    row["ItemID"] = item.Value.ToString();
                    row["ItemName"] = item.Text;
                    dtSelected.Rows.Add(row);
                }
                else
                {
                    DataRow row = dtNonSelected.NewRow();
                    row["ItemID"] = item.Value.ToString();
                    row["ItemName"] = item.Text;
                    dtNonSelected.Rows.Add(row);
                }
            }

            if (dtSelected.Rows.Count > 0)
            {
                DataView SelectedView = dtSelected.DefaultView;
                SelectedView.Sort = "ItemName ASC";
                dtSelected = SelectedView.ToTable();
                if (dtNonSelected.Rows.Count > 0)
                {
                    DataView NonSelectedView = dtNonSelected.DefaultView;
                    NonSelectedView.Sort = "ItemName ASC";
                    dtNonSelected = NonSelectedView.ToTable();
                    dtSelected.Merge(dtNonSelected);
                }
                //re-bind to show selections at uppermost 
                chkVesselList.DataSource = dtSelected;
                chkVesselList.DataTextField = "ItemName";
                chkVesselList.DataValueField = "ItemID";
                chkVesselList.DataBind();
            }
        } 
    }
    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
       try
	   {
		if (dce.CommandName.ToUpper().Equals("SENDMAIL"))
		{
			SendNotificationMail();
		}
		else if (dce.CommandName.ToUpper().Equals("BACK"))
		{
			Response.Redirect("../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["INVOICECODE"], false);
		}
	   }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
			return;
		}
    }
    protected void InvoiceEdit()
    {
        if (ViewState["INVOICECODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));            
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                
                txtTempSupplierName.Text = drInvoice["FLDTEMPSUPPLIERNAME"].ToString();
                SetValueCheckBoxList(chkVesselList, drInvoice["FLDVESSELLIST"].ToString());
                SortSelectedVesselList();
                SetValueCheckBoxList(chkVesselList, drInvoice["FLDVESSELLIST"].ToString());                        
            }
        }
    }
    public void SetValueCheckBoxList(CheckBoxList cbl, string sValues)
    { 
            if (!string.IsNullOrEmpty(sValues))
            { 
                    ArrayList values = StringToArrayList(sValues); 
                    foreach (ListItem li in cbl.Items) 
                    { 
                        if (values.Contains(li.Value))                         
                        li.Selected = true; 
                        else                         
                        li.Selected = false; 
                    } 
            } 
    }  
    private ArrayList StringToArrayList(string value) 
    { 
            ArrayList _al = new ArrayList(); 
            string[] _s = value.Split(new char[] { ',' }); 
            foreach (string item in _s) 
                _al.Add(item); return _al; 
    }
	private void SendNotificationMail()
	{
			string strPath = HttpContext.Current.Server.MapPath("~/Attachments/");
			int iArrLength = 0;
			DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["DTKEY"].ToString()));
			string[] strarrfilenames = new string[dt.Rows.Count];
            string to, cc, bcc, mailsubject, mailbody, sessionid = string.Empty;
			bool htmlbody = true;
			MailPriority priority = MailPriority.Normal;
			int? iEmployeeId = null;
			//DataSet ds = null;
			foreach (DataRow dr in dt.Rows)
			{

				string strCurfilename = string.Empty;
				strCurfilename = dr["FLDFILEPATH"].ToString();
				strCurfilename = strCurfilename.Replace("/", "\\");
				strCurfilename = strPath + strCurfilename;
				strarrfilenames[iArrLength] = strCurfilename;
				iArrLength++;
			}
			string vessellist = "";
            string vesselnamelist = "";
			foreach (ListItem item in chkVesselList.Items)
			{
				if (item.Selected)
					vessellist = vessellist + item.Value + ",";
			}
            foreach (ListItem item in chkVesselList.Items)
            {
                if (item.Selected)
                    vesselnamelist = vesselnamelist + item.Text + ",";
            }
            vesselnamelist = vesselnamelist.TrimEnd(',');
			if (vessellist.ToString() == "")
			{
				ucError.ErrorMessage = "Please Select Atleast One Vessel";
				ucError.Visible = true;
				return;
			}
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                mailbody = "Dear Sir/Madam, " + "<br /><br />";
                mailbody = mailbody + "<table><tr><td>Invoice Number </td><td>: </td><td> " + drInvoice["FLDINVOICENUMBER"].ToString()
                    + "</td><td> Invoice Status</td><td>: </td><td>" + drInvoice["FLDINVOICESTATUSNAME"].ToString()
                    + "</td></tr><tr><td>Supplier </td><td>: </td><td> " + drInvoice["FLDCODE"].ToString() + "-" + drInvoice["FLDNAME"].ToString()
                    + "</td><td> Vendor Invoice Number</td><td>: </td><td>" + drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString()
                    + "</td></tr><tr><td>Invoice Date  </td><td>: </td><td> " + General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString())
                    + "</td><td> Date Received </td><td>: </td><td>" + General.GetDateTimeToString(drInvoice["FLDINVOICERECEIVEDDATE"].ToString())
                    + "</td></tr><tr><td>Amount </td><td>: </td><td> " + string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]))
                    + "</td><td> Remarks</td><td>: </td><td>" + drInvoice["FLDREMARKS"].ToString()
                    + "</td></tr></table>";

                mailbody = mailbody + "<br />";
                mailbody = mailbody + "<table><tr><td> Vessels Involved : </td><td><tr><td>" + vesselnamelist + "</td></tr></table>";
                mailbody = mailbody + "<br /><br />";
                mailbody = mailbody + "<table><tr><td>Regards</td></tr><tr><td>" + PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString() + "</td></tr></table>";

                DataTable dte = PhoenixAccountsInvoice.VesselEmailForInvoice(General.GetNullableString(vessellist), General.GetNullableGuid(ViewState["INVOICECODE"].ToString()));
                if (dte.Rows.Count > 0)
                {

                    if (dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"] != null && !string.IsNullOrEmpty(dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"].ToString()))
                    {
                        to = dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"].ToString();
                        cc = dte.Rows[0]["FLDFLEETMGREMAIL"] != null ? dte.Rows[0]["FLDFLEETMGREMAIL"].ToString() : "";
                        bcc = "";
                        mailsubject = "Notification for the Invoice received without PO Number and registered same.";
                        //mailbody = "Notification for the missing Supplier details for the Invoice - " + ViewState["INVOICENUMBER"].ToString() + " By - " + PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString();                       	
                        PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid, iEmployeeId);
                        PhoenixAccountsInvoice.InvoiceNotifyDetailsUpdate(new Guid(ViewState["INVOICECODE"].ToString()), txtTempSupplierName.Text, vessellist, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        ucStatus.Text = "Email sent successfully";
                    }
                }
            }

			InvoiceEdit();
			
		
	}  
}
