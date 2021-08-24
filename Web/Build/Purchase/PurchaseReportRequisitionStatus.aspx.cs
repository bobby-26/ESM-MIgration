using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class PurchaseReportRequisitionStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            txtVenderID.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNNOTRECIVED"))
                {
                    Title1.Text = "Quotation Not Received";
                }
                else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNAWTAPP"))
                {
                    Title1.Text = "Quotation Awaiting Approval";
                }
                else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNQUOTED"))
                {
                    Title1.Text = "Quotation Quoted";
                }
                else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("POWATTOBEISSED"))
                {
                    Title1.Text = "PO Waiting To Be Issued";
                }
                else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("POISSUED"))
                {
                    Title1.Text = "PO Issued";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (txtVenderName.Text == "" && txtVenderCode.Text == "")
                        txtVenderID.Text = null;
                    
                    if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNNOTRECIVED"))
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=3&reportcode=QUOTATIONNOTRECEIVED&rowusercode=PhoenixSecurityContext.CurrentSecurityContext.UserCode&PhoenixSecurityContext.CurrentSecurityContext.VesselID&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&vendorid=" + txtVenderID.Text + "&showmenu=0";
                    }
                    else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNAWTAPP"))
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=3&reportcode=QUOTATIONAWAITINGAPPROVAL&rowusercode=PhoenixSecurityContext.CurrentSecurityContext.UserCode&PhoenixSecurityContext.CurrentSecurityContext.VesselID&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&vendorid=" + txtVenderID.Text + "&showmenu=0";
                    }
                    else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("QTNQUOTED"))
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=3&reportcode=QUOTATIONQUOTED&rowusercode=PhoenixSecurityContext.CurrentSecurityContext.UserCode&PhoenixSecurityContext.CurrentSecurityContext.VesselID&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&vendorid=" + txtVenderID.Text + "&showmenu=0";
                    }
                    else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("POWATTOBEISSED"))
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=3&reportcode=POWAITINGTOBEISSUED&rowusercode=PhoenixSecurityContext.CurrentSecurityContext.UserCode&PhoenixSecurityContext.CurrentSecurityContext.VesselID&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&vendorid=" + txtVenderID.Text + "&showmenu=0";                       
                    }
                    else if (Request.QueryString["reqstatus"] == null || Request.QueryString["reqstatus"].ToUpper().Equals("POISSUED"))
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=3&reportcode=POISSUED&rowusercode=PhoenixSecurityContext.CurrentSecurityContext.UserCode&PhoenixSecurityContext.CurrentSecurityContext.VesselID&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&vendorid=" + txtVenderID.Text + "&showmenu=0";      
                    }         
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }

}
