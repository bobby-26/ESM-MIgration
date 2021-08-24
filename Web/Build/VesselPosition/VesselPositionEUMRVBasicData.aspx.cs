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
using Telerik.Web.UI;

public partial class VesselPositionEUMRVBasicData : PhoenixBasePage
{
    DataSet jsonData = new DataSet();
    DataTable dummydt = new DataTable();
    DataTable dt1Pdf = new DataTable();
    DataTable dtB3 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("Export Pdf", "EXPORTPDF", ToolBarDirection.Right);
            toolbarvoyagetap.AddButton("Json Export", "EXPORT",ToolBarDirection.Right);
            MenuVoyageTap.AccessRights = this.ViewState;
            MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(MenuVoyageTap);

            if (!IsPostBack)
            {
                ucVessel.bind();
                ucVessel.DataBind();
                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
            }
            BindA();
            BindB1();
            BindData();
         //   gvEngine.Rebind();// BindB3();
            BindB4();
            BindB5();
            BindC1();
            BindC2_1();
            BindC2_2();
            BindC2_3();
            BindC2_4();
            BindC2_5();
            BindC2_6();
            BindC2_7();
            BindC2_8();
            BindC2_9();
            BindC2_10();
            BindC2_11();
            BindC2_12();
            BindC3();
            BindC4_1();
            BindC4_2();
            BindC5_1();
            BindC5_2();
            BindC6_1();
            BindC6_2();
            BindD1();
            BindD2();
            BindD3();
            BindD4();
            BindE1();
            BindE2();
            BindE3();
            BindE4();
            BindE5();
            BindE6();
            BindF1();
            BindF2();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXPORT"))
        {

            string json = ConvertIntoJson(jsonData);
            
            //System.IO.File.WriteAllText(Server.MapPath("~/Attachments/jsondata.json"), json);
            //string json = DataSetToJSONWithJavaScriptSerializer(jsonData);
            Response.Clear();
            Response.ClearHeaders();

            Response.AddHeader("Content-Length", json.Length.ToString());
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename=\"MP_" + txtIMOIdentificationNumber.Text + ".json\"");

