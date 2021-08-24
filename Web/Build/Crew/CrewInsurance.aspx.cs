using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewInsurance : PhoenixBasePage
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            CrewInsuranceTab.AccessRights = this.ViewState;
            CrewInsuranceTab.MenuList = toolbar2.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewInsurance.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewInsurance')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewInsuranceArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewInsurance.AccessRights = this.ViewState;
            MenuCrewInsurance.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewInsurance.aspx?e=2", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewInsuranceCover')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewIC.AccessRights = this.ViewState;
            CrewIC.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["INSURANCEID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                
                gvCrewInsurance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["ICPAGENUMBER"] = 1;
                ViewState["ICSORTEXPRESSION"] = null;
                ViewState["ICSORTDIRECTION"] = null;
                gvCrewInsuranceCover.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                SetEmployeePrimaryDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
                if (dt.Rows[0]["FLDOFFCREW"].ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            50, "OFF"))
                {
                    lblOffcrew.Text = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            50, "RAT");

                }
                else
                {
                    lblOffcrew.Text = dt.Rows[0]["FLDOFFCREW"].ToString();
                }

                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        ICRebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDQUICKNAME", "FLDINSURANCENUMBER", "FLDEFFECTIVEDATE", "FLDEXPIRYDATE", "FLDREMARKS" };
            string[] alCaptions = { "Insurer", "Policy Number", "Policy Start Date", "Policy End Date", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewInsurance.CrewInsuranceSearch(
                        General.GetNullableInteger(Filter.CurrentCrewSelection.ToString())
                        , 1
                        , sortexpression
                        , sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewInsurance.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewInsurance", "Crew Insurance", alCaptions, alColumns, ds);

            gvCrewInsurance.DataSource = ds;
            gvCrewInsurance.VirtualItemCount = iRowCount;
           
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void CrewInsurance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDQUICKNAME", "FLDINSURANCENUMBER", "FLDEFFECTIVEDATE", "FLDEXPIRYDATE", "FLDREMARKS" };
                string[] alCaptions = { "Insurer", "Policy Number", "Policy Start Date", "Policy End Date", "Remarks" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInsurance.CrewInsuranceSearch(
                            General.GetNullableInteger(Filter.CurrentCrewSelection.ToString())
                            , 1
                            , sortexpression, sortdirection
                            , (int)ViewState["PAGENUMBER"]
                            , iRowCount
                            , ref iRowCount
                            , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Insurance", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

            else if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "2")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDNAME", "FLDINCLUSIONDATE", "FLDEXCLUSIONDATE", "FLDDATEOFBIRTH" };
                string[] alCaptions = { "Family Member", "Inclusion Date", "Exclusion Date", "D.O.B" };
                string sortexpression = (ViewState["ICSORTEXPRESSION"] == null) ? null : (ViewState["ICSORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["ICSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["ICSORTDIRECTION"].ToString());

                if (ViewState["ICROWCOUNT"] == null || Int32.Parse(ViewState["ICROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ICROWCOUNT"].ToString());

                string Insuranceid = (ViewState["INSURANCEID"] == null) ? null : (ViewState["INSURANCEID"].ToString());
                
                DataSet ds = PhoenixCrewInsurance.CrewInsuranceCoverSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                            , General.GetNullableInteger(Insuranceid)
                            , sortexpression, sortdirection
                            , (int)ViewState["ICPAGENUMBER"]
                            , iRowCount
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Insurance cover", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewInsurance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewInsurance.CurrentPageIndex + 1;
        BindData();
    }
    protected void gvCrewInsurance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INSURANCE + "&cmdname=INSURANCEUPLOAD'); return false;");
            }
        }

        if (e.Item.IsInEditMode)
        {
            RadComboBox ddlinsuranceedit = (RadComboBox)e.Item.FindControl("ddlInsuranceEdit");
            RadLabel insurencevalue = (RadLabel)e.Item.FindControl("lblInsuranceIdEdit");
            ddlinsuranceedit.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 149);
            ddlinsuranceedit.DataTextField = "FLDQUICKNAME";
            ddlinsuranceedit.DataValueField = "FLDQUICKCODE";
            ddlinsuranceedit.DataBind();
            ddlinsuranceedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            DataRowView drv = (DataRowView)e.Item.DataItem;
            ddlinsuranceedit.SelectedValue = insurencevalue.Text;

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddlinsurance = (RadComboBox)e.Item.FindControl("ddlInsuranceAdd");
            ddlinsurance.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 149);
            ddlinsurance.DataTextField = "FLDQUICKNAME";
            ddlinsurance.DataValueField = "FLDQUICKCODE";
            ddlinsurance.DataBind();

            ddlinsurance.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        }

    }
 
    protected void gvCrewInsurance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                ViewState["INSURANCEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceId")).Text;
                ICRebind();
            }

            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                
                string ddlInsuranceNameAdd = ((RadComboBox)e.Item.FindControl("ddlInsuranceAdd")).SelectedItem.ToString();
                string lblInsuranceIdAdd = ((RadComboBox)e.Item.FindControl("ddlInsuranceAdd")).SelectedValue;
                string lblInsuranceNumberAdd = ((RadTextBox)e.Item.FindControl("txtInsuranceNumberAdd")).Text;
                string lblRemarksAdd = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                UserControlDate effectivedate = ((UserControlDate)e.Item.FindControl("txtEffectiveDateAdd"));
                UserControlDate expirydate = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));

                if (!IsValidInsurance(lblInsuranceIdAdd, effectivedate, expirydate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewInsurance.InsertCrewInsurance(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(lblInsuranceIdAdd)
                    , General.GetNullableString(lblInsuranceNumberAdd)
                    , General.GetNullableDateTime(effectivedate.Text)
                    , General.GetNullableDateTime(expirydate.Text)
                    , General.GetNullableString(lblRemarksAdd)
                   );

                ViewState["INSURANCEID"] = null;
                Rebind();
                ICRebind();
                
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string lblEmployeeInsuranceId = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceId")).Text;
                PhoenixCrewInsurance.ArchiveCrewInsurance(Int32.Parse(lblEmployeeInsuranceId), 0);

                Rebind();
                ICRebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvCrewInsurance_UpdateCommand(object sender, GridCommandEventArgs e)
    {    
        try
        {

            string ddlInsuranceNameAdd = ((RadComboBox)e.Item.FindControl("ddlInsuranceEdit")).SelectedItem.ToString();
            string lblInsuranceIdAdd = ((RadComboBox)e.Item.FindControl("ddlInsuranceEdit")).SelectedValue;
            string lblInsuranceNumberEdit = ((RadTextBox)e.Item.FindControl("txtInsuranceNumberEdit")).Text;
            string lblRemarksEdit = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

            UserControlDate effectivedate = ((UserControlDate)e.Item.FindControl("txtEffectiveDateEdit"));
            UserControlDate expirydate = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
            string employeeinsuranceid = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceIdEdit")).Text;

            if (!IsValidInsurance(lblInsuranceIdAdd, effectivedate, expirydate))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewInsurance.UpdateCrewInsurance(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(employeeinsuranceid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(lblInsuranceIdAdd)
                , General.GetNullableString(lblInsuranceNumberEdit)
                , General.GetNullableDateTime(effectivedate.Text)
                , General.GetNullableDateTime(expirydate.Text)
                , General.GetNullableString(lblRemarksEdit)
               );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        Rebind();
        ICRebind();
    }

    protected void gvCrewInsurance_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
   
    protected void gvCrewInsurance_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {          
            string employeeinsuranceid = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceId")).Text;

            PhoenixCrewInsurance.DeleteCrewInsurance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               Convert.ToInt32(employeeinsuranceid)
                , int.Parse(Filter.CurrentCrewSelection));

            ViewState["INSURANCEID"] = null;
            Rebind();
            ICRebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidInsurance(string lblInsuranceId, UserControlDate effectivedate, UserControlDate expirydate)
    {
        int resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(lblInsuranceId, out resultInt))
            ucError.ErrorMessage = "Insurance is required";

        if (string.IsNullOrEmpty(effectivedate.Text))
            ucError.ErrorMessage = "Effective Date is required.";
        else if (DateTime.TryParse(effectivedate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Effective Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(expirydate.Text))
            ucError.ErrorMessage = "Expiry Date is required.";

        if (expirydate.Text != null && expirydate.Text != null)
        {
            if ((DateTime.TryParse(effectivedate.Text, out resultDate)) && (DateTime.TryParse(expirydate.Text, out resultDate)))
                if ((DateTime.Parse(effectivedate.Text)) >= (DateTime.Parse(expirydate.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Effective Date'";
        }

        return (!ucError.IsError);
    }

    private bool IsValidInsuranceCover(string familyid, UserControlDate inclusiondate, UserControlDate exclusiondate, string employeeinsuranceid)
    {
        int resultInt;
        DateTime resultDate;

        if (!int.TryParse(familyid, out resultInt))
            ucError.ErrorMessage = "Family member is required";

        if (string.IsNullOrEmpty(inclusiondate.Text) && inclusiondate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Inclusion date is required.";
        else if (DateTime.TryParse(inclusiondate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Inclusion Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(exclusiondate.Text) && exclusiondate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Exclusion Date is required.";

        if (exclusiondate.Text != null && exclusiondate.Text != null)
        {
            if ((DateTime.TryParse(inclusiondate.Text, out resultDate)) && (DateTime.TryParse(exclusiondate.Text, out resultDate)))
                if ((DateTime.Parse(inclusiondate.Text)) >= (DateTime.Parse(exclusiondate.Text)))
                    ucError.ErrorMessage = "'Exclusion Date' should be greater than 'Inclusion Date'";
        }
        if (employeeinsuranceid == "")
        {
            ucError.ErrorMessage = "Please select Employee Insurance";
        }

        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewInsuranceCover_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["ICPAGENUMBER"] = ViewState["ICPAGENUMBER"] != null ? ViewState["ICPAGENUMBER"] : gvCrewInsuranceCover.CurrentPageIndex + 1;
        BindICData();
    }


    private void BindICData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNAME", "FLDINCLUSIONDATE", "FLDEXCLUSIONDATE", "FLDDATEOFBIRTH" };
            string[] alCaptions = { "Family Member", "Inclusion Date", "Exclusion Date", "D.O.B" };
            string sortexpression = (ViewState["ICSORTEXPRESSION"] == null) ? null : (ViewState["ICSORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["ICSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["ICSORTDIRECTION"].ToString());
        
            string Insuranceid = (ViewState["INSURANCEID"] == null) ? null : (ViewState["INSURANCEID"].ToString());

            DataSet ds = PhoenixCrewInsurance.CrewInsuranceCoverSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                        , General.GetNullableInteger(Insuranceid)
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["ICPAGENUMBER"].ToString())
                        , gvCrewInsuranceCover.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewInsuranceCover", "Insurance Cover", alCaptions, alColumns, ds);
            gvCrewInsuranceCover.DataSource = ds;
            gvCrewInsuranceCover.VirtualItemCount = iRowCount;

            ViewState["ICROWCOUNT"] = iRowCount;
            ViewState["ICTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvCrewInsuranceCover_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INSURANCECOVER + "&cmdname=INSURANCECOVERUPLOAD'); return false;");                
            }
        }

        if (e.Item.IsInEditMode)
        {
            RadComboBox ddlfamilymemberedit = (RadComboBox)e.Item.FindControl("ddlfamilymemberedit");
            RadLabel lblfamilyid = (RadLabel)e.Item.FindControl("lblfamilymemberedit");
            ddlfamilymemberedit.DataSource = PhoenixCrewFamilyNok.ListEmployeeFamily(Int32.Parse(Filter.CurrentCrewSelection.ToString()), null);
            ddlfamilymemberedit.DataTextField = "FLDFIRSTNAME";
            ddlfamilymemberedit.DataValueField = "FLDFAMILYID";
            ddlfamilymemberedit.DataBind();
            ddlfamilymemberedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ddlfamilymemberedit.SelectedValue = lblfamilyid.Text;            
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddlfamily = (RadComboBox)e.Item.FindControl("ddlfamilymemberadd");
            ddlfamily.DataSource = PhoenixCrewFamilyNok.ListEmployeeFamily(Int32.Parse(Filter.CurrentCrewSelection.ToString()), null);
            ddlfamily.DataTextField = "FLDNAME";
            ddlfamily.DataValueField = "FLDFAMILYID";
            ddlfamily.DataBind();
            ddlfamily.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
          
        }
    }

   
    protected void gvCrewInsuranceCover_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
            
                string Insuranceid = (ViewState["INSURANCEID"] == null) ? null : (ViewState["INSURANCEID"].ToString());

                string familyid = ((RadComboBox)e.Item.FindControl("ddlfamilymemberadd")).SelectedValue;
                UserControlDate inclusiondate = ((UserControlDate)e.Item.FindControl("txtInclusionDateAdd"));
                UserControlDate exclusiondate = ((UserControlDate)e.Item.FindControl("txtExclusionDateAdd"));
              
                if (!IsValidInsuranceCover(familyid, inclusiondate, exclusiondate, Insuranceid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewInsurance.InsertCrewInsuranceCover(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Insuranceid)
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(familyid)
                    , General.GetNullableDateTime(inclusiondate.Text)
                    , General.GetNullableDateTime(exclusiondate.Text)
                    );
                ICRebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string lblCoverId = ((RadLabel)e.Item.FindControl("lblCoverId")).Text;
                PhoenixCrewInsurance.ArchiveCrewFamily(Int32.Parse(lblCoverId), 0);
                ICRebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["ICPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCrewInsuranceCover_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string coverid = ((RadLabel)e.Item.FindControl("lblCoverIdEdit")).Text;
            string familyid = ((RadComboBox)e.Item.FindControl("ddlfamilymemberedit")).SelectedValue;
            UserControlDate inclusiondate = ((UserControlDate)e.Item.FindControl("txtInclusionDateEdit"));
            UserControlDate exclusiondate = ((UserControlDate)e.Item.FindControl("txtExclusionDateEdit"));
            
            string Insuranceid = (ViewState["INSURANCEID"] == null) ? null : (ViewState["INSURANCEID"].ToString());
            
            if (!IsValidInsuranceCover(familyid, inclusiondate, exclusiondate, Insuranceid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewInsurance.UpdateCrewInsuranceCover(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(coverid)
                , Convert.ToInt32(Insuranceid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(familyid)
                , General.GetNullableDateTime(inclusiondate.Text)
                , General.GetNullableDateTime(exclusiondate.Text)
               );

         ICRebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewInsuranceCover_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {         
            string coverid = ((RadLabel)e.Item.FindControl("lblCoverId")).Text;

            PhoenixCrewInsurance.DeleteCrewInsuranceCover(Convert.ToInt32(coverid));
            ICRebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    
    protected void gvCrewInsuranceCover_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void Rebind()
    {
        gvCrewInsurance.SelectedIndexes.Clear();
        gvCrewInsurance.EditIndexes.Clear();
        gvCrewInsurance.DataSource = null;
        gvCrewInsurance.Rebind();
    }
    protected void ICRebind()
    {
        gvCrewInsuranceCover.SelectedIndexes.Clear();
        gvCrewInsuranceCover.EditIndexes.Clear();
        gvCrewInsuranceCover.DataSource = null;
        gvCrewInsuranceCover.Rebind();
    }
}
