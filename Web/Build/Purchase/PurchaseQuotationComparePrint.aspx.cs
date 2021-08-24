using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Purchase;

public partial class Purchase_PurchaseQuotationComparePrint : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();

        short showcreditnote = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblDiscount.Visible = (showcreditnote == 1) ? true : false;
        lblDiscount1.Visible = (showcreditnote == 1) ? true : false;
        lblDiscount2.Visible = (showcreditnote == 1) ? true : false;
        lblDiscount3.Visible = (showcreditnote == 1) ? true : false;

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            lblEXECUTIVESHIPMANAGEMENT.Text = "EXECUTIVE SHIP MANAGEMENT";
        else
            lblEXECUTIVESHIPMANAGEMENT.Text = "EXECUTIVE OFFSHORE";

            BindData();
    }

    public void BindData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixPurchaseQuotation.QuotationComaparePrint(ViewState["quotationid"].ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            lblDate.Text = ds.Tables[0].Rows[0]["FLDTODAYSDATE"].ToString();
            //lblRefNo.Text = ds.Tables[0].Rows[0]["FLDVENDORQUOTATIONREF"].ToString();
            lblPoNo.Text = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
            lblDescription.Text = ds.Tables[0].Rows[0]["FLDTITLE"].ToString();
            

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    lblSupplier1.Text = ds.Tables[0].Rows[i]["FLDNAME"].ToString();
                    lblPort1.Text = ds.Tables[0].Rows[i]["FLDSEAPORTNAME"].ToString();
                    lblTotal1.Text = ds.Tables[0].Rows[i]["FLDLOCALCURRENCYWITHOUTDISC"].ToString();
                    lblExchangeRate1.Text = ds.Tables[0].Rows[i]["FLDEXCHANGERATE"].ToString();
                    lblUsdTotal1.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNTWITHOUTDISC"].ToString();
                    lblDeliveryTime1.Text = ds.Tables[0].Rows[i]["FLDDELIVERYTIME"].ToString();
                    lblDiscount1.Text = ds.Tables[0].Rows[i]["FLDDISCOUNT"].ToString();
                    lblTotalPrice1.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNT"].ToString();
                    lblRemarksName.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDNAME"].ToString()).Trim();
                    lblRemarks.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDREMARKS"].ToString()).Trim();
                }

                if (i == 1)
                {
                    lblSupplier2.Text = ds.Tables[0].Rows[i]["FLDNAME"].ToString();
                    lblPort2.Text = ds.Tables[0].Rows[i]["FLDSEAPORTNAME"].ToString();
                    lblTotal2.Text = ds.Tables[0].Rows[i]["FLDLOCALCURRENCYWITHOUTDISC"].ToString();
                    lblExchangeRate2.Text = ds.Tables[0].Rows[i]["FLDEXCHANGERATE"].ToString();
                    lblUsdTotal2.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNTWITHOUTDISC"].ToString();
                    lblDeliveryTime2.Text = ds.Tables[0].Rows[i]["FLDDELIVERYTIME"].ToString();
                    lblDiscount2.Text = ds.Tables[0].Rows[i]["FLDDISCOUNT"].ToString();
                    lblTotalPrice2.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNT"].ToString();
                    lblRemarksName2.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDNAME"].ToString()).Trim();
                    lblRemarks2.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDREMARKS"].ToString()).Trim();
                }

                if (i == 2)
                {
                    lblSupplier3.Text = ds.Tables[0].Rows[i]["FLDNAME"].ToString();
                    lblPort3.Text = ds.Tables[0].Rows[i]["FLDSEAPORTNAME"].ToString();
                    lblTotal3.Text = ds.Tables[0].Rows[i]["FLDLOCALCURRENCYWITHOUTDISC"].ToString();
                    lblExchangeRate3.Text = ds.Tables[0].Rows[i]["FLDEXCHANGERATE"].ToString();
                    lblUsdTotal3.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNTWITHOUTDISC"].ToString();
                    lblDeliveryTime3.Text = ds.Tables[0].Rows[i]["FLDDELIVERYTIME"].ToString();
                    lblDiscount3.Text = ds.Tables[0].Rows[i]["FLDDISCOUNT"].ToString();
                    lblTotalPrice3.Text = ds.Tables[0].Rows[i]["TOTALUSDAMOUNT"].ToString();
                    lblRemarksName3.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDNAME"].ToString()).Trim();
                    lblRemarks3.Text = Server.HtmlDecode(ds.Tables[0].Rows[i]["FLDREMARKS"].ToString()).Trim();
                }
            }

            
        }
    }
}
