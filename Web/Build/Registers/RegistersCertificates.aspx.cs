using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class RegistersCertificates : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersCertificates.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCertificates')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCertificates.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCertificates.aspx", "Find", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRegistersCertificates.AccessRights = this.ViewState;
        MenuRegistersCertificates.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvCertificates.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();
        ViewState["SORTEXPRESSION"] = "EXCEL";
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEXCELORDER", "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDSURVEYCYCLETYPEDESC", "FLDUSEANNIVERSARYDATE", "FLDDEPARTMENTNAME", "FLDVERIFICATIONREQUIRED", "FLDACTIVEYN" };
        string[] alCaptions = { "Sort Order", "Code", "Name", "Validity Cycle", "Use Ann.", "Department Responsible", "Verification Required Y/N", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersCertificates.CertificatesSearch(0, txtCertificateCode.Text, txtSearch.Text,General.GetNullableInteger(ddlCertificateCategory.SelectedCategory), sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Certificates.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"].ToString() + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>RECORD OF SHIPS' CERTIFICATES, SURVEYS AND INSPECTIONS</center></h5></td></tr>");
        Response.Write("<tr>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td align='left' style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 3).ToString() + "' align='left'>Date:" + date + "</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        ViewState["CategoryId"] = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (ViewState["CategoryId"].ToString().Trim().Equals("") || !ViewState["CategoryId"].ToString().Trim().Equals(dr["FLDCERTIFICATECATEGORY"].ToString()))
            {
                ViewState["CategoryId"] = dr["FLDCERTIFICATECATEGORY"].ToString();
                Response.Write("<tr>");
                Response.Write("<td style='font-family:Arial; font-size:12px; background-color:#FAEBD7; font-weight:bold;' colspan='" + (alColumns.Length).ToString() + "' align='center'>" + dr["FLDCATEGORYNAME"].ToString() + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (alColumns[i] == "FLDEXCELORDER")
                    Response.Write("<td align='center'>");
                else
                    Response.Write("<td>");
                Response.Write(dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersCertificates_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ddlCertificateCategory.SelectedCategory = "";
            txtCertificateCode.Text = "";
            txtSearch.Text = "";
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void BindData(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEXCELORDER", "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDSURVEYCYCLETYPEDESC", "FLDUSEANNIVERSARYDATE", "FLDCATEGORYNAME", "FLDDEPARTMENTNAME", "FLDVERIFICATIONREQUIRED", "FLDACTIVEYN" };
        string[] alCaptions = { "Sort Order", "Code", "Name", "Validity Cycle", "Category", "Use Ann.", "Department Responsible", "Verification Required Y/N", "Active Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCertificates.CertificatesSearch(0, txtCertificateCode.Text
            , txtSearch.Text,General.GetNullableInteger(ddlCertificateCategory.SelectedCategory), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCertificates.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCertificates", "Certificate Registers", alCaptions, alColumns, ds);

        gvCertificates.DataSource = ds;
        gvCertificates.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }    

    private void InsertCertificates(string certificatecode, string Certificatesname, string SurveyCycleType, int isauditinspectionrequired, int? activeyn, int? isExpiry, string Category,
        string Order, int? department, byte? useanniversarydateyn, byte? VerificationRequired)
    {
        PhoenixRegistersCertificates.InsertCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            certificatecode.Trim(), Certificatesname.Trim(), General.GetNullableGuid(SurveyCycleType), isauditinspectionrequired, activeyn
            , isExpiry, General.GetNullableInteger(Category)
            , Order, department, useanniversarydateyn, VerificationRequired);
    }

    private void UpdateCertificates(int certificatesid, string certificatecode, string Certificatesname, string SurveyCycleType, int isauditinspectionrequired, int? activeyn, int? isExpiry, string Category
        , string Order, int? departmentid, byte? useanniversarydateyn, byte? VerificationRequired)
    {
        if (!IsValidCertificates(certificatecode, Certificatesname, isExpiry, SurveyCycleType))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCertificates.UpdateCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            certificatesid, certificatecode.Trim(), Certificatesname.Trim(), General.GetNullableGuid(SurveyCycleType), isauditinspectionrequired, activeyn
            , isExpiry, General.GetNullableInteger(Category)
            , Order, departmentid, useanniversarydateyn, VerificationRequired);
        ucStatus.Text = "Vessel Certificate information updated";
    }

    private bool IsValidCertificates(string certificatecode, string Certificatesname, int? isExpiry, string SurveyCycleType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvCertificates;

        if (certificatecode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Certificatesname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (isExpiry == 1 && !SurveyCycleType.Trim().ToUpper().Equals("DUMMY"))//Having no Expiry(Permanent) and Survey Cycle is Selected
            ucError.ErrorMessage = "Type of Survey Cycle is not required for No Expiry Certificate.";

        return (!ucError.IsError);
    }

    private void DeleteCertificates(int Certificatescode)
    {
        PhoenixRegistersCertificates.DeleteCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Certificatescode);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCertificates.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCertificates_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlSurveyTemplateEdit = (RadComboBox)e.Item.FindControl("ddlSurveyTemplateEdit");
            RadLabel lblSurveyCycleId = (RadLabel)e.Item.FindControl("lblSurveyCycleId");
            RadLabel lblSurveyCycle = (RadLabel)e.Item.FindControl("lblSurveyCycle");


            if (ddlSurveyTemplateEdit != null && lblSurveyCycleId != null)
            {
                ddlSurveyTemplateEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlSurveyTemplateEdit.SelectedValue = lblSurveyCycleId.Text.Trim();
            }

            LinkButton cmdMapVesselType = (LinkButton)e.Item.FindControl("cmdMapVesselType");
            if (cmdMapVesselType != null)
            {
                cmdMapVesselType.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapVesselType.CommandName);
                cmdMapVesselType.Attributes.Add("onclick", "javascript:openNewWindow('VesselTypeMapping', '', '" + Session["sitepath"] + "/Registers/RegistersCertificateVesselTypeMapping.aspx?DTKEY=" + dr["FLDDTKEY"].ToString() + "&CERTIFICATEID=" + dr["FLDCERTIFICATEID"].ToString() + "');return false;");
            }
            LinkButton cmdExcludeVessels = (LinkButton)e.Item.FindControl("cmdExcludeVessels");
            if (cmdExcludeVessels != null)
            {
                cmdExcludeVessels.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcludeVessels.CommandName);
                cmdExcludeVessels.Attributes.Add("onclick", "javascript:openNewWindow('ExcludeVessels', '', '" + Session["sitepath"] + "/Registers/RegistersCertificateVesselMapping.aspx?DTKEY=" + dr["FLDDTKEY"].ToString() + "&CERTIFICATEID=" + dr["FLDCERTIFICATEID"].ToString() + "');return false;");
            }
            LinkButton cmdDistribute = (LinkButton)e.Item.FindControl("cmdDistribute");
            if (cmdDistribute != null)
            {
                cmdDistribute.Visible = SessionUtil.CanAccess(this.ViewState, cmdDistribute.CommandName);
                cmdDistribute.Attributes.Add("onclick", "javascript:openNewWindow('DistributeVessel', '', '" + Session["sitepath"] + "/Registers/RegistersCertificateDistribute.aspx?DTKEY=" + dr["FLDDTKEY"].ToString() + "&CERTIFICATEID=" + dr["FLDCERTIFICATEID"].ToString() + "');return false;");
            }

            UserControlToolTip ucValidityCycle = (UserControlToolTip)e.Item.FindControl("ucValidityCycle");
            if (ucValidityCycle != null && lblSurveyCycle != null)
            {
                lblSurveyCycle.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucValidityCycle.ToolTip + "', 'visible');");
                lblSurveyCycle.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucValidityCycle.ToolTip + "', 'hidden');");
            }
            UserControlDepartment ucDepartmentEdit = (UserControlDepartment)e.Item.FindControl("ucDepartmentEdit");
            DataRowView dv = (DataRowView)e.Item.DataItem;
            if (ucDepartmentEdit != null)
            {
                ucDepartmentEdit.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDepartmentEdit.SelectedDepartment = dv["FLDDEPARTMENTID"].ToString();
            }
            RadComboBox ddlCertificateCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCertificateCategoryEdit");
            if (ddlCertificateCategoryEdit != null)
            {
                ddlCertificateCategoryEdit.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
        }
        if(e.Item is GridEditableItem)
        {
            RadTextBox txtCertificatesCodeEdit = (RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit");
            if(txtCertificatesCodeEdit != null)
            {
                ((RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit")).Focus();
            }
            //((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).Focus();
            RadComboBox ddlCertificateCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCertificateCategoryEdit");
            RadLabel lblCertificateCategoryId = (RadLabel)e.Item.FindControl("lblCertificateCategoryId");
            if (ddlCertificateCategoryEdit != null && lblCertificateCategoryId != null)
            {
                ddlCertificateCategoryEdit.SelectedValue = lblCertificateCategoryId.Text.Trim();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            RadComboBox ddlCertificateCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCertificateCategoryAdd");
            RadComboBox ddlSurveyTemplateAdd = (RadComboBox)e.Item.FindControl("ddlSurveyTemplateAdd");
            ddlSurveyTemplateAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            ddlCertificateCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlDepartment ucDepartmentAdd = (UserControlDepartment)e.Item.FindControl("ucDepartmentAdd");
            if (ucDepartmentAdd != null)
            {
                ucDepartmentAdd.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDepartmentAdd.DataBind();
            }
        }
    }

    protected void gvCertificates_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string useanniversarydateyn = ((RadCheckBox)e.Item.FindControl("chkUseAnniversaryDateYN")).Checked == true ? "1" : "0";
            if (!IsValidCertificates(((RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit")).Text
                   , ((RadTextBox)e.Item.FindControl("txtCertificatesNameEdit")).Text
                   , ((RadCheckBox)e.Item.FindControl("chkIsNoExpiryEdit")).Checked == true ? 1 : 0
                   , ((RadComboBox)e.Item.FindControl("ddlSurveyTemplateEdit")).SelectedValue
                   ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateCertificates(
                Int32.Parse(((RadLabel)e.Item.FindControl("lblCertificatesIDEdit")).Text),
                 ((RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtCertificatesNameEdit")).Text,
                ((RadComboBox)e.Item.FindControl("ddlSurveyTemplateEdit")).SelectedValue,
                 (((RadCheckBox)e.Item.FindControl("chkAuditInspectionEdit")).Checked == true) ? 1 : 0,
                 (((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true) ? 1 : 0
                 , ((RadCheckBox)e.Item.FindControl("chkIsNoExpiryEdit")).Checked == true ? 1 : 0
                , ((RadComboBox)e.Item.FindControl("ddlCertificateCategoryEdit")).SelectedValue
                 , ((RadTextBox)e.Item.FindControl("txtOrderEdit")).Text
                , General.GetNullableInteger(((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).SelectedDepartment)
                , byte.Parse(useanniversarydateyn)
                , byte.Parse(((RadCheckBox)e.Item.FindControl("chkVerificationReq")).Checked == true ? "1" : "0")
                );
            Rebind();
        }
        catch (Exception ex)
        {
            string sErrorMsg = "";
            if (ex.GetBaseException().GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ex.Message.Contains("UNIQUE KEY"))
                sErrorMsg = "Code already exists.";
            else
                sErrorMsg = ex.Message;
            ucError.ErrorMessage = sErrorMsg;
            ucError.Visible = true;
        }
    }

    protected void gvCertificates_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCertificates_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvCertificates.MasterTableView.GetItems(GridItemType.Footer)[0];
                string code = ((RadTextBox)footerItem.FindControl("txtCertificatesCodeAdd")).Text;
                string name = ((RadTextBox)footerItem.FindControl("txtCertificatesNameAdd")).Text;
                int expiryyn = ((RadCheckBox)footerItem.FindControl("chkIsNoExpiryAdd")).Checked == true ? 1 : 0;
                string template = ((RadComboBox)footerItem.FindControl("ddlSurveyTemplateAdd")).SelectedValue;
                string useanniversarydateyn = ((RadCheckBox)footerItem.FindControl("chkUseAnniversaryDateYNAdd")).Checked == true? "1" : "0";
                if (!IsValidCertificates(code
                    , name
                    , expiryyn
                    , template)
                    )
                {
                    ucError.Visible = true;
                    return;
                }

                InsertCertificates(
                   code,
                    name,
                    template,
                    (((RadCheckBox)footerItem.FindControl("chkAuditInspectionAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)footerItem.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0
                    , expiryyn
                    , ((RadComboBox)footerItem.FindControl("ddlCertificateCategoryAdd")).SelectedValue
                    , ((RadTextBox)footerItem.FindControl("txtOrderAdd")).Text
                    , General.GetNullableInteger(((UserControlDepartment)footerItem.FindControl("ucDepartmentAdd")).SelectedDepartment)
                    , byte.Parse(useanniversarydateyn)
                    , byte.Parse(((RadCheckBox)footerItem.FindControl("chkVerificationReqAdd")).Checked == true ? "1" : "0")
                    );

                Rebind();
                ((RadTextBox)footerItem.FindControl("txtCertificatesNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CertificatesID"] = ((RadLabel)e.Item.FindControl("lblCertificatesID")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            string sErrorMsg = "";
            if (ex.GetBaseException().GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ex.Message.Contains("UNIQUE KEY"))
                sErrorMsg = "Invalid Code. Because code should be Unique.";
            else
                sErrorMsg = ex.Message;
            ucError.ErrorMessage = sErrorMsg;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCertificates.SelectedIndexes.Clear();
        gvCertificates.EditIndexes.Clear();
        gvCertificates.DataSource = null;
        gvCertificates.Rebind();
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteCertificates(Int32.Parse(ViewState["CertificatesID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
