using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementRAProcessList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAProcessList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAProcessList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["NEWDMSYN"] = 0;
            ucVessel.bind();
            txtRefNo.Text = "";
            ucType.SelectedValue = 0;
            ucVessel.SelectedVessel = string.Empty;

            if (Request.QueryString["NEWDMSYN"] != null)
                ViewState["NEWDMSYN"] = Request.QueryString["NEWDMSYN"].ToString();

            BindType();

            int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;

            if (installcode > 0)
            {
                ucVessel.Enabled = false;
                ucVessel.SelectedValue = installcode;
            }

            gvRiskAssessmentProcess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        if (ViewState["NEWDMSYN"].ToString().Equals("1"))
        {
            rblOldNew.SelectedValue = "0";
            ucType.Visible = false;
            ddlType.Visible = true;
        }
        else
        {
            rblOldNew.SelectedValue = "1";
            ucType.Visible = true;
            ddlType.Visible = false;

        }
        rblOldNew.Enabled = false;
        rblOldNew.Visible = false;

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDEDITEDBY" };
        string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date", "Edited By" };

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = new DataSet();
        if (rblOldNew.SelectedValue == "1")
        {
            ds = PhoenixDocumentManagementDocument.InspectionRiskAssessmentProcessSearch(
                                                                                     General.GetNullableString(txtRefNo.Text),
                                                                                     null,
                                                                                     null,
                                                                                     General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                     General.GetNullableInteger(ucVessel.SelectedVessel),
                                                                                     General.GetNullableInteger(ucType.SelectedCategory),
                                                                                     sortexpression,
                                                                                     sortdirection,
                                                                                     int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                     gvRiskAssessmentProcess.PageSize,
                                                                                     ref iRowCount,
                                                                                     ref iTotalPageCount,
                                                                                     companyid
                                                                                     );
        }
        else
        {
            ds = PhoenixDocumentManagementDocument.InspectionNonRoutineRASearchExtn(
                     int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentProcess.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression
                    , sortdirection
                    , General.GetNullableString(txtRefNo.Text)
                    , companyid
                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                    , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                    , General.GetNullableString(txtActivity.Text)
                    );
        }

        General.SetPrintOptions("gvRiskAssessmentProcess", "Risk Assessment-Process", alCaptions, alColumns, ds);

        gvRiskAssessmentProcess.DataSource = ds;
        gvRiskAssessmentProcess.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvRiskAssessmentProcess.SelectedIndexes.Clear();
        gvRiskAssessmentProcess.EditIndexes.Clear();
        gvRiskAssessmentProcess.DataSource = null;
        gvRiskAssessmentProcess.Rebind();
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDEDITEDBY" };
            string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date", "Edited By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = new DataSet();
            if (rblOldNew.SelectedValue == "1")
            {
                ds = PhoenixDocumentManagementDocument.InspectionRiskAssessmentProcessSearch(
                                                                                         General.GetNullableString(txtRefNo.Text),
                                                                                         null,
                                                                                         null,
                                                                                         General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                         General.GetNullableInteger(ucVessel.SelectedVessel),
                                                                                         General.GetNullableInteger(ucType.SelectedCategory),
                                                                                         sortexpression,
                                                                                         sortdirection,
                                                                                         int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                         gvRiskAssessmentProcess.PageSize,
                                                                                         ref iRowCount,
                                                                                         ref iTotalPageCount,
                                                                                         companyid
                                                                                         );
            }
            else
            {
                ds = PhoenixDocumentManagementDocument.InspectionNonRoutineRASearchExtn(
                         int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvRiskAssessmentProcess.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , sortexpression
                        , sortdirection
                        , General.GetNullableString(txtRefNo.Text)
                        , companyid
                        , General.GetNullableInteger(ucVessel.SelectedVessel)
                        , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                        , General.GetNullableString(txtActivity.Text)
                        );
            }

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRiskAssessmentProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentProcess.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            if (rblOldNew.SelectedValue == "1")
            {
                rblOldNew.SelectedValue = "1";
                ShowExcel();
            }
            else
            {
                rblOldNew.SelectedValue = "0";
                ShowExcel();
            }
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (rblOldNew.SelectedValue == "1")
            {
                rblOldNew.SelectedValue = "1";
                gvRiskAssessmentProcess.Rebind();
            }
            else
            {
                rblOldNew.SelectedValue = "0";
                gvRiskAssessmentProcess.Rebind();
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            txtActivity.Text = "";
            ddlType.ClearSelection();
            ucType.SelectedCategory = string.Empty;
            ucVessel.SelectedVessel = string.Empty;
            BindData();
            gvRiskAssessmentProcess.Rebind();
        }
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRiskAssessmentProcess.Rebind();
    }

    protected void ucVessel_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRiskAssessmentProcess.Rebind();
    }

    protected void rblOldNew_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblOldNew.SelectedValue == "1")
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            ucType.Visible = true;
            ucType.SelectedCategory = string.Empty;
            ddlType.Visible = false;
            ucVessel.SelectedVessel =string.Empty;
            rblOldNew.SelectedValue = "1";
            BindData();
            gvRiskAssessmentProcess.Rebind();
        }
        else
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            ucType.Visible = false;
            ddlType.Visible = true;
            ddlType.ClearSelection();
            ucVessel.SelectedVessel =string.Empty;
            rblOldNew.SelectedValue = "0";
            BindData();
            gvRiskAssessmentProcess.Rebind();
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvRiskAssessmentProcess.Rebind();
    }
    protected void BindType()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentType();
        ddlType.DataSource = dt;
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDCATEGORYID";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void gvRiskAssessmentProcess_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvRiskAssessmentProcess_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblProcessID = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
            LinkButton cmdRAProcess = (LinkButton)e.Item.FindControl("cmdRAProcess");
            if (cmdRAProcess != null)
            {
                cmdRAProcess.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAProcess.CommandName);
            }
            RadLabel lblRAType = (RadLabel)e.Item.FindControl("lblRAType");
            if (lblRAType.Text == "8")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAProcess', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + lblProcessID.Text + "&showmenu=0&showword=NO&showexcel=NO');return false;");
            }
            if (lblRAType.Text == "11")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            if (lblRAType.Text == "12")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAMachinary', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            if (lblRAType.Text == "13")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            if (lblRAType.Text == "14")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RACargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            if (lblRAType.Text == "1")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERICNEW&genericid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return true;");
            }
            if (lblRAType.Text == "2")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATIONNEW&navigationid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return true;");
            }
            if (lblRAType.Text == "3")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAMachinary', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return true;");
            }
            if (lblRAType.Text == "4")
            {
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RACargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGONEW&genericid=" + lblProcessID.Text + "&showmenu=0&showexcel=NO');return true;");
            }

        }

        UserControlToolTip ucJobActivity = (UserControlToolTip)e.Item.FindControl("ucJobActivity");
        RadLabel lblJobActivity = (RadLabel)e.Item.FindControl("lblJobActivity");
        if (lblJobActivity != null)
        {
            ucJobActivity.Position = ToolTipPosition.TopCenter;
            ucJobActivity.TargetControlId = lblJobActivity.ClientID;
        }

        UserControlToolTip ucProcessName = (UserControlToolTip)e.Item.FindControl("ucProcessName");
        RadLabel lblProcessName = (RadLabel)e.Item.FindControl("lblProcessName");
        if (lblProcessName != null)
        {
            ucProcessName.Position = ToolTipPosition.TopCenter;
            ucProcessName.TargetControlId = lblProcessName.ClientID;

        }
    }

}
