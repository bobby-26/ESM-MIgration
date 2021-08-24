using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Net;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class RegistersEUMRVRevisionRecordSheet : PhoenixBasePage
{
    DataSet jsonData = new DataSet();
    DataTable dummydt = new DataTable();
    DataTable dt1Pdf = new DataTable();
    DataTable dtB3 = new DataTable();

    String txtNameofthecompany = "";
    String txtIMOIdentificationNumber = "";
    String lblTitleF2 = "F.2 Additional information";
    String lblTitleF1 = "F.1	List of definitions and abbreviations";
    String lblTitleE6 = "";
    String lblTitleE5 = "";
    String lblTitleE4 = "";
    String lblTitleE3 = "";
    String lblTitleE2 = "";
    String lblTitleE1 = "";
    String lblTitleD4 = "";
    String lblTitleD3 = "";
    String lblTitleD2 = "";
    String lblTitleD1 = "";
    String lblTitleC6_2 = "";
    String lblTitleC6_1 = "";
    String lblTitleC5_2 = "";
    String lblTitleC5_1 = "";
    String lblTitleC4_2 = "";
    String lblTitleC4_1 = "";
    String lblTitleC3 = "";
    String lblTitleC2_9 = "";
    String lblTitleC2_8 = "";
    String lblTitleC2_7 = "";
    String lblTitleC2_6 = "";
    String lblTitleC2_5 = "";
    String lblTitleC2_3 = "";
    String lblTitleC2_2 = "";
    String lblTitleC2_12 = "";
    String lblTitleC2_11 = "";
    String lblTitleC2_10 = "";
    String lblTitleB5 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
            {
                MenuProcedureDetailList.Visible = false;
            }
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVRevisionRecordSheet.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSWS')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                UcVessel.bind();
                UcVessel.DataBind();

                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    if(PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                        UcVessel.Enabled = false;
                }
                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDVERSIONNO", "FLDREFERENCEDATE", "FLDNAME", "FLDEXPLANATION" };
                string[] alCaptions = { "Version No.", "Reference Date","Status", "Explanation" };
                DataTable dt = PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordList(General.GetNullableInteger(UcVessel.SelectedVessel));
                General.ShowExcel("Revision Record Sheet", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvSWS_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ddlStatusAdd = (((RadComboBox)e.Item.FindControl("ddlStatusAdd")).SelectedValue.ToString().Trim());
                string txtversionAdd = (((RadTextBox)e.Item.FindControl("txtversionAdd")).Text.ToString().Trim());
                string txtExplanationAdd = (((RadTextBox)e.Item.FindControl("txtExplanationAdd")).Text.ToString().Trim());
                string txtDateAdd = (((UserControlDate)e.Item.FindControl("txtDateAdd")).Text);
                if (!IsValidSeniority(txtversionAdd, txtDateAdd, ddlStatusAdd, txtExplanationAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordInsert(int.Parse(UcVessel.SelectedVessel), txtversionAdd, DateTime.Parse(txtDateAdd),int.Parse(ddlStatusAdd),txtExplanationAdd);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("REVISE"))
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    document.SetMargins(36f, 36f, 36f, 0f);

                    string Sourcefile = Server.MapPath("~") + "\\Attachments\\VESSELPOSITION\\" + ((RadLabel)e.Item.FindControl("lbldtKey")).Text + ".pdf";
                    string FilePath = "VESSELPOSITION/" + ((RadLabel)e.Item.FindControl("lbldtKey")).Text+".pdf";
                    string FileName = (((RadLabel)e.Item.FindControl("lblVesionno")).Text.ToString().Trim());

                    PdfWriter.GetInstance(document, new FileStream(Sourcefile, FileMode.Create));
                    document.Open();
                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(PdfDoc()), styles);
                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);
                    }
                    
                    document.Close();
                    var bytes = ms.ToArray();

                    string dtkey = string.Empty;
                    dtkey = PhoenixCommonFileAttachment.GenerateDTKey();
                    long length = new System.IO.FileInfo(Sourcefile).Length;

                    PhoenixCommonFileAttachment.InsertAttachment(
                                        new Guid(((RadLabel)e.Item.FindControl("lbldtKey")).Text),
                                        FileName,
                                        FilePath,
                                        (length/1024),
                                        null,
                                        null, //sync yes no
                                        null,
                                        new Guid(dtkey));
                }
                ucStatus.Text = "Published successfully";

                Rebind();

               
                
            }
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                RadLabel lblFilePath = ((RadLabel)e.Item.FindControl("lblFilePath"));
                string strURL = "~/Attachments/" + lblFilePath.Text;
                //WebClient req = new WebClient();
                //HttpResponse response = HttpContext.Current.Response;
                string filename = UcVessel.SelectedVesselName.Replace(" ", "_") + "-" + (((RadLabel)e.Item.FindControl("lblVesionno")).Text.ToString().Trim()).Replace(" ", "") + ".pdf";
                //response.Clear();
                //response.ClearContent();
                //response.ClearHeaders();
                //response.Buffer = true;
                //response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //byte[] data = req.DownloadData(Server.MapPath(strURL));
                //response.BinaryWrite(data);
                //response.End();

                //Response.ContentType = "Application/pdf";

                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filename));

                //Response.WriteFile(strURL);

                //Response.End();

                string filePath = Server.MapPath(strURL);
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSWS_RowDeleting(object sender, GridCommandEventArgs e)
    {
        try
        {
            string lbldtKey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;
            PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordDelete(new Guid(lbldtKey));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSWS_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lbldtKeyEdit")).Text;
            string ddlStatusEdit = (((RadComboBox)e.Item.FindControl("ddlStatusEdit")).SelectedValue.ToString().Trim());
            string txtversionEdit = (((RadTextBox)e.Item.FindControl("txtversionnoEdit")).Text.ToString().Trim());
            string txtExplanationEdit = (((RadTextBox)e.Item.FindControl("txtExplanationEdit")).Text.ToString().Trim());
            string txtDateEdit = (((UserControlDate)e.Item.FindControl("txtDateEdit")).Text);
            if (!IsValidSeniority(txtversionEdit, txtDateEdit, ddlStatusEdit, txtExplanationEdit))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordUpdate(int.Parse(UcVessel.SelectedVessel), txtversionEdit, DateTime.Parse(txtDateEdit), int.Parse(ddlStatusEdit), txtExplanationEdit,new Guid(dtkey));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSWS_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadComboBox ddlStatusEdit = (RadComboBox)e.Item.FindControl("ddlStatusEdit");
           
            if (ddlStatusEdit != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(5);
                ddlStatusEdit.DataSource = ds;
                ddlStatusEdit.DataTextField = "FLDNAME";
                ddlStatusEdit.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlStatusEdit.DataBind();
                ddlStatusEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlStatusEdit.SelectedIndex = 0;
                ddlStatusEdit.SelectedValue = drv["FLDSTATUS"].ToString();
            }
            LinkButton cmdRevise = (LinkButton)e.Item.FindControl("cmdRevise");
            if (cmdRevise != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    cmdRevise.Visible = true;
                else
                    cmdRevise.Visible = false;
               
            }
            LinkButton cmdDownload = (LinkButton)e.Item.FindControl("cmdDownload");
            if (cmdDownload != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    cmdDownload.Visible = false;
                else
                    cmdDownload.Visible = true;

            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            RadComboBox ddlStatusAdd = (RadComboBox)e.Item.FindControl("ddlStatusAdd");
           
            if (ddlStatusAdd != null)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(5);
                ddlStatusAdd.DataSource = ds;
                ddlStatusAdd.DataTextField = "FLDNAME";
                ddlStatusAdd.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlStatusAdd.DataBind();
                ddlStatusAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlStatusAdd.SelectedIndex = 0;
            }
        }
    }
    private bool IsValidSeniority(string Version, string Date, string Status,string Explanation)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";
        if (Version == "")
            ucError.ErrorMessage = "Version No is required.";
        if (Date == null || !DateTime.TryParse(Date, out resultdate))
            ucError.ErrorMessage = "Reference Date is required";
        if (Status == "Dummy")
            ucError.ErrorMessage = "Status is required.";
        if (Explanation == "")
            ucError.ErrorMessage = "Explanation is required.";
        return (!ucError.IsError);
    }
    private void BindB4()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMRV.EUMRVBasicData(int.Parse(UcVessel.SelectedVessel));
            DataTable a4 = jsonData.Tables.Add("4");
            a4.Columns.Add("1");
            a4.Columns.Add("2");
            a4.Columns.Add("3");
            foreach (DataRow drv in ds.Tables[2].Rows)
            {
                a4.Rows.Add((General.GetNullableString(drv["FLDREFERENCE"].ToString()) != null) ? drv["FLDOILTYPENAME"].ToString() + " (" + drv["FLDREFERENCE"].ToString() + " )" : drv["FLDOILTYPENAME"].ToString()
                    , drv["FLDEMISSIONFACTOR"].ToString(), drv["FLDREFERENCE"]);
            }
        }
    }
    private void BindData1()
    {
        DataSet ds = new DataSet();

        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMRV.EUMRVBasicData(int.Parse(UcVessel.SelectedVessel));
            DataSet dataset = new DataSet();

            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];

                DataRow drv = dt.Rows[0];

                txtNameofthecompany = dt.Rows[0]["FLDCOMPANYNAME"].ToString();

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
                a2.Rows.Add(drv["FLDCOMPANYNAME"].ToString(), drv["FLDADDRESS1"].ToString() + ", " + drv["FLDADDRESS2"].ToString(), drv["FLDCITYNAME"].ToString(), drv["FLDSTATENAME"].ToString(), drv["FLDPOSTALCODE"].ToString(), drv["FLDCONTRY"].ToString(), drv["FLDINCHARGE"].ToString()
                    , drv["FLDPHONE2"].ToString(), drv["FLDEMAIL1"].ToString(), drv["FLDIMONUMBER"].ToString());
            }
        }


    }
    private void BindA()
    {
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            dt = PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordList(General.GetNullableInteger(UcVessel.SelectedVessel));
            dt1Pdf = dt;
        }
    }
    private void BindB1()
    {
        DataSet ds = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetails(General.GetNullableInteger(UcVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            DataRow drv = dt.Rows[0];
            DataTable a = jsonData.Tables.Add("1");
            txtIMOIdentificationNumber = drv["FLDIMONUMBER"].ToString();

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
        DataSet ds = new DataSet();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            ds = PhoenixVesselPositionEUMREmissionSource.EUMRVEmissionSourcesList(int.Parse(UcVessel.SelectedVessel));
            dtB3 = ds.Tables[0];
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
                    a.Rows.Add(drv["FLDCOMPONENTNUMBER"].ToString(), drv["FLDCOMPONENTNAME"].ToString(),
                            "Power : " + drv["FLDPEFORMENCEPOWER"].ToString() + " " + drv["FLDPOWERUNITS"].ToString() + " , " +
                            drv["FLDSFOCLABEL"].ToString() + " " + drv["FLDIDENTIFICATIONNO"].ToString() + " " + drv["FLDSFOCUNITS"].ToString() +
                            ", Year of Installation : " + drv["FLDYEAROFINSTALLATION"].ToString() + " , Ser No : " + drv["FLDIDNO"].ToString()
                            , drv["FLDOILTYPENAMEJSON"].ToString(), "");
                }


            }
        }
    }
    private void BindB5()
    {

        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("B.5");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                lblTitleB5 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            else
                lblTitleB5 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtB5 = BindDummyTable("5");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtB5.Rows.Clear();
            dtB5.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtB5);

    }
    private void BindC1()
    {
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            dt = PhoenixVesselPositionEUMRVPlan.EUMRVExemptionArticleEdit(General.GetNullableInteger(UcVessel.SelectedVessel));
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
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringfuelconsumption(General.GetNullableInteger(UcVessel.SelectedVessel)
                                                                                , null, null, 1, 200, ref iRowCount, ref iTotalPageCount);
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                lblTitleC2_2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            else
                lblTitleC2_2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                lblTitleC2_3 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            else
                lblTitleC2_3 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVMesurementinstrument.EUMRVMesurementInstrumentSearch(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(UcVessel.SelectedVessel));
        }

        DataTable dtjson = new DataTable("10");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        dtjson.Columns.Add("3");
        if (dt.Rows.Count > 0)
        {

            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDMEASUREMENTEQUIPMENT"].ToString(), dr["FLDEMISSIONSOURCENAME"].ToString(), dr["FLDTECHNICALDESC"].ToString());
            }

        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC2_5()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.5");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                lblTitleC2_5 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            else
                lblTitleC2_5 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        lblTitleC2_6 = "C.2.6 Method for determination of density";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVDeterminationDensity(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(UcVessel.SelectedVessel));

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
    }
    private void BindC2_7()
    {
        lblTitleC2_7 = "C.2.7 Level of uncertainty associated with fuel monitoring";
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            dt = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringSearch(
                                                                                 null, null, 1, 200, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(UcVessel.SelectedVessel));
        }
        DataTable dtjson = new DataTable("13");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        dtjson.Columns.Add("3");
        if (dt.Rows.Count > 0)
        {

            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDMONITORINGMETHODNAME"].ToString(), dr["FLDAPPROACHUSEDNAME"].ToString(), dr["FLDVALUE"].ToString() + " %");
            }

        }
        jsonData.Tables.Add(dtjson);

    }
    private void BindC2_8()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.2.8");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
                lblTitleC2_8 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString() + " (Not Applicable)";
            else
                lblTitleC2_8 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVfreightandpassengerVesselEdit("C.2.9", General.GetNullableInteger(UcVessel.SelectedVessel));
        DataTable dt = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];
        if (dt.Rows.Count > 0)
        {
            desc = dt.Rows[0]["FLDMASSDESC"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (General.GetNullableInteger(dt2.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";
            }
            else if (General.GetNullableInteger(dt2.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }
            lblTitleC2_9 = dt2.Rows[0]["FLDNEWCODE"].ToString() + " " + dt2.Rows[0]["FLDPROCEDURE"].ToString();
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
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }    
            lblTitleC2_10 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.2.11", int.Parse(UcVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";
            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }
            lblTitleC2_11 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
         string desc = "";
         DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.2.12", int.Parse(UcVessel.SelectedVessel));
         if (ds.Tables[0].Rows.Count > 0)
         {
             DataTable dt = ds.Tables[0];
             desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
         }
         if (ds.Tables[1].Rows.Count > 0)
         {
             DataTable dt = ds.Tables[1];
             if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
             {
                 desc = "Not applicable to this vessel";

             }
             else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
             {
                 desc = "The Company does not wish to report on this procedure";
             }
             lblTitleC2_12 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleC3 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleC4_1 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }
            lblTitleC4_2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtjson = BindDummyTable("21");
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
    private void BindC5_1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("C.5.1");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleC5_1 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtjson = BindDummyTable("22");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDUNITOFCARGOORPASSENGERS"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindC5_2()
    {
        string desc = "";
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVVesselProcedureConfigDetailEdit("C.5.2", int.Parse(UcVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";
            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }
            lblTitleC5_2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleC6_1 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
            desc = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEFORVESSELTYPEYN"].ToString()) == 0)
            {
                desc = "Not applicable to this vessel";

            }
            else if (General.GetNullableInteger(dt.Rows[0]["FLDAPPLICABLEYN"].ToString()) == 0)
            {
                desc = "The Company does not wish to report on this procedure";
            }
            lblTitleC6_2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtjson = BindDummyTable("25");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDFORMULAE"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        else
        {
            dtjson.Rows.Add("", "", desc, "", "", "", "");
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindD1()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("D.1");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleD1 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleD2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleD3 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleD4 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE1 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtjson = BindDummyTable("30");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDDOCUMENTREVISIONNO"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.2");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE2 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
        }
        DataTable dtjson = BindDummyTable("31");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDFORMNAME"].ToString(), dr["FLDEUMRVPROCEDURE"].ToString(), dr["FLDRESPONSIBLEPERSION"].ToString(), dr["FLDRECORDLOCATION"].ToString(), dr["FLDITSYSTEMNAME"].ToString(), dr["FLDLISTMGNTSYSTEM"].ToString());
        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindE3()
    {
        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit("E.3");
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE3 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE4 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE5 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            lblTitleE6 = dt.Rows[0]["FLDNEWCODE"].ToString() + " " + dt.Rows[0]["FLDPROCEDURE"].ToString();
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
        DataTable dt = new DataTable();
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            dt = PhoenixRegistersEUMRVMesurementinstrument.Listdefinitionandabbreviation();
        }

        DataTable dtjson = new DataTable("36");
        dtjson.Columns.Add("1");
        dtjson.Columns.Add("2");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dtjson.Rows.Add(dr["FLDABBREVIATION"].ToString(), dr["FLDEXPLANATION"].ToString());
            }

        }
        jsonData.Tables.Add(dtjson);
    }
    private void BindF2()
    {
        DataSet ds = PhoenixVesselPositionEUMRVProcedureDetailsF2.ListEUMRVProcedureDetailsEdit("F.2");
        DataTable dtjson = BindDummyTable("37");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dtjson.Rows.Add(dr["FLDDESCRIPTION"].ToString());
        }
        jsonData.Tables.Add(dtjson);
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
    private string PdfDoc()
    {
        BindA();
        BindB1();
        BindData1();
        BindB3();
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
        DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
        DsHtmlcontent.Append("<font size='2' face='Helvetica'>");
        DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<font color='white'><tr><td height='9'>" + txtNameofthecompany + "</td><td align='right' height='9'>" + UcVessel.SelectedVesselName.ToString() + "-" + txtIMOIdentificationNumber + "</td></tr>");
        DsHtmlcontent.Append("<tr><td height='9'>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td><td height='9'></td></tr></font>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("<br/></br><table ID=\"headertable\" border='0.5' class=\"headertable\" cellpadding='7' cellspacing='0' >");
        DsHtmlcontent.Append("<tr><td><b>Part A     Revision record Sheet</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
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
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Year of Installation</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Power</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>SFOC</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>(Potential) Fuel types used</th></tr>");

        foreach (DataRow dr in dtB3.Rows)
        {

            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["FLDCOMPONENTNUMBER"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDCOMPONENTNAME"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDIDNO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDYEAROFINSTALLATION"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDPEFORMENCEPOWER"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDIDENTIFICATIONNO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["FLDOILTYPENAME"].ToString() + "</td>");
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
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>IMO emission Factors (in tonnes of CO2/ tonne fuel)</th></tr>");

        foreach (DataRow dr in dtPartB24.Rows)
        {
            DsHtmlcontent.Append("<tr>");//colspan='2'
            DsHtmlcontent.Append("<td>" + dr["1"].ToString() + "</td>");
            DsHtmlcontent.Append("<td>" + dr["2"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleB5 + " </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of Procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Managing the completeness of the list of emission sources</th></tr>");

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
        DataRow drC31 = dtPartC31.NewRow();
        if (dtPartC31.Rows.Count > 0)
        {
            drC31 = dtPartC31.Rows[0];
        }

        DsHtmlcontent.Append("<tr><td>Minimum number of expected voyages per reporting period falling under the scope of the EU MRV Regulation according to the ship's schedule</dt><td>" + drC31["a"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Are there expected voyages per reporting period not falling under the scope of the EU MRV Regulation according to the ship's schedule?</td><td>" + drC31["b"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>Conditions of Article 9 (2) fulfilled?</td><td>" + drC31["c"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("<tr><td>If yes, do you intend to make use of the derogation for monitoring the amount of fuel consumed on a per-voyage basis?</td><td>" + drC31["d"].ToString() + "</td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>C.2 Monitoring of fuel consumption </b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining fuel bunkered and fuel in tanks</th></tr>");

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

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_3 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Regular cross-checks between bunkering quantity as provided by BDN and bunkering quantity indicated by on-board measurement</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_5 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording, retrieving, transmitting and storing information regarding measurements</th></tr>");

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

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_6 + "</b></td></tr>");
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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_7 + "</b></td></tr>");
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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_8 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Ensuring quality assurance of measuring equipment</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_9 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining the split of fuel consumption into freight and passenger part</th></tr>");
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

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_10 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption on laden Voyages</th></tr>");

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

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_11 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption for cargo heating</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC2_12 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the fuel consumption for dynamic positioning</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC3 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and safeguarding completeness of voyages</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC4_1 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and determining the distance per voyage made</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC4_2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the distance travelled when navigating through ice</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC5_1 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Recording and determining the amount of cargo carried and/ or the number of passengers</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC5_2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the average density of the cargoes transported</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC6_1 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Determining and recording the time spent at sea from berth of port of departure to berth of the port of arrival</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleC6_2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Procedures for determining and recording the time spent at sea when navigating through ice</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleD1 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to be used to estimate fuel consumption</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleD2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding distance travelled</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleD3 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding cargo carried</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleD4 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method to treat data gaps regarding time spent at sea</th></tr>");

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
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/>Part E Management</h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE1 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Regular check of the adequancy of the monitoring plan</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE2 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of Procedure</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Information Technology Management (e.g. access controls, back up, recovery and security)</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE3 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Internal reviews and validation of EU MRV relevant data</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE4 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Corrections and corrective actions</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE5 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Outsourced activities</th></tr>");

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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleE6 + "</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Title of method</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Documentation</th></tr>");

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
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<br/><br/>");
        DsHtmlcontent.Append("<h><br/>Part F Further information</h><br/><br/>");
        DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleF1 + "</b></td></tr>");
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
        DsHtmlcontent.Append("<tr><td><b>" + lblTitleF2 + "</b></td></tr>");
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

    protected void gvSWS_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDVERSIONNO", "FLDREFERENCEDATE", "FLDNAME", "FLDEXPLANATION" };
        string[] alCaptions = { "Version No.", "Reference Date", "Status", "Explanation" };
        DataTable dt = PhoenixRegistersEUMRVRevisionRecord.EUMRVRevisionRecordList(General.GetNullableInteger(UcVessel.SelectedVessel));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Revision Record Sheet", alCaptions, alColumns, ds);
        gvSWS.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (Request.QueryString["view"] != null)
            {
                gvSWS.Columns[5].Visible = false;
              //  gvSWS.FooterRow.Visible = false;
            }
        }
    }
    protected void Rebind()
    {
        gvSWS.SelectedIndexes.Clear();
        gvSWS.EditIndexes.Clear();
        gvSWS.DataSource = null;
        gvSWS.Rebind();
    }
}
