using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDocumentMedical : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentMedical.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentMedical.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersDocumentMedicalAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDLICENCE");

        MenuRegistersDocumentMedical.AccessRights = this.ViewState;
        MenuRegistersDocumentMedical.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Medical Test", "MEDICALTEST");
        toolbar.AddButton("Cost of Medical", "COSTOFMEDICAL");
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            toolbar.AddButton("PMU Doctor List", "PMUDOCTOR");
        MenuMedicalCost.AccessRights = this.ViewState;
        MenuMedicalCost.MenuList = toolbar.Show();
        MenuMedicalCost.SelectedMenuIndex = 0;

        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {        
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDocumentMedical.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCATEGORYNAME", "FLDNAMEOFMEDICAL", "FLDCODE", "FLDMEDICALTYPE", "FLDACTIVE", "FLDEXPIRY", "FLDFREQUENCYNAME", "FLDSTAGE", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", 
                                 "FLDUSERGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYNNAME", "FLDPHOTOCOPYACCEPTABLEYNNAME" };
        string[] alCaptions = { "Document Category", "Test Name", "Code", "P&I/UK P&I", "Active Y/N", "Expiry Y/N", "Frequency", "Offshore Stage", "Mandatory Y/N", "Waiver Y/N", "User Group to allow Waiver",
                                 "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N", "Show in Master's checklist onboard Y/N", "Photocopy Acceptable Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string MedicalSearch = (txtSearchMedical.Text == null) ? "" : txtSearchMedical.Text;

        ds = PhoenixRegistersDocumentMedical.DocumentMedicalSearch(MedicalSearch, null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentMedical.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1")
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentMedical.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Medical</h3></td>");
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

    protected void MedicalCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("MEDICALTEST"))
        {
            MenuMedicalCost.SelectedMenuIndex = 0;
            return;
        }
        else if (CommandName.ToUpper().Equals("COSTOFMEDICAL"))
        {
            Response.Redirect("../Registers/RegistersMedicalCostMapping.aspx");
        }
        else if (CommandName.ToUpper().Equals("PMUDOCTOR"))
        {
            Response.Redirect("../Registers/RegistersPMUDoctor.aspx");
        }
    }

    protected void RegistersDocumentMedical_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentMedical.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME", "FLDNAMEOFMEDICAL", "FLDCODE", "FLDMEDICALTYPE", "FLDACTIVE", "FLDEXPIRY", "FLDFREQUENCYNAME", "FLDSTAGE", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", 
                                 "FLDUSERGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYNNAME", "FLDPHOTOCOPYACCEPTABLEYNNAME" };
        string[] alCaptions = { "Document Category", "Test Name", "Code", "P&I/UK P&I", "Active Y/N", "Expiry Y/N", "Frequency", "Offshore Stage", "Mandatory Y/N", "Waiver Y/N", "User Group to allow Waiver",
                                 "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N", "Show in Master's checklist onboard Y/N", "Photocopy Acceptable Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string MedicalSearch = (txtSearchMedical.Text == null) ? "" : txtSearchMedical.Text;

        DataSet ds = PhoenixRegistersDocumentMedical.DocumentMedicalSearch(MedicalSearch, null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
           gvDocumentMedical.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
              chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"));
        
        General.SetPrintOptions("gvDocumentMedical", "Medical", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentMedical.DataSource = ds;
            gvDocumentMedical.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentMedical.DataSource = "";
        }
    }
    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentOther.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
       // ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentMedical.Rebind();
    }
    
   
    private bool IsValidDocumentMedical(string nameofmedical,string code,string agefrom,string ageto,string frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (nameofmedical.Trim().Equals(""))
            ucError.ErrorMessage = "Medical Name is required.";

        if (General.GetNullableString(code) == null)
            ucError.ErrorMessage = "Code is required.";
        
        if (General.GetNullableInteger(frequency) == null)
            ucError.ErrorMessage = "Frequency is required.";


        return (!ucError.IsError);
    }

    private void DeleteDocumentMedical(int documentmedicalid)
    {
        PhoenixRegistersDocumentMedical.DeleteDocumentMedical(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentmedicalid);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentMedical.Rebind();
    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void chkExpiryYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox dc = (RadCheckBox)sender;
        GridDataItem Item = (GridDataItem)dc.NamingContainer;
        UserControlNumber ucExpiryPeriodEdit = (UserControlNumber)Item.FindControl("ucExpiryPeriodEdit");
        if (dc.Checked==true)
        {
            ucExpiryPeriodEdit.ReadOnly = "false";
            ucExpiryPeriodEdit.CssClass = "input";
        }
        else
        {
            ucExpiryPeriodEdit.ReadOnly = "true";
            ucExpiryPeriodEdit.CssClass = "readonlytextbox";
            ucExpiryPeriodEdit.Text = "";
        }

    }
    protected void chkExpiryYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox dc = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
        UserControlNumber ucExpiryPeriodAdd = (UserControlNumber)Item.FindControl("ucExpiryPeriodAdd");
        if (dc.Checked == true)
        {
            ucExpiryPeriodAdd.ReadOnly = "false";
            ucExpiryPeriodAdd.CssClass = "input";
        }
        else
        {
            ucExpiryPeriodAdd.ReadOnly = "true";
            ucExpiryPeriodAdd.CssClass = "readonlytextbox";
        }

    }

    protected void chkMandatoryYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridDataItem Item = (GridDataItem)cb.NamingContainer;
        RadCheckBox chkWaiverYNEdit = (RadCheckBox)Item.FindControl("chkWaiverYNEdit");
        RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)Item.FindControl("chkUserGroupEdit");
        if (chkWaiverYNEdit != null)
        {
            if (cb.Checked==true)
            {
                chkWaiverYNEdit.Enabled = true;
            }
            else
            {
                chkWaiverYNEdit.Checked = false;
                chkWaiverYNEdit.Enabled = false;

                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkMandatoryYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)cb.NamingContainer;
        RadCheckBox chkWaiverYNAdd = (RadCheckBox)Item.FindControl("chkWaiverYNAdd");
        RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)Item.FindControl("chkUserGroupAdd");
        if (chkWaiverYNAdd != null)
        {
            if (cb.Checked==true)
                chkWaiverYNAdd.Enabled = true;
            else
            {
                chkWaiverYNAdd.Checked = false;
                chkWaiverYNAdd.Enabled = false;

                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridDataItem Item = (GridDataItem)cb.NamingContainer;
        RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)Item.FindControl("chkUserGroupEdit");
        if (chkUserGroupEdit != null)
        {
            if (cb.Checked==true)
            {
                chkUserGroupEdit.Enabled = true;
            }
            else
            {
                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)cb.NamingContainer;
        RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)Item.FindControl("chkUserGroupAdd");

        if (chkUserGroupAdd != null)
        {
            if (cb.Checked==true)
            {
                chkUserGroupAdd.Enabled = true;
            }
            else
            {
                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }

    protected void gvDocumentMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT")) return;

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            string nameofmedical = ((RadTextBox)e.Item.FindControl("txtNameOfMedicalAdd")).Text;
            RadCheckBoxList cbl = (RadCheckBoxList)e.Item.FindControl("cblPIAdd");
            string type = string.Empty;
            foreach (ButtonListItem li in cbl.Items)
            {
                type += (li.Selected ? li.Value + "," : string.Empty);
            }
            RadCheckBoxList chka = (RadCheckBoxList)e.Item.FindControl("chkUserGroupAdd");
            string UGList = "";
            string UserGroupList = "";
            foreach (ButtonListItem li in chka.Items)
            {
                if (li.Selected)
                {
                    UGList += li.Value + ",";
                }
            }

            if (UGList != "")
            {
                UserGroupList = "," + UGList;
            }
            if (!IsValidDocumentMedical(nameofmedical
                    , ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text
                    , ((UserControlNumber)e.Item.FindControl("ucAgefromAdd")).Text
                    , ((UserControlNumber)e.Item.FindControl("ucAgeToAdd")).Text
                    , ((UserControlHard)e.Item.FindControl("ucFrequencyAdd")).SelectedHard
                    ))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersDocumentMedical.InsertDocumentMedical(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , nameofmedical
                , null, type
                , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true ? "1" : "0")
                , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkExpiryYNAdd")).Checked == true ? "1" : "0")
                , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucExpiryPeriodAdd")).Text)
                , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucFrequencyAdd")).SelectedHard)
                , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text)
                , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucAgefromAdd")).Text)
                , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucAgeToAdd")).Text)
                , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlStageAdd")).SelectedValue)
                , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkMandatoryYNAdd")).Checked == true ? "1" : "0")
                , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkWaiverYNAdd")).Checked == true ? "1" : "0")
                , General.GetNullableString(UserGroupList)
                , (((RadCheckBox)e.Item.FindControl("chkAdditionDocYnAdd")).Checked == true) ? 1 : 0
                , (((RadCheckBox)e.Item.FindControl("chkAuthReqYnAdd")).Checked == true) ? 1 : 0
                , General.GetNullableInteger(((UserControlDocumentCategory)e.Item.FindControl("ucCategoryAdd")).SelectedDocumentCategoryID)
                , (((RadCheckBox)e.Item.FindControl("chkShowInMasterChecklistYNAdd")).Checked == true) ? 1 : 0
                , (((RadCheckBox)e.Item.FindControl("chkPhotocopyAcceptableYnAdd")).Checked == true) ? 1 : 0
                );
            BindData();
            gvDocumentMedical.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            string name = ((RadTextBox)e.Item.FindControl("txtNameOfMedicalEdit")).Text;
            RadCheckBoxList cbl = (RadCheckBoxList)e.Item.FindControl("cblPIEdit");
            string type = string.Empty;
            foreach (ButtonListItem li in cbl.Items)
            {
                type += (li.Selected ? li.Value + "," : string.Empty);
            }
            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
            string UGList = "";
            string UserGroupList = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    UGList += li.Value + ",";
                }
            }

            if (UGList != "")
            {
                UserGroupList = "," + UGList;
            }
            if (!IsValidDocumentMedical(name
                    , ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text
                    , ((UserControlNumber)e.Item.FindControl("ucAgefromEdit")).Text
                    , ((UserControlNumber)e.Item.FindControl("ucAgeToEdit")).Text
                    , ((UserControlHard)e.Item.FindControl("ucFrequencyEdit")).SelectedHard
                ))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersDocumentMedical.UpdateDocumentMedical(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentMedicalIdEdit")).Text)
            , name
            , null, type.TrimEnd(',')
            , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true ? "1" : "0")
            , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkExpiryYNEdit")).Checked == true ? "1" : "0")
            , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucExpiryPeriodEdit")).Text)
            , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucFrequencyEdit")).SelectedHard)
            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text)
            , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucAgefromEdit")).Text)
            , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucAgeToEdit")).Text)
            , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlStageEdit")).SelectedValue)
            , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit")).Checked == true ? "1" : "0")
            , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkWaiverYNEdit")).Checked == true ? "1" : "0")
            , General.GetNullableString(UserGroupList)
            , (((RadCheckBox)e.Item.FindControl("chkAdditionDocYnEdit")).Checked == true) ? 1 : 0
            , (((RadCheckBox)e.Item.FindControl("chkAuthReqYnEdit")).Checked == true) ? 1 : 0
            , General.GetNullableInteger(((UserControlDocumentCategory)e.Item.FindControl("ucCategoryEdit")).SelectedDocumentCategoryID)
            , (((RadCheckBox)e.Item.FindControl("chkShowInMasterChecklistYNEdit")).Checked == true) ? 1 : 0
            , (((RadCheckBox)e.Item.FindControl("chkPhotocopyAcceptableYnEdit")).Checked == true) ? 1 : 0
            );
            ucStatus.Text = "Medical information updated";
            BindData();
            gvDocumentMedical.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteDocumentMedical(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentMedicalId")).Text));
            BindData();
            gvDocumentMedical.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvDocumentMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentMedical.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
           
            RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentMedicalId");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentMedicalAdd.aspx?MedicalId=" + l.Text + "');return false;");
            }
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkNameOfMedical");
            if (lbtn != null)
                lbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentMedicalAdd.aspx?MedicalId=" + l.Text + "');return false;");

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadCheckBoxList cbl = (RadCheckBoxList)e.Item.FindControl("cblPIEdit");
            if(cbl!=null)
            {
                foreach (ButtonListItem li in cbl.Items)
                {
                    if (("," + drv["FLDMEDICALTYPECODE"].ToString() + ",").Contains("," + li.Value + ","))
                        li.Selected = true;
                }
            }
            
            UserControlHard ucFrequencyEdit = (UserControlHard)e.Item.FindControl("ucFrequencyEdit");
            if (ucFrequencyEdit != null)
            {
                ucFrequencyEdit.SelectedHard = drv["FLDFREQUENCY"].ToString();
            }

            RadCheckBox chkExpiryYNEdit = (RadCheckBox)e.Item.FindControl("chkExpiryYNEdit");
            if (chkExpiryYNEdit != null)
                chkExpiryYNEdit.Checked = drv["FLDEXPIRYYN"].ToString() == "1" ? true : false;

            UserControlNumber ucExpiryPeriodEdit = (UserControlNumber)e.Item.FindControl("ucExpiryPeriodEdit");
            if (ucExpiryPeriodEdit != null)
            {
                ucExpiryPeriodEdit.ReadOnly = drv["FLDEXPIRYYN"].ToString() == "1" ? "false" : "true";
                ucExpiryPeriodEdit.CssClass = drv["FLDEXPIRYYN"].ToString() == "1" ? "input" : "readonlytextbox";
                ucExpiryPeriodEdit.Text = drv["FLDEXPIRYYN"].ToString() == "1" ? drv["FLDEXPIRYPERIOD"].ToString() : "";
            }

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucGroupEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDGROUP"].ToString();

            RadComboBox ddlStageEdit = (RadComboBox)e.Item.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = drvHard["FLDSTAGEID"].ToString();
            }


            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
            RadCheckBox chkMandatoryYNEdit = (RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit");
            RadCheckBox chkWaiverYNEdit = (RadCheckBox)e.Item.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked==true)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked == true)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataBindings.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
                foreach (ButtonListItem li in chk.Items)
                {
                    string[] slist = drv["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            UserControlDocumentCategory ucCategory = (UserControlDocumentCategory)e.Item.FindControl("ucCategoryEdit");
            if (ucCategory != null)
            {
                ucCategory.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategory.DataBind();
                if (General.GetNullableInteger(drv["FLDDOCUMENTCATEGORYID"].ToString()) != null)
                    ucCategory.SelectedDocumentCategoryID = drv["FLDDOCUMENTCATEGORYID"].ToString();
            }
            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");

            LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImgUserGroup");

            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
            LinkButton lblRankNameList = (LinkButton)e.Item.FindControl("lblRankNameList");
            if (lblRankNameList != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDRANKNAME").ToString() == string.Empty)
                    lblRankNameList.Visible = false;
            }

            LinkButton lblVesselType = (LinkButton)e.Item.FindControl("lblVesselType");
            if (lblVesselType != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDVESSELTYPE").ToString() == string.Empty)
                    lblVesselType.Visible = false;
            }
            LinkButton lblowner = (LinkButton)e.Item.FindControl("lblowner");
            if (lblowner != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDOWNERLIST").ToString() == string.Empty)
                    lblowner.Visible = false;
            }
            LinkButton lblCompany = (LinkButton)e.Item.FindControl("lblCompany");
            if (lblCompany != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPANIES").ToString() == string.Empty)
                    lblCompany.Visible = false;
            }
            LinkButton lblFlag = (LinkButton)e.Item.FindControl("lblFlag");
            if (lblFlag != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDFLAG").ToString() == string.Empty)
                    lblFlag.Visible = false;
            }
        }
       
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlNumber ucExpiryPeriodAdd = (UserControlNumber)e.Item.FindControl("ucExpiryPeriodAdd");
            if (ucExpiryPeriodAdd != null)
            {
                ucExpiryPeriodAdd.ReadOnly = "true";
                ucExpiryPeriodAdd.CssClass = "readonlytextbox";
            }

            RadComboBox ddlStageAdd = (RadComboBox)e.Item.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)e.Item.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataBindings.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataBindings.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }
            UserControlDocumentCategory ucCategoryAdd = (UserControlDocumentCategory)e.Item.FindControl("ucCategoryAdd");
            if (ucCategoryAdd != null)
            {
                ucCategoryAdd.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategoryAdd.DataBind();
            }
        }

    }

    protected void gvDocumentMedical_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;        
        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        gvDocumentMedical.Rebind();
    }
}