            Response.Write(json.ToString());
            Response.End();
        }
        if (CommandName.ToUpper().Equals("EXPORTPDF"))
        {
            ConvertToPdf(PrepareHtmlDoc());

        }
    }

    private void BindB4()
    {
        DataSet ds = new DataSet();

        docTitleB4.Title = "<b>B.4	Emission factors</b>";

        //int? vesselid = null;
        //vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMRV.EUMRVBasicData(int.Parse(ucVessel.SelectedVessel));
            DataSet dataset = new DataSet();

            gvFuelType.DataSource = ds.Tables[2];

            DataTable a4 = jsonData.Tables.Add("4");
            a4.Columns.Add("1");
            a4.Columns.Add("2");
            a4.Columns.Add("3");
            foreach (DataRow drv in ds.Tables[2].Rows)
            {
                a4.Rows.Add((General.GetNullableString(drv["FLDREFERENCE"].ToString())!=null)? drv["FLDOILTYPENAME"].ToString()+" ("+ drv["FLDREFERENCE"].ToString()+" )" : drv["FLDOILTYPENAME"].ToString()
                    , drv["FLDEMISSIONFACTOR"].ToString(), drv["FLDREFERENCE"]);
            }
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();


        docTitleB2.Title = "<b>B.2	Company information</b>";
        
        //int? vesselid = null;
        //vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMRV.EUMRVBasicData(int.Parse(ucVessel.SelectedVessel));
            DataSet dataset = new DataSet();
            
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                txtNameofthecompany.Text = dt.Rows[0]["FLDCOMPANYNAME"].ToString();
                txtIMONo.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
                txtAddress1.Text = dt.Rows[0]["FLDADDRESS1"].ToString();
                txtAddress2.Text = dt.Rows[0]["FLDADDRESS2"].ToString();
                txtCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
                txtStateProvinceRegion.Text = dt.Rows[0]["FLDSTATENAME"].ToString();
                txtPostalCode.Text = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                txtCountry.Text = dt.Rows[0]["FLDCONTRY"].ToString();
                txtContactPerson.Text = dt.Rows[0]["FLDINCHARGE"].ToString();
                txtTelephoneNumber.Text = dt.Rows[0]["FLDPHONE2"].ToString();
                txtEmailAddress.Text = dt.Rows[0]["FLDEMAIL1"].ToString();

                DataRow drv = dt.Rows[0];
                DataTable a2 = jsonData.Tables.Add("2");

                a2.Columns.Add("a");
                a2.Columns.Add("b");
                a2.Columns.Add("c");
                a2.Columns.Add("d");
                a2.Columns.Add("e");
                a2.Columns.Add("f");
                a2.Columns.Add("g");
                a2.Columns.Add("h");
                a2.Columns.Add("i");
                a2.Columns.Add("j");
                a2.Rows.Add(drv["FLDCOMPANYNAME"].ToString(), drv["FLDADDRESS1"].ToString()+", "+drv["FLDADDRESS2"].ToString(), drv["FLDCITYNAME"].ToString(), drv["FLDSTATENAME"].ToString(), drv["FLDPOSTALCODE"].ToString(), drv["FLDCONTRY"].ToString(), drv["FLDINCHARGE"].ToString()
                    , drv["FLDPHONE2"].ToString(), drv["FLDEMAIL1"].ToString(),drv["FLDIMONUMBER"].ToString());
            }
        }


    }
    private void BindA()
    {
        docTitleA.Title = "<b>A Revision record Sheet</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            dt = PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordList(General.GetNullableInteger(ucVessel.SelectedVessel));
            rptA.DataSource = dt;
            rptA.DataBind();

            if (dt.Rows.Count > 0)
            {
                rptA.Controls[rptA.Controls.Count - 1].Controls[0].FindControl("lblrptA").Visible = false;
            }
            dt1Pdf = dt;
        }


    }
    private void BindB1()
    {
        //lblTitleB1.Text = "B.1	Identification of the ship";
        docTitleB1.Title= "<b>B.1	Identification of the ship</b>";
        DataSet ds = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetails(General.GetNullableInteger(ucVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            txtVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtIMOIdentificationNumber.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
            txtPortofRegistry.Text = dt.Rows[0]["FLDPORTREGISTERED"].ToString();
            txtHomePort.Text = dt.Rows[0]["FLDHOMEPORT"].ToString();
            txtNameoftheShipOwner.Text = dt.Rows[0]["FLDOWNER"].ToString();
            txtIMOUniqueCompany.Text = dt.Rows[0]["FLDUNIQUCOMPANY"].ToString();
            txtTypeoftheShip.Text = dt.Rows[0]["FLDVESSELTYPE"].ToString();
            txtDeadweight.Text = dt.Rows[0]["FLDNET"].ToString();
            txtGrosstonnage.Text = dt.Rows[0]["FLDGROSSTONNAGE"].ToString();
            txtClassificationSociety.Text = dt.Rows[0]["FLDCLASSNAME"].ToString();
            txtIceClass.Text = dt.Rows[0]["FLDICECLASS"].ToString();
            txtFlagState.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
            txtAdditionalDescription.Text = dt.Rows[0]["FLDADDITIONALDECRIPTION"].ToString();

            DataRow drv = dt.Rows[0];
            DataTable a = jsonData.Tables.Add("1");

            a.Columns.Add("a");
            a.Columns.Add("b");
            a.Columns.Add("c");
            a.Columns.Add("d");
            a.Columns.Add("e");
            a.Columns.Add("f");
            a.Columns.Add("g");
            a.Columns.Add("h");
            a.Columns.Add("i");
            a.Columns.Add("j");
            a.Columns.Add("k");
            a.Columns.Add("l");
            a.Columns.Add("m");
            a.Rows.Add(drv["FLDVESSELNAME"].ToString(), drv["FLDIMONUMBER"].ToString(), drv["FLDPORTREGISTERED"].ToString(), drv["FLDHOMEPORT"].ToString(), drv["FLDOWNER"].ToString(), drv["FLDUNIQUCOMPANY"].ToString(), drv["FLDVESSELTYPE"].ToString(), drv["FLDNET"].ToString()
                , drv["FLDGROSSTONNAGE"].ToString(), drv["FLDCLASSNAME"].ToString(), drv["FLDICECLASS"].ToString(), drv["FLDFLAGNAME"].ToString(), drv["FLDADDITIONALDECRIPTION"].ToString());
        }
    }
    private void BindB3()
    {
        docTitleB3.Title = "<b>B.3	Emission sources and fuel types used</b>";
        DataSet ds = new DataSet();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMREmissionSource.EUMRVEmissionSourcesList(int.Parse(ucVessel.SelectedVessel));
            dtB3 = ds.Tables[0];
            gvEngine.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
               
                DataTable dt = ds.Tables[0];

                DataTable a = jsonData.Tables.Add("3");
                a.Columns.Add("1");
                a.Columns.Add("2");
                a.Columns.Add("3");
                a.Columns.Add("4");
                a.Columns.Add("5");
                foreach (DataRow drv in dt.Rows)
                {
                    a.Rows.Add(drv["FLDCOMPONENTNUMBER"].ToString(), drv["FLDCOMPONENTNAME"].ToString() ,
                        " Ser No : " + drv["FLDSERIALNUMBER"].ToString()+
                            ", "+ drv["FLDPOWERCAPTION"].ToString()+ " " + drv["FLDPEFORMENCEPOWER"].ToString()+" "+ drv["FLDPOWERUNITS"].ToString() +", "+
                            drv["FLDSFOCLABEL"].ToString() + " " + drv["FLDIDENTIFICATIONNO"].ToString()+" "+ drv["FLDSFOCUNITS"].ToString() +
                            ", Year of Installation : " +drv["FLDYEAROFINSTALLATION"].ToString() 
                            , drv["FLDOILTYPENAMEJSON"].ToString(), "");
                }


            }
        }
    }
    private void BindB5()
    {

        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("B.5");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            //lblb5_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblb5_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblb5_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblb5_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblb5_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblb5_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlb5_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlb5_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlb5_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");
        }
        else
        {
            lblb5_1.Text = "";
            lblb5_2.Text = "";
            lblb5_3.Text = "";
            lblb5_4.Text = "";
            lblb5_5.Text = "";
            lblb5_6.Text = "";
            hlb5_1.Text = "";
            hlb5_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                docTitleB5.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)</b>";
            else
                docTitleB5.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtB5 = BindDummyTable("5");
        if(ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtB5.Rows.Clear();
            dtB5.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtB5);
        
    }
    private void BindC1()
    {
        docTitleC1.Title = "<b>C.1	Conditions of exemption related to Article 9 (2)</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            dt = PhoenixVesselPositionEUMRVPlan.EUMRVExemptionArticleEdit(General.GetNullableInteger(ucVessel.SelectedVessel));
            if (dt.Rows.Count > 0)
            {

                lblC1_1.Text = dt.Rows[0]["FLDMINEXPECTEDVOYAGEFALLING"].ToString();
                lblC1_2.Text = dt.Rows[0]["FLDMINEXPECTEDVOYAGEFALLINGNOT"].ToString();
                lblC1_3.Text = dt.Rows[0]["FLDCONDITIONS"].ToString();
                lblC1_4.Text = dt.Rows[0]["FLDFUELCONSUMED"].ToString();
            }
            else
            {
                lblC1_1.Text = "";
                lblC1_2.Text = "";
                lblC1_3.Text = "";
                lblC1_4.Text = "";
            }
            DataTable dtjson = BindDummyTable("6");
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dtjson.Rows.Add(dr["FLDMINEXPECTEDVOYAGEFALLING"].ToString(), dr["FLDMINEXPECTEDVOYAGEFALLINGNOT"].ToString(), dr["FLDCONDITIONS"].ToString(), dr["FLDFUELCONSUMED"].ToString());
            }
            jsonData.Tables.Add(dtjson);

        }


    }
    private void BindC2_1()
    {
        docTitleC2_1.Title = "<b>C.2.1	Methods used to determine fuel consumption of each emission source</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringfuelconsumption(General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                                , null, null, 1, 200, ref iRowCount, ref iTotalPageCount);
            rptC2_1.DataSource = dt;
            rptC2_1.DataBind();

            if (dt.Rows.Count > 0)
            {
                rptC2_1.Controls[rptC2_1.Controls.Count - 1].Controls[0].FindControl("lblrptC2_1").Visible = false;
            }
        }
        DataTable dtjson = new DataTable("7");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDEMISSIONSOURCENAME"].ToString(), dr["FLDMONITORINGMETHODNAME"].ToString());
            }
            
        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_2_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_2_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_2_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_2_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_2_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_2_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_2_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_2_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_2_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");
            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();

        }
        else
        {
            lblC2_2_1.Text = "";
            lblC2_2_2.Text = "";
            lblC2_2_3.Text = "";
            lblC2_2_4.Text = "";
            lblC2_2_5.Text = "";
            lblC2_2_6.Text = "";
            hlC2_2_1.Text = "";
            hlC2_2_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                docTitleC2_2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)<b/>";
            else
                docTitleC2_2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("8");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
           dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_3()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.3");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_3_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_3_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_3_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_3_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            //lblC2_2_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            //lblC2_2_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_3_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_3_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_3_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC2_3_1.Text = "";
            lblC2_3_2.Text = "";
            lblC2_3_3.Text = "";
            lblC2_3_4.Text = "";
            hlC2_3_1.Text = "";
            hlC2_3_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                docTitleC2_3.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)</b>";
            else
                docTitleC2_3.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("9");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString());
        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_4()
    {
        docTitleC2_4.Title = "<b>C.2.4	Description of the measurement instruments involved</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentSearch(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(ucVessel.SelectedVessel));
            rptC2_4.DataSource = dt;
            rptC2_4.DataBind();
            
            if (dt.Rows.Count > 0)
            {
                rptC2_4.Controls[rptC2_4.Controls.Count - 1].Controls[0].FindControl("lblrptC2_4").Visible = false;
            }
        }

        DataTable dtjson = new DataTable("10");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        dtjson.Columns.Add("3");
        if (dt.Rows.Count > 0)
        {
            
            foreach(DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDMEASUREMENTEQUIPMENT"].ToString(), dr["FLDEMISSIONSOURCENAME"].ToString(), dr["FLDTECHNICALDESC"].ToString());
            }
            
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC2_5()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.5");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_5_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_5_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_5_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_5_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_5_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_5_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_5_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_5_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_5_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC2_5_1.Text = "";
            lblC2_5_2.Text = "";
            lblC2_5_3.Text = "";
            lblC2_5_4.Text = "";
            lblC2_5_5.Text = "";
            lblC2_5_6.Text = "";

            hlC2_5_1.Text = "";
            hlC2_5_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                docTitleC2_5.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)</b>";
            else
                docTitleC2_5.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("11");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_6()
    {
        docTitleC2_6.Title = "<b>C.2.6 Method for determination of density</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensity(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(ucVessel.SelectedVessel));
            rptC2_6.DataSource = dt;
            rptC2_6.DataBind();
            
            if (dt.Rows.Count > 0)
            {
                rptC2_6.Controls[rptC2_6.Controls.Count - 1].Controls[0].FindControl("lblrptC2_6").Visible = false;
            }
        }

        DataTable dtjson = new DataTable("12");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        dtjson.Columns.Add("3");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDFUELTYPENAME"].ToString(), dr["FLDFULEBUNKEREDNAME"].ToString(), dr["FLDFULETANKNAME"].ToString());
            }

        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC2_7()
    {
        docTitleC2_7.Title = "<b>C.2.7 Level of uncertainty associated with fuel monitoring</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringSearch(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(ucVessel.SelectedVessel));
            rptC2_7.DataSource = dt;
            rptC2_7.DataBind();

            if (dt.Rows.Count > 0)
            {
                rptC2_7.Controls[rptC2_7.Controls.Count - 1].Controls[0].FindControl("lblrptC2_7").Visible = false;
            }
        }
        DataTable dtjson = new DataTable("13");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        dtjson.Columns.Add("3");
        if (dt.Rows.Count > 0)
        {

            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDMONITORINGMETHODNAME"].ToString(), dr["FLDAPPROACHUSEDNAME"].ToString(), dr["FLDVALUE"].ToString()+" %");
            }

        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_8()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.8");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_8_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_8_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_8_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_8_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_8_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_8_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
            hlC2_8_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_8_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_8_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

        }
        else
        {
            lblC2_8_1.Text = "";
            lblC2_8_2.Text = "";
            lblC2_8_3.Text = "";
            lblC2_8_4.Text = "";
            lblC2_8_5.Text = "";
            lblC2_8_6.Text = "";
            hlC2_8_1.Text = "";
            hlC2_8_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                docTitleC2_8.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)</b>";
            else
                docTitleC2_8.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("14");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_9()
    {
        
        string desc = "";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            return;
        }
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVfreightandpassengerVesselEdit("C.2.9", int.Parse(ucVessel.SelectedVessel));
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                lblC2_9_1.Text = dt.Rows[0]["FLDAPPLIEDALLOCATIONNAME"].ToString();
                lblC2_9_2.Text = dt.Rows[0]["FLDMASSDESC"].ToString();
                desc = dt.Rows[0]["FLDMASSDESC"].ToString();
                lblC2_9_3.Text = dt.Rows[0]["FLDAREADESC"].ToString();
                lblC2_9_4.Text = dt.Rows[0]["FLDFUELCONSUMPTION"].ToString();
                lblC2_9_5.Text = dt.Rows[0]["FLDPERSONRESPONDIBLE"].ToString();
                lblC2_9_6.Text = dt.Rows[0]["FLDFOMULAE"].ToString();
                lblC2_9_7.Text = dt.Rows[0]["FLDLOCATION"].ToString();
                lblC2_9_8.Text = dt.Rows[0]["FLDNAMEITSYSTEM"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (General.GetNullableInteger(dt2.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
                {
                    lblC2_9_2.Text = "Not applicable to this vessel";
                    desc = "Not applicable to this vessel";
                }
                else if (General.GetNullableInteger(dt2.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                {
                    docTitleC2_9.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
                lblC2_9_2.Text = "The Company does not wish to report on this procedure";
                    desc = "The Company does not wish to report on this procedure";
                }



                docTitleC2_9.Title = "<b>" + dt2.Rows[0]["FLDNEWCODE"].ToString() + " " + dt2.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
            }

            DataTable dtjson = BindDummyTable("15");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                dtjson.Rows.Add(dr["FLDAPPLIEDALLOCATIONNAME"].ToString(), desc, dr["FLDAREADESC"].ToString(), dr["FLDFUELCONSUMPTION"].ToString(), dr["FLDPERSONRESPONDIBLE"].ToString(), dr["FLDFOMULAE"].ToString(), dr["FLDLOCATION"].ToString(), dr["FLDNAMEITSYSTEM"].ToString());
            }
            else
            {
                dtjson.Rows.Add("", desc, "", "", "", "", "", "");
            }
            jsonData.Tables.Add(dtjson);
    }
    private void BindC2_10()
    {
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.10");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_10_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_10_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_10_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_10_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_10_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC2_10_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_10_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_10_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_10_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_10_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC2_10_1.Text = "";
            lblC2_10_2.Text = "";
            lblC2_10_3.Text = "";
            lblC2_10_4.Text = "";
            lblC2_10_5.Text = "";
            lblC2_10_6.Text = "";
            lblC2_10_7.Text = "";
            hlC2_10_1.Text = "";
            hlC2_10_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];

            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC2_10_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC2_10_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                docTitleC2_10.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            }                
                
            
            docTitleC2_10.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("16");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), desc, dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "", "");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC2_11()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            return;
        }
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.2.11",int.Parse(ucVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_11_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_11_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_11_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc= dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_11_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_11_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC2_11_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_11_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_11_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_11_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_11_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC2_11_1.Text = "";
            lblC2_11_2.Text = "";
            lblC2_11_3.Text = "";
            lblC2_11_4.Text = "";
            lblC2_11_5.Text = "";
            lblC2_11_6.Text = "";
            lblC2_11_7.Text = "";
            hlC2_11_1.Text = "";
            hlC2_11_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];

            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC2_11_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC2_11_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                docTitleC2_11.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            }

            docTitleC2_11.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("17");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), desc, dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "", "");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC2_12()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            return;
        }
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.2.12",int.Parse(ucVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC2_12_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC2_12_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC2_12_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC2_12_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC2_12_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC2_12_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC2_12_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC2_12_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC2_12_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC2_12_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC2_12_1.Text = "";
            lblC2_12_2.Text = "";
            lblC2_12_3.Text = "";
            lblC2_12_4.Text = "";
            lblC2_12_5.Text = "";
            lblC2_12_6.Text = "";
            lblC2_12_7.Text = "";
            hlC2_12_1.Text = "";
            hlC2_12_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];

            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC2_12_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC2_12_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                docTitleC2_12.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            }

            docTitleC2_12.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("18");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), desc, dr["FLDPERSONRESPONDIBLE"].ToString(), dr["FLDFOMULAE"].ToString(), dr["FLDLOCATION"].ToString(), dr["FLDNAMEITSYSTEM"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "", "");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC3()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.3");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC3_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC3_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC3_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC3_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC3_5.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblC3_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC3_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
            hlC3_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC3_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC3_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC3_1.Text = "";
            lblC3_2.Text = "";
            lblC3_3.Text = "";
            lblC3_4.Text = "";
            lblC3_5.Text = "";
            lblC3_6.Text = "";
            lblC3_7.Text = "";
            hlC3_1.Text = "";
            hlC3_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleC3.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleC3.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("19");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC4_1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.4.1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC4_1_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC4_1_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC4_1_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC4_1_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC4_1_5.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblC4_1_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC4_1_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC4_1_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC4_1_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC4_1_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC4_1_1.Text = "";
            lblC4_1_2.Text = "";
            lblC4_1_3.Text = "";
            lblC4_1_4.Text = "";
            lblC4_1_5.Text = "";
            lblC4_1_6.Text = "";
            lblC4_1_7.Text = "";
            hlC4_1_1.Text = "";
            hlC4_1_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleC4_1.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleC4_1.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("20");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC4_2()
    {
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.4.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC4_2_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC4_2_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC4_2_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC4_2_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC4_2_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC4_2_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC4_2_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC4_2_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC4_2_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC4_2_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC4_2_1.Text = "";
            lblC4_2_2.Text = "";
            lblC4_2_3.Text = "";
            lblC4_2_4.Text = "";
            lblC4_2_5.Text = "";
            lblC4_2_6.Text = "";
            lblC4_2_7.Text = "";
            hlC4_2_1.Text = "";
            hlC4_2_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC4_2_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC4_2_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                //docTitleC4_2.Style.Add("color", "blue");
                docTitleC4_2.TitlebarContainer.ForeColor= System.Drawing.Color.Blue;
            }

            docTitleC4_2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("21");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), desc, dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "","");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC5_1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.5.1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC5_1_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC5_1_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC5_1_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC5_1_4.Text = dt.Rows[0]["FLDUNITOFCARGOORPASSENGERS"].ToString();
            lblC5_1_5.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC5_1_6.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC5_1_7.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC5_1_8.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC5_1_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC5_1_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC5_1_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC5_1_1.Text = "";
            lblC5_1_2.Text = "";
            lblC5_1_3.Text = "";
            lblC5_1_4.Text = "";
            lblC5_1_5.Text = "";
            lblC5_1_6.Text = "";
            lblC5_1_7.Text = "";
            lblC5_1_8.Text = "";
            hlC5_1_1.Text = "";
            hlC5_1_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleC5_1.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleC5_1.Title = "<b>"+dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("22");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(),dr["FLDUNITOFCARGOORPASSENGERS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC5_2()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            return;
        }
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.5.2",int.Parse(ucVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC5_2_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC5_2_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC5_2_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC5_2_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC5_2_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC5_2_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC5_2_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
            

            hlC5_2_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC5_2_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC5_2_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");


            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC5_2_1.Text = "";
            lblC5_2_2.Text = "";
            lblC5_2_3.Text = "";
            lblC5_2_4.Text = "";
            lblC5_2_5.Text = "";
            lblC5_2_6.Text = "";
            lblC5_2_7.Text = "";
            hlC5_2_1.Text = "";
            hlC5_2_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC5_2_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC5_2_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                docTitleC5_2.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            }
            

            docTitleC5_2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("23");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), desc, dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "", "");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC6_1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.6.1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC6_1_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC6_1_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC6_1_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC6_1_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC6_1_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC6_1_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC6_1_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC6_1_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC6_1_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC6_1_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC6_1_1.Text = "";
            lblC6_1_2.Text = "";
            lblC6_1_3.Text = "";
            lblC6_1_4.Text = "";
            lblC6_1_5.Text = "";
            lblC6_1_6.Text = "";
            lblC6_1_7.Text = "";
            hlC6_1_1.Text = "";
            hlC6_1_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleC6_1.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleC6_1.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("24");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC6_2()
    {
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.6.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblC6_2_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblC6_2_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblC6_2_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblC6_2_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblC6_2_5.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblC6_2_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblC6_2_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlC6_2_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlC6_2_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlC6_2_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblC6_2_1.Text = "";
            lblC6_2_2.Text = "";
            lblC6_2_3.Text = "";
            lblC6_2_4.Text = "";
            lblC6_2_5.Text = "";
            lblC6_2_6.Text = "";
            lblC6_2_7.Text = "";
            hlC6_2_1.Text = "";
            hlC6_2_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                lblC6_2_3.Text = "Not applicable to this vessel";
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                lblC6_2_3.Text = "The Company does not wish to report on this procedure";
                desc = "The Company does not wish to report on this procedure";
                docTitleC6_2.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            }
            
            docTitleC6_2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("25");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "","");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindD1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("D.1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblD1_1.Text = dt.Rows[0]["FLDBACKUPMONITORMETHOD"].ToString();
            lblD1_2.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblD1_3.Text = dt.Rows[0]["FLDTREATDATAGAPS"].ToString();
            lblD1_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblD1_5.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblD1_6.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblD1_7.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblD1_1.Text = "";
            lblD1_2.Text = "";
            lblD1_3.Text = "";
            lblD1_4.Text = "";
            lblD1_5.Text = "";
            lblD1_6.Text = "";
            lblD1_7.Text = "";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    lblTitleD1.Text = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleD1.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("26");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDBACKUPMONITORMETHOD"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDTREATDATAGAPS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindD2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("D.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblD2_1.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblD2_2.Text = dt.Rows[0]["FLDTREATDATAGAPS"].ToString();
            lblD2_3.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblD2_4.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblD2_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblD2_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblD2_1.Text = "";
            lblD2_2.Text = "";
            lblD2_3.Text = "";
            lblD2_4.Text = "";
            lblD2_5.Text = "";
            lblD2_6.Text = "";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleD2.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleD2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("27");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMULAE"].ToString(), dr["FLDTREATDATAGAPS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindD3()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("D.3");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblD3_1.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblD3_2.Text = dt.Rows[0]["FLDTREATDATAGAPS"].ToString();
            lblD3_3.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblD3_4.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblD3_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblD3_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblD3_1.Text = "";
            lblD3_2.Text = "";
            lblD3_3.Text = "";
            lblD3_4.Text = "";
            lblD3_5.Text = "";
            lblD3_6.Text = "";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleD3.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleD3.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("28");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMULAE"].ToString(), dr["FLDTREATDATAGAPS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindD4()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("D.4");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblD4_1.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            lblD4_2.Text = dt.Rows[0]["FLDTREATDATAGAPS"].ToString();
            lblD4_3.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblD4_4.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            lblD4_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblD4_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblD4_1.Text = "";
            lblD4_2.Text = "";
            lblD4_3.Text = "";
            lblD4_4.Text = "";
            lblD4_5.Text = "";
            lblD4_6.Text = "";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleD4.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleD4.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("29");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMULAE"].ToString(), dr["FLDTREATDATAGAPS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDDATASOURCE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE1_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE1_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblE1_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE1_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE1_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE1_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlE1_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE1_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE1_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE1_1.Text = "";
            lblE1_2.Text = "";
            lblE1_3.Text = "";
            lblE1_4.Text = "";
            lblE1_5.Text = "";
            lblE1_6.Text = "";
            hlE1_1.Text = "";
            hlE1_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleE1.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleE1.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("30");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(),dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE2_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE2_2.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE2_3.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE2_4.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE2_5.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
            lblE2_6.Text = dt.Rows[0]["FLDLISTMGNTSYSTEM"].ToString();

            hlE2_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE2_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE2_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE2_1.Text = "";
            lblE2_2.Text = "";
            lblE2_3.Text = "";
            lblE2_4.Text = "";
            lblE2_5.Text = "";
            lblE2_6.Text = "";
            hlE2_1.Text = "";
            hlE2_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleE2.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleE2.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("31");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString(),dr["FLDLISTMGNTSYSTEM"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE3()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.3");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE3_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE3_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblE3_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE3_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE3_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE3_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlE3_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE3_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE3_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE3_1.Text = "";
            lblE3_2.Text = "";
            lblE3_3.Text = "";
            lblE3_4.Text = "";
            lblE3_5.Text = "";
            lblE3_6.Text = "";
            hlE3_1.Text = "";
            hlE3_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleE3.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleE3.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("32");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE4()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.4");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE4_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE4_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblE4_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE4_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE4_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE4_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlE4_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE4_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE4_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE4_1.Text = "";
            lblE4_2.Text = "";
            lblE4_3.Text = "";
            lblE4_4.Text = "";
            lblE4_5.Text = "";
            lblE4_6.Text = "";
            hlE4_1.Text = "";
            hlE4_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleE4.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleE4.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("33");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE5()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.5");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE5_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE5_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblE5_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE5_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE5_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE5_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlE5_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE5_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE5_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE5_1.Text = "";
            lblE5_2.Text = "";
            lblE5_3.Text = "";
            lblE5_4.Text = "";
            lblE5_5.Text = "";
            lblE5_6.Text = "";
            hlE5_1.Text = "";
            hlE5_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                //    docTitleE5.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
                docTitleE5.TitlebarContainer.ForeColor = System.Drawing.Color.Blue;
            docTitleE5.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }

        DataTable dtjson = BindDummyTable("34");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE6()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.6");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            lblE6_1.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            lblE6_2.Text = dt.Rows[0]["FLDDOCUMENTREVISIONNO"].ToString();
            lblE6_3.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            lblE6_4.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            lblE6_5.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            lblE6_6.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();

            hlE6_1.Text = dt.Rows[0]["FLDFORMNAME"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                hlE6_1.NavigateUrl = "../Common/download.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString();
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                hlE6_1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");

            //txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
        }
        else
        {
            lblE6_1.Text = "";
            lblE6_2.Text = "";
            lblE6_3.Text = "";
            lblE6_4.Text = "";
            lblE6_5.Text = "";
            lblE6_6.Text = "";
            hlE6_1.Text = "";
            hlE6_1.Attributes.Add("onclick", "");
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            //if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            //    docTitleE6.Title = dt.Rows[0]["FLDCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            //else
                docTitleE6.Title = "<b>" + dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString()+ "</b>";
        }
        DataTable dtjson = BindDummyTable("35");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindF1()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            return;
        }
        docTitleF1.Title = "<b>F.1	List of definitions and abbreviations</b>";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            dt = PhoenixRegistersEUMRVMesurementinstrument.Listdefinitionandabbreviation();
            
            rptF1.DataSource = dt;
            rptF1.DataBind();
            
            if (dt.Rows.Count > 0)
            {
                rptF1.Controls[rptF1.Controls.Count - 1].Controls[0].FindControl("lblrptF1").Visible = false;
            }
        }

        DataTable dtjson = new DataTable("36");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        if (dt.Rows.Count > 0)
        {
            foreach(DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDABBREVIATION"].ToString(), dr["FLDEXPLANATION"].ToString());
            }
            
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindF2()
    {
        docTitleF2.Title = "<b>F.2 Additional information</b>";

        DataSet ds = PhoenixVesselPositionEUMRVProcedureDetailsF2.ListEUMRVProcedureDetailsEdit("F.2");
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblF2.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
        }

        DataTable dtjson = BindDummyTable("37");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDDESCRIPTION"].ToString());
        }
        jsonData.Tables.Add(dtjson);
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
                        if(dt.TableName=="3" && dt.Columns[j].ColumnName=="4")
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
        String[]  strArr = s.Split(',');

        //return strArr.ToString();
        
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        

        var jsonString = new StringBuilder();
        jsonString.Append("[");
        for(int i=0;i<strArr.Length;i++)
        {
            jsonString.Append( i==strArr.Length-1 ? "\"" + strArr[i] + "\"": "\"" + strArr[i] + "\",");
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
        DataTable t1 = new DataTable();
        t1 = dt1Pdf;
        DataTable dtPartB21 = new DataTable();
        dtPartB21 = jsonData.Tables["1"];
        DataTable dtPartB22 = new DataTable();
        dtPartB22 = jsonData.Tables["2"];
        DataTable dtPartB23 = new DataTable();
        dtPartB23 = jsonData.Tables["3"];
        DataTable dtPartB24 = new DataTable();
        dtPartB24 = jsonData.Tables["4"];
        DataTable dtPartB25 = new DataTable();
        dtPartB25 = jsonData.Tables["5"];
        DataTable dtPartC31 = new DataTable();
        dtPartC31 = jsonData.Tables["6"];
        DataTable dtPartC321 = new DataTable();
        dtPartC321 = jsonData.Tables["7"];
        DataTable dtPartC322 = new DataTable();
        dtPartC322 = jsonData.Tables["8"];
        DataTable dtPartC323 = new DataTable();
        dtPartC323 = jsonData.Tables["9"];
        DataTable dtPartC324 = new DataTable();
        dtPartC324 = jsonData.Tables["10"];
        DataTable dtPartC325 = new DataTable();
        dtPartC325 = jsonData.Tables["11"];
        DataTable dtPartC326 = new DataTable();
        dtPartC326 = jsonData.Tables["12"];
        DataTable dtPartC327 = new DataTable();
        dtPartC327 = jsonData.Tables["13"];
        DataTable dtPartC328 = new DataTable();
        dtPartC328 = jsonData.Tables["14"];
        DataTable dtPartC329 = new DataTable();
        dtPartC329 = jsonData.Tables["15"];
        DataTable dtPartC3210 = new DataTable();
        dtPartC3210 = jsonData.Tables["16"];
        DataTable dtPartC3211 = new DataTable();
        dtPartC3211 = jsonData.Tables["17"];
        DataTable dtPartC3212 = new DataTable();
        dtPartC3212 = jsonData.Tables["18"];
        DataTable dtPartC33 = new DataTable();
        dtPartC33 = jsonData.Tables["19"];
        DataTable dtPartC41 = new DataTable();
        dtPartC41 = jsonData.Tables["20"];
        DataTable dtPartC42 = new DataTable();
        dtPartC42 = jsonData.Tables["21"];
        DataTable dtPartC51 = new DataTable();
        dtPartC51 = jsonData.Tables["22"];
        DataTable dtPartC52 = new DataTable();
        dtPartC52 = jsonData.Tables["23"];
        DataTable dtPartC61 = new DataTable();
        dtPartC61 = jsonData.Tables["24"];
        DataTable dtPartC62 = new DataTable();
        dtPartC62 = jsonData.Tables["25"];
        DataTable dtPartD41 = new DataTable();
        dtPartD41 = jsonData.Tables["26"];
        DataTable dtPartD42 = new DataTable();
        dtPartD42 = jsonData.Tables["27"];
        DataTable dtPartD43 = new DataTable();
        dtPartD43 = jsonData.Tables["28"];
        DataTable dtPartD44 = new DataTable();
        dtPartD44 = jsonData.Tables["29"];
        DataTable dtPartE51 = new DataTable();
        dtPartE51 = jsonData.Tables["30"];
        DataTable dtPartE52 = new DataTable();
        dtPartE52 = jsonData.Tables["31"];
        DataTable dtPartE53 = new DataTable();
        dtPartE53 = jsonData.Tables["32"];
        DataTable dtPartE54 = new DataTable();
        dtPartE54 = jsonData.Tables["33"];
        DataTable dtPartE55 = new DataTable();
        dtPartE55 = jsonData.Tables["34"];
        DataTable dtPartE56 = new DataTable();
        dtPartE56 = jsonData.Tables["35"];
        DataTable dtPartF61 = new DataTable();
        dtPartF61 = jsonData.Tables["36"];
        DataTable dtPartF62 = new DataTable();
        dtPartF62 = jsonData.Tables["37"];
        StringBuilder DsHtmlcontent = new StringBuilder();
        //DsHtmlcontent.Append("<style>#tbl1 th{ background: lightgray; } </style> ");
        //DsHtmlcontent.Append("<head><style>'.mystyle{font-family:Arial, Helvetica, sans-serif;font-size:70px;'}</style></head>");
        DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
        DsHtmlcontent.Append("<font size='2' face='Helvetica'>");
        //DsHtmlcontent.Append("<h2>Basic Data</h2>");
        DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<font color='white'><tr><td height='9'>" + txtNameofthecompany.Text + "</td><td align='right' height='9'>" + ucVessel.SelectedVesselName.ToString() + "-" + txtIMOIdentificationNumber.Text + "</td></tr>");
        DsHtmlcontent.Append("<tr><td height='9'>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td><td height='9'></td></tr></font>");
        DsHtmlcontent.Append("</table>");
        //DsHtmlcontent.Append("<h>Part A</h><br/><br/>");
        //DsHtmlcontent.Append("<br/></br><table ID='tbl1' border='0.5' bgcolor='#f1f1f1'>");
        DsHtmlcontent.Append("<br/></br><table ID=\"headertable\" border='0.5' class=\"headertable\" cellpadding='7' cellspacing='0' >");
        DsHtmlcontent.Append("<tr><td><b>Part A     Revision record Sheet</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        //DsHtmlcontent.Append("<colgroup><col span='0.5' style='background-color:#E3E8E8'><col style='background-color:#cccccc'></colgroup>");
        DsHtmlcontent.Append("<table ID=\"tbl1\" border ='0.5'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Version No</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Reference date</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Status at reference date</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Reference to Chapters where revision or modifications have been made, including a brief explanation of changes</th></tr>");
        foreach (DataRow dr in t1.Rows)
        {
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDVERSIONNO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDREFERENCEDATE"].ToString()) + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDNAME"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDEXPLANATION"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<h><br/><b>Part B     Basic data</b></h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td><b>B.1. Identification of the ship</b> </td></tr>");
        DsHtmlcontent.Append("</table>");
        DataRow dr1 = dtPartB21.NewRow();
        if (dtPartB21.Rows.Count > 0)
        {
            dr1 = dtPartB21.Rows[0];
        }
        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0' >");
        //DsHtmlcontent.Append("<colgroup><col span='0.5' style='background-color:#E3E8E8'></colgroup>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Name of the ship</td> <td>" + dr1["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>IMO Identification Number</td><td>" + dr1["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Port of Registry</td><td>" + dr1["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Home Port (if not identical with port of registry)</td><td>" + dr1["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Name of the Shipowner</td><td>" + dr1["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>IMO Unique Company and registered owner identification number</td><td>" + dr1["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Type of the Ship</td><td>" + dr1["g"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Deadweight (in metric tonnes)</td><td>" + dr1["h"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Gross tonnage</td><td>" + dr1["i"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Classification Society (voluntary)</td><td>" + dr1["j"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Ice Class (voluntary)<td>" + dr1["k"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Flag State (voluntary)</td><td>" + dr1["l"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Voluntary open description field for additional information about the characterestics of the ship</td><td>" + dr1["m"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
        DsHtmlcontent.Append("<tr><td><b>B.2. Company information</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DataRow dr2 = dtPartB22.Rows[0];
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Name of the company</td> <td>" + dr2["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>IMO No</td> <td>" + dr2["j"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Address Line 1</td><td>" + dr2["b"].ToString().Substring(0, dr2["b"].ToString().IndexOf(',')) + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Address Line 2</td><td>" + dr2["b"].ToString().Substring(dr2["b"].ToString().IndexOf(',') + 1) + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>City</td><td>" + dr2["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>State/Province/Region</td><td>" + dr2["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Postcode/ZIP</td><td>" + dr2["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Country</td><td>" + dr2["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Contact person</td><td>" + dr2["g"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Telephone number</td><td>" + dr2["h"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Email address</td><td>" + dr2["i"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>B.3. Emission sources and fuel types used</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Emission source reference no.</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Emission source(name,type)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ID No</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan='4'>Technical description of emission source (performance/power, </br>specific fuel oil consumption(SFOC), year of installation, </br>identification number in case of multiple identical emission sources, etc.) </th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Power</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>SFOC</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>(Potential) Fuel types used</th></tr>");
              

        //string s;
        //s = "ID No:154SFOC No";
        //s = s.Substring(s.indexOf("[") + 1, s.indexOf("]"));
        foreach (DataRow dr in dtB3.Rows)
        {

            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDCOMPONENTNUMBER"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDCOMPONENTNAME"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDIDNO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td colspan='4'>" + " Ser No : " + dr["FLDSERIALNUMBER"].ToString() +
                            ", "+ dr["FLDPOWERCAPTION"].ToString()+" " + dr["FLDPEFORMENCEPOWER"].ToString() + " " + dr["FLDPOWERUNITS"].ToString() + ", " +
                            dr["FLDSFOCLABEL"].ToString() + "" + dr["FLDIDENTIFICATIONNO"].ToString() + " " + dr["FLDSFOCUNITS"].ToString() +
                            ", Year of Installation : " + dr["FLDYEAROFINSTALLATION"].ToString() + "</td>");

            //DsHtmlcontent.Append("<td>" + dr["FLDYEAROFINSTALLATION"].ToString() + "</td>");
            //DsHtmlcontent.Append("<td>" + dr["FLDPEFORMENCEPOWER"].ToString() + "</td>");
            //DsHtmlcontent.Append("<td>" + dr["FLDIDENTIFICATIONNO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDOILTYPENAME"].ToString() + "</td>");
            //DsHtmlcontent.Append("<td>" + dr["5"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>B.4. Emission factors</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Fuel Type</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Reference(in tonnes of CO2/ tonne fuel)</th></tr>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>IMO emission Factors (in tonnes of CO2/ tonne fuel)</th></tr>");
        
        foreach (DataRow dr in dtPartB24.Rows)
        {
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td>");
           // DsHtmlcontent.Append("<td>" + dr["3"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleB5.Title + " </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of Procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Managing the completeness of the list of emission sources</th></tr>");
        //foreach (DataRow dr in dtPartB25.Rows)
        //{
        //DataRow dr = dtPartB25.Rows[0];
        DataRow drB25 = dtPartB25.NewRow();
        if (dtPartB25.Rows.Count > 0)
        {
            drB25 = dtPartB25.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drB25["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drB25["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drB25["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drB25["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drB25["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drB25["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/><b>Part C     Activity data</b></h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>C.1. Conditions of exemption related to Article 9 (2) </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Item</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Confirmation field</th></tr>");
        //foreach (DataRow dr in dtPartC31.Rows)
        //{
        DataRow drC31 = dtPartC31.NewRow();
        if (dtPartC31.Rows.Count > 0)
        {
            drC31 = dtPartC31.Rows[0];
        }
        //DataRow drC31 = dtPartC31.Rows[0];
        DsHtmlcontent.Append("<tr><td>Minimum number of expected voyages per reporting period falling under the scope of the EU MRV Regulation according to the ship's schedule</dt><td>" + drC31["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Are there expected voyages per reporting period not falling under the scope of the EU MRV Regulation according to the ship's schedule?</td><td>" + drC31["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Conditions of Article 9 (2) fulfilled?</td><td>" + drC31["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>If yes, do you intend to make use of the derogation for monitoring the amount of fuel consumed on a per-voyage basis?</td><td>" + drC31["d"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        //DsHtmlcontent.Append("<tr><td>C.2. Methods used to determine fuel consumption of each emission source </td></tr>");
        DsHtmlcontent.Append("<tr><td><b>C.2 Monitoring of fuel consumption </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        //DsHtmlcontent.Append("<tr><td>C.2. Methods used to determine fuel consumption of each emission source </td></tr>");
        DsHtmlcontent.Append("<tr><td><b>C.2.1 Methods used to determine fuel consumption of each emission source </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Emission source</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Chosen methods for fuel consumption</th></tr>");
        foreach (DataRow dr in dtPartC321.Rows)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td><td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining fuel bunkered and fuel in tanks</th></tr>");
        //foreach (DataRow dr in dtPartC322.Rows)
        //{
        //DataRow drC322 = dtPartC322.Rows[0];

        DataRow drC322 = dtPartC322.NewRow();
        if (dtPartC322.Rows.Count > 0)
        {
            drC322 = dtPartC322.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC322["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC322["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drC322["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC322["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC322["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC322["f"].ToString() + "</td></tr></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_3.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Regular cross-checks between bunkering quantity as provided by BDN and bunkering quantity indicated by on-board measurement</th></tr>");
        //foreach (DataRow dr in dtPartC323.Rows)
        //{
        //DataRow drC323 = dtPartC323.Rows[0];

        DataRow drC323 = dtPartC323.NewRow();
        if (dtPartC323.Rows.Count > 0)
        {
            drC323 = dtPartC323.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC323["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC323["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drC323["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC323["d"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>C.2.4 Description of the measurement instruments involved</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Measurement equipment (name)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Elements applied to (eg.emission sources,tanks)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Technical description (specification, age, maintenance intervals)</th></tr>");
        foreach (DataRow dr in dtPartC324.Rows)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["3"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_5.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording, retrieving, transmitting and storing information regarding measurements</th></tr>");
        //foreach (DataRow dr in dtPartC325.Rows)
        //{
        //DataRow drC325 = dtPartC325.Rows[0];

        DataRow drC325 = dtPartC325.NewRow();
        if (dtPartC325.Rows.Count > 0)
        {
            drC325 = dtPartC325.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC325["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC325["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drC325["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC325["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC325["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC325["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_6.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Fuel type/tank</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to determine actual density values of fuel bunkered</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to determine actual density values of fuel in tanks</th></tr>");
        foreach (DataRow dr in dtPartC326.Rows)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["3"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_7.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Monitoring method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Approach used</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Value</th></tr>");
        foreach (DataRow dr in dtPartC327.Rows)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["3"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_8.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Ensuring quality assurance of measuring equipment</th></tr>");
        //foreach (DataRow dr in dtPartC328.Rows)
        //{
        //DataRow drC328 = dtPartC328.Rows[0];

        DataRow drC328 = dtPartC328.NewRow();
        if (dtPartC328.Rows.Count > 0)
        {
            drC328 = dtPartC328.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC328["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC328["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drC328["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC328["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC328["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC328["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_9.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining the split of fuel consumption into freight and passenger part</th></tr>");
        //foreach (DataRow dr in dtPartC329.Rows)
        //{
        DataRow drC329 = dtPartC329.NewRow();
        if (dtPartC329.Rows.Count > 0)
        {
            drC329 = dtPartC329.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Applied allocation method according to EN16258</td><td>" + drC329["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of method to determine the mass of freight and passengers including possible use of default values for the weight of cargo units/ lane meters (if mass method is used)</td><td>" + drC329["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of method to determine the deck area assigned to freight and passengers including the consideration of hanging decks and of passenger cars on freight decks (if area method is used only)</td><td>" + drC329["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Split of fuel consumption into freight and passenger part (if area method is used only)</td><td>" + drC329["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drC329["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC329["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC329["g"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC329["h"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_10.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption on laden Voyages</th></tr>");
        //foreach (DataRow dr in dtPartC3210.Rows)
        //{
        //DataRow drC3210 = dtPartC3210.Rows[0];

        DataRow drC3210 = dtPartC3210.NewRow();
        if (dtPartC3210.Rows.Count > 0)
        {
            drC3210 = dtPartC3210.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC3210["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC3210["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure if not already existing outside the MP</td><td>" + drC3210["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC3210["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC3210["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC3210["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC3210["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_11.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption for cargo heating</th></tr>");
        //foreach (DataRow dr in dtPartC3211.Rows)
        //{
        //DataRow drC3211 = dtPartC3211.Rows[0];

        DataRow drC3211 = dtPartC3211.NewRow();
        if (dtPartC3211.Rows.Count > 0)
        {
            drC3211 = dtPartC3211.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC3211["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC3211["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure if not already existing outside the MP</td><td>" + drC3211["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC3211["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC3211["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC3211["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC3211["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC2_12.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption for dynamic positioning</th></tr>");
        //foreach (DataRow dr in dtPartC3212.Rows)
        //{
        //DataRow drC3212 = dtPartC3212.Rows[0];

        DataRow drC3212 = dtPartC3212.NewRow();
        if (dtPartC3212.Rows.Count > 0)
        {
            drC3212 = dtPartC3212.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC3212["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC3212["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure if not already existing outside the MP</td><td>" + drC3212["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC3212["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC3212["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC3212["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC3212["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC3.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and safeguarding completeness of voyages</th></tr>");
        //foreach (DataRow dr in dtPartC33.Rows)
        //{
        //DataRow drC33 = dtPartC33.Rows[0];

        DataRow drC33 = dtPartC33.NewRow();
        if (dtPartC33.Rows.Count > 0)
        {
            drC33 = dtPartC33.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC33["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC33["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure (including recording voyages, monitoring voyages etc.) if not already existing outside the MP</td><td>" + drC33["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC33["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Data Sources</td><td>" + drC33["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC33["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC33["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC4_1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and determining the distance per voyage made</th></tr>");
        //foreach (DataRow dr in dtPartC41.Rows)
        //{
        //DataRow drC41 = dtPartC41.Rows[0];

        DataRow drC41 = dtPartC41.NewRow();
        if (dtPartC41.Rows.Count > 0)
        {
            drC41 = dtPartC41.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC41["a"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Version of existing procedure</td><td>" + drC41["b"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Description of EU MRV procedure (including recording and managing distance information) if not already existing outside the MP</td><td>" + drC41["c"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of person or position responsible for this procedure</td><td>" + drC41["d"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Data Sources</td><td>" + drC41["e"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Location where records are kept</td><td>" + drC41["f"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of IT system used (where applicable)</td><td>" + drC41["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC4_2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the distance travelled when navigating through ice</th></tr>");
        //foreach (DataRow dr in dtPartC42.Rows)
        //{
        //DataRow drC42 = dtPartC42.Rows[0];

        DataRow drC42 = dtPartC42.NewRow();
        if (dtPartC42.Rows.Count > 0)
        {
            drC42 = dtPartC42.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC42["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC42["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure (including recording and managing distance and winter conditions information) if not already existing outside the MP</td><td>" + drC42["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC42["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC42["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC42["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC42["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC5_1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and determining the amount of cargo carried and/ or the number of passengers</th></tr>");
        //foreach (DataRow dr in dtPartC51.Rows)
        //{
        //DataRow drC51 = dtPartC51.Rows[0];

        DataRow drC51 = dtPartC51.NewRow();
        if (dtPartC51.Rows.Count > 0)
        {
            drC51 = dtPartC51.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC51["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC51["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure (including recording and determining the amount of cargo carried and/or the number of passengers and the use of default values for the mass of cargo units, if applicable) if not already existing outside the MP</td><td>" + drC51["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Unit of cargo/passengers</td><td>" + drC51["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC51["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC51["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC51["g"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC51["h"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC5_2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the average density of the cargoes transported</th></tr>");
        //foreach (DataRow dr in dtPartC52.Rows)
        //{
        //DataRow drC52 = dtPartC52.Rows[0];

        DataRow drC52 = dtPartC52.NewRow();
        if (dtPartC52.Rows.Count > 0)
        {
            drC52 = dtPartC52.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC52["a"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Version of existing procedure</td><td>" + drC52["b"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Description of EU MRV procedure (including recording and managing cargo density information) if not already existing outside the MP</td><td>" + drC52["c"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of person or position responsible for this procedure</td><td>" + drC52["d"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Formulae and data sources</td><td>" + drC52["e"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Location where records are kept</td><td>" + drC52["f"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of IT system used (where applicable)</td><td>" + drC52["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC6_1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the time spent at sea from berth of port of departure to berth of the port of arrival</th></tr>");
        //foreach (DataRow dr in dtPartC61.Rows)
        //{
        //DataRow drC61 = dtPartC61.Rows[0];

        DataRow drC61 = dtPartC61.NewRow();
        if (dtPartC61.Rows.Count > 0)
        {
            drC61 = dtPartC61.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC61["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC61["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure (including recording and managing port departure and arrival information) if not already existing outside the MP</td><td>" + drC61["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC61["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC61["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC61["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC61["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleC6_2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Procedures for determining and recording the time spent at sea when navigating through ice</th></tr>");
        //foreach (DataRow dr in dtPartC62.Rows)
        //{
        //DataRow drC62 = dtPartC62.Rows[0];

        DataRow drC62 = dtPartC62.NewRow();
        if (dtPartC62.Rows.Count > 0)
        {
            drC62 = dtPartC62.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drC62["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drC62["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedure (including recording and managing port departure and arrival and winter conditions information) if not already existing outside the MP</td><td>" + drC62["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this procedure</td><td>" + drC62["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formulae and data sources</td><td>" + drC62["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drC62["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drC62["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/>Part D Data gaps</h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleD1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to be used to estimate fuel consumption</th></tr>");
        //foreach (DataRow dr in dtPartD41.Rows)
        //{
        //DataRow drD41 = dtPartD41.Rows[0];

        DataRow drD41 = dtPartD41.NewRow();
        if (dtPartD41.Rows.Count > 0)
        {
            drD41 = dtPartD41.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Back-up monitoring method (A/B/C/D)</td><td>" + drD41["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Formula used</td><td>" + drD41["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of method to estimate fuel consumption</td><td>" + drD41["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drD41["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Data sources</td><td>" + drD41["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drD41["f"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drD41["g"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleD2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding distance travelled</th></tr>");
        //foreach (DataRow dr in dtPartD42.Rows)
        //{
        //DataRow drD42 = dtPartD42.Rows[0];

        DataRow drD42 = dtPartD42.NewRow();
        if (dtPartD42.Rows.Count > 0)
        {
            drD42 = dtPartD42.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Formula used</td><td>" + drD42["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of method to treat data gaps</td><td>" + drD42["B"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drD42["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Data sources</td><td>" + drD42["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drD42["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drD42["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleD3.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding cargo carried</th></tr>");
        //foreach (DataRow dr in dtPartD43.Rows)
        //{
        //DataRow drD43 = dtPartD43.Rows[0];

        DataRow drD43 = dtPartD43.NewRow();
        if (dtPartD43.Rows.Count > 0)
        {
            drD43 = dtPartD43.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Formula used</td><td>" + drD43["a"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Description of method to treat data gaps</td><td>" + drD43["b"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of person or position responsible for this method</td><td>" + drD43["c"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Data sources</td><td>" + drD43["d"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Location where records are kept</td><td>" + drD43["e"].ToString() + "</td>");
        DsHtmlcontent.Append("<td>Name of IT system used (where applicable)</td><td>" + drD43["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleD4.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding time spent at sea</th></tr>");
        //foreach (DataRow dr in dtPartD44.Rows)
        //{
        //DataRow drD44 = dtPartD44.Rows[0];

        DataRow drD44 = dtPartD44.NewRow();
        if (dtPartD44.Rows.Count > 0)
        {
            drD44 = dtPartD44.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Formula used</td><td>" + drD44["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of method to treat data gaps</td><td>" + drD44["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drD44["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Data sources</td><td>" + drD44["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drD44["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drD44["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/>Part E Management</h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Regular check of the adequancy of the monitoring plan</th></tr>");
        //foreach (DataRow dr in dtPartE51.Rows)
        //{
        //DataRow drE51 = dtPartE51.Rows[0];

        DataRow drE51 = dtPartE51.NewRow();
        if (dtPartE51.Rows.Count > 0)
        {
            drE51 = dtPartE51.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE51["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drE51["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drE51["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE51["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE51["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE51["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of Procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Information Technology Management (e.g. access controls, back up, recovery and security)</th></tr>");
        //foreach (DataRow dr in dtPartE52.Rows)
        //{
        //DataRow drE52 = dtPartE52.Rows[0];

        DataRow drE52 = dtPartE52.NewRow();
        if (dtPartE52.Rows.Count > 0)
        {
            drE52 = dtPartE52.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE52["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Brief description of procedure</td><td>" + drE52["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE52["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE52["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE52["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>List of relevant existing management systems</td><td>" + drE52["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE3.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Internal reviews and validation of EU MRV relevant data</th></tr>");
        //foreach (DataRow dr in dtPartE53.Rows)
        //{
        //DataRow drE53 = dtPartE53.Rows[0];

        DataRow drE53 = dtPartE53.NewRow();
        if (dtPartE53.Rows.Count > 0)
        {
            drE53 = dtPartE53.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE53["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drE53["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drE53["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE53["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE53["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE53["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE4.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Corrections and corrective actions</th></tr>");
        //foreach (DataRow dr in dtPartE54.Rows)
        //{
        //DataRow drE54 = dtPartE54.Rows[0];

        DataRow drE54 = dtPartE54.NewRow();
        if (dtPartE54.Rows.Count > 0)
        {
            drE54 = dtPartE54.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE54["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drE54["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drE54["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE54["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE54["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE54["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE5.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Outsourced activities</th></tr>");
        //foreach (DataRow dr in dtPartE55.Rows)
        //{
        //DataRow drE55 = dtPartE55.Rows[0];

        DataRow drE55 = dtPartE55.NewRow();
        if (dtPartE55.Rows.Count > 0)
        {
            drE55 = dtPartE55.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE55["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drE55["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drE55["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE55["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE55["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE55["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleE6.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Documentation</th></tr>");
        //foreach (DataRow dr in dtPartE56.Rows)
        //{
        //DataRow drE56 = dtPartE56.Rows[0];

        DataRow drE56 = dtPartE56.NewRow();
        if (dtPartE56.Rows.Count > 0)
        {
            drE56 = dtPartE56.Rows[0];
        }
        DsHtmlcontent.Append("<tr><td>Reference to existing procedure</td><td>" + drE56["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Version of existing procedure</td><td>" + drE56["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Description of EU MRV procedures if not already existing outside the MP</td><td>" + drE56["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of person or position responsible for this method</td><td>" + drE56["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Location where records are kept</td><td>" + drE56["e"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Name of IT system used (where applicable)</td><td>" + drE56["f"].ToString() + "</td></tr>");
        //}
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/>Part F Further information</h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleF1.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Abbreviation,acronym,definition</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Explanation</th></tr>");
        foreach (DataRow dr in dtPartF61.Rows)
        {
            DsHtmlcontent.Append("<tr><td>" + dr["1"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + docTitleF2.Title + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5'>");
        foreach (DataRow dr in dtPartF62.Rows)
        {
            DsHtmlcontent.Append("<tr><td>" + dr["a"].ToString() + "</td>");
        }
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("</font>");
        DsHtmlcontent.Append("</div>");
        return DsHtmlcontent.ToString();

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

                    string filefullpath = "EUMRV Basic Data" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

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

    protected void gvEngine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindB3();
    }

    protected void gvFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindB4();
    }
}
