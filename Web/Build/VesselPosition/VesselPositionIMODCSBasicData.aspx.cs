using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class VesselPositionIMODCSBasicData : PhoenixBasePage
{
    DataSet jsonData = new DataSet();
    DataTable dummydt = new DataTable();
    DataTable dt1Pdf = new DataTable();
    DataTable dtB3 = new DataTable();
    DataTable dtPartB24 = new DataTable();
    DataTable dtPartB2 = new DataTable();
    DataSet data = new DataSet();
    DataSet data9 = new DataSet();
    DataTable dtPartB5 = new DataTable();
    DataTable dtPartB6 = new DataTable();
    DataTable dtPartB7 = new DataTable();
    DataTable dtPartB8 = new DataTable();
    DataTable dtPartB5_2 = new DataTable();
    DataTable dtPartB6_2 = new DataTable();
    DataTable dtPartB7_2 = new DataTable();
    DataTable dtPartB8_2 = new DataTable();
    DataTable dtPartQ9_1 = new DataTable();
    DataTable dtPartQ9_2 = new DataTable();
    DataTable dtPartQ9_3 = new DataTable();
    DataTable dtPartQ9_4 = new DataTable();
    DataTable dtPartQ9_5 = new DataTable();
    DataTable dtPartQ9_6 = new DataTable();
    string str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("Export Pdf", "EXPORTPDF",ToolBarDirection.Right);

            MenuVoyageTap.AccessRights = this.ViewState;
            MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(MenuVoyageTap);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucVessel.bind();
                ucVessel.DataBind();
                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
            }
            BindShipData();
            BindMethodEdit();
            Bindmeasure();
            Bindmeasure1();
            Bindmeasure2();
            BindQualityData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindQualityData()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSDMSQualityDataEdit();
        DataTable dt = ds.Tables[0];

        DataSet dsc1 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("FC"));
        DataTable dt1 = dsc1.Tables[0];
        dtPartQ9_1 = dsc1.Tables[0];
        if (dt1.Rows.Count > 0)
        {
            Q1.DataSource = dt1;
            Q1.DataBind();
        }

        DataSet dsc2 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("DT"));
        DataTable dt2 = dsc2.Tables[0];
        dtPartQ9_2 = dsc2.Tables[0];
        if (dt2.Rows.Count > 0)
        {
            Q2.DataSource = dt2;
            Q2.DataBind();
        }

        DataSet dsc3 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("HU"));
        DataTable dt3 = dsc3.Tables[0];
        dtPartQ9_3 = dsc3.Tables[0];
        if (dt3.Rows.Count > 0)
        {
            Q3.DataSource = dt3;
            Q3.DataBind();
        }

        DataSet dsc4 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("ADC"));
        DataTable dt4 = dsc4.Tables[0];
        dtPartQ9_4 = dsc4.Tables[0];
        if (dt4.Rows.Count > 0)
        {
            Q4.DataSource = dt4;
            Q4.DataBind();
        }

        DataSet dsc5 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("IT"));
        DataTable dt5 = dsc5.Tables[0];
        dtPartQ9_5 = dsc5.Tables[0];
        if (dt5.Rows.Count > 0)
        {
            Q5.DataSource = dt5;
            Q5.DataBind();
        }

        DataSet dsc6 = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("IRV"));
        DataTable dt6 = dsc6.Tables[0];
        dtPartQ9_6 = dsc6.Tables[0];
        if (dt6.Rows.Count > 0)
        {
            Q6.DataSource = dt6;
            Q6.DataBind();
        }

        DataTable a1 = data9.Tables.Add("1");

        a1.Columns.Add("a");
        a1.Columns.Add("b");
        a1.Columns.Add("c");
        a1.Columns.Add("d");
        a1.Columns.Add("e");
        a1.Columns.Add("f");
        a1.Columns.Add("g");
        a1.Columns.Add("h");
        a1.Columns.Add("i");
        a1.Columns.Add("j");
        a1.Columns.Add("k");
        a1.Columns.Add("l");

        if (dt.Rows.Count > 0)
        {
            txteuprocedure1.Content = dt.Rows[0]["FLDFUELCONSDMSDESCRIPTION"].ToString();            
            txteuprocedure2.Content = dt.Rows[0]["FLDDISTANCETRAVELLEDDESCRIPTION"].ToString();            
            txteuprocedure3.Content = dt.Rows[0]["FLDHOURSUNDERWAYDMSDESCRIPTION"].ToString();            
            txteuprocedure4.Content = dt.Rows[0]["FLDADEQUECYDATADESCRIPTION"].ToString();            
            txteuprocedure5.Content = dt.Rows[0]["FLDITDMSDESCRIPTION"].ToString();            
            txteuprocedure6.Content = dt.Rows[0]["FLDINTERNALREVIEWSDMSDESCRIPTION"].ToString();
            
            DataRow drv = dt.Rows[0];
            a1.Rows.Add(HttpUtility.HtmlDecode(drv["FLDFUELCONSDMSDESCRIPTION"].ToString()), HttpUtility.HtmlDecode(drv["FLDDISTANCETRAVELLEDDESCRIPTION"].ToString()), HttpUtility.HtmlDecode(drv["FLDHOURSUNDERWAYDMSDESCRIPTION"].ToString()), HttpUtility.HtmlDecode(drv["FLDADEQUECYDATADESCRIPTION"].ToString()), HttpUtility.HtmlDecode(drv["FLDITDMSDESCRIPTION"].ToString()), HttpUtility.HtmlDecode(drv["FLDINTERNALREVIEWSDMSDESCRIPTION"].ToString())
                        , drv["FLDDMSFUELCONSNAME"].ToString()
                        , drv["FLDDMSDISTANCETRAVELLEDNAME"].ToString()
                        , drv["FLDDMSHOURSUNDERWAYNAME"].ToString()
                        , drv["FLDDMSADEQUECYDATANAME"].ToString()
                        , drv["FLDDMSITNAME"].ToString()
                        , drv["FLDDMSINTERNALREVIEWNAME"].ToString());
        }
    }
    private void Bindmeasure()
    {
        DataSet ds1 = PhoenixVesselPositionEUMRVConfig.IMODCSDMSProcedureConfigDetailEdit(General.GetNullableString("MDT"));
        DataTable dt1 = ds1.Tables[0];
        dtPartB6 = ds1.Tables[0];
        DataSet dsc = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("MDT"));
        DataTable dt6 = dsc.Tables[0];
        dtPartB6_2 = dsc.Tables[0];
        if (dt6.Rows.Count > 0)
        {
            R6.DataSource = dt6;
            R6.DataBind();
        }
        if (dt1.Rows.Count > 0)
        {
            txtprocedure.Content = dt1.Rows[0]["FLDDESCRIPTION"].ToString();            
        }
    }

    private void Bindmeasure1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSDMSProcedureConfigDetailEdit(General.GetNullableString("MHU"));
        DataTable dt = ds.Tables[0];
        dtPartB7 = ds.Tables[0];
        DataSet dsc = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("MHU"));
        DataTable dt7 = dsc.Tables[0];
        dtPartB7_2 = dsc.Tables[0];
        if (dt7.Rows.Count > 0)
        {
            R7.DataSource = dt7;
            R7.DataBind();
        }
        if (dt.Rows.Count > 0)
        {
            txtprocedure1.Content = dt.Rows[0]["FLDDESCRIPTION"].ToString();           
        }
    }

    private void Bindmeasure2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSDMSProcedureConfigDetailEdit(General.GetNullableString("RDA"));
        DataTable dt = ds.Tables[0];
        dtPartB8 = ds.Tables[0];
        DataSet dsc = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("RDA"));
        DataTable dt8 = dsc.Tables[0];
        dtPartB8_2 = dsc.Tables[0];
        if (dt8.Rows.Count > 0)
        {
            R8.DataSource = dt8;
            R8.DataBind();
        }
        if (dt.Rows.Count > 0)
        {
            txtprocedure2.Content = dt.Rows[0]["FLDDESCRIPTION"].ToString();            
        }
    }
    private void BindMethodEdit()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSProcedureConfigDetailEdit(int.Parse(ucVessel.SelectedVessel));
            DataTable dt = ds.Tables[0];
            dtPartB5 = ds.Tables[0];
            //hl5.InnerHtml = "";
            txteuprocedure.Content = "";
            txtmethodname.Text = "";

            DataSet dsc = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("MFC"));
            DataTable dt2 = dsc.Tables[0];
            dtPartB5_2 = dsc.Tables[0];
            if (dt2.Rows.Count > 0)
            {
                rptC2_1.DataSource = dt2;
                rptC2_1.DataBind();

                //if (dt.Rows.Count > 0)
                //{
                //    rptC2_1.Controls[rptC2_1.Controls.Count - 1].Controls[0].FindControl("lblrptC2_1").Visible = false;
                //}
                foreach (DataRow dr1 in dt2.Rows)
                {
                    str = str + dr1["FLDDMSREVNO"].ToString() + "</br>";
                 
                    //hl5.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");
                }
                
                //hl5.InnerHtml = str;
            }

            if (dt.Rows.Count > 0)
            {
                txteuprocedure.Content = dt.Rows[0]["FLDDESCRIPTION"].ToString();
                txtmethodname.Text = dt.Rows[0]["FLDMETHOD"].ToString();                              
            }
        }
    }

    public void R1_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl hi5 = (HtmlGenericControl)e.Item.FindControl("hl5");
        hi5.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            hi5.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','"+Session["sitepath"]+"/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void R6_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl hi6 = (HtmlGenericControl)e.Item.FindControl("hl6");
        hi6.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            hi6.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void R7_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl hi7 = (HtmlGenericControl)e.Item.FindControl("hl7");
        hi7.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            hi7.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void R8_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl hi8 = (HtmlGenericControl)e.Item.FindControl("hl8");
        hi8.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            hi8.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }
    private void BindShipData()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            DataSet ds = new DataSet();

            ds = PhoenixRegistersEUMRVEmisionSource.RegistersIMODCSShipDetails(General.GetNullableInteger(ucVessel.SelectedVessel));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                txtIMOIdentificationNumber.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
                txtIMOUniqueCompany.Text = dt.Rows[0]["FLDUNIQUCOMPANY"].ToString();
                txtDeadweight.Text = dt.Rows[0]["FLDDWT"].ToString();
                txtnt.Text = dt.Rows[0]["FLDNET"].ToString();
                txtGrosstonnage.Text = dt.Rows[0]["FLDGROSSTONNAGE"].ToString();
                txtIceClass.Text = dt.Rows[0]["FLDICECLASS"].ToString();
                txtFlagState.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
                txtEEDI.Text = dt.Rows[0]["FLDEEDI"].ToString();
                txtvessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtshiptype.Text = dt.Rows[0]["FLDVESSELTYPE"].ToString();
                txtEVI.Text = dt.Rows[0]["FLDEIV"].ToString();

                DataRow drv = dt.Rows[0];
                DataTable a1 = data.Tables.Add("1");

                a1.Columns.Add("a");
                a1.Columns.Add("b");
                a1.Columns.Add("c");
                a1.Columns.Add("d");
                a1.Columns.Add("e");
                a1.Columns.Add("f");
                a1.Columns.Add("g");
                a1.Columns.Add("h");
                a1.Columns.Add("i");
                a1.Columns.Add("j");
                a1.Columns.Add("k");
                a1.Columns.Add("l");
                a1.Columns.Add("m");
                a1.Columns.Add("n");
                a1.Rows.Add(drv["FLDVESSELNAME"].ToString(), drv["FLDIMONUMBER"].ToString(), drv["FLDUNIQUCOMPANY"].ToString(), drv["FLDFLAGNAME"].ToString(), drv["FLDVESSELTYPE"].ToString(), drv["FLDGROSSTONNAGE"].ToString(), drv["FLDNET"].ToString(), drv["FLDDWT"].ToString()
                    , drv["FLDEEDI"].ToString(), drv["FLDICECLASS"].ToString(), drv["FLDEIV"].ToString(), drv["FLDPORTREGISTERED"].ToString());
            }
        }
    }

    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXPORTPDF"))
        {
            ConvertToPdf(PrepareHtmlDoc());
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    //public string DataSetToJSONWithJavaScriptSerializer(DataSet dataset)
    //{
    //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

    //    Dictionary<string, object> ssvalue = new Dictionary<string, object>();

    //    foreach (DataTable table in dataset.Tables)
    //    {
    //        List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
    //        Dictionary<string, object> childRow;

    //        string tablename = table.TableName;
    //        foreach (DataRow row in table.Rows)
    //        {
    //            childRow = new Dictionary<string, object>();
    //            foreach (DataColumn col in table.Columns)
    //            {
    //                childRow.Add(col.ColumnName, row[col]);
    //            }
    //            parentRow.Add(childRow);
    //        }

    //        ssvalue.Add(tablename, parentRow);
    //    }

    //    return jsSerializer.Serialize(ssvalue);
    //}

    public static string ConvertIntoJson(DataTable dt)
    {
        var jsonString = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            if (dt.Columns[0].ColumnName == "1")
            {
                jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.TableName == "3" && dt.Columns[j].ColumnName == "4")
                        {
                            string s = ConvertDataTableB32JSON(dt.Rows[i][j].ToString());

                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":"
                            + s + (j < dt.Columns.Count - 1 ? "," : ""));
                        }
                        else
                        {
                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":\""
                            + dt.Rows[i][j].ToString().Replace('"', '\"') + (j < dt.Columns.Count - 1 ? "\"," : "\""));
                        }

                    }


                    jsonString.Append(i < dt.Rows.Count - 1 ? "}," : "}");
                }
                return jsonString.Append("]").ToString();
            }
            else
            {
                //jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                        jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":\""
                            + dt.Rows[i][j].ToString().Replace('"', '\"') + (j < dt.Columns.Count - 1 ? "\"," : "\""));

                    jsonString.Append(i < dt.Rows.Count - 1 ? "}," : "}");
                }
                //return jsonString.Append("]").ToString();
                return jsonString.ToString();
            }

        }
        else
        {
            return "[]";
        }
    }
    public static string ConvertIntoJson(DataSet ds)
    {
        var jsonString = new StringBuilder();
        jsonString.Append("{");
        for (int i = 0; i < ds.Tables.Count; i++)
        {
            jsonString.Append("\"" + ds.Tables[i].TableName + "\":");
            if (ds.Tables[i].TableName == "4")
            {
                jsonString.Append("[]");
            }
            else
            {
                jsonString.Append(ConvertIntoJson(ds.Tables[i]));
            }

            if (i < ds.Tables.Count - 1)
                jsonString.Append(",");
        }
        jsonString.Append("}");
        return jsonString.ToString();
    }
    public static string ConvertDataTableB32JSON(string s)
    {
        String[] strArr = s.Split(',');

        //return strArr.ToString();

        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();


        var jsonString = new StringBuilder();
        jsonString.Append("[");
        for (int i = 0; i < strArr.Length; i++)
        {
            jsonString.Append(i == strArr.Length - 1 ? "\"" + strArr[i] + "\"" : "\"" + strArr[i] + "\",");
        }

        jsonString.Append("]");
        return jsonString.ToString();
    }
    protected DataTable BindDummyTable(string name)
    {
        DataTable dt = new DataTable(name);
        dt.Columns.Add("a");
        dt.Columns.Add("b");
        dt.Columns.Add("c");
        dt.Columns.Add("d");
        dt.Columns.Add("e");
        dt.Columns.Add("f");
        dt.Columns.Add("g");
        dt.Columns.Add("h");
        dt.Columns.Add("i");
        dt.Columns.Add("j");
        dt.Columns.Add("k");
        dt.Columns.Add("l");
        dt.Columns.Add("m");
        dt.Columns.Add("n");
        dt.Columns.Add("o");
        dt.Columns.Add("p");
        dt.Columns.Add("q");
        dt.Columns.Add("r");
        dt.Columns.Add("s");
        dt.Columns.Add("t");
        dt.Columns.Add("u");
        dt.Columns.Add("v");
        dt.Columns.Add("w");
        dt.Columns.Add("x");
        dt.Columns.Add("y");
        dt.Columns.Add("z");
        return dt;
    }
    private string PrepareHtmlDoc()
    {
        DataTable dtPart1 = new DataTable();
        dtPart1 = data.Tables["1"];

        DataTable dtPart9 = new DataTable();
        dtPart9 = data9.Tables["1"];

        StringBuilder DsHtmlcontent = new StringBuilder();
        DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
        DsHtmlcontent.Append("<font size='2' face='Helvetica'>");

        DataRow drPart1 = dtPart1.NewRow();
        if (dtPart1.Rows.Count > 0)
        {
            drPart1 = dtPart1.Rows[0];
        }

        DsHtmlcontent.Append("<table ID='tblhdr'  cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td align='center'><b>"+ drPart1["c"].ToString() + "</b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table ID='tblhdr'  cellpadding='7' cellspacing='10'>");
        DsHtmlcontent.Append("<tr><td align='center'><b>IMO Data Collection Plan</b> </td></tr>");
        DsHtmlcontent.Append("</table></br></br>");

        DsHtmlcontent.Append("<table ID='tbl1' cellpadding='4' cellspacing='4'>");
        DsHtmlcontent.Append("<tr><td></td><td></td><td></td><td></td><td colspan=2>Vessel</td><td align='left'>:</td> <td colspan=4>" + drPart1["a"].ToString() + "</td></td><td><td></td></tr>");
        DsHtmlcontent.Append("<tr><td></td><td></td><td></td><td></td><td  colspan=2>Port&nbsp;of&nbsp;Registry</td><td align='left'>:</td><td colspan=4>" + drPart1["l"].ToString() + "</td></td><td><td></td></tr>");
        DsHtmlcontent.Append("<tr><td></td><td></td><td></td><td></td><td  colspan=2>IMO Number</td><td align='left'>:</td> <td colspan=4>" + drPart1["b"].ToString() + "</td></td><td><td></td></tr>");       
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>");

        DsHtmlcontent.Append("<table ID='tbl' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td align='center'><b>The Data Collection Plan (SEEMP Part II)</b> </td></tr>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td><b>1  Ship Particulars</b> </td></tr>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/>");
        

        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Name of the ship</td> <td colspan=3>" + drPart1["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>IMO Number</td> <td colspan=3>" + drPart1["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Company</td><td colspan=3>" + drPart1["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Flag</td><td colspan=3>" + drPart1["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Ship Type</td><td colspan=3>" + drPart1["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Gross Tonnage</td><td colspan=3>" + drPart1["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>NT</td><td colspan=3>" + drPart1["g"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>DWT</td><td colspan=3>" + drPart1["h"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>EEDI (if applicable)</td><td>" + drPart1["i"].ToString() + "</td><td bgcolor='#f1f1f1'>EIV</td><td>"+ drPart1["k"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Ice class</td><td colspan=3>" + drPart1["j"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>2  Record of revision of Fuel Oil Consumption Data Collection Plan</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Date of revision</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Reference(in tonnes of CO2/ tonne fuel)</th></tr>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Revised Provision</th></tr>");

        foreach (DataRow dr in dtPartB2.Rows)
        {
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDREVISIEDDATE"].ToString()  + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDREVISIONPROVISION"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>3 Emission sources and fuel types used</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>No.</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan='2'>Engines or other fuel oil consumers</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan='3'>Power</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Fuel oil types</th></tr>");

        foreach (DataRow dr in dtB3.Rows)
        {

            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDROWNUMBER"].ToString() + "</td>");
            DsHtmlcontent.Append("<td colspan='2'>" + dr["FLDCOMPONENTNAME"].ToString() + " Type:"+ dr["FLDENGINETYPE"].ToString()+ " Model:"+ dr["FLDENGINEMODEL"].ToString()+ "</td>");
            DsHtmlcontent.Append("<td colspan='3'>" + " Ser No : " + dr["FLDSERIALNUMBER"].ToString() +
                            ", " + dr["FLDPOWERCAPTION"].ToString() + " " + dr["FLDPEFORMENCEPOWER"].ToString() + " " + dr["FLDPOWERUNITS"].ToString() + ", " +
                            dr["FLDSFOCLABEL"].ToString() + "" + dr["FLDIDENTIFICATIONNO"].ToString() + " " + dr["FLDSFOCUNITS"].ToString() +
                            ", Year of Installation : " + dr["FLDYEAROFINSTALLATION"].ToString() + "</td>");

            DsHtmlcontent.Append("<td>" + dr["FLDOILTYPENAME"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>4  Emission factors</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Fuel Type</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Reference(in tonnes of CO2/ tonne fuel)</th></tr>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>CF(t - CO2 / t - Fuel)</th></tr>");

        foreach (DataRow dr in dtPartB24.Rows)
        {
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDFUELCATEGORY"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDEMISSIONFACTOR"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>5  Methods to measure fuel oil consumption</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");

        if (dtPartB5.Rows.Count > 0)
        {
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Reference to Existing Procedure</b></td> <td>");

            if (dtPartB5_2.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartB5_2.Rows)
                {
                    DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
                }
            }
            DsHtmlcontent.Append("</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Method</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=center colspan='3'><b>Description</b></th></tr>");
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dtPartB5.Rows[0]["FLDMETHOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td colspan='3'>"+ HttpUtility.HtmlDecode(dtPartB5.Rows[0]["FLDDESCRIPTION"].ToString()) + "</td>");
            DsHtmlcontent.Append("</tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>6  Method to measure distance travelled</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        if (dtPartB6.Rows.Count > 0)
        {
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Reference to Existing Procedure</b></td> <td>");

            if (dtPartB6_2.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartB6_2.Rows)
                {
                    DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
                }
            }
            DsHtmlcontent.Append("</td></tr>");
            DsHtmlcontent.Append("<td colspan=2 bgcolor='#f1f1f1' align=center><b>Description</b></td></tr>");
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td colspan=2>" + HttpUtility.HtmlDecode(dtPartB6.Rows[0]["FLDDESCRIPTION"].ToString()) + "</td>");
            DsHtmlcontent.Append("</tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>7  Method to measure hours under way</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        if (dtPartB7.Rows.Count > 0)
        {
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Reference to Existing Procedure</b></td> <td>");

            if (dtPartB7_2.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartB7_2.Rows)
                {
                    DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
                }
            }
            DsHtmlcontent.Append("</td></tr>");
            DsHtmlcontent.Append("<td colspan=2 bgcolor='#f1f1f1' align=center><b>Description</b></td></tr>");
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td colspan=2>" + HttpUtility.HtmlDecode(dtPartB7.Rows[0]["FLDDESCRIPTION"].ToString()) + "</td>");
            DsHtmlcontent.Append("</tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>8  Processes that will be used to report the data to the Administration</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        if (dtPartB8.Rows.Count > 0)
        {
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Reference to Existing Procedure</b></td> <td>");

            if (dtPartB8_2.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartB8_2.Rows)
                {
                    DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
                }
            }
            DsHtmlcontent.Append("</td></tr>");
            DsHtmlcontent.Append("<td colspan=2 bgcolor='#f1f1f1' align=center><b>Description</b></td></tr>");
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td colspan=2>" + HttpUtility.HtmlDecode(dtPartB8.Rows[0]["FLDDESCRIPTION"].ToString()) + "</td>");
            DsHtmlcontent.Append("</tr>");
        }

        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td><b>9  Data Quality</b> </td></tr>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("</br>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td><b>The Quality of Data received is checked for any Gaps prior finalizing and submitting the data. The possible data gaps are listed below with the measures in place to treat any gaps</b> </td></tr>");
        DsHtmlcontent.Append("</table>");
        DataRow drPart9 = dtPart9.NewRow();
        if (dtPart9.Rows.Count > 0)
        {
            drPart9 = dtPart9.Rows[0];
        }

        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' align=center><b>Data</b> </td> <td bgcolor='#f1f1f1' align=center colspan='3'><b>Method to treat the Gaps</b></td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_1.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_1.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Fuel Consumption</td> <td colspan='3'>" + drPart9["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_2.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_2.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>"); DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Distance Travelled</td> <td colspan='3'>" + drPart9["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_3.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_3.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>"); DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Hours Underway</td><td colspan='3'>" +  drPart9["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_4.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_4.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>"); DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Adequacy of the Data Collection Plan</td><td colspan='3'>" + drPart9["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_5.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_5.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>"); DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Information Technology</td><td colspan='3'>"  + drPart9["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Reference to Existing Procedure</td> <td colspan=3>");
        if (dtPartQ9_6.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPartQ9_6.Rows)
            {
                DsHtmlcontent.Append(dr["FLDDMSREVNO"].ToString() + "<br/>");
            }
        }
        DsHtmlcontent.Append("</td></tr>"); DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Internal reviews and Validation of Data</td><td colspan='3'>" + drPart9["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("</font>");
        DsHtmlcontent.Append("</div>");
        return DsHtmlcontent.ToString();
    }

    public void Q1_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL1 = (HtmlGenericControl)e.Item.FindControl("QL1");
        QL1.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','"+Session["sitepath"]+"/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void Q2_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL2 = (HtmlGenericControl)e.Item.FindControl("QL2");
        QL2.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL2.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void Q3_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL3 = (HtmlGenericControl)e.Item.FindControl("QL3");
        QL3.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL3.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void Q4_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL4 = (HtmlGenericControl)e.Item.FindControl("QL4");
        QL4.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL4.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void Q5_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL5 = (HtmlGenericControl)e.Item.FindControl("QL5");
        QL5.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL5.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }

    public void Q6_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        HtmlGenericControl QL6 = (HtmlGenericControl)e.Item.FindControl("QL6");
        QL6.InnerText = drv["FLDDMSREVNO"].ToString();
        if (drv["FLDDMSREVISIONID"].ToString() != "")
        {
            QL6.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + drv["FLDDMSDOCUMENTID"].ToString() + "&SECTIONID=" + drv["FLDDMSREFERENCEID"].ToString() + "&REVISONID=" + drv["FLDDMSREVISIONID"].ToString() + "'); return false;");
        }
    }
    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    // string path = HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/");
                    // string month = DateTime.Parse(txtEndDate.Text).ToString("m");
                    //string year = DateTime.Parse(txtEndDate.Text).ToString("y");
                    //string filefullpath = year.Replace(" ", "_") + "_Payslip_for_" + txtFileNo.Text + ".pdf";
                    string filefullpath = "IMODCS_" + ucVessel.SelectedVesselName.Replace(" ", "_")  + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";


                    //PdfPTable table = new PdfPTable(3);

                    //table.AddCell("Cell 1");


                    //PdfPCell cell = new PdfPCell(new Phrase("Cell 2", new Font(Font.HELVETICA, 8f, Font.NORMAL, Color.YELLOW)));

                    //cell.BackgroundColor = new Color(0, 150, 0);

                    //cell.BorderColor = new Color(255, 242, 0);

                    //cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    //cell.BorderWidthBottom = 3f;

                    //cell.BorderWidthTop = 3f;

                    //cell.PaddingBottom = 10f;

                    //cell.PaddingLeft = 20f;

                    //cell.PaddingTop = 4f;

                    //table.AddCell(cell);

                    //table.AddCell("Cell 3");

                    //document.Add(table);

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Server.MapPath("../../" + Session["images"] + "/esmlogo4_small.png"));
                    image.Alignment= iTextSharp.text.Image.ALIGN_CENTER;
                    document.Add(image);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.End();
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOilType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            {
                DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFuelOilrevisonconsumptionSearch(int.Parse(ucVessel.SelectedVessel),
                sortexpression, sortdirection, 1,
                General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                dtPartB2 = ds.Tables[0];
                gvOilType.DataSource = ds;

            }
        }

    }

    protected void gvEmissionSource_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixVesselPositionIMODCSEmissionSource.IMODCSEmissionSourceREPORT(General.GetNullableInteger(ucVessel.SelectedVessel), 1);
            dtB3 = ds.Tables[0];
            gvEmissionSource.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Request.QueryString["view"] != null)
                {
                    gvEmissionSource.Columns[9].Visible = false;
                }
            }
 
        }
    }

    protected void gvEUMRVFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSEmissionSource.IMODCSEmissionFactorList();
            dtPartB24 = ds.Tables[0];
            gvEUMRVFuelType.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
