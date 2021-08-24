using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Export2XL;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using SouthNests.Phoenix.Reports;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

public partial class Purchase_PurchaseQuotationVendorExportExcel : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{

			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("PDF", "PDF",ToolBarDirection.Right);
            toolbarmain.AddButton("Excel", "Excel",ToolBarDirection.Right);
			
			MenuFormFilter.MenuList = toolbarmain.Show();
			if (!IsPostBack)
			{

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void MenuFormFilter_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
			{
				PhoenixPurchase2XL.Export2XLQuotationComaparision(txtRequisionNos.Text);
			}
			if (CommandName.ToUpper().Equals("PDF"))
			{
				QuotationPdfExport();
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

	}
	protected void QuotationPdfExport()
	{
		DataTable dtQuotation = new DataTable();
		string zipfilepath;
		zipfilepath = Guid.NewGuid().ToString();
		string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath +"/TEMP";
		string newfolderpath = Path.Combine(path, zipfilepath);
		Directory.CreateDirectory(newfolderpath);
		try
		{
			dtQuotation = PhoenixPurchaseQuotationvendorExportExcel.GetQuotationList(txtRequisionNos.Text);
			if (dtQuotation.Rows.Count > 0)
			{
				int i = 0;
				while (i < dtQuotation.Rows.Count)
				{

					DataRow dr = dtQuotation.Rows[i];
					Filter.CurrentPurchaseStockType = dr["FLDSTOCKTYPE"].ToString();
					//string filename = "Quotation_" + dr["FLDQUOTATIONID"].ToString();
					GenerateGRNPDF(dr["FLDQUOTATIONID"].ToString(), dr["FLDORDERID"].ToString(), zipfilepath);

					i += 1;
				}
				createZipFile(zipfilepath);
				//clearDirectory();

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void createZipFile(string zipfilename)
	{

		try
		{
			PhoenixPurchaseQuotationvendorExportExcel.createQuotationZip(new Guid(zipfilename));

			Response.Clear();
			Response.AddHeader("Content-Disposition", "attachment; filename=" + zipfilename + ".zip");
			Response.ContentType = "application/zip";

			Response.TransmitFile(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath +"/TEMP/" + zipfilename + ".zip");

			Response.End();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void GenerateGRNPDF(string quotationid, string orderid, string zipfilepath)
	{
		string[] reportfile = new string[4];
		string fullfilename = "";

		if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
		{

			PurchaseOrderVendorQuotationGetWithSubReportsForRDLC(orderid, quotationid, zipfilepath);
		}
		else
		{
			DataSet ds = new DataSet();
			ds = PurchaseOrderVendorQuotationGetWithSubReports(new Guid(orderid), new Guid(quotationid), ref reportfile, ref fullfilename, zipfilepath);
			PhoenixReportClass.ExportReport(reportfile, fullfilename, ds);
		}
	}

	private  static DataSet PurchaseOrderVendorQuotationGetWithSubReports(Guid orderid, Guid quotationId, ref string[] reportfile, ref string filename, string zipfilepath)
	{
		DataSet ds = new DataSet();
		if (quotationId != null)
		{
			ds = PhoenixPurchaseOrderForm.PurchaseOrderVendorGetWithSubReports(quotationId, orderid);
		}
		else
		{
			ds = PhoenixPurchaseOrderForm.PurchaseOrderVendorGetWithSubReports(orderid);
		}
		if (ds.Tables[0].Rows.Count > 0)
		{
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				dr["FLDNOTES"] = Regex.Replace(dr["FLDNOTES"].ToString(), @"<(.|\n)*?>", string.Empty);
				dr.AcceptChanges();
			}
		}
		ds.Tables[0].AcceptChanges();
		ds.AcceptChanges();

		if (ds.Tables[0].Rows[0]["FLDSTOCKTYPE"].ToString().ToUpper().Equals("SPARE"))
		{
			reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsPurchaseVendorQuotation.rpt");
		}
		else
		{
			reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsPurchaseVendorStoreQuotation.rpt");
		}
		reportfile[1] = "ReportsPurchaseOrderTaxAndCharge.rpt";

		string vendorName = ds.Tables[0].Rows[0]["FLDVENDORNAME"].ToString();
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		vendorName = rgx.Replace(vendorName, "");
		vendorName = Regex.Replace(vendorName, @"\s", "_");

		filename = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString() + "-" + vendorName + "-" + quotationId.ToString().Substring(0, 5) + ".pdf";
		filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath +"/TEMP/" + zipfilepath + "/" + filename;

		return ds;
	}
	private void clearDirectory()
	{
		string pathname = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath +"/TEMP/Quotations";
		DirectoryInfo di = new DirectoryInfo(pathname);
		if (Directory.Exists(pathname))
			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}
	}
	private void PurchaseOrderVendorQuotationGetWithSubReportsForRDLC(string orderid, string quotationid, string zipfilepath)
	{
		DataSet ds = new DataSet();
		string Tmpfilelocation = string.Empty;
		string[] reportfile = new string[1];

		if (quotationid != null && quotationid != "")
		{
			ds = PhoenixPurchaseOrderForm.PurchaseOrderVendorGetWithSubReports(new Guid(quotationid), new Guid(orderid));
		}
		else
		{
			ds = PhoenixPurchaseOrderForm.PurchaseOrderVendorGetWithSubReports(new Guid(orderid));
		}

		if (ds.Tables[0].Rows.Count > 0)
		{
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				dr["FLDNOTES"] = Regex.Replace(dr["FLDNOTES"].ToString(), @"<(.|\n)*?>", string.Empty);
				dr.AcceptChanges();
			}
		}
		ds.Tables[0].AcceptChanges();
		ds.AcceptChanges();

		NameValueCollection nvc = new NameValueCollection();
		nvc.Add("applicationcode", "3");
		nvc.Add("reportcode", "VENDORQUOTATION");

		if (ds.Tables[0].Rows[0]["FLDSTOCKTYPE"].ToString().ToUpper().Equals("SPARE"))
		{
			nvc.Add("CRITERIA", "0");
		}
		else
		{
			nvc.Add("CRITERIA", "1");
		}
		Session["PHOENIXREPORTPARAMETERS"] = nvc;

		//Tmpfilelocation = System.Web.HttpContext.Current.Server.MapPath("~/");
		string vendorName = ds.Tables[0].Rows[0]["FLDVENDORNAME"].ToString();
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		vendorName = rgx.Replace(vendorName, "");
		vendorName = Regex.Replace(vendorName, @"\s", "_");

		string filename = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString() + "-" + vendorName + "-" + quotationid.ToString().Substring(0, 5) + ".pdf";
		//filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + zipfilepath + "/") + filename;
		Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath +"/TEMP/" + zipfilepath + "/" + filename;

		PhoenixSsrsReportsCommon.getVersion();
		PhoenixSsrsReportsCommon.getLogo();
		PhoenixSSRSReportClass.ExportSSRSReportWithCriteria(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, Tmpfilelocation, ref Tmpfilelocation);

	}
}
