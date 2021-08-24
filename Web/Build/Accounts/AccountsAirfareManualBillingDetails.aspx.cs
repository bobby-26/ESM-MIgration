using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Integration;
//using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsAirfareManualBillingDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
        ViewState["TicketNo"] = Request.QueryString["ticketNo"];
        ViewState["VesselId"] = Request.QueryString["vesselId"];

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Billing", "BILLING", ToolBarDirection.Right);
        toolbar.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);
        MenuAirfareBilling.AccessRights = this.ViewState;
        MenuAirfareBilling.MenuList = toolbar.Show();
        MenuAirfareBilling.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarBill = new PhoenixToolbar();
        toolbarBill.AddButton("Bill", "BILL", ToolBarDirection.Right);
        MenuBill.AccessRights = this.ViewState;
        MenuBill.MenuList = toolbarBill.Show();

        txtBudgetId.Attributes.Add("style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
        txtAccountId.Attributes.Add("style", "visibility:hidden");
        txtAccountSource.Attributes.Add("style", "visibility:hidden");
        txtAccountUsage.Attributes.Add("style", "visibility:hidden");

        txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden;");
        txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
        txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        toolbarMain.AddButton("Billed", "BILLED", ToolBarDirection.Right);
        toolbarMain.AddButton("Unbilled", "UNBILLED", ToolBarDirection.Right);
        MenuAirfareBillingMain.AccessRights = this.ViewState;
        MenuAirfareBillingMain.MenuList = toolbarMain.Show();
        MenuAirfareBillingMain.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;

            BindData();
            BillToCompany();
            btnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VesselId"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
        }
        BindFlightList();

        if (ViewState["EmployeeId"] != null)
        {
            lnkPassengerName.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + ViewState["EmployeeId"].ToString() + "&history=show'); return false;");
        }

    }

    protected void MenuAirfareBillingMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UNBILLED"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBilling.aspx");
            }

            else if (CommandName.ToUpper().Equals("BILLED"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuAirfareBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBilling.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuBill_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BILL"))
            {
                Guid iDtKey = new Guid();
                Guid iRevenueDtKey = new Guid();

                if (!IsValidMain(txtAccountId.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAirfareManualBillingDetails.AirfareInvoiceUpdate(new Guid(Request.QueryString["agentInvoiceNo"]),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            General.GetNullableInteger(ddlBillToCompany.SelectedCompany),
                                                            General.GetNullableInteger(txtBudgetId.Text),
                                                            Int32.Parse(txtAccountId.Text),
                                                            General.GetNullableGuid(txtOwnerBudgetIdEdit.Text),
                                                            ref iDtKey,
                                                            ref iRevenueDtKey,
                                                            chkWriteOff.Checked ? 1 : 0);
                ucStatus.Text = "Bill has been posted";
                //if (iDtKey.ToString() != "00000000-0000-0000-0000-000000000000")
                //    CreateReport(new Guid(Request.QueryString["agentInvoiceNo"]), iDtKey);
                //if (iRevenueDtKey.ToString() != "00000000-0000-0000-0000-000000000000")
                //    CreateReport(new Guid(Request.QueryString["agentInvoiceNo"]), iRevenueDtKey);
                Response.Redirect("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixAccountsAirfareManualBillingDetails.InvoiceDetails(ViewState["TicketNo"].ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["EmployeeId"] = dr["FLDEMPLOYEEID"].ToString();
            txtEmployee.Text = dr["FLDEMPLOYEENAME"].ToString() + "/" + dr["FLDEMPLOYEECODE"].ToString();
            lnkPassengerName.Text = dr["FLDPASSENGERNAME"].ToString();
            txtDepartureDate.Text = General.GetDateTimeToString(dr["FLDDEPARTUREDATE"].ToString());
            txtSector1.Text = dr["FLDSECTOR1"].ToString();
            txtSector2.Text = dr["FLDSECTOR2"].ToString();
            txtSector3.Text = dr["FLDSECTOR3"].ToString();
            txtSector4.Text = dr["FLDSECTOR4"].ToString();
            txtInvoiceNumber.Text = dr["FLDINVOICENUMBER"].ToString();
            txtInvoiceDate.Text = General.GetDateTimeToString(dr["FLDINVOICEDATE"].ToString());
            txtAgentName.Text = dr["FLDAGENTNAME"].ToString();
            txtTicketNo.Text = dr["FLDTICKETNO"].ToString();
            txtVesselChargeable.Text = dr["FLDCHARGEDVESSEL"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();
            txtAccountId.Text = dr["FLDVESSELACCOUNTID"].ToString();
            txtOwnerBudgetIdEdit.Text = dr["FLDOWNERBUDGETCODEID"].ToString();
            txtOwnerBudgetCodeEdit.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
            txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETDESCRIPTION"].ToString();
        }
    }

    private void BillToCompany()
    {
        int iBillToCompany = 0;
        DataSet ds = PhoenixAccountsAirfareManualBillingDetails.AirfareBillToCompany(int.Parse(txtAccountId.Text), ref iBillToCompany);
        ddlBillToCompany.SelectedCompany = iBillToCompany.ToString();

    }

    private void BindFlightList()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        if (ViewState["EmployeeId"] != null)
        {
            ds = PhoenixAccountsAirfareManualBillingDetails.FlightList(new Guid(Request.QueryString["agentInvoiceNo"]),
                                                                                General.GetNullableInteger(ViewState["EmployeeId"].ToString()),
                                                                                Convert.ToDateTime(txtDepartureDate.Text),
                                                                                gvFlightList.CurrentPageIndex + 1,
                                                                                gvFlightList.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

            gvFlightList.DataSource = ds;
            gvFlightList.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
    }

    protected void lnkPassengerName_Click(Object sender, EventArgs e)
    {

    }

    private void CreateReport(Guid agentInvoiceId, Guid dtkey)
    {
        DataSet ds = PhoenixIntegrationAccounts.TravelPostReport(agentInvoiceId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            AddAttachment(ds, dtkey);
        }
    }
    private string AddAttachment(DataSet ds, Guid dtkey)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));


        string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string filefullpath = path + dtkey + ".pdf";

        ConvertToPdf(HtmlTableDataContent(ds), filefullpath);
        return filefullpath;
    }

    private string HtmlTableDataContent(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<table ID='tbl1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<img src='http://" + Request.Url.Authority + Session["images"] + "/UnisonTravel_Logo.png" + "' />");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='8'>");
        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:Calibri (Body);font-size:20px;\">");
        sbHtmlContent.Append("Unison Universals Travel Limited");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' valign='top'>");
        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");

        sbHtmlContent.Append("<td width='40%' valign='top' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");

        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:12px;\">");
        sbHtmlContent.Append("BILL TO");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYADDRESS"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width=\"20%\" align='center' valign='middle'>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='40%' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>INVOICE NO:</b>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDREVENUEVOUCHERNUMBER"].ToString());
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>DATE:</b>");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString()));
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>ORDER:</b>");
        //sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("REF :" + ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width=\"5%\" style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>S.NO.</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='2' width='35%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>DESCRIPTION</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>FLIGHT DATE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAX</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TOTAL FARE (USD)</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("1");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDDESCRIPTION"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDDEPARTUREDATE"].ToString()));
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<table ID='tbl4' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>Our Banking Details:</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("The Hongkong and Shanghai Bank");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Swift Code: HSBCHKHK");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Account Number: 801-042771-838");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='50px' rowspan='3'  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<table ID='tbl3' width='100%' border='0'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE FARE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAXES AND SURCHARGES</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");


        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>AMOUNT PAYABLE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<footer>");
        sbHtmlContent.Append("<p align=\"center\">");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("Suite B, 11th Floor, Hang Seng Causeway Bay Building, 28 Yee Wo Street, Causeway Bay, Hong Kong");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("E-mail: travel@unisonuniversals.com");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</p>");
        sbHtmlContent.Append("</footer>");

        return sbHtmlContent.ToString();
    }

    public void ConvertToPdf(string HTMLString, string fileLocation)
    {
        //Document document = new Document(PageSize.A4, 50, 50, 25, 25);
        Document document = new Document(new Rectangle(595f, 842f));
        document.SetMargins(36f, 36f, 36f, 0f);

        PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        document.Open();

        StyleSheet styles = new StyleSheet();

        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
        //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(HTMLString), null);

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }

    private bool IsValidMain(string account, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (chkWriteOff.Checked == false)
        {
            if (company == "Dummy")
                ucError.ErrorMessage = "Bill to company is required.";

            if (account.Trim().Equals(""))
                ucError.ErrorMessage = "Account to be charged is required.";
        }

        return (!ucError.IsError);

    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        BillToCompany();
        DataSet ds = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                        , txtAccountCode.Text
                                                                        , ""
                                                                        , null
                                                                        , null
                                                                        , null
                                                                        , 1
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
        }
        btnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VesselId"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
    }
    protected void txtAccountDescription_OnTextChanged(object sender, EventArgs e)
    {
        txtOwnerBudgetCodeEdit.Text = "";
    }
    protected void chkWriteOff_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkWriteOff.Checked == true)
        {
            txtAccountCode.CssClass = "readonlytextbox";
            txtAccountCode.ReadOnly = true;
            txtAccountDescription.CssClass = "readonlytextbox";
            txtAccountDescription.ReadOnly = true;
            imgShowAccount.Visible = false;
            txtBudgetCode.CssClass = "readonlytextbox";
            txtBudgetCode.ReadOnly = true;
            txtBudgetName.CssClass = "readonlytextbox";
            txtBudgetName.ReadOnly = true;
            imgShowBudget.Visible = false;
            txtOwnerBudgetCodeEdit.CssClass = "readonlytextbox";
            txtOwnerBudgetCodeEdit.ReadOnly = true;
            btnShowOwnerBudgetEdit.Visible = false;
            ddlBillToCompany.CssClass = "readonlytextbox";
            ddlBillToCompany.Enabled = false;
        }
        else
        {
            txtAccountCode.CssClass = "input_mandatory";
            txtAccountCode.ReadOnly = false;
            txtAccountDescription.CssClass = "input_mandatory";
            txtAccountDescription.ReadOnly = false;
            imgShowAccount.Visible = true;
            txtBudgetCode.CssClass = "input";
            txtBudgetCode.ReadOnly = false;
            txtBudgetName.CssClass = "input";
            txtBudgetName.ReadOnly = false;
            imgShowBudget.Visible = true;
            txtOwnerBudgetCodeEdit.CssClass = "input";
            txtOwnerBudgetCodeEdit.ReadOnly = false;
            btnShowOwnerBudgetEdit.Visible = true;
            ddlBillToCompany.CssClass = "input_mandatory";
            ddlBillToCompany.Enabled = true;
        }
    }

    protected void gvFlightList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindFlightList();
    }

    protected void gvFlightList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
