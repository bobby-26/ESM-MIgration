using Newtonsoft.Json.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;

public partial class PlannedMaintenanceHistoryTemplateDoneView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                Session["msg"] = "Saved successfully";
                ViewState["FID"] = "";
                ViewState["WID"] = Request.QueryString["wid"];
                ViewState["RID"] = Request.QueryString["rid"];
                ViewState["FRM"] = string.IsNullOrEmpty(Request.QueryString["frm"]) ? "" : Request.QueryString["frm"];
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"] != string.Empty)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                }
                LoadExcelTemplate(ViewState["RID"].ToString());
                btnExport.Visible = SessionUtil.CanAccess(this.ViewState, "EXPORT");
                
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
        DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.EditMaintenanceFormReport(int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["RID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["FID"] = dt.Rows[0]["FLDFORMID"].ToString();            
            string exceljson = dt.Rows[0]["FLDEXCELJSONREPORT"].ToString();
            ViewState["rdate"] = dt.Rows[0]["FLDREPORTDATECELL"].ToString();
            if (!string.IsNullOrEmpty(exceljson))
            {
                Workbook workbook = Workbook.FromJson(exceljson);
                RadSpreadsheet1.Sheets.AddRange(workbook.Sheets);                
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
                double d;
                double.TryParse(workbook.Sheets[0].Rows.Find(x => x.Index == int.Parse(row) - 1).Cells[col - 1].Value.ToString(), out d);
                if (d > 0)
                {
                    rd = DateTime.FromOADate(d);                    
                }
            }            
        }
        Guid? g = General.GetNullableGuid(ViewState["RID"].ToString());
        PhoenixPlannedMaintenanceHistoryTemplate.InsertMaintenanceFormDone(int.Parse(ViewState["VESSELID"].ToString())
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