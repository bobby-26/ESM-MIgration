using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;

public partial class PlannedMaintenanceHistoryTemplateDone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                Session["msg"] = "Saved successfully";
                ViewState["FID"] = Request.QueryString["fid"];
                ViewState["WID"] = Request.QueryString["wid"];
                ViewState["RID"] = string.IsNullOrEmpty(Request.QueryString["rid"]) ? "" : Request.QueryString["rid"];
                ViewState["FRM"] = string.IsNullOrEmpty(Request.QueryString["frm"]) ? "" : Request.QueryString["frm"];
                LoadExcelTemplate(ViewState["FID"].ToString());                
                if (!SessionUtil.CanAccess(this.ViewState, "EXPORT"))
                    RadScriptManager.RegisterStartupScript(this.Page, typeof(Page), "hide", "setTimeout(f, 200);", true);
            }
            if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet1.UniqueID)
            {
                string json = Request.Params["__CALLBACKPARAM"];
                JObject o = JObject.Parse(json);
                IEnumerable<JToken> validation = o.SelectTokens("$..validation");
                string msg = "";
                foreach (JToken item in validation)
                {
                    object s = item.Parent.Parent["value"];
                    if (s == null)
                    {
                        string colref = ColumnIndexToColumnLetter(int.Parse(((JValue)(item["col"])).Value + "") + 1);
                        string cellref = int.Parse(((JValue)(item["row"])).Value + "") + 1 + "";
                        if (item["messageTemplate"] == null)
                            msg = msg + "Enter a value that satisfies the condition in " + (colref + cellref) + "<br/> ";
                        else
                            msg = msg + ((JValue)(item["messageTemplate"])).Value + " in " + (colref + cellref) + "<br/>";
                    }
                }
                if (msg != string.Empty)
                    Session["msg"] = "<strong>Provide</strong> the following information: <h3 style='color: #ff0000;font-size: inherit'>"+ msg + " </h3>";
                else
                {
                    SaveExcelTemplate(json);
                    Session["msg"] = "Saved successfully";
                }
            }                 
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
            ViewState["rdate"] = dt.Rows[0]["FLDREPORTDATECELL"].ToString();
            string exceljson = dt.Rows[0]["FLDEXCELJSON"].ToString();
            if (!string.IsNullOrEmpty(exceljson))
            {
                string[] key = { "VESSEL", "COMPONENTNAME", "COMPONENTNUMBER", "MAKE", "TYPE" };
                foreach(string s in key)
                {
                    if (exceljson.IndexOf("{#" + s + "#}") > -1)
                    {
                        DataTable keydt = PhoenixPlannedMaintenanceHistoryTemplate.FetchMaintenanceFormKeyValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , new Guid(ViewState["WID"].ToString())
                            , s);
                        if (keydt.Rows.Count > 0)
                        {
                            exceljson = exceljson.Replace("{#" + s + "#}", keydt.Rows[0][0].ToString());
                        }
                    }
                }
                string[] tokens = exceljson.Split(new[] { "{#" }, StringSplitOptions.None);
                Workbook workbook = Workbook.FromJson(exceljson);
                RadSpreadsheet1.Sheets.AddRange(workbook.Sheets);
                Cell cell = workbook.Sheets[0].Rows.Find(x => x.Index == 0).Cells[0];
                cell.Value = FormId + "~" + ViewState["WID"].ToString();
                cell.Color = "#ffffff";
            }
            else
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
        }
    }
    private void SaveExcelTemplate(string ExcelJson)
    {
        DateTime? rd = null;
        if (ViewState["rdate"].ToString() != string.Empty)
        {
            Workbook workbook = Workbook.FromJson(ExcelJson);
            var column = Regex.Replace(ViewState["rdate"].ToString(), "[0-9]+", string.Empty);
            var row = Regex.Replace(ViewState["rdate"].ToString(), "[^0-9]+", string.Empty);
            var col = ColumnLetterToColumnIndex(column);
            if (General.GetNullableInteger(row).HasValue)
            {
                if (workbook.Sheets[0].Rows.Find(x => x.Index == int.Parse(row) - 1).Cells[col - 1].Value != null)
                {
                    double d;
                    double.TryParse(workbook.Sheets[0].Rows.Find(x => x.Index == int.Parse(row) - 1).Cells[col - 1].Value.ToString(), out d);
                    if (d > 0)
                    {
                        rd = DateTime.FromOADate(d);
                    }
                }
            }            
        }
        Guid? g = General.GetNullableGuid(ViewState["RID"].ToString());
        PhoenixPlannedMaintenanceHistoryTemplate.InsertMaintenanceFormDone(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , new Guid(ViewState["FID"].ToString())
            , new Guid(ViewState["WID"].ToString())            
            , ref g
            , ExcelJson
            , rd);
        ViewState["RID"] = g;        
    }
    private string ColumnIndexToColumnLetter(int colIndex)
    {
        int div = colIndex;
        string colLetter = String.Empty;
        int mod = 0;

        while (div > 0)
        {
            mod = (div - 1) % 26;
            colLetter = (char)(65 + mod) + colLetter;
            div = (int)((div - mod) / 26);
        }
        return colLetter;
    }
    public static int ColumnLetterToColumnIndex(string columnLetter)
    {
        columnLetter = columnLetter.ToUpper();
        int sum = 0;

        for (int i = 0; i < columnLetter.Length; i++)
        {
            sum *= 26;
            sum += (columnLetter[i] - 'A' + 1);
        }
        return sum;
    }
    [WebMethod]
    public static string Message(string args)
    {
        return HttpContext.Current.Session["msg"].ToString();
    }
}