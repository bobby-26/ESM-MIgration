using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;
using OfficeOpenXml;
public partial class PlannedMaintenanceHistoryExcelTemplate : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["excel"] = "";
                ViewState["FID"] = Request.QueryString["fid"];
                LoadExcelTemplate(ViewState["FID"].ToString());
                btnExport.Visible = SessionUtil.CanAccess(this.ViewState, "EXPORT");
                btnImport.Visible = SessionUtil.CanAccess(this.ViewState, "IMPORT");
            }
            if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet1.UniqueID)
            {
                ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
                SaveExcelTemplate(ViewState["FID"].ToString(), ViewState["excel"].ToString(), txtReportDateCell.Text);                
            }
            if (!SessionUtil.CanAccess(this.ViewState, "SAVE"))
                RadScriptManager.RegisterStartupScript(this.Page, typeof(Page), "hide", "setTimeout(f, 200);", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void LoadExcelTemplate(string FormId)
    {
        DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.EditMaintenanceForm(new Guid(FormId));
        if (dt.Rows.Count > 0)
        {
            txtReportDateCell.Text = dt.Rows[0]["FLDREPORTDATECELL"].ToString();
            string exceljson = dt.Rows[0]["FLDEXCELJSON"].ToString();
            if (!string.IsNullOrEmpty(exceljson))
            {
                Workbook workbook = Workbook.FromJson(exceljson);
                RadSpreadsheet1.Sheets.AddRange(workbook.Sheets);                
            }
            else if(!string.IsNullOrEmpty(dt.Rows[0]["FLDFORMURL"].ToString()))
            {
                string resultFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx";
                string file = Server.MapPath(dt.Rows[0]["FLDFORMURL"].ToString());                
                //string resultFile = Server.MapPath("~/Template/PlannedMaintenance/SpreadSheet.xlsx");
                FileInfo fiTemplate = new FileInfo(file);
                FileInfo output = new FileInfo(resultFile);
                using (ExcelPackage pck = new ExcelPackage(fiTemplate))
                {
                    pck.SaveAs(output);
                }                
                SpreadsheetDocumentProvider provider = new SpreadsheetDocumentProvider(resultFile);
                RadSpreadsheet1.Provider = provider;
            }
            ViewState["excel"] = exceljson;
        }
    }
    private void SaveExcelTemplate(string FormId, string ExcelJson, string ReportDateCell)
    {
        PhoenixPlannedMaintenanceHistoryTemplate.UpdateMaintenanceFormExcelTemplate(new Guid(FormId), ExcelJson, ReportDateCell);
    }

    protected void txtReportDateCell_TextChanged(object sender, EventArgs e)
    {
        PhoenixPlannedMaintenanceHistoryTemplate.UpdateMaintenanceFormExcelTemplate(new Guid(ViewState["FID"].ToString()), ViewState["excel"].ToString(), txtReportDateCell.Text);
    }
}