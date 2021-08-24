using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;


public partial class DryDockQuotation : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Quotation", "LIST");
            toolbar.AddButton("Jobs", "DETAILS");
            toolbar.AddButton("Project", "PROJECT");
           
           
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();

            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarsave.AddButton("New", "NEW", ToolBarDirection.Right);
            
            MenuQuotation.AccessRights = this.ViewState;
            MenuQuotation.MenuList = toolbarsave.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvQuotation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] , "Compare Quotation", "<i class=\"fas fa-clipboard\"></i>", "COMPARE");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] , "Send Quote", "<i class=\"fas fa-envelope\"></i>", "SENDQUOTE");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] , "Create Yard PO", "<i class=\"fas fa-file-alt\"></i>", "CREATEPO");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] , "Create Maker PO", "<i class=\"fas fa-file-alt\"></i>", "CREATEMAKERPO");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] , "Create Maker Quotation", "<i class=\"fab fa-quora\"></i>", "CREATEQUOTE");
            MenuQuotationGrid.AccessRights = this.ViewState;
            MenuQuotationGrid.MenuList = toolbargrid.Show();

            txtYardID.Attributes.Add("style", "visibility:hidden");


            if (!IsPostBack)
            {               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["QUOTATIONID"] = "";
                ViewState["EXCHANGERATEID"] = "";
                ViewState["ORDERID"] = "";
                ViewState["VSLID"] = Request.QueryString["vslid"];
                
                //OnClientClick = "showPickList('spnPickListYard', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=133&txtsupcode='+ document.getElementById('txtYardCode').value +'&txtsupname='+ document.getElementById('txtYardName').value, true); return false;"
                btnPickVender.Attributes.Add("onclick", "return showPickList('spnPickListYard', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=133',true);");
                gvQuotation.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
               
            }
            //if (Request.QueryString["quotationid"] != null)
            //{
            //    ViewState["QUOTATIONID"] = Request.QueryString["quotationid"];
               
            //}
            MenuHeader.SelectedMenuIndex = 0;
            //BindData();
            if (ViewState["QUOTATIONID"] == null)
            {
                if (Request.QueryString["Quotationid"] != null)
                    ViewState["QUOTATIONID"] = Request.QueryString["Quotationid"];
            }
          


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    private string FormatDecimal2Places(string value)
    {
       
            decimal? d = General.GetNullableDecimal(value);
            if (d == null)
                return "0.00";

            return String.Format("{0:f2}", d);
      
    }

	protected void BindFields()
	{
		try
		{
            if (ViewState["QUOTATIONID"] == null || ViewState["QUOTATIONID"].ToString() == "")
            {
                SetDefualtValue();
                return;
            }

            DataSet ds = PhoenixDryDockQuotation.EditDryDockquotation(new Guid(ViewState["QUOTATIONID"].ToString()), int.Parse(ViewState["VSLID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtYardCode.Text = dr["FLDCODE"].ToString();
                txtYardName.Text = dr["FLDNAME"].ToString();
                txtYardID.Text = dr["FLDYARDID"].ToString();
                txtYardReferenceno.Text = dr["FLDYARDREFNO"].ToString();
                ucSentDate.Text = dr["FLDSENTDATE"].ToString();
                ucRecievedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                ucExpiryDate.Text = dr["FLDEXCHANGERATEDATE"].ToString();
                txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();

                txtTotalPrice.Text = FormatDecimal2Places(dr["FLDTOTALPRICE"].ToString());
                ucTotalDiscount.Text = dr["FLDTOTALDISCOUNT"].ToString();
                txtNetPrice.Text = FormatDecimal2Places(dr["FLDNETPRICE"].ToString());


                decimal? totalprice = General.GetNullableDecimal(dr["FLDTOTALPRICE"].ToString());
                decimal? exchangerate = General.GetNullableDecimal(dr["FLDEXCHANGERATE"].ToString());
                decimal? discount = General.GetNullableDecimal(dr["FLDTOTALDISCOUNT"].ToString());

                exchangerate = exchangerate == 0 ? 1 : exchangerate;

                txtUsdPrice.Text = FormatDecimal2Places((totalprice / exchangerate).ToString());
                txtTotalDiscount.Text = FormatDecimal2Places((discount / exchangerate).ToString());


                ucAgreedpercent.Text = dr["FLDAGREEDPERCETAGE"].ToString();
                ucAgreedLumpsum.Text = dr["FLDAGREEDLUMPSUM"].ToString();

                YardQuote1.Text = dr["FLDDAYINDRYDOCKQUOTE1"].ToString();
                Adjustment1.Text = dr["FLDDAYINDRYDOCKADJUSTMENT1"].ToString();

                decimal? daysindrydockquote1 = General.GetNullableDecimal(dr["FLDDAYINDRYDOCKQUOTE1"].ToString());
                daysindrydockquote1 = (daysindrydockquote1 != null) ? daysindrydockquote1 : 0.00M;

                decimal? daysindrydockadjustment1 = General.GetNullableDecimal(dr["FLDDAYINDRYDOCKADJUSTMENT1"].ToString());
                daysindrydockadjustment1 = (daysindrydockadjustment1 != null) ? daysindrydockadjustment1 : 0.00M;

                Total1.Text = FormatDecimal2Places((daysindrydockquote1 + daysindrydockadjustment1).ToString());


                YardQuote2.Text = dr["FLDDAYINDRYDOCKQUOTE2"].ToString();
                Adjustment2.Text = dr["FLDDAYINDRYDOCKADJUSTMENT2"].ToString();

                decimal? daysindrydockquote2 = General.GetNullableDecimal(dr["FLDDAYINDRYDOCKQUOTE2"].ToString());
                daysindrydockquote2 = (daysindrydockquote2 != null) ? daysindrydockquote2 : 0.00M;

                decimal? daysindrydockadjustment2 = General.GetNullableDecimal(dr["FLDDAYINDRYDOCKADJUSTMENT2"].ToString());
                daysindrydockadjustment2 = (daysindrydockadjustment2 != null) ? daysindrydockadjustment2 : 0.00M;

                Total2.Text = FormatDecimal2Places((daysindrydockadjustment2 + daysindrydockquote2).ToString());

                TotalYardQuote.Text = FormatDecimal2Places((daysindrydockquote2 + daysindrydockquote1).ToString());

                TotalAdjustment.Text = FormatDecimal2Places((daysindrydockadjustment1 + daysindrydockadjustment2).ToString());

                Total.Text = FormatDecimal2Places((daysindrydockquote2 + daysindrydockquote1 + daysindrydockadjustment1 + daysindrydockadjustment2).ToString());

                ucDeviationDeliveryPoint.Text = FormatDecimal2Places(dr["FLDDEVIATIONDELIVERY"].ToString());
                ucDeviationReDeliveryPoint.Text = FormatDecimal2Places(dr["FLDDEVIATIONREDELIVERY"].ToString());
                ucTotalDeviation.Text = FormatDecimal2Places(dr["FLDDEVIATIONTOTAL"].ToString());
                ucHFOCost.Text = FormatDecimal2Places(dr["FLDHFOCOST"].ToString());
                ucHFOConsumption.Text = FormatDecimal2Places(dr["FLDHFOCONSUMPTION"].ToString());
                ucHFOTotalCost.Text = FormatDecimal2Places(dr["FLDHFOTOTALCOST"].ToString());
                ucMDOCost.Text = FormatDecimal2Places(dr["FLDMDOCOST"].ToString());
                ucMDOConsumption.Text = FormatDecimal2Places(dr["FLDMDOCONSUMPTION"].ToString());
                ucMDOTotalCost.Text = FormatDecimal2Places(dr["FLDMDOTOTALCOST"].ToString());
                ucDeviationDays.Text = FormatDecimal2Places(dr["FLDDEVIATIONDAYS"].ToString());
                ucServiceSpeed.Text = FormatDecimal2Places(dr["FLDSERVICESPEED"].ToString());

                txtValidUntil.Text = dr["FLDVALIDUNTIL"].ToString();

                if (dr["FLDREPORTTOOWNERYN"].ToString() == "1")
                    chkReportOwnerYN.Checked = true;
                else
                    chkReportOwnerYN.Checked = false;

                ViewState["ORDERID"] = dr["FLDORDERID"].ToString();
            }            
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void QuotationGrid_TabStripCommand(object sender, EventArgs e)
	{
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("COMPARE"))
            {
                ShowQuotationComparison();
            }
            if (CommandName.ToUpper().Equals("SENDQUOTE"))
            {
                string selectedquotation = string.Empty;
                foreach (GridDataItem gvr in gvQuotation.Items)
                {
                    if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)gvr.FindControl("chkSelect")).Checked == true)
                    {
                        selectedquotation = selectedquotation + ((RadLabel)(gvr.FindControl("lblQuotationid"))).Text + ",";
                    }
                }
                selectedquotation = selectedquotation.TrimEnd(',');
                if (!IsValidRFQ(selectedquotation))
                {
                    ucError.Visible = true;
                    return;
                }
                if (selectedquotation != string.Empty)
                {
                    string jvscript = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DryDock/DryDockSendQuery.aspx?type=sendquote&orderid=" + ViewState["ORDERID"].ToString() + "&quotationid=" + selectedquotation + "&vslid=" + ViewState["VSLID"].ToString() + "');";
                    
                    ClientScript.RegisterStartupScript(GetType(), "SendMail", jvscript, true);
                }
            }
            if (CommandName.ToUpper().Equals("CREATEPO"))
            {
                if (ViewState["ORDERID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Yard is not selected, Select a Yard to place the order.";
                    ucError.Visible = true;
                    return;
                }
                string dtkey = PhoenixCommonFileAttachment.GenerateDTKey();
                Guid? Poid = Guid.Empty;
                string path = Server.MapPath("..\\Attachments\\TEMP\\") + dtkey + ".xlsm";
                DataSet ds = PhoenixDryDockOrder.DryDockOrderCreatePO(int.Parse(ViewState["VSLID"].ToString()), new Guid(ViewState["ORDERID"].ToString()), ref Poid);
                Guid quotationid = new Guid(ds.Tables[1].Rows[0]["FLDDRYDOCKQUOTATIONID"].ToString());
                Guid attachmentdtkey = new Guid(ds.Tables[1].Rows[0]["FLDDTKEY"].ToString());
                //PhoenixDryDockQuotation.DeleteDrydockMakerFileAttachment(attachmentdtkey);
                PhoenixDryDock2XL.Export2XLDryDockQuotation(new Guid(ViewState["ORDERID"].ToString()), quotationid, int.Parse(ViewState["VSLID"].ToString()), path , Poid);
                FileInfo fi = new FileInfo(path);
                PhoenixCommonFileAttachment.InsertAttachment(attachmentdtkey, "PhoenixDryDockQuotation.xlsm", "TEMP/" + dtkey + ".xlsm", fi.Length, int.Parse(ViewState["VSLID"].ToString()), byte.Parse("1"), string.Empty, new Guid(dtkey));
                ucStatus.Text = "PO Created.";
            }
            if (CommandName.ToUpper().Equals("CREATEMAKERPO"))
            {
                string dtkey;
                Guid quotationid ;
                Guid attachmentdtkey;
                string path;
                string filename;
                
                DataSet ds = PhoenixDryDockOrder.DryDockOrderCreateMakerPO(int.Parse(ViewState["VSLID"].ToString()), new Guid(Filter.CurrentSelectedDryDockProject.ToString()));
                int rowcount = ds.Tables[0].Rows.Count;
                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    DataRow dr = ds.Tables[0].Rows[index];
                    dtkey = PhoenixCommonFileAttachment.GenerateDTKey();
                    
                    quotationid = new Guid(ds.Tables[0].Rows[index]["FLDQUOTATIONID"].ToString());
                    attachmentdtkey = new Guid(ds.Tables[0].Rows[index]["FLDDTKEY"].ToString());

                    filename=PhoenixDryDockQuotation.GetDryDockQuotationYardZipFileName(quotationid, int.Parse(ViewState["VSLID"].ToString()));
                    path = PhoenixDryDock2XL.Export2XLDryDockMakerDocuments(General.GetNullableGuid(Filter.CurrentSelectedDryDockProject.ToString()), General.GetNullableGuid(quotationid.ToString()), int.Parse(ViewState["VSLID"].ToString()), null);
                    FileInfo fi = new FileInfo(path);
                    PhoenixDryDockQuotation.DeleteDrydockMakerFileAttachment(attachmentdtkey);
                    PhoenixCommonFileAttachment.InsertAttachment(attachmentdtkey, "PhoenixDryDockMakerQuotation.zip", "DryDock/" + filename+".zip", fi.Length, int.Parse(ViewState["VSLID"].ToString()), byte.Parse("1"), "383", new Guid(dtkey));
                }
                ucStatus.Text = "PO Created.";
            }
            if (CommandName.ToUpper().Equals("CREATEQUOTE"))
            {
                PhoenixDryDockQuotation.CreateDryDockMakerQuotation(int.Parse(ViewState["VSLID"].ToString()),
                                                                    General.GetNullableGuid(Filter.CurrentSelectedDryDockProject.ToString()));
                BindData();
                ucStatus.Text = "Quotation Created.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
	}

    private void ShowQuotationComparison()
    {
        try
        {
            string selectedquotation = string.Empty;
            foreach (GridDataItem gvr in gvQuotation.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)gvr.FindControl("chkSelect")).Checked == true)
                {
                    selectedquotation = selectedquotation + ((RadLabel)(gvr.FindControl("lblQuotationid"))).Text + ",";
                }
            }
            selectedquotation = selectedquotation.TrimEnd(',');
            if (selectedquotation == string.Empty)
            {
                selectedquotation = "";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + Filter.CurrentSelectedDryDockProject.ToString() + "','" + selectedquotation + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','COMPARE');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ShowExcel()
	{
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDYARDREFNO", "FLDNAME", "FLDMAKERYN", "FLDCURRENCYCODE", "FLDCREATEDDATE" };
            string[] alCaptions = { "Reference Number", "Yard Name", "MakerY/N", "Currency", "Received Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixDryDockQuotation.DryDockQuotationSearch
               (int.Parse(ViewState["VSLID"].ToString()),
                   General.GetNullableGuid(Filter.CurrentSelectedDryDockProject.ToString()), sortexpression,
                       sortdirection,
                   1,
                   iRowCount, ref iRowCount, ref iTotalPageCount);


            Response.AddHeader("Content-Disposition", "attachment; filename=DryDockQuotation.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Dry Dock Quotation</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
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
            if (Filter.CurrentSelectedDryDockProject == null)
                return;

			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDYARDREFNO", "FLDNAME", "FLDMAKERYN","FLDCURRENCYCODE", "FLDCREATEDDATE" };
			string[] alCaptions = { "Reference No", "Yard Name","MakerY/N", "Currency", "Received Date" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet dsOrder = PhoenixDryDockOrder.EditDryDockOrder(int.Parse(ViewState["VSLID"].ToString()),
                new Guid(Filter.CurrentSelectedDryDockProject.ToString()));

            DataTable dtOrder = dsOrder.Tables[0];
            ucTitle.Text = dtOrder.Rows[0]["FLDNUMBER"].ToString();

			DataSet ds;

			ds = PhoenixDryDockQuotation.DryDockQuotationSearch
                (int.Parse(ViewState["VSLID"].ToString()),
					General.GetNullableGuid(Filter.CurrentSelectedDryDockProject.ToString()),
                    sortexpression,
                    sortdirection,
					gvQuotation.CurrentPageIndex+1,
					gvQuotation.PageSize, ref iRowCount, ref iTotalPageCount);



			General.SetPrintOptions("gvQuotation", "Quotation", alCaptions, alColumns, ds);
            gvQuotation.DataSource = ds;
            gvQuotation.VirtualItemCount = iRowCount;
            

			//ViewState["ROWCOUNT"] = iRowCount;
			//ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			//SetPageNavigator();
            //SetRowSelection();
                       

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			BindData();
            BindFields();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PROJECT"))
            {
                Response.Redirect("../DryDock/DryDockProject.aspx?vslid=" + Request.QueryString["vslid"], false);
            } 
            
          

			if (CommandName.ToUpper().Equals("DETAILS"))
			{
                if (ViewState["QUOTATIONID"] == null || ViewState["QUOTATIONID"].ToString() == "")
                {
                    ShowError();
                    return;
                }

                Response.Redirect("../DryDock/DryDockQuotationLineItem.aspx?vslid=" + Request.QueryString["vslid"] + "&Quotationid=" + ViewState["QUOTATIONID"], false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Select a Query/Quotation to view the Jobs";
		ucError.Visible = true;
	}

	protected void Quotation_TabStripCommand(object sender, EventArgs e)
	{
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidQuotation())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? quotationidout = Guid.NewGuid();
                int? x = General.GetNullableInteger(ucServiceSpeed.Text);
                PhoenixDryDockQuotation.InsertDryDockquotation(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    int.Parse(ViewState["VSLID"].ToString()),
                                                    ViewState["QUOTATIONID"] == null ? null : General.GetNullableGuid(ViewState["QUOTATIONID"].ToString()),
                                                    new Guid(Filter.CurrentSelectedDryDockProject),
                                                    Convert.ToInt64(txtYardID.Text),
                                                    txtYardReferenceno.Text,
                                                    General.GetNullableInteger(ucCurrency.SelectedCurrency),
                                                    General.GetNullableDecimal(txtExchangeRate.Text),
                                                    General.GetNullableDateTime(ucExpiryDate.Text),
                                                    General.GetNullableDecimal(YardQuote1.Text),
                                                    General.GetNullableDecimal(Adjustment1.Text),
                                                    General.GetNullableDecimal(YardQuote2.Text),
                                                    General.GetNullableDecimal(Adjustment2.Text),
                                                    General.GetNullableDecimal(ucAgreedpercent.Text),
                                                    General.GetNullableDecimal(ucAgreedLumpsum.Text),
                                                    null,
                                                    General.GetNullableDecimal(ucDeviationDeliveryPoint.Text),
                                                    General.GetNullableDecimal(ucDeviationReDeliveryPoint.Text),
                                                    General.GetNullableDecimal(ucHFOCost.Text),
                                                    General.GetNullableDecimal(ucHFOConsumption.Text),
                                                    General.GetNullableDecimal(ucMDOCost.Text),
                                                    General.GetNullableDecimal(ucMDOConsumption.Text),
                                                    General.GetNullableDecimal(ucServiceSpeed.Text),
                                                    ref quotationidout
                                                    , General.GetNullableDateTime(txtValidUntil.Text)
                                                    , (byte?)General.GetNullableInteger(chkReportOwnerYN.Checked ? "1" : null)
                                                    );

                ViewState["QUOTATIONID"] = quotationidout;

                ucStatus.Text = "Details Updated.";
                BindData();
                BindFields();
                gvQuotation.Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {


                Filter.CurrentSelectedDryDockQuotation = null;
                txtYardCode.Text = "";
                txtYardName.Text = "";
                txtYardID.Text = "";
                txtYardReferenceno.Text = "";
                ucRecievedDate.Text = "";
                ucSentDate.Text = "";
                ucCurrency.SelectedCurrency = "";
                ucExpiryDate.Text = "";
                txtExchangeRate.Text = "";
                txtTotalPrice.Text = "";
                ucTotalDiscount.Text = "";
                txtUsdPrice.Text = "";
                txtTotalDiscount.Text = "";
                ucAgreedpercent.Text = "";
                ucAgreedLumpsum.Text = "";
                YardQuote1.Text = "";
                Adjustment1.Text = "";
                Total1.Text = "";
                YardQuote2.Text = "";
                Adjustment2.Text = "";
                Total2.Text = "";
                TotalYardQuote.Text = "";
                TotalAdjustment.Text = "";
                Total.Text = "";
                txtValidUntil.Text = "";
                chkReportOwnerYN.Checked = false;
                ucDeviationDeliveryPoint.Text = "";
                ucDeviationReDeliveryPoint.Text = "";
                ucTotalDeviation.Text = "";
                ucHFOCost.Text = "";
                ucHFOConsumption.Text = "";
                ucHFOTotalCost.Text = "";
                ucMDOCost.Text = "";
                ucMDOConsumption.Text = "";
                ucMDOTotalCost.Text = "";
                ucDeviationDays.Text = "";
                ucServiceSpeed.Text = "";

                ViewState["QUOTATIONID"] = null;
                SetDefualtValue();
                //gvQuotation.SelectedIndex = -1;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

	}
    private bool IsValidQuotation()
    {   
            ucError.HeaderMessage = "Please provide the following required information";

            if (txtYardID.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Yard is required.";

            return (!ucError.IsError);
       
    }
    private bool IsValidRFQ(string csvquotationid)
    {
            ucError.HeaderMessage = "Please provide the following required information";

            if (csvquotationid.Trim().Equals(""))
                ucError.ErrorMessage = "Select atleast One Yard to Send Quote.";

            return (!ucError.IsError);
    }
    

	//protected void gvQuotation_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	//{
	//	gvQuotation.SelectedIndex = se.NewSelectedIndex;
 //       Filter.CurrentSelectedDryDockQuotation = ((Label)gvQuotation.Rows[se.NewSelectedIndex].FindControl("lblQuotationid")).Text;
	//	ViewState["QUOTATIONID"] = ((Label)gvQuotation.Rows[se.NewSelectedIndex].FindControl("lblQuotationid")).Text;
	//	ViewState["DTKEY"] = ((Label)gvQuotation.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
 //       BindData();
 //       BindFields();
	//}

    //protected void gvQuotation_RowCommand(object sender, GridViewCommandEventArgs gce)
    //{
    //    try
    //    {
    //        if (gce.CommandName.ToUpper().Equals("SORT"))
    //        {
    //            return;
    //        }


    //        GridView _gridView = (GridView)sender;
    //        int nRow = int.Parse(gce.CommandArgument.ToString());

    //        if (gce.CommandName.ToUpper().Equals("REFRESH"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            PhoenixDryDockQuotation.DryDockquotationLineItemInsert(
    //                General.GetNullableGuid(lblquotationid.Text),
    //                General.GetNullableGuid(lblorderid.Text),
    //                int.Parse(ViewState["VSLID"].ToString()));
    //        }

    //        if (gce.CommandName.ToUpper().Equals("EXPORT2XL"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            int lblmakeryn = ((Label)_gridView.Rows[nRow].FindControl("lblMakerYN")).Text == "Yes"? 1 : 0;
    //            //PhoenixDryDock2XL.Export2XLDryDockQuotation(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
    //            //PhoenixDryDock2XL.Export2XLDryDockCompare(General.GetNullableGuid(lblorderid.Text));
    //            if (lblmakeryn == 1)
    //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDockMaker('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','QUOTATION');", true);
    //            else
    //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','QUOTATION');", true);
    //        }

    //        if (gce.CommandName.ToUpper().Equals("EXPORT2XLINCREMENT"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            int lblmakeryn = ((Label)_gridView.Rows[nRow].FindControl("lblMakerYN")).Text == "Yes" ? 1 : 0;
    //            //PhoenixDryDock2XL.Export2XLDryDockQuotationIncremental(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
    //            //PhoenixDryDock2XL.Export2XLDryDockCompare(General.GetNullableGuid(lblorderid.Text));
    //            if (lblmakeryn == 1)
    //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDockMaker('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','INCREMENTALQUOTATION');", true);
    //            else
    //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','INCREMENTALQUOTATION');", true);
    //        }

    //        if (gce.CommandName.ToUpper().Equals("PREPAREQUOTE"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            int lblmakeryn = ((Label)_gridView.Rows[nRow].FindControl("lblMakerYN")).Text == "Yes"? 1 : 0;
    //            // calling referesh before preparing quote
    //            PhoenixDryDockQuotation.DryDockquotationLineItemInsert(
    //                General.GetNullableGuid(lblquotationid.Text),
    //                General.GetNullableGuid(lblorderid.Text),
    //                int.Parse(ViewState["VSLID"].ToString()));                
    //            PhoenixDryDock2XL.Export2XLDryDockDocuments(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
    //            ucStatus.Text = "DryDock Query Zip File created";
    //        }

    //        if (gce.CommandName.ToUpper().Equals("JOBS"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            //PhoenixDryDock2XL.Export2XLDryDockJobStatus(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','JOBSTATUS');", true);
    //        }
    //        if (gce.CommandName.ToUpper().Equals("SELECTYARD"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            PhoenixDryDockQuotation.Drydockfinalizeyard(new Guid(lblorderid.Text), new Guid(lblquotationid.Text), 1);
    //            PhoenixDryDockQuotation.UpdateDryDockQuotationYard(int.Parse(ViewState["VSLID"].ToString()), General.GetNullableGuid(lblquotationid.Text).Value, General.GetNullableGuid(lblorderid.Text).Value, 1);
    //            BindData();
    //        }
    //        if (gce.CommandName.ToUpper().Equals("DESELECT"))
    //        {
    //            Label lblorderid = (Label)_gridView.Rows[nRow].FindControl("lblOrderid");
    //            Label lblquotationid = (Label)_gridView.Rows[nRow].FindControl("lblQuotationid");
    //            PhoenixDryDockQuotation.Drydockfinalizeyard(new Guid(lblorderid.Text), new Guid(lblquotationid.Text), 0);
    //            PhoenixDryDockQuotation.UpdateDryDockQuotationYard(int.Parse(ViewState["VSLID"].ToString()), General.GetNullableGuid(lblquotationid.Text).Value, General.GetNullableGuid(lblorderid.Text).Value, 0);
    //            BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {            
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    //protected void gvQuotation_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}
    //private void SetRowSelection()
    //{
    //    try
    //    {
    //        if (ViewState["QUOTATIONID"] == null || ViewState["QUOTATIONID"].ToString() == string.Empty)
    //            return;

    //        string a = ViewState["QUOTATIONID"].ToString();

    //        foreach (GridDataItem item in gvQuotation.MasterTableView.Items)
    //        {
    //            if (gvQuotation.MasterTableView.Items[0].GetDataKeyValue("FLDQUOTATIONID").ToString().Equals(ViewState[""].ToString()))

    //            //string b = item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDQUOTATIONID"].ToString();
               
    //                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
    //                break;
                
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

	//private void ShowNoRecordsFound(DataTable dt, GridView gv)
	//{

	//	try
	//	{
	//		dt.Rows.Add(dt.NewRow());
	//		gv.DataSource = dt;
	//		gv.DataBind();

	//		int colcount = gv.Columns.Count;
	//		gv.Rows[0].Cells.Clear();
	//		gv.Rows[0].Cells.Add(new TableCell());
	//		gv.Rows[0].Cells[0].ColumnSpan = colcount;
	//		gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
	//		gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
	//		gv.Rows[0].Cells[0].Font.Bold = true;
	//		gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
	//		gv.Rows[0].Attributes["onclick"] = "";
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//protected void gvQuotation_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	//{
	//	try
	//	{
 //           DataRowView drv = (DataRowView)e.Row.DataItem;
 //           if (e.Row.RowType == DataControlRowType.Header)
 //           {
 //               if (ViewState["SORTEXPRESSION"] != null)
 //               {
 //                   HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
 //                   if (img != null)
 //                   {
 //                       if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
 //                           img.Src = Session["images"] + "/arrowUp.png";
 //                       else
 //                           img.Src = Session["images"] + "/arrowDown.png";

 //                       img.Visible = true;
 //                   }
 //               }
 //           }
 //           if (e.Row.RowType == DataControlRowType.DataRow)
 //           {
 //               ImageButton ib = (ImageButton)e.Row.FindControl("cmdExport2XL");
 //               if (ib != null) ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
 //               Label lblorderid = (Label)e.Row.FindControl("lblOrderid");
 //               Label lblquotationid = (Label)e.Row.FindControl("lblQuotationid");

 //               string jvscript = "javascript:parent.Openpopup('codehelp1','','../DryDock/DryDockExport2XL.aspx?exportoption=quotation&orderid=" + lblorderid.Text + "&quotationid=" + lblquotationid.Text + "&vslid=" + ViewState["VSLID"].ToString() + "'); return false;";
 //               //if (ib != null) ib.Attributes.Add("onclick", jvscript);


 //               ImageButton sb = (ImageButton)e.Row.FindControl("cmdSendQuery");
 //               if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);
 //               jvscript = "javascript:parent.Openpopup('codehelp1','','../DryDock/DryDockSendQuery.aspx?type=sendquote&orderid=" + lblorderid.Text + "&quotationid=" + lblquotationid.Text + "&vslid=" + ViewState["VSLID"].ToString() + "'); return false;";
 //               if (sb != null) sb.Attributes.Add("onclick", jvscript);

 //               ImageButton sel = (ImageButton)e.Row.FindControl("cmdSelect");
 //               ImageButton desel = (ImageButton)e.Row.FindControl("cmdDeSelect");
 //               Image flg = (Image)e.Row.FindControl("imgFlag");
 //               if (sel != null && desel != null)
 //               {
 //                   if (drv["FLDISSELECTED"].ToString() == string.Empty || drv["FLDISSELECTED"].ToString() == "0")
 //                   {
 //                       sel.Visible = true;
 //                       desel.Visible = false;
 //                       flg.ImageUrl = Session["images"] + "/spacer.gif";
 //                   }
 //                   else if (drv["FLDISSELECTED"].ToString() == "1")
 //                   {
 //                       sel.Visible = false;
 //                       desel.Visible = true;
 //                       flg.ImageUrl = Session["images"] + "/14.png";
 //                   }
 //               }

 //               ImageButton pre = (ImageButton)e.Row.FindControl("cmdPrepareQuote");
 //               if (pre != null) pre.Visible = SessionUtil.CanAccess(this.ViewState, pre.CommandName);
 //               ImageButton status = (ImageButton)e.Row.FindControl("cmdJobs");
 //               if (status != null) status.Visible = SessionUtil.CanAccess(this.ViewState, status.CommandName);
 //               if (drv["FLDMAKERYN"].ToString() == "Yes")
 //               {
 //                   sel.Visible = false;
 //                   desel.Visible = false;
 //                   pre.Visible = false;
 //                   sb.Visible = false;
 //                   status.Visible = false;
 //                   CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
 //                   chk.Visible = false;

 //               }
 //           }
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	
    private void SetDefualtValue()
    {
        try
        {
            DataSet ds = PhoenixDryDockOrder.EditDryDockOrder(int.Parse(ViewState["VSLID"].ToString()), new Guid(Filter.CurrentSelectedDryDockProject));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucHFOCost.Text = FormatDecimal2Places(dr["FLDHFOCOST"].ToString());
                ucHFOConsumption.Text = FormatDecimal2Places(dr["FLDHFOCONSUMPTION"].ToString());
                ucMDOCost.Text = FormatDecimal2Places(dr["FLDMDOCOST"].ToString());
                ucMDOConsumption.Text = FormatDecimal2Places(dr["FLDMDOCONSUMPTION"].ToString());
            }
        }
        catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }

    protected void gvQuotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvQuotation_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
           


            if (gce.CommandName.ToUpper().Equals("SORT"))
            {
                return;
            }
            if (gce.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");

                Guid? quotationid = General.GetNullableGuid(lblquotationid.Text);
                ViewState["QUOTATIONID"] = quotationid;
                BindFields();
                //string script = "Edit('" + quotationid + "')";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            if (gce.CommandName.ToUpper().Equals("REFRESH"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                PhoenixDryDockQuotation.DryDockquotationLineItemInsert(
                    General.GetNullableGuid(lblquotationid.Text),
                    General.GetNullableGuid(lblorderid.Text),
                    int.Parse(ViewState["VSLID"].ToString()));
            }

            if (gce.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                int lblmakeryn = ((RadLabel)gce.Item.FindControl("lblMakerYN")).Text == "Yes" ? 1 : 0;
                //PhoenixDryDock2XL.Export2XLDryDockQuotation(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
                //PhoenixDryDock2XL.Export2XLDryDockCompare(General.GetNullableGuid(lblorderid.Text));
                if (lblmakeryn == 1)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDockMaker('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','QUOTATION');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','QUOTATION');", true);
            }

            if (gce.CommandName.ToUpper().Equals("EXPORT2XLINCREMENT"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                int lblmakeryn = ((RadLabel)gce.Item.FindControl("lblMakerYN")).Text == "Yes" ? 1 : 0;
                //PhoenixDryDock2XL.Export2XLDryDockQuotationIncremental(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
                //PhoenixDryDock2XL.Export2XLDryDockCompare(General.GetNullableGuid(lblorderid.Text));
                if (lblmakeryn == 1)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDockMaker('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','INCREMENTALQUOTATION');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','INCREMENTALQUOTATION');", true);
            }

            if (gce.CommandName.ToUpper().Equals("PREPAREQUOTE"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                int lblmakeryn = ((RadLabel)gce.Item.FindControl("lblMakerYN")).Text == "Yes" ? 1 : 0;
                // calling referesh before preparing quote
                PhoenixDryDockQuotation.DryDockquotationLineItemInsert(
                    General.GetNullableGuid(lblquotationid.Text),
                    General.GetNullableGuid(lblorderid.Text),
                    int.Parse(ViewState["VSLID"].ToString()));
                PhoenixDryDock2XL.Export2XLDryDockDocuments(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()), null);
                ucStatus.Text = "DryDock Query Zip File created";
            }

            if (gce.CommandName.ToUpper().Equals("JOBS"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                //PhoenixDryDock2XL.Export2XLDryDockJobStatus(General.GetNullableGuid(lblorderid.Text), General.GetNullableGuid(lblquotationid.Text), int.Parse(ViewState["VSLID"].ToString()));
                string a = PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportDryDock('" + lblorderid.Text + "','" + lblquotationid.Text + "','" + ViewState["VSLID"].ToString() + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','JOBSTATUS');", true);
            }
            

            if (gce.CommandName.ToUpper().Equals("SELECTYARD"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                RadLabel lvlvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                PhoenixDryDockQuotation.Drydockfinalizeyard(new Guid(lblorderid.Text),  new Guid(lblquotationid.Text),General.GetNullableInteger(lvlvesselid.Text), 1);
                PhoenixDryDockQuotation.UpdateDryDockQuotationYard(int.Parse(ViewState["VSLID"].ToString()), General.GetNullableGuid(lblquotationid.Text).Value, General.GetNullableGuid(lblorderid.Text).Value, 1);
                ViewState["ORDERID"] = lblorderid.Text;
                ViewState["QUOTATIONID"] = lblquotationid.Text;
                BindData();
                gvQuotation.Rebind();
                BindFields();
            }
            if (gce.CommandName.ToUpper().Equals("DESELECT"))
            {
                RadLabel lblorderid = (RadLabel)gce.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)gce.Item.FindControl("lblQuotationid");
                RadLabel lvlvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");
                PhoenixDryDockQuotation.Drydockfinalizeyard(new Guid(lblorderid.Text), new Guid(lblquotationid.Text), General.GetNullableInteger(lvlvesselid.Text), 0);
                PhoenixDryDockQuotation.UpdateDryDockQuotationYard(int.Parse(ViewState["VSLID"].ToString()), General.GetNullableGuid(lblquotationid.Text).Value, General.GetNullableGuid(lblorderid.Text).Value, 0);
                BindData();
                gvQuotation.Rebind();
            }
        }
        catch (Exception ex)
         {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuotation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton ib = (LinkButton)e.Item.FindControl("cmdExport2XL");
                LinkButton flg = (LinkButton)e.Item.FindControl("imgFlag");
                HtmlGenericControl html = new HtmlGenericControl();
                if (ib != null) ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
                RadLabel lblorderid = (RadLabel)e.Item.FindControl("lblOrderid");
                RadLabel lblquotationid = (RadLabel)e.Item.FindControl("lblQuotationid");
                RadLabel lblpoissued = (RadLabel)e.Item.FindControl("lblpoissued");
                string jvscript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] +"/DryDock/DryDockExport2XL.aspx?exportoption=quotation&orderid=" + lblorderid.Text + "&quotationid=" + lblquotationid.Text + "&vslid=" + ViewState["VSLID"].ToString() + "'); return false;";
                if (ib != null) ib.Attributes.Add("onclick", jvscript);
                if (lblpoissued.Text == "1")
                {
                    html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-star-yellow\"></i></span>";
                    flg.Controls.Add(html);
                }

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSendQuery");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);
                jvscript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/DryDock/DryDockSendQuery.aspx?type=sendquote&orderid=" + lblorderid.Text + "&quotationid=" + lblquotationid.Text + "&vslid=" + ViewState["VSLID"].ToString() + "'); return false;";
                if (sb != null) sb.Attributes.Add("onclick", jvscript);

                LinkButton sel = (LinkButton)e.Item.FindControl("cmdSelect");
                LinkButton desel = (LinkButton)e.Item.FindControl("cmdDeSelect");

                LinkButton jobprogress = (LinkButton)e.Item.FindControl("cmdjobprogress");
                if (jobprogress != null)
                {
                    jobprogress.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Dry Dock Job Progress','Drydock/DryDockJobProgress.aspx?orderid=" + lblorderid.Text + "&quotationid=" + lblquotationid.Text + "&vslid=" + ViewState["VSLID"].ToString()+ "','false','900px','500px');return false");

                }


                if (sel != null && desel != null)
                {
                    if ((DataBinder.Eval(e.Item.DataItem, "FLDISSELECTED").ToString() == string.Empty || DataBinder.Eval(e.Item.DataItem, "FLDISSELECTED").ToString() == "0") & (DataBinder.Eval(e.Item.DataItem, "FLDPOISSUED").ToString() == "0"))
                    {
                        sel.Visible = true;
                        desel.Visible = false;

                        html.InnerHtml = "<span class=\"icon\" style=\"color: white;\"><i class=\"icon sign-blank\"></i></span>";
                        flg.Controls.Add(html);
                    }
                    if (DataBinder.Eval(e.Item.DataItem, "FLDISSELECTED").ToString() == "1")
                    {
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-star-red\"></i></span>";
                        flg.Controls.Add(html);
                        sel.Visible = false;
                        desel.Visible = true;
                    }

                    if (DataBinder.Eval(e.Item.DataItem, "FLDISSELECTED").ToString() == string.Empty || DataBinder.Eval(e.Item.DataItem, "FLDISSELECTED").ToString() == "0")
                    {
                        sel.Visible = true;
                        desel.Visible = false;

                    }

                }

                LinkButton pre = (LinkButton)e.Item.FindControl("cmdPrepareQuote");
                if (pre != null) pre.Visible = SessionUtil.CanAccess(this.ViewState, pre.CommandName);
                //LinkButton status = (LinkButton)e.Item.FindControl("cmdJobs");
                //if (status != null) status.Visible = SessionUtil.CanAccess(this.ViewState, status.CommandName);
                LinkButton status1 = (LinkButton)e.Item.FindControl("cmdjobprogress");
                if (status1 != null) status1.Visible = SessionUtil.CanAccess(this.ViewState, status1.CommandName);

                if (DataBinder.Eval(e.Item.DataItem, "FLDMAKERYN").ToString() == "Yes")
                {
                    sel.Visible = false;
                    desel.Visible = false;
                    pre.Visible = false;
                    sb.Visible = false;
                    //status.Visible = false;
                    status1.Visible = false;
                    RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");
                    chk.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //  protected void gvQuotation_SelectedIndexChanged(object sender, EventArgs e)
    //  {
    //      try
    //      {
    //          var dataItem = gvQuotation.SelectedItems[0] as GridDataItem;
    //          if (dataItem != null)
    //          {
    //              Filter.CurrentSelectedDryDockQuotation = ((RadLabel)dataItem.FindControl("lblQuotationid")).Text;
    //              ViewState["QUOTATIONID"] = ((RadLabel)dataItem.FindControl("lblQuotationid")).Text;
    //              ViewState["DTKEY"] = ((RadLabel)dataItem.FindControl("lbldtkey")).Text;
    //              //BindData();
    //              BindFields();
    //          }
    //      }
    //      catch (Exception ex)
    //      {
    //          ucError.ErrorMessage = ex.Message;
    //          ucError.Visible = true;
    //      }
    //  }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        BindFields();
    }
}