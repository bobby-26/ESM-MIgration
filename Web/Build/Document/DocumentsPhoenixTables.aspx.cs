using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;
using Telerik.Web.UI;
public partial class DocumentsPhoenixTables : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            CreateMenu();
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["tablenumber"] = "";
                ViewState["tablename"] = "";

                if (Request.QueryString["tablenumber"] != null && Request.QueryString["tablename"] != null)
                {
                    ViewState["tablenumber"] = Request.QueryString["tablenumber"].ToString();
                    ViewState["tablename"] = Request.QueryString["tablename"].ToString();

                    if (Request.QueryString["tablenumber"].ToString() != "Dummy")
                        ddlTables.SelectedTable = Request.QueryString["tablenumber"].ToString();
                }

                ucVessel.bind();
                ucVessel.DataBind();
            }
            CreateMenu();
            //BindData();
            ProConnection();

            imgbtnSend.Visible = SessionUtil.CanAccess(this.ViewState, imgbtnSend.CommandName);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            //if (!IsValidTable(ddlTables.SelectedTable))
            //{
            //    ucError.Visible = true;
            //    return;
            //}


            if (ViewState["TempTable"] != null)
            {
                DataTable temporarytableDt = new DataTable();
                temporarytableDt = (DataTable)(ViewState["TempTable"]);

                if (temporarytableDt.Rows.Count == 1)
                {
                    if (temporarytableDt.Rows[0]["FLD1"].ToString() == string.Empty)
                    {
                        ucError.ErrorMessage = "Please give the value to search";
                        ucError.Visible = true;
                        return;
                    }
                }

                if (ViewState["tablename"].ToString() != "--Select--")
                {
                    if (ViewState["tablename"].ToString() != "")
                    {

                        NameValueCollection nvc = Filter.CurrentPhoenixTablesSelection;

                        //DataTable dt = PhoenixDocumentsTables.TableRecordsSearch(ViewState["tablename"].ToString()
                        //                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                        //                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtFromDate") : string.Empty)
                        //                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtToDate") : string.Empty)
                        //                                                                    , General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty));

                        string Query = General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty);

                        if (Query != null)
                        {
                            DataTable dt = PhoenixDocumentsTables.TableQuerySearch(General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty));

                            DataSet ds = new DataSet();
                            ds.Tables.Add(dt.Copy());

                            //if (dt.Rows.Count > 0)
                            //{
                                gvTableList.DataSource = dt;
                                gvTableList.DataBind();
                            //}
                        }
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
    protected void MenuTables_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                StringBuilder strQuery = new StringBuilder();
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                DataTable dtt = PhoenixDocumentsTables.TableColumnNameSearch(ViewState["tablename"].ToString());
                if (dtt.Rows.Count > 0)
                {
                    foreach (GridDataItem gvr in gridtodisplaytempData.Items)
                    {

                        RadLabel lblFLD1 = (RadLabel)gvr.FindControl("lblFLD1");
                        if (gvr.ItemIndex == 0)
                        {
                            strQuery.Append("SELECT * FROM " + ViewState["tablename"].ToString() + " (NOLOCK) WHERE ");
                            lblFLD1.Text = "";
                        }

                        RadLabel lblFLD2 = (RadLabel)gvr.FindControl("lblFLD2");
                        RadLabel lblFLD3 = (RadLabel)gvr.FindControl("lblFLD3");
                        RadLabel lblFLD4 = (RadLabel)gvr.FindControl("lblFLD4");
                        RadLabel lblFLD5 = (RadLabel)gvr.FindControl("lblFLD5");

                        if (gridtodisplaytempData.Items.Count > 0)
                        {
                            if (lblFLD3.Text == "IN")
                                strQuery.Append(lblFLD1.Text + " " + lblFLD2.Text + " " + lblFLD3.Text + " (" + lblFLD4.Text + ")" + " " + lblFLD5.Text);
                            else if (lblFLD3.Text == "LIKE")
                                strQuery.Append(lblFLD1.Text + " " + lblFLD2.Text + " " + lblFLD3.Text + " '%" + lblFLD4.Text + "%'" + " " + lblFLD5.Text);
                            else
                                strQuery.Append(lblFLD1.Text + " " + lblFLD2.Text + " " + lblFLD3.Text + " '" + lblFLD4.Text + "'" + " " + lblFLD5.Text);
                        }
                    }


                    int cf = 0;
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        if (dtt.Rows[i]["COLUMN_NAME"].ToString() == "FLDAUDITDATETIME")
                        {
                            strQuery.Append(" ORDER BY FLDAUDITDATETIME");
                            cf = 1;
                        }
                        if (cf == 0)
                        {
                            if (dtt.Rows[i]["COLUMN_NAME"].ToString() == "FLDCREATEDDATE")
                                strQuery.Append(" ORDER BY FLDCREATEDDATE");
                        }
                    }


                    //if (Dr["COLUMN_NAME"].ToString() == "FLDAUDITDATETIME")
                    //    strQuery.Append(" ORDER BY FLDAUDITDATETIME");
                    //else if (Dr["COLUMN_NAME"].ToString() == "FLDCREATEDDATE")
                    //    strQuery.Append(" ORDER BY FLDCREATEDDATE");



                }
                criteria.Add("txtQuery", strQuery.ToString());
                Filter.CurrentPhoenixTablesSelection = criteria;

                BindData();
                ProConnection();

                lblSql.Text = strQuery.ToString();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                if (ViewState["TempTable"] != null)
                {
                    DataTable temporarytableDt = new DataTable();
                    temporarytableDt = (DataTable)(ViewState["TempTable"]);

                    if (temporarytableDt.Rows.Count == 1)
                    {
                        if (temporarytableDt.Rows[0]["FLD1"].ToString() == string.Empty)
                        {
                            ucError.ErrorMessage = "Please give the value to search";
                            ucError.Visible = true;
                            return;
                        }
                    }


                    if (!IsValidTable(ddlTables.SelectedTable))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    NameValueCollection nvc = Filter.CurrentPhoenixTablesSelection;

                    if (nvc != null)
                    {
                        if (ViewState["tablename"].ToString() != "--Select--")
                        {


                            //DataTable dt = PhoenixDocumentsTables.TableRecordsSearch(ViewState["tablename"].ToString()
                            //                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                            //                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtFromDate") : string.Empty)
                            //                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtToDate") : string.Empty)
                            //                                                                    , General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty));

                            DataTable dt = PhoenixDocumentsTables.TableQuerySearch(General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty));

                            ShowExcel("Table Records", dt);
                        }
                    }
                }

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPhoenixTablesSelection = null;
                Response.Redirect("../Document/DocumentsPhoenixTables.aspx");
            }
            else if (CommandName.ToUpper().Equals("SEND"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Document/DocumentsPhoenixTables.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageButton("../Document/DocumentsPhoenixTables.aspx", "Find", "search.png", "FIND");
        //toolbar.AddImageButton("../Document/DocumentsPhoenixTablesFilter.aspx?tablenumber=" + ViewState["tablenumber"].ToString() + "&tablename=" + ViewState["tablename"].ToString() + "", "Custom Search", "search.png", "CUSTOMSEARCH");
        toolbar.AddImageButton("../Document/DocumentsPhoenixTables.aspx", "Clear", "clear-filter.png", "CLEAR");
        //toolbar.AddImageButton("../Document/DocumentsPhoenixTables.aspx", "Send to Vessel", "24.png", "SEND");
        MenuTables.AccessRights = this.ViewState;
        MenuTables.MenuList = toolbar.Show();
    }

    protected void ddlTables_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["tablename"] = ddlTables.SelectedTableName;
        ViewState["tablenumber"] = ddlTables.SelectedTable;
        CreateMenu();
        BindData();
        ProConnection();
    }

    public static void ShowExcel(string strHeading, DataTable dt)
    {
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td><h3>" + strHeading + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");

        HttpContext.Current.Response.Write("<tr>");
        foreach (DataColumn c in dt.Columns)
        {
            HttpContext.Current.Response.Write("<td><b>");
            HttpContext.Current.Response.Write(c.ColumnName);
            HttpContext.Current.Response.Write("</b></td>");
        }
        HttpContext.Current.Response.Write("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<td>");
                HttpContext.Current.Response.Write(dr[i].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[i].ToString()) : dr[i]);
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        HttpContext.Current.Response.End();
    }

    private void ExportExcel(string strHeading, DataTable dt)
    {
        using (ExcelPackage pck = new ExcelPackage())
        {
            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Phoenix");

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            ws.Cells["A1"].LoadFromDataTable(dt, true);

            //prepare the range for the column headers
            string cellRange = "A1:" + Convert.ToChar('A' + dt.Columns.Count - 1) + 1;

            //Format the header for columns
            using (ExcelRange rng = ws.Cells[cellRange])
            {
                rng.Style.WrapText = false;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                rng.Style.Font.Color.SetColor(Color.White);
            }

            //prepare the range for the rows
            string rowsCellRange = "A2:" + Convert.ToChar('A' + dt.Columns.Count - 1) + dt.Rows.Count * dt.Columns.Count;

            //Format the rows
            using (ExcelRange rng = ws.Cells[rowsCellRange])
            {
                rng.Style.WrapText = false;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }

            //Read the Excel file in a byte array
            Byte[] fileBytes = pck.GetAsByteArray();

            //Clear the response
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Cookies.Clear();

            //Add the header & other information
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.CacheControl = "private";
            Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
            Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
            Response.AppendHeader("Pragma", "cache");
            Response.AppendHeader("Expires", "60");
            Response.AppendHeader("Content-Disposition",
            "attachment; " +
            "filename=\"" + strHeading + ".xlsx\"; " +
            "size=" + fileBytes.Length.ToString() + "; " +
            "creation-date=" + DateTime.Now.ToString("R") + "; " +
            "modification-date=" + DateTime.Now.ToString("R") + "; " +
            "read-date=" + DateTime.Now.ToString("R"));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //Write it back to the client
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }

    private bool IsValidTable(string StrTableName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (StrTableName == "Dummy")
        {
            ucError.ErrorMessage = "Please Select a Table";
        }

        return (!ucError.IsError);
    }
    // New Code

    private DataTable ProConnection()
    {
        //ClsBusinessConnection objBus = new ClsBusinessConnection();
        //DataTable BindAllVal = objBus.DClsBusinessConnection();
        //gridtodisplaytempData.DataSource = BindAllVal;
        // gridtodisplaytempData.DataBind();

        //above  4 line code need to  bind gridview with database  connectivity.
        //but  currently if you don't have database and you need to do some gridview functionality //then you can create temporary table.
        DataTable temporarytableDt = new DataTable();
        if (ViewState["TempTable"] != null)
        {
            temporarytableDt = (DataTable)(ViewState["TempTable"]);
        }
        else
        {
            temporarytableDt.Columns.Add("FLD1", typeof(String));
            temporarytableDt.Columns.Add("FLD2", typeof(String));
            temporarytableDt.Columns.Add("FLD3", typeof(String));
            temporarytableDt.Columns.Add("FLD4", typeof(String));
            temporarytableDt.Columns.Add("FLD5", typeof(String));

            DataRow datarowvar = temporarytableDt.NewRow();
            temporarytableDt.Rows.Add(datarowvar);
        }

        if (temporarytableDt.Rows.Count <= 0 && ViewState["TempTable"] != null)
        {
            DataRow datarowvar = temporarytableDt.NewRow();
            temporarytableDt.Rows.Add(datarowvar);
        }

        gridtodisplaytempData.DataSource = temporarytableDt;
        gridtodisplaytempData.DataBind();
        return temporarytableDt;
    }

    public void gridtodisplaytempData_Click(object sender, System.EventArgs e)
    {
        ProConnection();
    }
    private bool IsValidFields(string v1, string v2, string v3)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (v1 == "--Select--")
            ucError.ErrorMessage = "Operator is Required";
        if (v2 == "")
            ucError.ErrorMessage = "Field Name is Required";
        if (v3 == "--Select--")
            ucError.ErrorMessage = "Condition is Required";

        return (!ucError.IsError);
    }
    protected void imgbtnSend_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentPhoenixTablesSelection;

        if (nvc != null && ViewState["tablename"].ToString() != "--Select--" && (General.GetNullableInteger(ucVessel.SelectedVessel) != null || ddlTableType.SelectedValue == "1"))
        {
            // String script = String.Format("javascript:parent.fnConfirmDelete('event','Are you sure you want to send data to vessel?');");
            //ScriptManager.RegisterClientScriptBlock(Page,Page.GetType(), "script", script,true);

            PhoenixDocumentsTables.TableData2Vessel(General.GetNullableString(nvc != null ? nvc.Get("txtQuery") : string.Empty), ViewState["tablename"].ToString(), General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlAuditType.SelectedValue), int.Parse(ddlTableType.SelectedValue));

            ucStatus.Text = "Data Sent to Vessel";
        }

        BindData();
        ProConnection();
    }

    protected void gridtodisplaytempData_ItemCommand(object sender, GridCommandEventArgs e)
    {
        DataTable temporarytableDt = new DataTable();

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidFields(((RadDropDownList)e.Item.FindControl("ddlAddOperater1")).SelectedText, ((RadComboBox)e.Item.FindControl("ddlAddField")).SelectedValue, ((RadDropDownList)e.Item.FindControl("ddlAddOperater2")).SelectedText))
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["TempTable"] == null)
            {
                temporarytableDt.Columns.Add("FLD1", typeof(String));
                temporarytableDt.Columns.Add("FLD2", typeof(String));
                temporarytableDt.Columns.Add("FLD3", typeof(String));
                temporarytableDt.Columns.Add("FLD4", typeof(String));
                temporarytableDt.Columns.Add("FLD5", typeof(String));
            }
            else
            {
                temporarytableDt = (DataTable)(ViewState["TempTable"]);
            }
            DataRow datarowvar = temporarytableDt.NewRow();

            datarowvar["FLD1"] = (((RadDropDownList)e.Item.FindControl("ddlAddOperater1")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlAddOperater1")).SelectedText;
            datarowvar["FLD2"] = ((RadComboBox)e.Item.FindControl("ddlAddField")).SelectedValue;
            datarowvar["FLD3"] = (((RadDropDownList)e.Item.FindControl("ddlAddOperater2")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlAddOperater2")).SelectedText;
            datarowvar["FLD4"] = ((RadTextBox)e.Item.FindControl("txtAddValue")).Text.Trim();
            datarowvar["FLD5"] = (((RadDropDownList)e.Item.FindControl("ddlAddOperater3")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlAddOperater3")).SelectedText;
            temporarytableDt.Rows.Add(datarowvar);
            ViewState["TempTable"] = temporarytableDt;

            if (temporarytableDt.Rows[0]["FLD1"].ToString() == string.Empty)
            {
                DataTable dtnew = (DataTable)(ViewState["TempTable"]);
                dtnew.Rows.RemoveAt(0);
            }
            ProConnection();
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["TempTable"] != null)
            {
                if (!IsValidFields(((RadDropDownList)e.Item.FindControl("ddlEditOperater1")).SelectedText, ((RadComboBox)e.Item.FindControl("ddlEditField")).SelectedValue, ((RadDropDownList)e.Item.FindControl("ddlEditOperater2")).SelectedText))
                {
                    ucError.Visible = true;
                    return;
                }

                temporarytableDt = (DataTable)(ViewState["TempTable"]);

                temporarytableDt.Rows[e.Item.ItemIndex]["FLD1"] = (((RadDropDownList)e.Item.FindControl("ddlEditOperater1")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlEditOperater1")).SelectedText;
                temporarytableDt.Rows[e.Item.ItemIndex]["FLD2"] = ((RadComboBox)e.Item.FindControl("ddlEditField")).SelectedValue;
                temporarytableDt.Rows[e.Item.ItemIndex]["FLD3"] = (((RadDropDownList)e.Item.FindControl("ddlEditOperater2")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlEditOperater2")).SelectedText;
                temporarytableDt.Rows[e.Item.ItemIndex]["FLD4"] = ((RadTextBox)e.Item.FindControl("txtEditValue")).Text;
                temporarytableDt.Rows[e.Item.ItemIndex]["FLD5"] = (((RadDropDownList)e.Item.FindControl("ddlEditOperater3")).SelectedText == "--Select--") ? "" : ((RadDropDownList)e.Item.FindControl("ddlEditOperater3")).SelectedText;
            }
            ProConnection();
            gridtodisplaytempData.SelectedIndexes.Clear();
            gridtodisplaytempData.EditIndexes.Clear();
            gridtodisplaytempData.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            if (ViewState["TempTable"] != null)
            {
                DataTable dtnew = (DataTable)(ViewState["TempTable"]);
                dtnew.Rows.RemoveAt(e.Item.ItemIndex);
                gridtodisplaytempData.SelectedIndexes.Clear();
                gridtodisplaytempData.EditIndexes.Clear();
                gridtodisplaytempData.Rebind();
                Filter.CurrentPhoenixTablesSelection = null;
                BindData();
                gvTableList.SelectedIndexes.Clear();
                gvTableList.EditIndexes.Clear();
                gvTableList.Rebind();
            }
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            
        }
        else
        {
        }
    }

    protected void gridtodisplaytempData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            DataTable temporarytableDt = new DataTable();
            if (ViewState["TempTable"] != null)
            {
                temporarytableDt = (DataTable)(ViewState["TempTable"]);

                RadDropDownList ddlFld1 = (RadDropDownList)e.Item.FindControl("ddlEditOperater1");
                if (ddlFld1 != null) ddlFld1.SelectedText = temporarytableDt.Rows[e.Item.ItemIndex]["FLD1"].ToString();

                RadDropDownList ddlFld3 = (RadDropDownList)e.Item.FindControl("ddlEditOperater2");
                if (ddlFld3 != null) ddlFld3.SelectedText = temporarytableDt.Rows[e.Item.ItemIndex]["FLD3"].ToString();

                RadTextBox ddlFld4 = (RadTextBox)e.Item.FindControl("txtEditValue");
                if (ddlFld4 != null) ddlFld4.Text = temporarytableDt.Rows[e.Item.ItemIndex]["FLD4"].ToString();

                RadDropDownList ddlFld5 = (RadDropDownList)e.Item.FindControl("ddlEditOperater3");
                if (ddlFld5 != null) ddlFld5.SelectedText = temporarytableDt.Rows[e.Item.ItemIndex]["FLD5"].ToString();

                RadComboBox ddlTableCol = (RadComboBox)e.Item.FindControl("ddlEditField");
                if (ddlTableCol != null)
                {
                    DataTable dt = PhoenixDocumentsTables.TableColumnNameSearch(ViewState["tablename"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        ddlTableCol.DataSource = dt;
                        ddlTableCol.DataBind();
                        ddlTableCol.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                    }

                    if (ddlTableCol != null) ddlTableCol.SelectedValue = temporarytableDt.Rows[e.Item.ItemIndex]["FLD2"].ToString();
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlAddTableCol = (RadComboBox)e.Item.FindControl("ddlAddField");
            if (ddlAddTableCol != null)
            {
                if (ViewState["tablename"].ToString() != "--Select--")
                {
                    if (ViewState["tablename"].ToString() != "")
                    {
                        DataTable dt = PhoenixDocumentsTables.TableColumnNameSearch(ViewState["tablename"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            ddlAddTableCol.DataSource = dt;
                            ddlAddTableCol.DataBind();
                            ddlAddTableCol.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                        }
                        else
                            ddlAddTableCol.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                    }
                    else
                        ddlAddTableCol.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                }
                else
                    ddlAddTableCol.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
        }
    }

    protected void gridtodisplaytempData_EditCommand(object sender, GridCommandEventArgs e)
    {
        ProConnection();
    }
}
