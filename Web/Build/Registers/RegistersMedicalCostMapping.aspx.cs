using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersMedicalCostMapping : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalCostMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTestCostMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");           
            MenuMedicalTestCostMapping.AccessRights = this.ViewState;
            MenuMedicalTestCostMapping.MenuList = toolbar.Show();
           

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalCostMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagMedicalCostMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuFlagMedical.AccessRights = this.ViewState;
            MenuFlagMedical.MenuList = toolbar.Show();
          

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalCostMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVaccinationCostMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuVaccinationCostMap.AccessRights = this.ViewState;
            MenuVaccinationCostMap.MenuList = toolbar.Show();
          

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Medical Test", "MEDICALTEST");
            toolbar.AddButton("Cost of Medical", "COSTOFMEDICAL");
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                toolbar.AddButton("PMU Doctor List", "PMUDOCTOR");
            MenuMedicalCost.AccessRights = this.ViewState;
            MenuMedicalCost.MenuList = toolbar.Show();
            MenuMedicalCost.SelectedMenuIndex = 1;
            toolbar = new PhoenixToolbar();
            //MenuTitle.AccessRights = this.ViewState;
            //MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
				ViewState["CLINICID"] = "";
                ViewState["DTKEY"] = "";
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                
                ViewState["PAGENUMBERMT"] = 1;
                ViewState["SORTEXPRESSIONMT"] = null;
                ViewState["SORTDIRECTIONMT"] = null;
                gvMedicalTestCostMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERFM"] = 1;
                ViewState["SORTEXPRESSIONFM"] = null;
                ViewState["SORTDIRECTIONFM"] = null;
                gvFlagMedicalCostMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERVC"] = 1;
                ViewState["SORTEXPRESSIONVC"] = null;
                ViewState["SORTDIRECTIONVC"] = null;
                gvVaccinationCostMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                DataSet ds = PhoenixRegistersAddress.EditAddress(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    long.Parse(General.GetNullableInteger(ucClinic.SelectedClinic) == null ? "0" : ucClinic.SelectedClinic));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCLINICCURRENCY"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    if (cmdAttachment != null)
                    {
                        cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                        + PhoenixModule.REGISTERS + "');return true;");
                        cmdAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? false : true;
                    }
                    if (cmdNoAttachment != null)
                    {
                        cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                        + PhoenixModule.REGISTERS + "');return true;");
                        cmdNoAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? true : false;
                    }

                }
                else
                {
                    cmdAttachment.Visible = false;
                    cmdNoAttachment.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelMedicalTest()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Medical Test", "Cost" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONMT"] == null) ? null : (ViewState["SORTEXPRESSIONMT"].ToString());
        if (ViewState["SORTDIRECTIONMT"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONMT"].ToString());
        if (ViewState["ROWCOUNTMT"] == null || Int32.Parse(ViewState["ROWCOUNTMT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTMT"].ToString());

        ds = PhoenixRegistersMedicalCostMapping.MedicalTestCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERMT"].ToString()),
                gvMedicalTestCostMapping.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MedicalTestCostMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Medical Cost Mapping</h3></td>");
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

    protected void ucClinic_Changed(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersAddress.EditAddress(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            long.Parse(General.GetNullableInteger(ucClinic.SelectedClinic) == null ? "0" : ucClinic.SelectedClinic));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCLINICCURRENCY"].ToString();
            ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                + PhoenixModule.REGISTERS + "');return true;");
                cmdAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? false : true;
            }
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                + PhoenixModule.REGISTERS + "');return true;");
                cmdNoAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? true : false;
            }
        }
        else if (ds.Tables.Count == 0)
        {
            ucCurrency.SelectedCurrency = "";
            cmdAttachment.Visible = false;
            cmdNoAttachment.Visible = false;
        }
		ViewState["CLINICID"] = ucClinic.SelectedClinic;
		PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalCostMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagMedicalCostMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddImageButton("../Registers/RegistersMedicalCostMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
        //toolbar.AddImageLink("javascript:CallPrint('gvFlagMedicalCostMap')", "Print Grid", "icon_print.png", "PRINT");
        if (ViewState["CLINICID"].ToString() != "Dummy")
		{
			toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersMedicalCostMappingHistory.aspx?clinicid=" + ViewState["CLINICID"].ToString() + "'); return false;", "History", "<i class=\"fas fa-list-alt\"></i>", "HISTORY");
		}
		MenuFlagMedical.AccessRights = this.ViewState;
		MenuFlagMedical.MenuList = toolbar.Show();
		
    }

    protected void MedicalCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COSTOFMEDICAL"))
        {
            MenuMedicalCost.SelectedMenuIndex = 1;
            return;
        }
        else if (CommandName.ToUpper().Equals("MEDICALTEST"))
        {
            Response.Redirect("../Registers/RegistersDocumentMedical.aspx");
        }
        else if (CommandName.ToUpper().Equals("PMUDOCTOR"))
        {
            Response.Redirect("../Registers/RegistersPMUDoctor.aspx");
        }
    }

    protected void MedicalTestCostMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelMedicalTest();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMedicalTest()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Medical Test", "Cost" };

        string sortexpression = (ViewState["SORTEXPRESSIONMT"] == null) ? null : (ViewState["SORTEXPRESSIONMT"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONMT"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONMT"].ToString());

        DataSet ds = PhoenixRegistersMedicalCostMapping.MedicalTestCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERMT"].ToString()),
                gvMedicalTestCostMapping.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvMedicalTestCostMapping", "Medical Cost Mapping", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMedicalTestCostMapping.DataSource = ds;
            gvMedicalTestCostMapping.VirtualItemCount = iRowCount;
        }
        else
        {
            gvMedicalTestCostMapping.DataSource = "";
        }
        
    }
    private void InsertMedicalTestCostMapping(int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.InsertMedicalTestCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, clinicid, medicalid, cost, currency);
    }

    private void UpdateMedicalTestCostMapping(int medicalcostmappingid, int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.UpdateMedicalTestCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , medicalcostmappingid, clinicid, medicalid, cost, currency);
    }

    private bool IsValidMedicalTestCostMapping(string cost, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(ucClinic.SelectedClinic) == null)
            ucError.ErrorMessage = "Clinic is required.";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    private void DeleteMedicalTestCostMapping(int medicalcostmappingid)
    {
        PhoenixRegistersMedicalCostMapping.DeleteMedicalTestCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, medicalcostmappingid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    ///////////////////////////////////////////// FLAG MEDICAL COST MAPPING

    protected void ShowExcelFlagMedical()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Medical", "Cost" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONFM"] == null) ? null : (ViewState["SORTEXPRESSIONFM"].ToString());
        if (ViewState["SORTDIRECTIONFM"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONFM"].ToString());
        if (ViewState["ROWCOUNTFM"] == null || Int32.Parse(ViewState["ROWCOUNTFM"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTFM"].ToString());

        ds = PhoenixRegistersMedicalCostMapping.FlagMedicalCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERFM"].ToString()),
                gvFlagMedicalCostMap.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FlagMedicalCostMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Flag Medical Cost</h3></td>");
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

    protected void FlagMedicalCostMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelFlagMedical();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFlagMedical()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Medical", "Cost" };

        string sortexpression = (ViewState["SORTEXPRESSIONFM"] == null) ? null : (ViewState["SORTEXPRESSIONFM"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONFM"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONFM"].ToString());

        DataSet ds = PhoenixRegistersMedicalCostMapping.FlagMedicalCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERFM"].ToString()),
                gvFlagMedicalCostMap.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvFlagMedicalCostMap", "Flag Medical Cost", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFlagMedicalCostMap.DataSource = ds;
            gvFlagMedicalCostMap.VirtualItemCount = iRowCount;
        }
        else
        {
            gvFlagMedicalCostMap.DataSource = "";
        }
    }
    
    private void InsertFlagMedicalCostMapping(int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.InsertFlagMedicalCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, clinicid, medicalid, cost, currency);
    }

    private void UpdateFlagMedicalCostMapping(int medicalcostmappingid, int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.UpdateFlagMedicalCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , medicalcostmappingid, clinicid, medicalid, cost, currency);
    }

    private bool IsValidFlagMedicalCostMapping(string cost, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(ucClinic.SelectedClinic) == null)
            ucError.ErrorMessage = "Clinic is required.";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    private void DeleteFlagMedicalCostMapping(int medicalcostmappingid)
    {
        PhoenixRegistersMedicalCostMapping.DeleteFlagMedicalCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, medicalcostmappingid);
    }


    ///////////////////////////////////////////// VACCINATION COST MAPPING

    protected void ShowExcelVaccination()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Vaccination", "Cost" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONVC"] == null) ? null : (ViewState["SORTEXPRESSIONVC"].ToString());
        if (ViewState["SORTDIRECTIONVC"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONVC"].ToString());
        if (ViewState["ROWCOUNTVC"] == null || Int32.Parse(ViewState["ROWCOUNTVC"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTVC"].ToString());

        ds = PhoenixRegistersMedicalCostMapping.VaccinationCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERVC"].ToString()),
                gvVaccinationCostMap.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VaccinationCost.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vaccination Cost</h3></td>");
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

    protected void VaccinationCostMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelVaccination();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindVaccination()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDCOST" };
        string[] alCaptions = { "Vaccination", "Cost" };

        string sortexpression = (ViewState["SORTEXPRESSIONVC"] == null) ? null : (ViewState["SORTEXPRESSIONVC"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONVC"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONVC"].ToString());

        DataSet ds = PhoenixRegistersMedicalCostMapping.VaccinationCostMappingSearch(
                General.GetNullableInteger(ucClinic.SelectedClinic) == null ? 0 : General.GetNullableInteger(ucClinic.SelectedClinic),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERVC"].ToString()),
                gvVaccinationCostMap.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvVaccinationCostMap", "Vaccination Cost", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVaccinationCostMap.DataSource = ds;
            gvVaccinationCostMap.VirtualItemCount = iRowCount;
        }
        else
        {
            gvVaccinationCostMap.DataSource = "";
        }
    }
    private void InsertVaccinationCostMapping(int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.InsertVaccinationCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, clinicid, medicalid, cost, currency);
    }

    private void UpdateVaccinationCostMapping(int medicalcostmappingid, int clinicid, int medicalid, decimal cost, int currency)
    {
        PhoenixRegistersMedicalCostMapping.UpdateVaccinationCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , medicalcostmappingid, clinicid, medicalid, cost, currency);
    }

    private bool IsValidVaccinationCostMapping(string cost, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(ucClinic.SelectedClinic) == null)
            ucError.ErrorMessage = "Clinic is required.";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    private void DeleteVaccinationCostMapping(int medicalcostmappingid)
    {
        PhoenixRegistersMedicalCostMapping.DeleteVaccinationCostMapping(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, medicalcostmappingid);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = PhoenixRegistersAddress.EditAddress(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    long.Parse(General.GetNullableInteger(ucClinic.SelectedClinic) == null ? "0" : ucClinic.SelectedClinic));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCLINICCURRENCY"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                if (cmdAttachment != null)
                {
                    cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                    cmdAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? false : true;
                }
                if (cmdNoAttachment != null)
                {
                    cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                    cmdNoAttachment.Visible = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDATTACHMENTCOUNT"].ToString()) == 0) ? true : false;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlagMedicalCostMap_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text) != null)
                    DeleteFlagMedicalCostMapping(Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text));
                BindFlagMedical();
                gvFlagMedicalCostMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidFlagMedicalCostMapping(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text
                 , ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text) == null)
                {
                    InsertFlagMedicalCostMapping(
                        int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }
                else
                {
                    UpdateFlagMedicalCostMapping(
                        Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text)
                        , int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }
                BindFlagMedical();
                gvFlagMedicalCostMap.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERFM"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlagMedicalCostMap_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERFM"] = ViewState["PAGENUMBERFM"] != null ? ViewState["PAGENUMBERFM"] : gvFlagMedicalCostMap.CurrentPageIndex + 1;
        BindFlagMedical();
    }

    protected void gvFlagMedicalCostMap_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
            DataRowView drview = (DataRowView)e.Item.DataItem;
            if (ucCurrency != null) ucCurrency.SelectedCurrency = drview["FLDCURRENCY"].ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
    }
    
    protected void gvMedicalTestCostMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text) != null)
                    DeleteMedicalTestCostMapping(Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text));
                BindMedicalTest();
                gvMedicalTestCostMapping.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidMedicalTestCostMapping(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text
                   , ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text) == null)
                {
                    InsertMedicalTestCostMapping(
                        int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }
                else
                {
                    UpdateMedicalTestCostMapping(
                        Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text)
                        , int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }
                BindMedicalTest();
                gvMedicalTestCostMapping.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERMT"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvMedicalTestCostMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERMT"] = ViewState["PAGENUMBERMT"] != null ? ViewState["PAGENUMBERMT"] : gvMedicalTestCostMapping.CurrentPageIndex + 1;
        BindMedicalTest();
    }

    protected void gvMedicalTestCostMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
            DataRowView drview = (DataRowView)e.Item.DataItem;
            if (ucCurrency != null) ucCurrency.SelectedCurrency = drview["FLDCURRENCY"].ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
    }

    protected void gvVaccinationCostMap_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text) != null)
                    DeleteVaccinationCostMapping(Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintId")).Text));
                BindVaccination();
                gvVaccinationCostMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidVaccinationCostMapping(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text
                  , ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text) == null)
                {
                    InsertVaccinationCostMapping(
                        int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }
                else
                {
                    UpdateVaccinationCostMapping(
                        Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalCostMappintIdEdit")).Text)
                        , int.Parse(ucClinic.SelectedClinic)
                        , Int32.Parse(((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text)
                        , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text)
                        , Int32.Parse(ucCurrency.SelectedCurrency));
                }               
                BindVaccination();
                gvVaccinationCostMap.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERVC"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVaccinationCostMap_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERVC"] = ViewState["PAGENUMBERVC"] != null ? ViewState["PAGENUMBERVC"] : gvVaccinationCostMap.CurrentPageIndex + 1;
        BindVaccination();
    }

    protected void gvVaccinationCostMap_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
            DataRowView drview = (DataRowView)e.Item.DataItem;
            if (ucCurrency != null) ucCurrency.SelectedCurrency = drview["FLDCURRENCY"].ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
    }
}
