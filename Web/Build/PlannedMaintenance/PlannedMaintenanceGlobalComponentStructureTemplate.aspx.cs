using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;
using OfficeOpenXml;
using System.Web.Services;
using System.Web;
using System.Collections.Generic;

public partial class PlannedMaintenanceGlobalComponentStructureTemplate : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["excel"] = "";
                Session["msg"] = "Saved successfully";
                //LoadExcelTemplate(ViewState["FID"].ToString());
                //btnExport.Visible = SessionUtil.CanAccess(this.ViewState, "EXPORT");
                //btnImport.Visible = SessionUtil.CanAccess(this.ViewState, "IMPORT");

                if (Request.QueryString["VESSELID"] != null&& Request.QueryString["VESSELID"].ToString()!="")
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                    PopulateWorkSheet();
                }

                
            }
            if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet1.UniqueID)
            {
                //ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
                Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(Request.Params["__CALLBACKPARAM"]);
                SaveExcelTemplate(workbook);
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
    private void SaveExcelTemplate(Workbook workbook)
    {
        int index = 0;
        try
        {
            Worksheet w = workbook.Sheets[0];

            if(ViewState["VESSELID"] !=null && General.GetNullableInteger(ViewState["VESSELID"].ToString()) != null)
            {
                PhoenixPlannedMaintenanceGlobalComponent.RefreshGlobalVesselMigrationTempComponent(int.Parse(ViewState["VESSELID"].ToString()));

                foreach (Row r in w.Rows)
                {
                    if (r.Index > 0)
                    {
                        if (r.Cells[0].Value != null
                                 && r.Cells[1].Value != null
                                 && r.Cells[2].Value != null
                                 && r.Cells[3].Value != null
                                 && r.Cells[4].Value != null
                                 )
                        {
                            int.TryParse(r.Index.ToString(), out index);

                            PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselMigrationScript(
                                                r.Cells[0].Value.ToString()
                                                , r.Cells[1].Value.ToString()
                                                , r.Cells[2].Value.ToString()
                                                , r.Cells[3].Value.ToString()
                                                , r.Cells[4].Value.ToString()
                                                , r.Cells[5].Value != null ? General.GetNullableString(r.Cells[5].Value.ToString()) : null
                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                );
                        }
                    }
                }

                PhoenixPlannedMaintenanceGlobalComponent.RefreshGlobalVesselMigrationScript(int.Parse(ViewState["VESSELID"].ToString()));
                PhoenixPlannedMaintenanceGlobalComponent.RefreshGlobalVesselMigrationScriptPopulate(int.Parse(ViewState["VESSELID"].ToString()));


                Session["msg"] = "Saved successfully";
            }
            else
            {
                ucError.ErrorMessage = "Vessel is required.";
                ucError.Visible = true;
                return;
            }


            
        }
        catch (Exception ex)
        {
            Session["msg"] = ex.Message;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private void PopulateWorkSheet()
    {
        try
        {
            DataTable dt = PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselComponentsList(int.Parse(ViewState["VESSELID"].ToString()));
            Worksheet s = FillWorksheet(dt);
            RadSpreadsheet1.Sheets.Add(s);

        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    public static Worksheet FillWorksheet(DataTable data)
    {

        var workbook = new Workbook();
        var sheet = workbook.AddSheet();

        sheet.Columns = new List<Column>();

        var row = new Row() { Index = 0 };
        int columnIndex = 0;

        foreach (DataColumn dataColumn in data.Columns)
        {
            sheet.Columns.Add(new Column());

            string cellValue = dataColumn.ColumnName;

            var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true };

            row.AddCell(cell);
        }

        sheet.AddRow(row);

        int rowIndex = 1;
        foreach (DataRow dataRow in data.Rows)
        {
            row = new Row() { Index = rowIndex++ };

            columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
                string cellValue = dataRow[dataColumn.ColumnName].ToString();

                var cell = new Cell() { Index = columnIndex++, Value = cellValue };

                row.AddCell(cell);
            }

            sheet.AddRow(row);
        }

        return sheet;



    }

    [WebMethod]
    public static string Message(string args)
    {
        return HttpContext.Current.Session["msg"].ToString();
    }

}