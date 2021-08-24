using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewTravelAgentInvoiceGeneral : PhoenixBasePage
{
  
    decimal? value1, value2,result;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["INVOICEBULK"] = "0";
            ViewState["INVOICEBULK"] = Request.QueryString["INVOICEBULK"].ToString();
            PopulateData();
            InvoiceDifference();
            CheckData();
            HideLables();
            
        }
    }
    private void PopulateData()
    {
        try
        {
            populateInvoiceData();
            populateTicketData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InvoiceDifference()
    {
        result = ((value1 - value2) / (value2 + (value2 / 100)));
    }
    private void populateInvoiceData()
    {
        DataSet ds = new DataSet();
        if (ViewState["INVOICEBULK"].ToString() == "1")
        {
            ds = PhoenixCrewTravelInvoice.AgentInvoiceDataBulkEdit(new Guid(Request.QueryString["INVOICEID"]));
        }
        else
        ds = PhoenixCrewTravelInvoice.AgentInvoiceDataEdit(new Guid(Request.QueryString["INVOICEID"]),0);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtInvoiceNo.Text = dr["FLDINVOICENUMBER"].ToString();
            txtinvRequestno.Text = dr["FLDREQUISITIONNO"].ToString().Trim();
            txtinvpassname.Text = dr["FLDPASSENGERNAME"].ToString().Trim();
            txtinvticketno.Text = dr["FLDTICKETNO"].ToString().Trim();
            txtinvPNR.Text = dr["FLDPNRNO"].ToString().Trim();
            txtinvdepdate.Text = dr["FLDDEPARTUREDATE"].ToString().Trim();
            txtinvvessel.Text = dr["FLDVESSELNAME"].ToString();
            txtinvairlinecode.Text = dr["FLDAIRLINENUMBER"].ToString().Trim();
            txtinvbasic.Text = dr["FLDBASIC"].ToString().Trim();
            txtinvtax.Text = dr["FLDTOTALTAX"].ToString().Trim();            
            txtinvdiscount.Text = dr["FLDDISCOUNT"].ToString().Trim();
            txtinvcancell.Text = dr["FLDCANCELLATIONCHARGE"].ToString().Trim();
            txtinvtotal.Text = dr["FLDTOTAL"].ToString().Trim();
            txtCreditNote.Text = dr["FLDCREDITNOTE"].ToString().Trim();
            txtAgentRemarks.Text = dr["FLDAGENTREMARKS"].ToString().Trim();
            value1 = General.GetNullableDecimal(dr["FLDTOTAL"].ToString().Trim());
        }
    }
    private void populateTicketData()
    {
        DataSet ds = new DataSet();

        if (ViewState["INVOICEBULK"].ToString() == "1")
        {

            ds = PhoenixCrewTravelInvoice.AgentTicketDataBulkEdit(new Guid(Request.QueryString["INVOICEID"]));
        }
        else
            ds = PhoenixCrewTravelInvoice.AgentTicketDataEdit(new Guid(Request.QueryString["INVOICEID"]),0);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtRequestNo.Text = dr["FLDREQUISITIONNO"].ToString().Trim();
            txtpassengername.Text = dr["FLDPASSENGERNAME"].ToString().Trim();
            txtticket.Text = dr["FLDTICKETNO"].ToString().Trim();
            txtpnr.Text = dr["FLDPNRNO"].ToString().Trim();
            txtdepdate.Text = dr["FLDDEPARTUREDATE"].ToString().Trim();
            txtvessel.Text = dr["FLDVESSELNAME"].ToString().Trim();
            txtairlinecode.Text = dr["FLDAIRLINENUMBER"].ToString().Trim();
            txtbasic.Text = dr["FLDBASIC"].ToString().Trim();
            txttax.Text = dr["FLDTOTALTAX"].ToString().Trim();            
            txtdiscount.Text = dr["FLDDISCOUNT"].ToString().Trim();
            txtTktStatus.Text = dr["FLDTICKETSTATUS"].ToString().Trim();
            txttotal.Text = dr["FLDTOTAL"].ToString().Trim();
            value2 = General.GetNullableDecimal(dr["FLDTOTAL"].ToString().Trim());
        }
    }

    private void CheckData()
    {
        if ((txtinvRequestno.Text.ToString().Trim()).Replace(" ", "") != (txtRequestNo.Text.ToString().Trim()).Replace(" ", ""))
            txtinvRequestno.BorderColor = System.Drawing.Color.Red;        
    }

    private void HideLables()
    {
        lbl1.Attributes.Add("style", "visibility:hidden");
        lbl2.Attributes.Add("style", "visibility:hidden");
        lbl3.Attributes.Add("style", "visibility:hidden");
        lbl4.Attributes.Add("style", "visibility:hidden");
        lbl5.Attributes.Add("style", "visibility:hidden");
        lbl6.Attributes.Add("style", "visibility:hidden");
    }
}
